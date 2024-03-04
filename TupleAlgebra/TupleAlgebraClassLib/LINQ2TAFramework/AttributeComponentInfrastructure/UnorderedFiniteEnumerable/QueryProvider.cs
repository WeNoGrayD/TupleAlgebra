using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Reflection;
using LINQProvider;
using LINQProvider.QueryPipelineInfrastructure;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.UnorderedFiniteEnumerable
{
    /// <summary>
    /// Провайдер запросов к упорядоченной конечной перечислимой компоненте атрибута.
    /// </summary>
    public class UnorderedFiniteEnumerableAttributeComponentQueryProvider
        : AttributeComponentQueryProvider
    {
        #region Instance methods

        protected override QueryContext CreateQueryContext()
        {
            return new UnorderedFiniteEnumerableAttributeComponentQueryContext();
        }

        protected override QueryPipelineScheduler CreateQueryPipelineExecutor(
            QueryContext queryContext,
            IEnumerable<MethodCallExpression> methodCallChain,
            IEnumerable dataSource)
        {
            return new UnorderedFiniteEnumerableAttributeComponentQueryPipelineScheduler(queryContext, methodCallChain, dataSource);
        }

        #endregion
    }
}
