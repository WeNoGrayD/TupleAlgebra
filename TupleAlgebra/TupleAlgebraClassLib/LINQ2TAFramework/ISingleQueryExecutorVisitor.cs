using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    /// <summary>
    /// Посетитель исполнителей одиночных запросов.
    /// </summary>
    public interface ISingleQueryExecutorVisitor
    {
        #region Methods

        /// <summary>
        /// Обобщённая установка источника данных.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="dataSource"></param>
        void SetDataSource<TData>(IEnumerable<TData> dataSource);

        /// <summary>
        /// Выполнение запроса с ожидаемым агрегируемым результатом.
        /// </summary>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <returns></returns>
        TPipelineQueryResult ExecuteWithExpectedAggregableResult<TPipelineQueryResult>();

        /// <summary>
        /// Выполнение запроса с ожидаемым перечислимым результатом.
        /// </summary>
        /// <typeparam name="TPipelineQueryResultData"></typeparam>
        /// <returns></returns>
        IEnumerable<TPipelineQueryResultData> ExecuteWithExpectedEnumerableResult<TPipelineQueryResultData>();

        /// <summary>
        /// Продолжение выполнение конвейера запросов со следующего компонента, как будто он первый.
        /// </summary>
        /// <typeparam name="TQueryResultData"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="nextMiddleware"></param>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        TPipelineQueryResult ContinueQueryExecuting<TQueryResultData, TPipelineQueryResult>(
            IQueryPipelineMiddleware nextMiddleware,
            IEnumerable<TQueryResultData> dataSource);

        /// <summary>
        /// Посещение потокового исполнителя запросов.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultParam"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="iResultEnumerable"></param>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        TPipelineQueryResult VisitStreamingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
            bool iResultEnumerable, StreamingQueryExecutor<TData, TQueryResult> queryExecutor);

        /// <summary>
        /// Посещение буферизирующего исполнителя запросов.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <typeparam name="TPipelineQueryResultParam"></typeparam>
        /// <typeparam name="TPipelineQueryResult"></typeparam>
        /// <param name="iResultEnumerable"></param>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        TPipelineQueryResult VisitBufferingQueryExecutor<TData, TQueryResult, TPipelineQueryResultParam, TPipelineQueryResult>(
            bool iResultEnumerable, BufferingQueryExecutor<TData, TQueryResult> queryExecutor);

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
