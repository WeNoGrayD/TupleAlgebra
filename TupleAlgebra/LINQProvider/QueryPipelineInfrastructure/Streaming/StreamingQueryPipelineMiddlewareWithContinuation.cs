using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider.QueryPipelineInfrastructure.Buffering;

namespace LINQProvider.QueryPipelineInfrastructure.Streaming
{
    /// <summary>
    /// Компонент конвейера запросов с аккумулированием результата вида "один к одному" потокового исполнителя.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public class StreamingQueryPipelineMiddlewareWithContinuationAndOneToOneResult<TData, TQueryResultData>
        : QueryPipelineMiddlewareWithContinuation<
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
            TData,
            TQueryResultData>
    {
        #region Instance events

        /// <summary>
        /// Событие данных, которые на следующий компонент конвейера запросов не должны быть пропущены.
        /// </summary>
        private event Action<IEnumerable<TQueryResultData>>? DataNotPassed;

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="nextExecutor"></param>
        public StreamingQueryPipelineMiddlewareWithContinuationAndOneToOneResult(
            LinkedListNode<IQueryPipelineMiddleware> node,
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IQueryPipelineMiddlewareWithContinuationAcceptor<TQueryResultData> nextExecutor)
            : base(node, innerExecutor, nextExecutor)
        {
            innerExecutor.DataNotPassed += OnDataNotPassed;

            return;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Вызов события непропуска данных с готовым промежуточным результатом.
        /// </summary>
        /// <param name="outputData"></param>
        protected void OnDataNotPassed(IEnumerable<TQueryResultData> outputData)
        {
            DataNotPassed?.Invoke(outputData);
        }

        #endregion

        #region IQueryPipelineModdleware<TData, TQueryResult> methods implementation

        /// <summary>
        /// Подписка компонента конвейера запросов на событие пропуска промежуточных данных своего исполнителя запросов.
        /// </summary>
        /// <param name="queryResultPassingHandler"></param>
        public override void SubscribeOnInnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            base.SubscribeOnInnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            //DataNotPassed += queryResultPassingHandler;
        }

        #endregion

        #region IQueryPipelineMiddlewareWithContinuationAcceptor<TQueryResultData>

        public override void Accept(IQueryPipelineMiddlewareVisitor<TData> visitor)
        {
            visitor.VisitStreamingQueryExecutor(_innerExecutorImpl);

            return;
        }

        #endregion

        #region IQueryPipelineMiddlewareVisitor<TData> implemention

        public override void VisitStreamingQueryExecutor<TNextQueryResult>(
            StreamingQueryExecutor<TQueryResultData, TNextQueryResult> nextStreaming)
        {
            nextStreaming.PrepareToQueryStart();
            SubscribeOnInnerExecutorEventsOnDataInstanceProcessing((intermediateResult) =>
            {
                (NextExecutor.ResultProvided, MustGoOn) = nextStreaming.ExecuteOverDataInstance(intermediateResult.First());
                MustGoOn &= NextExecutor.MustGoOn;
            });
        }

        public override void VisitBufferingQueryExecutor<TNextQueryResult>(
            BufferingQueryExecutor<TQueryResultData, TNextQueryResult> nextBuffering)
        {
            /*
            Type nextQueryResultType = typeof(TNextQueryResult);
            string ienumerableName = nameof(System.Collections.IEnumerable);
            if (nextQueryResultType.Name.StartsWith(ienumerableName) ||
                nextQueryResultType.GetInterface(ienumerableName) is not null)
            {
                throw new InvalidOperationException("Потоковой исполнитель запроса не поддерживает продолжение " +
                                                    "буферизированным исполнителем запроса с ожидаемым " +
                                                    "перечислимым результатом.");
            }

            base.VisitNextBufferingQueryExecutorOnInit(nextBuffering);
            InnerExecutor.DataPassed += (intermediateResult) =>
                nextBuffering.ExecuteOverDataInstance(intermediateResult.First());
            */
        }

        #endregion
    }

