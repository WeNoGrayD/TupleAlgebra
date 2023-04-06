using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    /// <summary>
    /// Фабрика для производства конечных компонентов конвейера запросов.
    /// </summary>
    public static class QueryPipelineMiddlewareWithAccumulationFactory
    {
        /// <summary>
        /// Фабричное создание конечного компонента конвейера запросов.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public static IQueryPipelineMiddleware<TData, TQueryResult> Create<TData, TQueryResult>(
            SingleQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return CreateSpecific((dynamic)queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, TQueryResult> CreateSpecific<TData, TQueryResult>(
            StreamingQueryExecutorWithAggregableResult<TData, TQueryResult> queryExecutor)
        {
            return new StreamingQueryPipelineMiddlewareWithAccumulationOfAggregableResult<TData, TQueryResult>(queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> CreateSpecific<TData, TQueryResultData>(
            StreamingQueryExecutorWithEnumerableResult<TData, TQueryResultData> queryExecutor)
        {
            return new StreamingQueryPipelineMiddlewareWithAccumulationOfEnumerableResult<TData, TQueryResultData>(
                queryExecutor, queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> CreateSpecific<TData, TQueryResultData>(
            StreamingQueryExecutorWithEnumerableOneToManyResult<TData, TQueryResultData> queryExecutor)
        {
            return new StreamingQueryPipelineMiddlewareWithAccumulationOfEnumerableResult<TData, TQueryResultData>(
                queryExecutor, queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, TQueryResult> CreateSpecific<TData, TQueryResult>(
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return new BufferingQueryExecutorWithAccumulation<TData, TQueryResult, TQueryResult>(queryExecutor,
                (queryExecutor as IAccumulatePositiveQueryResult<TQueryResult, TQueryResult>));
        }
    }
}
