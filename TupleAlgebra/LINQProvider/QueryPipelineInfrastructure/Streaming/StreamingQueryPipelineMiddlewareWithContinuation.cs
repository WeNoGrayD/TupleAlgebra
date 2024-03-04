using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider.QueryPipelineInfrastructure.Buffering;

namespace LINQProvider.QueryPipelineInfrastructure.Streaming
{
    public abstract class StreamingQueryPipelineMiddlewareWithContinuation<TData, TQueryResultData>
        : QueryPipelineMiddleware<
            TData, 
            IEnumerable<TQueryResultData>,
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>>,
          IStreamingQueryPipelineMiddleware<TData>
    {
        #region Instance fields

        protected LinkedListNode<IQueryPipelineMiddleware> _node;

        public IStreamingQueryExecutor<TQueryResultData> _nextExecutor;

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        protected bool _mustGoOn = true;

        #endregion

        #region Instance properties

        public LinkedListNode<IQueryPipelineMiddleware> PipelineScheduleNode { get => _node; }

        public IQueryPipelineMiddleware PreviousMiddleware { get => (_node.Previous?.Value)!; }

        /// <summary>
        /// Следующий компонент в конвейере.
        /// </summary>
        public IQueryPipelineMiddleware NextMiddleware 
        { 
            get => _node.Next!.Value;
            /*
             * При явном изменении ссылки на следующий компонент конвейера 
             * также обновляется ссылка на исполнитель запроса следующего компонента.
             */
            set
            {
                _node.Next!.Value = value;
                _nextExecutor = (value as IStreamingQueryPipelineMiddleware<TQueryResultData>)!.Executor;
            }
        }

        public IStreamingQueryExecutor<TData> Executor { get => _innerExecutorImpl; }

        public override bool ResultProvided
        {
            get => base.ResultProvided;
            set
            {
                _resultProvided = value;
            }
        }

        public override bool MustGoOn
        {
            get => _mustGoOn;
            set
            {
                if (value != _mustGoOn)
                {
                    _mustGoOn = value;
                    /*
                     * Оповещение предыдущего компонента конвейера, при его наличии,
                     * об изменении параметра MustGoOn.
                     * Необходимо для корректной работы конвейера в случае присутствия в нём
                     * потокового компонента со множественными выходами к одному входу,
                     * поскольку расчёт параметра MustGoOn от последующих компонентов
                     * производится в отложенном режиме.
                     */
                    if (PreviousMiddleware is not null)
                        PreviousMiddleware.MustGoOn &= value;
                    //if (!value) OnFailingQuery();
                }
            }
        }

        public override IQueryPipelineEndpoint PipelineEndpoint
        { get => NextMiddleware.PipelineEndpoint; }//(_node.List!.Last!.Value as IQueryPipelineEndpoint)!; }

        #endregion

        #region Instance events

        /// <summary>
        /// Событие пропуска данных на возможный следующий компонент конвейера запросов. 
        /// </summary>
        public event Action<IEnumerable<TQueryResultData>> DataPassed;

        /// <summary>
        /// Событие данных, которые на следующий компонент конвейера запросов не должны быть пропущены.
        /// </summary>
        public event Action<IEnumerable<TQueryResultData>>? DataNotPassed;

        public event Action FailingQuery;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="nextExecutor"></param>
        public StreamingQueryPipelineMiddlewareWithContinuation(
            LinkedListNode<IQueryPipelineMiddleware> node,
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IStreamingQueryPipelineMiddleware<TQueryResultData> nextMiddleware)
            : base(innerExecutor)
        {
            node.Value = this;
            node.List!.AddAfter(node, nextMiddleware.PipelineScheduleNode);
            _node = node;
            _nextExecutor = nextMiddleware.Executor;

            innerExecutor.DataPassed += OnDataPassed;
            innerExecutor.DataNotPassed += OnDataNotPassed;

            DataPassed += DataPassedHandler;

            return;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Вызов события пропуска данных с готовым промежуточным результатом.
        /// </summary>
        /// <param name="outputData"></param>
        protected void OnDataPassed(IEnumerable<TQueryResultData> outputData)
        {
            DataPassed?.Invoke(outputData);

            return;
        }

        /// <summary>
        /// Вызов события непропуска данных с готовым промежуточным результатом.
        /// </summary>
        /// <param name="outputData"></param>
        protected void OnDataNotPassed(IEnumerable<TQueryResultData> outputData)
        {
            DataNotPassed?.Invoke(outputData);
        }

        /// <summary>
        /// Обработчик события пропуска промежуточных данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="outputData"></param>
        protected virtual void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            ResultProvided = true;

            return;
        }

        #endregion

        #region IQueryPipelineMiddleware implementation

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов.
        /// будет какое-либо агрегируемое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public override void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            _innerExecutorImpl.PrepareToQueryStart();
            NextMiddleware.PrepareToAggregableResult<TPipelineQueryResult>(pipelineQueryExecutor);

            return;
        }

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов.
        /// будет какое-либо перечислимое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public override void PrepareToEnumerableResult<TPipelineQueryResultData>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            _innerExecutorImpl.PrepareToQueryStart();
            NextMiddleware.PrepareToEnumerableResult<TPipelineQueryResultData>(pipelineQueryExecutor);

