using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using LINQProvider;
using LINQProvider.QueryPipelineInfrastructure;

namespace TupleAlgebraTests.CustomLinqTests
{
    internal class MockQueryPipelineScheduler : QueryPipelineScheduler
    {
        public MockQueryPipelineScheduler(
            QueryContext queryContext,
            IEnumerable<MethodCallExpression> methodCallChain,
            IEnumerable dataSource)
            : base(queryContext, methodCallChain, dataSource) { }
    }
}
