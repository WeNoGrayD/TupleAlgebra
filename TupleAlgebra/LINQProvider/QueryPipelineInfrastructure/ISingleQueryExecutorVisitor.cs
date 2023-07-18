using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Посетитель исполнителей одиночных запросов.
    /// </summary>
    public interface ISingleQueryExecutorVisitor
    {
        #region Methods

        /// <summary>
        /// Посещение буферизирующего исполнителя запросов с ожидаемым агрегируемым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        TPipelineQueryResult VisitBufferingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor);

        /// <summary>
        /// Посещение буферизирующего исполнителя запросов с ожидаемым перечислимым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        IEnumerable<TPipelineQueryResultData>
            VisitBufferingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultData>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor);

        /// <summary>
        /// Посещение потокового исполнителя запросов с ожидаемым агрегируемым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        TPipelineQueryResult
            VisitStreamingQueryExecutorWithExpectedAggregableResult<TData, TQueryResult, TPipelineQueryResult>(
            StreamingQueryExecutor<TData, TQueryResult> queryExecutor);

        /// <summary>
        /// Посещение потокового исполнителя запросов с ожидаемым перечислимым результатом.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        IEnumerable<TPipelineQueryResultData>
            VisitStreamingQueryExecutorWithExpectedEnumerableResult<TData, TQueryResult, TPipelineQueryResultData>(
            StreamingQueryExecutor<TData, TQueryResult> queryExecutor);

        #endregion
    }
}
