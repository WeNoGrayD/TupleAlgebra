using LINQProvider.QueryPipelineInfrastructure;
using LINQProvider;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default
{
    public class DefaultAttributeComponentQueryProvider
        : AttributeComponentQueryProvider
    {
        #region Instance methods

        protected override QueryContext CreateQueryContext()
        {
            return new DefaultAttributeComponentQueryContext();
        }

        /*
        protected override QueryPipelineScheduler CreateQueryPipelineExecutor(
            QueryContext queryContext,
            IEnumerable<MethodCallExpression> methodCallChain,
            IEnumerable dataSource)
        {
            return new DefaultAttributeComponentQueryPipelineScheduler(queryContext, methodCallChain, dataSource);
        }
        */

        #endregion
    }
}
