using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider
{
    /// <summary>
    /// Компонент конвейера запросов с аккумулированием результатов запроса. 
    /// </summary>
    /// <typeparam name="TInnerQueryExecutor"></typeparam>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    /// <typeparam name="TAccumulator"></typeparam>
    public abstract class QueryPipelineMiddlewareWithAccumulation<TInnerQueryExecutor, TData, TQueryResult, TAccumulator>
        : QueryPipelineMiddleware<TInnerQueryExecutor, TData, TQueryResult>, IQueryPipelineEndpoint
        where TInnerQueryExecutor : SingleQueryExecutor<TData, TQueryResult>
        where TAccumulator : TQueryResult
    {
        #region Instance fields

        /// <summary>
        /// 
        /// </summary>
        private IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> _innerExecutorAsAccumulating;

        /// <summary>
        /// Аккумулятор результата конвейера запросов.
        /// </summary>
        protected TAccumulator _accumulator;

        #endregion

        #region Instance properties

        /// <summary>
        /// Флаг продолжения обхода по входным данным.
        /// </summary>
        public override sealed bool MustGoOn { get => true; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="innerExecutorAsAccumulating"></param>
        protected QueryPipelineMiddlewareWithAccumulation(
            TInnerQueryExecutor innerExecutor,
            IAccumulatePositiveQueryResult<TQueryResult, TAccumulator> innerExecutorAsAccumulating)
            : base(innerExecutor)
        {
            _innerExecutorAsAccumulating = innerExecutorAsAccumulating;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Обработчик пропуска данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="outputData"></param>
        protected virtual void DataPassedHandler(TQueryResult outputData)
        {
            _innerExecutorAsAccumulating.AccumulateIfDataPassed(ref _accumulator, outputData);

            return;
        }

        /// <summary>
        /// Обработчик непропуска данных внутренним исполнителем запроса.
        /// </summary>
        /// <param name="fullCoveringReader"></param>
        /// <param name="outputData"></param>
        protected virtual void DataNotPassedHandler(
            IAccumulateNegativeQueryResult<TQueryResult, TAccumulator> fullCoveringReader,
            TQueryResult outputData)
        {
            fullCoveringReader.AccumulateIfDataNotPassed(ref _accumulator, outputData);

            return;
        }

        /// <summary>
        /// Продолжение компонента конвейера следующим.
        /// </summary>
        /// <typeparam name="TContinuingQueryData"></typeparam>
        /// <typeparam name="TContinuingQueryResult"></typeparam>
        /// <param name="continuingExecutor"></param>
        /// <returns></returns>
        public override IQueryPipelineMiddleware<TData, TQueryResult>
            ContinueWith<TContinuingQueryData, TContinuingQueryResult>(
                IQueryPipelineMiddleware<TContinuingQueryData, TContinuingQueryResult> continuingExecutor)
        {
            return QueryPipelineMiddlewareWithContinuationFactory.Create(
                    this as IQueryPipelineMiddleware<TData, IEnumerable<TContinuingQueryData>>, 
                    continuingExecutor)
                as IQueryPipelineMiddleware<TData, TQueryResult>;
        }

        #endregion

        #region IQueryPipelineMiddleware implementation

        /// <summary>
        /// Получение конечного компонента конвейера запросов.
        /// </summary>
        /// <returns></returns>
        public override IQueryPipelineEndpoint GetPipelineEndpoint()
        {
            return this;
        }

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо агрегируемое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public override void PrepareToAggregableResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor) 
        { }

        /// <summary>
        /// Подготовка компонента конвейера к тому, что итоговым результатом конвейера запросов
        /// будет какое-либо перечислимое значение. 
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        public override void PrepareToEnumerableResult<TPipelineQueryResultData>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        { }

        /// <summary>
        /// Подписка компонента конвейера запросов на событие пропуска промежуточных данных своего исполнителя запросов.
        /// </summary>
        /// <param name="queryResultPassingHandler"></param>
        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            _innerExecutorImpl.DataPassed += queryResultPassingHandler;

            return;
        }

        #endregion

        #region IQueryPipelineEndpoint implementation

        /// <summary>
        /// Инициализация компонента в качестве конечного компонента конвейера запросов.
        /// </summary>
        public void InitializeAsQueryPipelineEndpoint()
        {
            _accumulator = _innerExecutorAsAccumulating.InitAccumulator();
            _innerExecutorImpl.DataPassed += DataPassedHandler;
            if (_innerExecutorImpl is IAccumulateNegativeQueryResult<TQueryResult, TAccumulator> fullCoveringReader)
                fullCoveringReader.DataNotPassed +=
                    (TQueryResult outputData) => DataNotPassedHandler(fullCoveringReader, outputData);
        }

        #endregion
    }
}
