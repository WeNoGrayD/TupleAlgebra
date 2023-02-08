using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Linq.Expressions;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure
{
    public class OrderedFiniteEnumerableNonFictionalAttributeComponentFactoryArgs<TValue> : AttributeComponentFactoryArgs<TValue>
    {
        public readonly IEnumerable<TValue> Values;

        public OrderedFiniteEnumerableNonFictionalAttributeComponentFactoryArgs(
            AttributeDomain<TValue> domain,
            IEnumerable<TValue> values,
            NonFictionalAttributeComponentQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(domain, queryProvider, queryExpression)
        {
            Values = values;
        }
    }
}
