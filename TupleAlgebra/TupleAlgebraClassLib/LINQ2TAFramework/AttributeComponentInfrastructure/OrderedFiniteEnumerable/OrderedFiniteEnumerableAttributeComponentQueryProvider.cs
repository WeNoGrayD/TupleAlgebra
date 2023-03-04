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
        : QueryProvider
    {
        #region Static fields

        private static QueryContext _queryContext;

        #endregion

        protected override QueryContext QueryContext { get => _queryContext; }

        static OrderedFiniteEnumerableAttributeComponentQueryProvider()
        {
            _queryContext = null;// new QueryContext();
        }

        public override IQueryable<TData> CreateQuery<TData>(Expression queryExpression)
        {
            return null;
        }

        protected override QueryPipelineExecutor2 CreateQueryPipelineExecutor(object dataSource)
        {
            throw new NotImplementedException();
        }
    }
}
