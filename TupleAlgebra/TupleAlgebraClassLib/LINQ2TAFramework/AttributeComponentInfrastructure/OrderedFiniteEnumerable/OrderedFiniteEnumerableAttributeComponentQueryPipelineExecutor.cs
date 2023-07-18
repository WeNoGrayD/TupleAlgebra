using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using LINQProvider.QueryPipelineInfrastructure;
using System.Linq.Expressions;
using LINQProvider;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable
{
    /// <summary>
    /// Провайдер запросов к упорядоченной конечной перечислимой компонентой аттрибута.
    /// </summary>
    public class OrderedFiniteEnumerableAttributeComponentQueryPipelineScheduler
        : QueryPipelineScheduler
    {
        #region Contstructors

        public OrderedFiniteEnumerableAttributeComponentQueryPipelineScheduler(
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
