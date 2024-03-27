﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable
{
    public interface IFiniteEnumerableAttributeComponentEqualityComparer<TData>
        : IAttributeComponentBooleanOperator<
            TData,
            IFiniteEnumerableAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>>,
          IFiniteEnumerableSequenceComparer<TData>,
          IFiniteEnumerableXFilteringEqualityComparer<TData>
    {
        bool IInstantBinaryOperator<
            IFiniteEnumerableAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>,
            bool>
            .Accept(
                IFiniteEnumerableAttributeComponent<TData> greater,
                IFiniteEnumerableAttributeComponent<TData> lower)
        {
            return SequenceEquals(greater, lower);
        }
    }
}
