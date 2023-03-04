using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable
{
    public class OrderedFiniteEnumerableAttributeComponentFactory<TData>
        : AttributeComponentFactory<TData>,
          INonFictionalAttributeComponentFactory<TData, OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    {
        public NonFictionalAttributeComponent<TData> CreateSpecificNonFictional(OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData> args)
        {
            return new OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>(
                args.Domain, 
                args.Values, 
                args.QueryProvider, 
                args.QueryExpression);
        }
    }
}
