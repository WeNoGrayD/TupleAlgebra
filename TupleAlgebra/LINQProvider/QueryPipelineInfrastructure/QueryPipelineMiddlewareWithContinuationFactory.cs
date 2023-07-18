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
    /// Фабрика для производства компонентов конвейера запросов с продолжением.
    /// </summary>
    public class QueryPipelineMiddlewareWithContinuationFactory
    {
        #region Static methods

        /// <summary>
        /// Фабричное создание компонента конвейера запросов с продолжением.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResultData"></typeparam>
        /// <typeparam name="TNextQueryResult"></typeparam>
        /// <param name="continuedExecutor"></param>
        /// <param name="nextExecutor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public virtual IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> Create<TData, TQueryResultData, TNextQueryResult>(
            LinkedListNode<IQueryPipelineMiddleware> node,
            IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> continuedExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
        {
            return continuedExecutor.InnerExecutor switch
            {
                StreamingQueryExecutorWithEnumerableOneToManyResult<TData, TQueryResultData> streaming =>
                    new StreamingQueryPipelineMiddlewareWithContinuationAndOneToManyResult<TData, TQueryResultData, TNextQueryResult>(
                        node,
                        streaming,
                        nextExecutor),
                StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> streaming =>
                    new StreamingQueryPipelineMiddlewareWithContinuationAndOneToOneResult<TData, TQueryResultData, TNextQueryResult>(
                        node,
                        streaming, 
                        nextExecutor),
                /*
                BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData> buffering =>
                    new BufferingQueryPipelineMiddlewareWithContinuation<TData, TQueryResultData, TNextQueryResult>(
                        node,
                        buffering, 
                        nextExecutor),
                */
                _ => throw new ArgumentException("Обёртка в запрос с продолжением " +
                                                $"не поддерживается для следующих типов: {continuedExecutor.GetType().Name}.")
            };
        }

        #endregion
    }
}
