using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Reflection;
using LINQProvider;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable
{
    /// <summary>
    /// Провайдер запросов к упорядоченной конечной перечислимой компоненте аттрибута.
    /// </summary>
    public class OrderedFiniteEnumerableAttributeComponentQueryProvider
        : AttributeComponentQueryProvider
    {
        #region Instance methods

        protected override QueryContext CreateQueryContext()
        {
            return new OrderedFiniteEnumerableAttributeComponentQueryContext();
        }

        protected override QueryPipelineExecutor CreateQueryPipelineExecutor(
            IEnumerable dataSource,
            IQueryPipelineMiddleware firstQueryExecutor)
        {
            return new OrderedFiniteEnumerableAttributeComponentQueryPipelineExecutor(dataSource, firstQueryExecutor);
        }

        #endregion
    }
}
