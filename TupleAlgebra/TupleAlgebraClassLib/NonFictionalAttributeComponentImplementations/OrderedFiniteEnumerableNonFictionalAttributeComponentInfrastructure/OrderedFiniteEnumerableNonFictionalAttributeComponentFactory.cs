using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure
{
    public class OrderedFiniteEnumerableNonFictionalAttributeComponentFactory<TValue>
        : AttributeComponentFactory<TValue>,
          INonFictionalAttributeComponentFactory<TValue, OrderedFiniteEnumerableNonFictionalAttributeComponentFactoryArgs<TValue>>
    {
        public NonFictionalAttributeComponent<TValue> CreateSpecificNonFictional(OrderedFiniteEnumerableNonFictionalAttributeComponentFactoryArgs<TValue> args)
        {
            return new OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>(
                args.Domain, 
                args.Values, 
                args.QueryProvider, 
                args.QueryExpression);
        }
    }
}
