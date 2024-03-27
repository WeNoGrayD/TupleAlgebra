using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraFrameworkTests.CustomLinqTests
{
    internal class MockQueryProvider<T> : QueryProvider
    {
        #region Instance methods

        protected override QueryContext CreateQueryContext()
        {
            return new MockQueryContext();
        }

        protected override IQueryable<TQueryResult> CreateQueryImpl<TQueryResult>(
            Expression queryExpression,
            IQueryable dataSource)
        {
            return new GenericMockQueryable<TQueryResult>(queryExpression, this);
        }

        protected override QueryPipelineScheduler CreateQueryPipelineExecutor(
            QueryContext queryContext,
            IEnumerable<MethodCallExpression> methodCallChain,
            IEnumerable dataSource)
            => new MockQueryPipelineScheduler(queryContext, methodCallChain, dataSource);

        #endregion
    }
}
