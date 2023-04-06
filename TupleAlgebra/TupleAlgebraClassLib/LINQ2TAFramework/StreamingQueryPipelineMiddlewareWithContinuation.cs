using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{

    /// <summary>
    /// Компонент конвейера запросов с аккумулированием результата вида "один к одному" потокового исполнителя.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    /// <typeparam name="TNextQueryResult"></typeparam>
    public class StreamingQueryPipelineMiddlewareWithContinuationAndOneToOneResult<TData, TQueryResultData, TNextQueryResult>
        : QueryPipelineMiddlewareWithContinuation<
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
            TData,
            TQueryResultData,
            TNextQueryResult>
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
        public StreamingQueryPipelineMiddlewareWithContinuationAndOneToOneResult(
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
            : base(innerExecutor, nextExecutor)
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
        }

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
        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<IEnumerable<TQueryResultData>> queryResultPassingHandler)
        {
            base.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            this.DataNotPassed += queryResultPassingHandler;
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
    public class StreamingQueryPipelineMiddlewareWithContinuationAndOneToManyResult<TData, TQueryResultData, TNextQueryResult>
    : QueryPipelineMiddlewareWithContinuation<
        StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>>,
        TData,
        TQueryResultData,
        TNextQueryResult>
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
        public StreamingQueryPipelineMiddlewareWithContinuationAndOneToManyResult(
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
        /// результат конвейера запросов - перечислимый.
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
            this.DataNotPassed += queryResultPassingHandler;
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
            foreach (TQueryResultData intermediateResultData in _intermediateQueryResult)
            {
                _nextExecuteOverResutDataInstance(intermediateResultData);
                foreach (TPipelineQueryResultData queryResultData
                         in NextExecutor.GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(pipelineQueryExecutor))
                {
                    _mustGoOn &= NextExecutor.MustGoOn;
                    yield return queryResultData;
                    if (!_mustGoOn) yield break;
                }
            }
        }

        #endregion
    }
}
