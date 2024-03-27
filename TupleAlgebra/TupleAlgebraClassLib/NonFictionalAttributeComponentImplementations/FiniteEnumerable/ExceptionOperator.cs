using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable
{
    public interface IFiniteEnumerableAttributeComponentExceptionOperator<
        TData,
        TAttributeComponent,
        TFactory,
        TFactoryArgs>
        : IFiniteEnumerableAttributeComponentBinaryOperator<
            TData,
            TAttributeComponent,
            TFactory,
            TFactoryArgs>,
          IFiniteEnumerableXFilteringExceptionOperator<
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
            IEnumerable<TData> resultElements =
                Enumerable.Except(first, second);

            return factory.CreateNonFictional(first, resultElements);
        }
    }
}
