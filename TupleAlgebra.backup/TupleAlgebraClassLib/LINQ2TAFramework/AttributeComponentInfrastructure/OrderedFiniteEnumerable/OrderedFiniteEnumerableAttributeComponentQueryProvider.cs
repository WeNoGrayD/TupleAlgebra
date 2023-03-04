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
        #region Static fields

        private static AttributeComponentQueryContext _queryContext;

        #endregion

        protected override AttributeComponentQueryContext QueryContext { get => _queryContext; }

        static OrderedFiniteEnumerableAttributeComponentQueryProvider()
        {
            _queryContext = new AttributeComponentQueryContext();
        }
    }
}
