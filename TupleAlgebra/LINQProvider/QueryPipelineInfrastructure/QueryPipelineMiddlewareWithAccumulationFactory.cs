using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;
using LINQProvider.QueryResultAccumulatorInfrastructure;

namespace LINQProvider.QueryPipelineInfrastructure
{
    /// <summary>
    /// Фабрика для производства конечных компонентов конвейера запросов.
    /// </summary>
    public class QueryPipelineMiddlewareWithAccumulationFactory
    {
        /// <summary>
        /// Фабричное создание конечного компонента конвейера запросов.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <param name="queryExecutor"></param>
        /// <returns></returns>
        public virtual IQueryPipelineEndpoint Create<TData, TQueryResult>(
            SingleQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return CreateSpecific((dynamic)queryExecutor);
        }

        public virtual IQueryPipelineEndpoint Create(object queryExecutor)
        {
            return CreateSpecific((dynamic)queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, TQueryResult> CreateSpecific<TData, TQueryResult>(
            StreamingQueryExecutorWithAggregableResult<TData, TQueryResult> queryExecutor)
        {
            // Создаётся новый узел компонента конвейера.
            LinkedListNode<IQueryPipelineMiddleware> newNode = new LinkedListNode<IQueryPipelineMiddleware>(null!);

            return new StreamingQueryPipelineMiddlewareWithAccumulation<TData, TQueryResult>(
                newNode, 
                queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> CreateSpecific<TData, TQueryResultData>(
            StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> queryExecutor)
        {
            // Создаётся новый узел компонента конвейера.
            LinkedListNode<IQueryPipelineMiddleware> newNode = new LinkedListNode<IQueryPipelineMiddleware>(null!);

            return new StreamingQueryPipelineMiddlewareWithEnumerableResultAccumulation<TData, TQueryResultData>(
                newNode,
                queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, TQueryResult> CreateSpecific<TData, TQueryResult>(
            BufferingQueryExecutorWithAggregableResult<TData, TQueryResult> queryExecutor)
        {
            return new BufferingQueryPipelineMiddlewareWithAccumulation<TData, TQueryResult>(
                queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> CreateSpecific<TData, TQueryResultData>(
            BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData> queryExecutor)
        {
            return new BufferingQueryPipelineMiddlewareWithAccumulationOfEnumerableResult<TData, TQueryResultData>(
                queryExecutor);
        }
    }
}
