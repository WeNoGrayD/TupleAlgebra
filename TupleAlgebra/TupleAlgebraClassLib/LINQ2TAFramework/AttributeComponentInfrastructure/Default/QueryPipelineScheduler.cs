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
    public class DefaultAttributeComponentQueryPipelineScheduler
        : QueryPipelineScheduler
    {
        #region Constructors

        public DefaultAttributeComponentQueryPipelineScheduler(
            QueryContext queryContext,
            IEnumerable<MethodCallExpression> methodCallChain,
            IEnumerable dataSource)
            : base(queryContext, methodCallChain, dataSource)
        {
            return;
        }

        #endregion
    }
}
