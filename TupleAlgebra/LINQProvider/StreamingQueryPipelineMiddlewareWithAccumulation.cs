using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQProvider
{
    /// <summary>
    /// Компонент конвейера запросов с аккумулированием результата потокового исполнителя.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public abstract class StreamingQueryPipelineMiddlewareWithAccumulation<TData, TQueryResult>
        : QueryPipelineMiddlewareWithAccumulation<
            StreamingQueryExecutor<TData, TQueryResult>,
            TData,
            TQueryResult,
            TQueryResult>
    {
        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="innerExecutorAsAccumulating"></param>
        public StreamingQueryPipelineMiddlewareWithAccumulation(
            StreamingQueryExecutor<TData, TQueryResult> innerExecutor,
            IAccumulatePositiveQueryResult<TQueryResult, TQueryResult> innerExecutorAsAccumulating)
            : base(innerExecutor, innerExecutorAsAccumulating)
        {
            return;
        }

        #endregion

        #region IQueryPipelineMiddleware<TData, TQueryResult> implementation

        /// <summary>
        /// Подписка компонента конвейера запросов на событие пропуска промежуточных данных своего исполнителя запросов.
        /// </summary>
        /// <param name="queryResultPassingHandler"></param>
        public override void SubscribeOninnerExecutorEventsOnDataInstanceProcessing(
            Action<TQueryResult> queryResultPassingHandler)
        {
            base.SubscribeOninnerExecutorEventsOnDataInstanceProcessing(queryResultPassingHandler);
            _innerExecutorImpl.DataNotPassed += queryResultPassingHandler;

            return;
        }

        #endregion
    }

    /// <summary>
    /// Компонент конвейера запросов с аккумулированием агрегируемого результата потокового исполнителя.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public class StreamingQueryPipelineMiddlewareWithAccumulationOfAggregableResult<TData, TQueryResult>
        : StreamingQueryPipelineMiddlewareWithAccumulation<TData, TQueryResult>
    {
        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        public StreamingQueryPipelineMiddlewareWithAccumulationOfAggregableResult(
            StreamingQueryExecutorWithAggregableResult<TData, TQueryResult> innerExecutor)
            : base(innerExecutor, innerExecutor)
        {
            return;
        }

        #endregion

        #region IQueryPipelineMiddleware implementation

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (this as StreamingQueryPipelineMiddlewareWithAccumulationOfAggregableResult<TData, TPipelineQueryResult>)._accumulator;
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Компонент конвейера запросов с аккумулированием перечислимого результата потокового исполнителя.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TQueryResultData"></typeparam>
    public class StreamingQueryPipelineMiddlewareWithAccumulationOfEnumerableResult<TData, TQueryResultData>
        : StreamingQueryPipelineMiddlewareWithAccumulation<TData, IEnumerable<TQueryResultData>>
    {
        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="innerExecutor"></param>
        /// <param name="innerExecutorAsAccumulating"></param>
        public StreamingQueryPipelineMiddlewareWithAccumulationOfEnumerableResult(
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> innerExecutor,
            IAccumulatePositiveQueryResult<IEnumerable<TQueryResultData>, IEnumerable<TQueryResultData>> innerExecutorAsAccumulating)
            : base(innerExecutor, innerExecutorAsAccumulating)
        {
            return;
        }

        #endregion

        #region IQueryPipelineMiddleware implementation

        /// <summary>
        /// Получение агрегируемого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="pipelineQueryExecutor"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override TPipelineQueryResult GetAggregablePipelineQueryResult<TPipelineQueryResult>(ISingleQueryExecutorVisitor pipelineQueryExecutor)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получение перечислимого результата конвейера запросов.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public override IEnumerable<TPipelineQueryResultData> GetEnumerablePipelineQueryResult<TPipelineQueryResultData>(
            ISingleQueryExecutorVisitor pipeline)
        {
            return (this as StreamingQueryPipelineMiddlewareWithAccumulationOfEnumerableResult<TData, TPipelineQueryResultData>)._accumulator;
        }

        #endregion
    }
}
