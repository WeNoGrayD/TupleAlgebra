using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable
{
    public class OrderedFiniteEnumerableAttributeComponentFactory
        : AttributeComponentFactory,
          INonFictionalAttributeComponentFactory<OrderedFiniteEnumerableAttributeComponentFactoryArgs>
    {
        public NonFictionalAttributeComponent<TData> CreateSpecificNonFictional<TData>(
            OrderedFiniteEnumerableAttributeComponentFactoryArgs args)
        {
            return new OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>(
                args.Values as IEnumerable<TData>,
                args.OrderingComparer as IComparer<TData>,
                args.QueryProvider, 
                args.QueryExpression);
        }
    }
}
