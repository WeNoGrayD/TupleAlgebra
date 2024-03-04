using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered
{
    public interface IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>
        : IOrderedFiniteEnumerableAttributeComponentFactory<
              TData,
              BufferedOrderedFiniteEnumerableAttributeComponent<TData>,
              BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    {
        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory<
                TData,
                BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictional(
                BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
                    args)
        {
            return new BufferedOrderedFiniteEnumerableAttributeComponent<TData>(
                args.Power,
                args.Values,
                args.OrderingComparer,
                args.ValuesAreOrdered,
                args.QueryProvider,
                args.QueryExpression);
        }
    }
}
