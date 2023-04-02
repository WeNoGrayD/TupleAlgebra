using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    /// <summary>
    /// Декоратор исполнителя запроса для передачи результата запроса на следующий исполнитель запросов в конвейере.
    /// </summary>
    /// <typeparam name="TInnerQueryExecutor"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public abstract class SingleQueryExecutorWithContinuation<TInnerQueryExecutor, TData, TQueryResultData, TNextQueryResult>
        : SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>,
          IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>
        where TInnerQueryExecutor : SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
    {
        #region Instance fields

        /// <summary>
        /// Конкретно типизированный внутренний исполнитель запроса.
        /// </summary>
        protected TInnerQueryExecutor _innerExecutorImpl;

        #endregion

        #region Instance properties

        /// <summary>
        /// Конкретно типизированный внутренний исполнитель запроса.
        /// </summary>
        protected TInnerQueryExecutor InnerExecutorImpl
        {
            get => _innerExecutorImpl;
            private set => _innerExecutorImpl = value;
        }

        /// <summary>
        /// Внутренний сполнитель запроса.
        /// </summary>
        public SingleQueryExecutor<TData, IEnumerable<TQueryResultData>> InnerExecutor => _innerExecutorImpl;

        /// <summary>
        /// Следующий компонент в конвейере.
        /// </summary>
        public IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> NextExecutor { get; protected set; }

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        public virtual bool MustGoOn { get => NextExecutor.MustGoOn; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="nextExecutor"></param>
        /// <exception cref="ArgumentException"></exception>
        protected SingleQueryExecutorWithContinuation(
            TInnerQueryExecutor innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base()
        {
            InnerExecutorImpl = innerExecutor;
            NextExecutor = nextExecutor;
            innerExecutor.DataPassed += OnDataPassed;

            switch (NextExecutor.InnerExecutor)
            {
                case StreamingQueryExecutor<TQueryResultData, TNextQueryResult> streaming:
                    {
                        VisitNextStreamingQueryExecutorOnInit(streaming);
                        break;
                    }
                case BufferingQueryExecutor<TQueryResultData, TNextQueryResult> buffering:
                    {
                        VisitNextBufferingQueryExecutorOnInit(buffering);
                        break;
                    }
                default: throw new ArgumentException();
            }
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// потоковым компонентом.
        /// </summary>
        /// <param name="nextStreaming"></param>
        protected virtual void VisitNextStreamingQueryExecutorOnInit(
            StreamingQueryExecutor<TQueryResultData, TNextQueryResult> nextStreaming)
        {
            return;
        }

        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// буферизирующим компонентов.
        /// </summary>
        /// <param name="nextBuffering"></param>
        protected virtual void VisitNextBufferingQueryExecutorOnInit(
            BufferingQueryExecutor<TQueryResultData, TNextQueryResult> nextBuffering)
        {
            nextBuffering.InitDataSource();

            return;
        }

        /// <summary>
        /// Обработчик события пропуска промежуточных данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="outputData"></param>
        protected abstract void DataPassedHandler(IEnumerable<TQueryResultData> outputData);

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <typeparam name="TContinuingQueryData"></typeparam>
        /// <typeparam name="TContinuingQueryResult"></typeparam>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        public IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>
            ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor)
        {
            NextExecutor = NextExecutor.ContinueWith(continuingExecutor);

            return this;
        }

        #endregion

        #region IQueryPipelineAcceptor implementation

        /// <summary>
        /// Приём конвейера запросов для вызова метода запуска конвейера.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultParam"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="isResultEnumerable"></param>
        /// <param name="queryPipeline"></param>
        /// <returns></returns>
        public override TPipelineQueryResult Accept<TPipelineQueryResultParam, TPipelineQueryResult>(
                bool isResultEnumerable, ISingleQueryExecutorVisitor queryPipeline)
        {
            return InnerExecutor.Accept<TPipelineQueryResultParam, TPipelineQueryResult>(isResultEnumerable, queryPipeline);
        }

        #endregion

        #region IQueryPipelineMiddleware implementation

        /// <summary>
        /// Продолжение компонента конвейера следующим (явная реализация).
        /// </summary>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        IQueryPipelineMiddleware IQueryPipelineMiddleware.ContinueWith(IQueryPipelineMiddleware continuingExecutor)
        {
            return ContinueWith((dynamic)continuingExecutor);
        }

        /// <summary>
        /// Получение конечного компонента конвейера запросов.
        /// </summary>
        /// <returns></returns>
        public IQueryPipelineEndpoint GetPipelineEndpoint()
        {
            return NextExecutor.GetPipelineEndpoint();
        }

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо агрегируемое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public virtual void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            NextExecutor.PrepareToAggregableResult<TPipelineQueryResult>(pipelineQueryExecutor);

            return;
        }

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо перечислимое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public virtual void PrepareToEnumerableResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            NextExecutor.PrepareToEnumerableResult<TPipelineQueryResultData>(pipelineQueryExecutor);

            return;
        }

        /// <summary>
        /// Получение результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public abstract TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline);

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public virtual TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            return NextExecutor.GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipelineQueryExecutor);
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        public virtual IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            return NextExecutor.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipelineQueryExecutor);
        }

        #endregion

        #region IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> implementation

        /// <summary>
        /// Подписка компонента конвейера запросов на событие пропуска промежуточных данных своего исполнителя запросов.
        /// </summary>
        /// <param name="queryResultPassingHandler"></param>
        public virtual void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            //InnerExecutor.DataPassed += queryResultPassingHandler;
            this.DataPassed += queryResultPassingHandler;

            return;
        }

        /// <summary>
        /// Продолжение компонента конвейера следующим (явная реализация).
        /// </summary>
        /// <typeparam name="TContinuingQueryData"></typeparam>
        /// <typeparam name="TContinuingQueryResult"></typeparam>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>>
            .ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor)
        {
            return ContinueWith(continuingExecutor);
        }

        #endregion
    }

    /// <summary>
    /// Декоратор потокового исполнителя запроса для передачи результата запроса "один к одному"
    /// на следующий исполнитель запросов в конвейере.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public class StreamingQueryExecutorWithContinuation<TData, TQueryResultData, TNextQueryResult>
        : SingleQueryExecutorWithContinuation<
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
            TData,
            TQueryResultData,
            TNextQueryResult>,
          IStreamingQueryPipelineMiddleware
    {
        #region Instance fields

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        private bool _mustGoOn = true;

        #endregion

        #region Instance properties

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        public override bool MustGoOn { get => _mustGoOn; }

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
        public StreamingQueryExecutorWithContinuation(
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(innerExecutor, nextExecutor)
        {
            //innerExecutor.DataNotPassed += OnDataNotPassed;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Обработчик события пропуска промежуточных данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="outputData"></param>
        protected override void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            return;
        }

        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// потоковым компонентом.
        /// </summary>
        /// <param name="nextStreaming"></param>
        protected override void VisitNextStreamingQueryExecutorOnInit(
            StreamingQueryExecutor<TQueryResultData, TNextQueryResult> nextStreaming)
        {
            nextStreaming.PrepareToQueryStart();
            SubscribeOninnerExecutorEventsOnDataInstanceProcessing((intermediateResult) =>
            {
                foreach (TQueryResultData intermediateResultData in intermediateResult)
                {
                    _mustGoOn = nextStreaming.ExecuteOverDataInstance(intermediateResultData) && NextExecutor.MustGoOn;
                    //nextStreaming.ExecuteOverDataInstance(intermediateResultData);
                    //_mustGoOn = NextExecutor.MustGoOn;
                    if (!_mustGoOn) break;
                }
            });
        }

        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// буферизирующим компонентов.
        /// </summary>
        /// <param name="nextBuffering"></param>
        /// <exception cref="InvalidOperationException"></exception>
        protected override void VisitNextBufferingQueryExecutorOnInit(
            BufferingQueryExecutor<TQueryResultData, TNextQueryResult> nextBuffering)
        {
            Type nextQueryResultType = typeof(TNextQueryResult);
            string ienumerableName = nameof(IEnumerable);
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
        }

        /// <summary>
        /// Вызов события пропуска данных с готовым промежуточным результатом.
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
        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            base.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            //InnerExecutorImpl.DataNotPassed += queryResultPassingHandler;
            this.DataNotPassed += queryResultPassingHandler;
        }

        /// <summary>
        /// Получение результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline)
        {
            return NextExecutor.GetPipelineQueryResult<TPipelineQueryResult>(pipeline);
        }

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipeline"></param>
        /// <param name="mustGoOn"></param>
        /// <returns></returns>
        public TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline,
            ref bool mustGoOn)
        {
            mustGoOn = _mustGoOn;

            return GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipeline);
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public (IEnumerable<TPipelineQueryResultData> QueryResult, bool MustGoOn)
            GetEnumerablePipelineQueryResult2<TPipelineQueryResultData>(ISingleQueryExecutorVisitor pipeline)
        {
            return (GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipeline), _mustGoOn);
        }

        #endregion
    }

    /// <summary>
    /// Декоратор потокового исполнителя запроса для передачи результата запроса "один ко многим"
    /// на следующий исполнитель запросов в конвейере.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public class StreamingQueryExecutorWithContinuationAndOneToManyResult<TData, TQueryResultData, TNextQueryResult>
    : SingleQueryExecutorWithContinuation<
        StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
        TData,
        TQueryResultData,
        TNextQueryResult>,
      IStreamingQueryPipelineMiddleware
    {
        #region Instance fields

        /// <summary>
        /// Делегат передачи данных на следующий компонент конвейера запросов.
        /// </summary>
        private Action<TQueryResultData> _nextExecuteOverResutDataInstance;

        /// <summary>
        /// Перечисление промежуточных результатов выполнения запроса.
        /// </summary>
        private IEnumerable<TQueryResultData> _intermediateQueryResult;

        /// <summary>
        /// Ссылка на конвейер, который использует данный компонент.
        /// </summary>
        private ISingleQueryExecutorVisitor _pipelineQueryExecutor;

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        private bool _mustGoOn = true;

        #endregion

        #region Instance properties

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        public override bool MustGoOn { get => _mustGoOn; }

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
        public StreamingQueryExecutorWithContinuationAndOneToManyResult(
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(innerExecutor, nextExecutor)
        {
            //innerExecutor.DataNotPassed += OnDataNotPassed;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// потоковым компонентом.
        /// </summary>
        /// <param name="nextStreaming"></param>
        protected override void VisitNextStreamingQueryExecutorOnInit(
            StreamingQueryExecutor<TQueryResultData, TNextQueryResult> nextStreaming)
        {
            _nextExecuteOverResutDataInstance = (outputDataInstance) =>
                _mustGoOn = nextStreaming.ExecuteOverDataInstance(outputDataInstance);

            /*
            InnerExecutor.DataPassed += (intermediateResult) =>
            {
                IEnumerator<TQueryResultData> intermediateResultEnumerator = intermediateResult.GetEnumerator();
                bool isMovingOn;
                nextStreaming.DataPassed += (_) =>
                {
                    isMovingOn = intermediateResultEnumerator.MoveNext();
                    if (isMovingOn) nextStreaming.ExecuteOverDataInstance(intermediateResultEnumerator.Current);
                };
            };
            */
        }

        /// <summary>
        /// Обработка случая, когда следующий компонент конвейера запросов является
        /// буферизирующим компонентов.
        /// </summary>
        /// <param name="nextBuffering"></param>
        protected override void VisitNextBufferingQueryExecutorOnInit(
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

            _nextExecuteOverResutDataInstance = (outputDataInstance) => 
                nextBuffering.ExecuteOverDataInstance(outputDataInstance);
            
            base.VisitNextBufferingQueryExecutorOnInit(nextBuffering);
            InnerExecutor.DataPassed += (intermediateResult) =>
                nextBuffering.ExecuteOverDataInstance(intermediateResult.First());
            */
        }

        /// <summary>
        /// Вызов события пропуска данных с готовым промежуточным результатом.
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
            _intermediateQueryResult = outputData;

            return;
        }

        /// <summary>
        /// Обработчик события пропуска промежуточных данных внутренним исполнителем запроса, если ожидаемый 
        /// результат конвейера запросов - агрегируемый.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="outputData"></param>
        protected void DataPassedWithExpectedAggregableResultHandler<TPipelineQueryResult>(
            IEnumerable<TQueryResultData> outputData)
        {
            _intermediateQueryResult = outputData;
            NextExecutor.GetAggregablePipelineQueryResult<TPipelineQueryResult>(_pipelineQueryExecutor);

            return;
        }

        /// <summary>
        /// Обработчик события пропуска промежуточных данных внутренним исполнителем запроса, если ожидаемый 
        /// результат конвейера запросов - агрегируемый.
        /// </summary>
        /// <param name="outputData"></param>
        protected void DataPassedWithExpectedEnumerableResultHandler(IEnumerable<TQueryResultData> outputData)
        {
            _intermediateQueryResult = outputData;

            return;
        }

        /// <summary>
        /// Обработчик события непропуска промежуточных данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="outputData"></param>
        protected void DataNotPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
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
            this.DataPassed += DataPassedWithExpectedAggregableResultHandler<TPipelineQueryResult>;
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
            this.DataPassed += DataPassedWithExpectedEnumerableResultHandler;
        }

        /// <summary>
        /// Подписка компонента конвейера запросов на событие пропуска промежуточных данных своего исполнителя запросов.
        /// </summary>
        /// <param name="queryResultPassingHandler"></param>
        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            base.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            //InnerExecutorImpl.DataNotPassed += queryResultPassingHandler;
            InnerExecutorImpl.DataNotPassed += queryResultPassingHandler;
        }

        /// <summary>
        /// Предполагается несколько вариантов выходного результата:
        /// 1) одиночный;
        /// 2) множественный с отношением "1 выход на 1 вход";
        /// 3) множественный с отношением "N выходов на 1 вход".
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipeline)
        {
            return default(TPipelineQueryResult);
            /*
            return typeof(TPipelineQueryResult).GetInteface(nameof(IEnumerable)) is null ?
                NextExecutor.GetPipelineQueryResult<TPipelineQueryResult>(pipeline) :
                (dynamic)GetPipelineQueryEnumerableResult(pipeline);
            */
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
                _nextExecuteOverResutDataInstance(intermediateResultData);
                if (!_mustGoOn) break;
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
            //IStreamingQueryPipelineMiddleware nextPipelineMiddleware = NextExecutor as IStreamingQueryPipelineMiddleware;
            IEnumerable<TPipelineQueryResultData> queryResult;
            //bool mustGoOnFromNextLayers;

            foreach (TQueryResultData intermediateResultData in _intermediateQueryResult)
            {
                _nextExecuteOverResutDataInstance(intermediateResultData);
                //(queryResult, mustGoOnFromNextLayers) = 
                //    nextPipelineMiddleware.GetEnumerablePipelineQueryResult2<TPipelineQueryResultData>(pipelineQueryExecutor);
                queryResult = NextExecutor.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipelineQueryExecutor);
                foreach (TPipelineQueryResultData queryResultData in queryResult)
                {
                    _mustGoOn &= NextExecutor.MustGoOn;
                    yield return queryResultData;
                    //if (!(_mustGoOn &= mustGoOnFromNextLayers)) yield break;
                    if (!_mustGoOn) yield break;
                }
            }
        }

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipeline"></param>
        /// <param name="mustGoOn"></param>
        /// <returns></returns>
        public TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline,
            ref bool mustGoOn)
        {
            TPipelineQueryResult queryResult = GetAggregablePipelineQueryResult<TPipelineQueryResult>(pipeline);

            mustGoOn = _mustGoOn;

            return queryResult;
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public (IEnumerable<TPipelineQueryResultData> QueryResult, bool MustGoOn)
            GetEnumerablePipelineQueryResult2<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipeline), _mustGoOn);
        }

        #endregion
    }

    public class BufferingQueryExecutorWithContinuation<TData, TQueryResultData, TNextQueryResult>
        : SingleQueryExecutorWithContinuation<
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData>,
            TData,
            TQueryResultData,
            TNextQueryResult>
    {
        public BufferingQueryExecutorWithContinuation(
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(innerExecutor, nextExecutor)
        { }

        protected override void DataPassedHandler(IEnumerable<TQueryResultData> outputData)
        {
            return;
        }

        public override TPipelineQueryResult GetPipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline)
        {
            // Явное уведомление исполнителя запроса о требовании подытоживания запроса.
            IEnumerable<TQueryResultData> queryResult = InnerExecutorImpl.GetQueryResult();

            // Переход к следующему исполнителю запроса.
            return pipeline.ContinueQueryExecuting<TQueryResultData, TPipelineQueryResult>(NextExecutor, queryResult);
        }
    }
}
