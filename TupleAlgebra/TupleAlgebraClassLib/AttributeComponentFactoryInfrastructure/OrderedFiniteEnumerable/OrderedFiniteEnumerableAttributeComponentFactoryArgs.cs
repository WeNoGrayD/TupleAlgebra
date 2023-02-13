using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Linq.Expressions;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable
{
    public class OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData> : AttributeComponentFactoryArgs<TData>
    {
        public IEnumerable<TData> Values { get; private set; }

        public OrderedFiniteEnumerableAttributeComponentFactoryArgs(
            AttributeDomain<TData> domain,
            IEnumerable<TData> values = null,
            OrderedFiniteEnumerableAttributeComponentQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(domain, queryProvider, queryExpression)
        {
            Values = values;
        }
    }
}
