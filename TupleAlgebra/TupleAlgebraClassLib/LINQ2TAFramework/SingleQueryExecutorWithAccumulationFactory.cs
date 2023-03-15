using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public static class SingleQueryExecutorWithAccumulationFactory
    {
        public static SingleQueryExecutor<TData, TQueryResult>
            Create<TData, TQueryResult>(SingleQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return CreateSpecific((dynamic)queryExecutor);
        }

        private static SingleQueryExecutor<TData, TQueryResult>
            CreateSpecific<TData, TQueryResult>(
                StreamingQueryExecutorWithAggregableResult<TData, TQueryResult> queryExecutor)
        {
            return new StreamingQueryExecutorWithAccumulationOfAggregableResult<TData, TQueryResult>(queryExecutor);
        }

        private static SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
            CreateSpecific<TData, TQueryResultData>(
                StreamingQueryExecutorWithEnumerableResult<TData, TQueryResultData> queryExecutor)
        {
            return new StreamingQueryExecutorWithAccumulationOfEnumerableResult<TData, TQueryResultData>(
                queryExecutor, queryExecutor);
        }

        private static SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
            CreateSpecific<TData, TQueryResultData>(
                StreamingQueryExecutorWithEnumerableOneToManyResult<TData, TQueryResultData> queryExecutor)
        {
            return new StreamingQueryExecutorWithAccumulationOfEnumerableResult<TData, TQueryResultData>(
                queryExecutor, queryExecutor);
        }

        private static SingleQueryExecutor<TData, TQueryResult>
            CreateSpecific<TData, TQueryResult>(
                BufferingQueryExecutor<TData, TQueryResult> queryExecutor)
        {
            return new BufferingQueryExecutorWithAccumulation<TData, TQueryResult, TQueryResult>(queryExecutor,
                (queryExecutor as IAccumulatePositiveQueryResult<TQueryResult, TQueryResult>));
        }
    }
}
