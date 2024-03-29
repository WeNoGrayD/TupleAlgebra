﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    public sealed class FullAttributeComponentIntersectionOperator<TData>
        : CrossContentTypesInstantAttributeComponentAcceptor<TData, FullAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public override AttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second)
        {
            return second;
        }

        public override AttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second)
        {
            return second;
        }

        public override AttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return first;
        }
    }
}
