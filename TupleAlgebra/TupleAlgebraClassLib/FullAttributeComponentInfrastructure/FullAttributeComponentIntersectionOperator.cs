﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    public sealed class FullAttributeComponentIntersectionOperator<TData>
        : FictionalAttributeComponentCrossTypeInstantBinaryVisitor<
            TData, 
            FullAttributeComponent<TData>, 
            IAttributeComponent<TData>>
    {
        public override IAttributeComponent<TData> Visit(
            FullAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second)
        {
            return second;
        }

        public override IAttributeComponent<TData> Visit(
            FullAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second)
        {
            return second;
        }

        public override IAttributeComponent<TData> Visit(
            FullAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return first;
        }
    }
}
