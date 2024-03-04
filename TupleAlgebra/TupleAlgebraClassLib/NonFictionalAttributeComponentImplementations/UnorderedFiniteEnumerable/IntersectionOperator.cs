﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    public class IntersectionOperator<TData>
        : NonFictionalAttributeComponentIntersectionOperator<TData, UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>, IUnorderedFiniteEnumerableAttributeComponentFactory<TData>, UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IUnorderedFiniteEnumerableAttributeComponentBinaryOperator<TData>
    {
        public AttributeComponent<TData> Accept(
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> first,
            IFiniteEnumerableAttributeComponent<TData> second,
            IUnorderedFiniteEnumerableAttributeComponentFactory<TData> factory)
        {
            HashSet<TData> intersectedElements = first.Values.Intersect(second.Values).ToHashSet();

            return factory.CreateNonFictional(first, intersectedElements);
        }
    }
}
