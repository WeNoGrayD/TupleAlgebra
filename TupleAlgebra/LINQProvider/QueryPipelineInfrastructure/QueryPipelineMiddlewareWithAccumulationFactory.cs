using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider.QueryPipelineInfrastructure.Buffering;
using LINQProvider.QueryPipelineInfrastructure.Streaming;
using LINQProvider.QueryResultAccumulatorInterfaces;

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
        public virtual IQueryPipelineMiddleware Create(
            LinkedListNode<IQueryPipelineMiddleware> node, 
            IQueryPipelineAcceptor queryExecutor)
        {
            return CreateSpecific(node, (dynamic)queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, TQueryResult> CreateSpecific<TData, TQueryResult>(
            LinkedListNode<IQueryPipelineMiddleware> node,
            StreamingQueryExecutorWithAggregableResult<TData, TQueryResult> queryExecutor)
        {
            return new StreamingQueryPipelineMiddlewareWithAccumulationOfAggregableResult<TData, TQueryResult>(
                node, 
                queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> CreateSpecific<TData, TQueryResultData>(
            LinkedListNode<IQueryPipelineMiddleware> node,
            StreamingQueryExecutorWithEnumerableOneToOneResult<TData, TQueryResultData> queryExecutor)
        {
            return new StreamingQueryPipelineMiddlewareWithAccumulationOfEnumerableResult<TData, TQueryResultData>(
                node,
                queryExecutor, 
                queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> CreateSpecific<TData, TQueryResultData>(
            LinkedListNode<IQueryPipelineMiddleware> node,
            StreamingQueryExecutorWithEnumerableOneToManyResult<TData, TQueryResultData> queryExecutor)
        {
            return new StreamingQueryPipelineMiddlewareWithAccumulationOfEnumerableResult<TData, TQueryResultData>(
                node,
                queryExecutor, 
                queryExecutor);
        }

        private static IQueryPipelineMiddleware<TData, TQueryResult> CreateSpecific<TData, TQueryResult>(
            LinkedListNode<IQueryPipelineMiddleware> node,
            BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return new BufferingQueryPipelineMiddlewareWithAccumulation<TData, TQueryResult, TQueryResult>(
                node,
                queryExecutor,
                queryExecutor as IAccumulatePositiveQueryResult<TQueryResult, TQueryResult>);
        }
    }
}
