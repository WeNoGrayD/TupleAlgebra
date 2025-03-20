using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
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
        where TFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
    {
        IAttributeComponent<TData>
             IFactoryBinaryOperator<
                TAttributeComponent,
                IFiniteEnumerableAttributeComponent<TData>,
                TFactory,
                IAttributeComponent<TData>>
            .Visit(
                TAttributeComponent first,
                IFiniteEnumerableAttributeComponent<TData> second,
                TFactory factory)
        {
            OperationResultEnumerableResultProvider<TData> resultElements =
                new OperationResultEnumerableResultProvider<TData>(
                    Enumerable.Except(first, second), false);

            return factory.CreateNonFictional(first, resultElements);
        }
    }
}
