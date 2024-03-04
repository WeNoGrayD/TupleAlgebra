using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable
{
    public interface IFiniteEnumerableAttributeComponentIntersectionOperator<
        TData, 
        TAttributeComponent, 
        TFactory, 
        TFactoryArgs>
        : IFactoryBinaryAttributeComponentAcceptor<
            TData, 
            TAttributeComponent, 
            IFiniteEnumerableAttributeComponent<TData>,
            TFactory,
            TFactoryArgs,
            AttributeComponent<TData>>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IFiniteEnumerableAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TAttributeComponent, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    {
        AttributeComponent<TData>
             IFactoryBinaryOperator<
                TAttributeComponent,
                IFiniteEnumerableAttributeComponent<TData>,
                TFactory,
                AttributeComponent<TData>>
            .Accept(
                TAttributeComponent first,
                IFiniteEnumerableAttributeComponent<TData> second,
                TFactory factory)
        {
            IEnumerable<TData> intersectedElements =
                Enumerable.Intersect(first, second);

            return factory.CreateNonFictional(first, intersectedElements);
        }
    }
}