    /// <summary>
    /// Компонент конвейера запросов с аккумулированием результата вида "один ко многим" потокового исполнителя.
    /// на следующий исполнитель запросов в конвейере.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public class StreamingQueryPipelineMiddlewareWithContinuationAndOneToManyResult<TData, TQueryResultData>
    : QueryPipelineMiddlewareWithContinuation<
        StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
        TData,
        TQueryResultData>
    {
        #region Instance fields

        IQueryPipelineEndpoint _queryPipelineEndpoint;

        /// <summary>
        /// Делегат передачи данных на следующий компонент конвейера запросов.
        /// </summary>
        private Action<TQueryResultData> _nextExecuteOverResultDataInstance;

        /// <summary>
        /// Перечисление промежуточных результатов выполнения запроса.
        /// </summary>
        private IEnumerable<TQueryResultData> _intermediateQueryResult;

        /// <summary>
        /// Ссылка на конвейер, который использует данный компонент.
        /// </summary>
        private ISingleQueryExecutorVisitor _pipelineQueryExecutor;

        protected bool _resultsProvided;

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

        #endregion

        #region Instance events

        /// <summary>
        /// Событие данных, которые на следующий компонент конвейера запросов не должны быть пропущены.
        /// </summary>
        private event Action<IEnumerable<TQueryResultData>> DataNotPassed;

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
            IQueryPipelineMiddlewareWithContinuationAcceptor<TQueryResultData> nextExecutor)
            : base(node, innerExecutor, nextExecutor)
        {
            innerExecutor.DataNotPassed += OnDataNotPassed;

            return;
        }

        #endregion

        #region Instance methods

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
        protected override void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            base.DataPassedHandler(outputData);
            _intermediateQueryResult = outputData;

            return;
        }

        #endregion

        #region IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> implementation

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо агрегируемое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public override void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            base.PrepareToAggregableResult<TPipelineQueryResult>(pipelineQueryExecutor);
            _pipelineQueryExecutor = pipelineQueryExecutor;
            //DataPassed += DataPassedWithExpectedAggregableResultHandler<TPipelineQueryResult>;
        }

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо перечислимое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public override void PrepareToEnumerableResult<TPipelineQueryResultData>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            base.PrepareToEnumerableResult<TPipelineQueryResultData>(pipelineQueryExecutor);
            _pipelineQueryExecutor = pipelineQueryExecutor;
            //DataPassed += DataPassedWithExpectedEnumerableResultHandler;
        }

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            foreach (TQueryResultData intermediateResultData in _intermediateQueryResult)
            {
                _nextExecuteOverResultDataInstance(intermediateResultData);
                if (!MustGoOn) break;
            }

            return NextExecutor.GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipelineQueryExecutor);
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            foreach (TQueryResultData intermediateResultData in _intermediateQueryResult)
            {
                _nextExecuteOverResultDataInstance(intermediateResultData);
                if (_queryPipelineEndpoint.ResultProvided)
                {
                    foreach (TPipelineQueryResultData queryResultData
                         in NextExecutor.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipelineQueryExecutor))
                    {
                        yield return queryResultData;
                        if (!(MustGoOn &= NextExecutor.MustGoOn)) yield break;
                    }
                }
                if (!(MustGoOn &= NextExecutor.MustGoOn)) yield break;
            }
        }

        #endregion

        #region IQueryPipelineMiddlewareWithContinuationAcceptor<TQueryResultData>

        public override void Accept(IQueryPipelineMiddlewareVisitor<TData> visitor)
        {
            visitor.VisitStreamingQueryExecutor(_innerExecutorImpl);

            return;
        }

        #endregion

        #region IQueryPipelineMiddlewareVisitor<TData> implemention

        public override void VisitStreamingQueryExecutor<TNextQueryResult>(
            StreamingQueryExecutor<TQueryResultData, TNextQueryResult> nextStreaming)
        {
            _nextExecuteOverResultDataInstance = (outputDataInstance) =>
                (NextExecutor.ResultProvided, MustGoOn) = nextStreaming.ExecuteOverDataInstance(outputDataInstance);

            return;
        }

        public override void VisitBufferingQueryExecutor<TNextQueryResult>(
            BufferingQueryExecutor<TQueryResultData, TNextQueryResult> nextBuffering)
        {
            /*
            Type nextQueryResultType = typeof(TNextQueryResult);
            string ienumerableName = nameof(IEnumerable);
            if (nextQueryResultType.Name.StartsWith(ienumerableName) ||
                nextQueryResultType.GetInterface(ienumerableName) is not null)
            {
                throw new InvalidOperationException("Потоковой исполнитель запроса не поддерживает продолжение " +
                                                    "буферизированным исполнителем запроса с ожидаемым " +
                                                    "перечислимым результатом.");
            }

            _nextExecuteOverResultDataInstance = (outputDataInstance) => 
                nextBuffering.ExecuteOverDataInstance(outputDataInstance);
            
            base.VisitNextBufferingQueryExecutorOnInit(nextBuffering);
            InnerExecutor.DataPassed += (intermediateResult) =>
                nextBuffering.ExecuteOverDataInstance(intermediateResult.First());
            */
        }

        #endregion
    }
}
