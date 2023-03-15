using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    /// <summary>
    /// Фабрика для производства исполнителей запросов с продолжением.
    /// </summary>
    public static class SingleQueryExecutorWithContinuationFactory
    {
        public static SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
            Create<TData, TQueryResultData, TNextQueryResult>(
            IQueryPipelineMiddleware<TData, IEnumerable<TQueryResultData>> continuedExecutor,
            IQueryPipelineMiddleware<TQueryResultData, TNextQueryResult> nextExecutor)
        {
            return continuedExecutor.InnerExecutor switch
            {
                StreamingQueryExecutorWithEnumerableOneToManyResult<TData, TQueryResultData> streaming =>         
                    new StreamingQueryExecutorWithContinuationAndOneToManyResult<TData, TQueryResultData, TNextQueryResult>(streaming, nextExecutor),
                StreamingQueryExecutor<TData, IEnumerable<TQueryResultData>> streaming =>
                    new StreamingQueryExecutorWithContinuation<TData, TQueryResultData, TNextQueryResult>(streaming, nextExecutor),
                BufferingQueryExecutorWithEnumerableResult<TData, TQueryResultData> buffering =>
                    new BufferingQueryExecutorWithContinuation<TData, TQueryResultData, TNextQueryResult>(buffering, nextExecutor),
                _ => throw new ArgumentException("Обёртка в запрос с продолжением " +
                                                $"не поддерживается для следующих типов: {continuedExecutor.GetType().Name}.")
            };
        }
    }
}
