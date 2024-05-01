using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable
{
    public interface IFiniteEnumerableAttributeComponentIntersectionOperator<
        TData, 
        TAttributeComponent, 
        TFactory, 
        TFactoryArgs>
        : IFiniteEnumerableAttributeComponentBinaryOperator<
            TData,
            TAttributeComponent,
            TFactory,
            TFactoryArgs>,
          IFiniteEnumerableXFilteringIntersectionOperator<
            TData,
            TAttributeComponent,
            TFactory,
            TFactoryArgs>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IFiniteEnumerableAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, IEnumerable<TData>, TAttributeComponent, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    {
        IAttributeComponent<TData>
             IFactoryBinaryOperator<
                TAttributeComponent,
                IFiniteEnumerableAttributeComponent<TData>,
                TFactory,
                IAttributeComponent<TData>>
            .Accept(
                TAttributeComponent first,
                IFiniteEnumerableAttributeComponent<TData> second,
                TFactory factory)
        {
            OperationResultEnumerableResultProvider<TData> intersectedElements =
                new OperationResultEnumerableResultProvider<TData>(
                    Enumerable.Intersect(first, second), false);

            return factory.CreateNonFictional(first, intersectedElements);
        }
    }
}
