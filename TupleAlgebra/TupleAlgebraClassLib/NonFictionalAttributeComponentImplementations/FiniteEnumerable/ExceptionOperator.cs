﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable
{
    public interface IFiniteEnumerableAttributeComponentExceptionOperator<
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
            IEnumerable<TData> resultElements =
                Enumerable.Except(first, second);

            return factory.CreateNonFictional(first, resultElements);
        }
    }
}
