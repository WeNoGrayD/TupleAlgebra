﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable
{
    public interface IFiniteEnumerableAttributeComponentUnionOperator<
        TData,
        TAttributeComponent,
        TFactory,
        TFactoryArgs>
        : IFiniteEnumerableAttributeComponentBinaryOperator<
            TData,
            TAttributeComponent,
            TFactory,
            TFactoryArgs>,
          IFiniteEnumerableXFilteringUnionOperator<
            TData,
            TAttributeComponent,
            TFactory,
            TFactoryArgs>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IFiniteEnumerableAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, IEnumerable<TData>, TFactoryArgs>
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
            OperationResultEnumerableResultProvider<TData> unitedElements =
                new OperationResultEnumerableResultProvider<TData>(
                    Enumerable.Union(first, second), false);

            return factory.CreateNonFictional(unitedElements);
        }
    }
}
