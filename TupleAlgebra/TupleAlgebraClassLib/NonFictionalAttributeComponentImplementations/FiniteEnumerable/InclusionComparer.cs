﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable
{
    public interface IFiniteEnumerableAttributeComponentInclusionComparer<TData>
        : IAttributeComponentBooleanOperator<
            TData,
            IFiniteEnumerableAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>>,
          IFiniteEnumerableSequenceComparer<TData>,
          IFiniteEnumerableXFilteringInclusionComparer<TData>
    {
        bool IInstantBinaryOperator<
            IFiniteEnumerableAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>,
            bool>
            .Visit(
                IFiniteEnumerableAttributeComponent<TData> greater,
                IFiniteEnumerableAttributeComponent<TData> lower)
        {
            return SequenceInclude(greater, lower);
        }
    }
}