            return;
        }

        public virtual void PreparePipelineResultProvider()
        {
            (NextMiddleware as IStreamingQueryPipelineMiddleware<TQueryResultData>)!
                .PreparePipelineResultProvider();

            return;
        }

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            return NextMiddleware.GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipelineQueryExecutor);
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            return NextMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipelineQueryExecutor);
        }

        #endregion

        #region IQueryPipelineAcceptor implementation

        public override TPipelineQueryResult AcceptToExecuteWithAggregableResult<TPipelineQueryResult>(
            ISingleQueryExecutorResultRequester resultRequster)
        {
            return resultRequster.VisitStreamingQueryExecutorWithExpectedAggregableResult<
                TData, 
                IEnumerable<TQueryResultData>, 
                TPipelineQueryResult>(
                    _innerExecutorImpl);
        }

        public override IEnumerable<TPipelineQueryResultData> AcceptToExecuteWithEnumerableResult<TPipelineQueryResultData>(
            System.Collections.IEnumerable dataSource,
            ISingleQueryExecutorResultRequester resultRequster)
        {
            return resultRequster.VisitStreamingQueryExecutorWithExpectedEnumerableResult<
                TData,
                IEnumerable<TQueryResultData>,
                TPipelineQueryResultData>(
                    dataSource,
                    this,
                    _innerExecutorImpl);
        }

        #endregion

        #region IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> implementation

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <param name="continuingExecutor"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public override IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> ContinueWith(
            IQueryPipelineEndpoint continuingExecutor,
            IQueryPipelineScheduler scheduler)
        {
            NextMiddleware.ContinueWith(continuingExecutor, scheduler);

            return this;
        }

        public void OnFailingQuery()
        {
            FailingQuery?.Invoke();

            return;
        }

        #endregion

        #region IAcceptor<ISingleQueryExecutorVisitor<TData>> implementation

        public override void Accept(ISingleQueryExecutorVisitor<TData> visitor)
        {
            visitor.VisitStreamingQueryExecutor(_innerExecutorImpl);

            return;
        }

        #endregion
    }

    /// <summary>
    /// Компонент конвейера запросов с аккумулированием результата вида "один к одному" потокового исполнителя.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public class StreamingQueryPipelineMiddlewareWithContinuationAndOneToOneResult<TData, TQueryResultData>
        : StreamingQueryPipelineMiddlewareWithContinuation<TData, TQueryResultData>
    {
        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="nextExecutor"></param>
        public StreamingQueryPipelineMiddlewareWithContinuationAndOneToOneResult(
            LinkedListNode<IQueryPipelineMiddleware> node,
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IStreamingQueryPipelineMiddleware<TQueryResultData> nextMiddleware)
            : base(node, innerExecutor, nextMiddleware)
        {
            return;
        }

        #endregion
        protected override void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            base.DataPassedHandler(outputData);

            (NextMiddleware.ResultProvided, MustGoOn) = _nextExecutor.ExecuteOverDataInstance(outputData.First());
            MustGoOn &= NextMiddleware.MustGoOn;

            return;
        }
    }

    /// <summary>
    /// Компонент конвейера запросов с аккумулированием результата вида "один ко многим" потокового исполнителя.
    /// на следующий исполнитель запросов в конвейере.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public class StreamingQueryPipelineMiddlewareWithContinuationAndOneToManyResult<TData, TQueryResultData>
    : StreamingQueryPipelineMiddlewareWithContinuation<TData, TQueryResultData>
    {
        #region Instance fields

        IQueryPipelineEndpoint _queryPipelineEndpoint;

        /// <summary>
        /// Перечисление промежуточных результатов выполнения запроса.
        /// </summary>
        private IEnumerable<TQueryResultData> _queryResult;

        private IQueryPipelineExecutor _queryPipelineExecutor;

        private Func<bool> _pipelineResultProvided;

        #endregion

        #region Instance properties

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        //public override bool MustGoOn { get => _mustGoOn; }

        public override IQueryPipelineEndpoint PipelineEndpoint
        {
            get
            {
                IQueryPipelineEndpoint endpoint = base.PipelineEndpoint;

                _queryPipelineEndpoint = endpoint;

                return endpoint;
            }
        }

        /*
        public override bool MustGoOn
        {
            get => _mustGoOn;
            set
            {
                if (value != _mustGoOn)
                {
                    _mustGoOn = value;
                    if (PreviousMiddleware is not null)
                        PreviousMiddleware.MustGoOn &= value;
                    //if (!value) OnFailingQuery();
                }
            }
        }
        */

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="nextExecutor"></param>
        public StreamingQueryPipelineMiddlewareWithContinuationAndOneToManyResult(
            LinkedListNode<IQueryPipelineMiddleware> node,
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IStreamingQueryPipelineMiddleware<TQueryResultData> nextMiddleware)
            : base(node, innerExecutor, nextMiddleware)
        {
            return;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Обработчик события пропуска промежуточных данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="outputData"></param>
        protected override void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            base.DataPassedHandler(outputData);
            _queryResult = outputData;

            return;
        }

        public override void PrepareToEnumerableResult<TPipelineQueryResultData>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            base.PrepareToEnumerableResult<IEnumerable<TPipelineQueryResultData>>(pipelineQueryExecutor);
            InterruptPipelineResultProviding(pipelineQueryExecutor);

            return;
        }

        private void InterruptPipelineResultProviding(ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            _queryPipelineExecutor = (pipelineQueryExecutor as IQueryPipelineExecutor)!;
            _queryPipelineExecutor.PutPipelineResultProvider(() => ResultProvided);

            return;
        }

        public override void PreparePipelineResultProvider()
        {
            _queryPipelineExecutor.PipelineResultProviders.MoveNext();
            _pipelineResultProvided = _queryPipelineExecutor.PipelineResultProviders.Current;

            base.PreparePipelineResultProvider();

            return;
        }

        #endregion

        #region IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> implementation

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            foreach (TQueryResultData intermediateResultData in _queryResult)
            {
                (NextMiddleware.ResultProvided, MustGoOn) = 
                    _nextExecutor.ExecuteOverDataInstance(intermediateResultData);
                if (!MustGoOn) break;
            }

            return NextMiddleware.GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipelineQueryExecutor);
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorResultRequester pipelineQueryExecutor)
        {
            foreach (TQueryResultData intermediateResultData in _queryResult)
            {
                (NextMiddleware.ResultProvided, MustGoOn) =
                    _nextExecutor.ExecuteOverDataInstance(intermediateResultData);
                if (_pipelineResultProvided())
                {
                    foreach (TPipelineQueryResultData queryResultData
                     in NextMiddleware.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipelineQueryExecutor))
                    {
                        yield return queryResultData;
                        if (!(MustGoOn &= NextMiddleware.MustGoOn)) yield break;
                        //if (!MustGoOn) yield break;
                    }
                }
                if (!(MustGoOn &= NextMiddleware.MustGoOn)) yield break;
                //if (!MustGoOn) yield break;
            }
        }

        #endregion
    }
}
