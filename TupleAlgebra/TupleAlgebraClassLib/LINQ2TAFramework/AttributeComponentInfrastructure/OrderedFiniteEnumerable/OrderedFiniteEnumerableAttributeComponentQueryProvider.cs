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

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable
{
    /// <summary>
    /// Провайдер запросов к упорядоченной конечной перечислимой компоненте аттрибута.
    /// </summary>
    public class OrderedFiniteEnumerableAttributeComponentQueryProvider
        : AttributeComponentQueryProvider
    {
        #region Instance fields

        private QueryContext _queryContext = new OrderedFiniteEnumerableAttributeComponentQueryContext();

        #endregion

        protected override QueryContext QueryContext { get => _queryContext; }

        /*
        public override IQueryable<TData> CreateQuery<TData>(Expression queryExpression)
        {
            return null;
        }
        */

        protected override QueryPipelineExecutor CreateQueryPipelineExecutor(
            object dataSource,
            IQueryPipelineMiddleware firstQueryExecutor) =>
            new OrderedFiniteEnumerableAttributeComponentQueryPipelineExecutor(dataSource, firstQueryExecutor);
    }
}
