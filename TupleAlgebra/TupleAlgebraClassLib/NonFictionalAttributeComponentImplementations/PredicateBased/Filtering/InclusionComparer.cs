﻿using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    public class InclusionComparer<TData>
        : NonFictionalAttributeComponentInclusionComparer<
            TData,
            FilteringAttributeComponent<TData>>,
          IInstantBinaryAttributeComponentVisitor<
            TData,
            FilteringAttributeComponent<TData>,
            FilteringAttributeComponent<TData>,
            bool>,
          IInstantBinaryOperator<
            FilteringAttributeComponent<TData>,
            NonFictionalAttributeComponent<TData>,
            bool>
    {
        bool IInstantBinaryOperator<
            FilteringAttributeComponent<TData>,
            FilteringAttributeComponent<TData>,
            bool>
            .Visit(
                 FilteringAttributeComponent<TData>  greater,
                 FilteringAttributeComponent<TData> lower)
        {
            return greater.ToIterableAttributeComponent()
                .Includes(lower.ToIterableAttributeComponent());
        }

        bool IInstantBinaryOperator<
            FilteringAttributeComponent<TData>,
            NonFictionalAttributeComponent<TData>,
            bool>
            .Visit(
                 FilteringAttributeComponent<TData> greater,
                 NonFictionalAttributeComponent<TData> lower)
        {
            return lower <= (greater.ToIterableAttributeComponent());
        }
    }
}
