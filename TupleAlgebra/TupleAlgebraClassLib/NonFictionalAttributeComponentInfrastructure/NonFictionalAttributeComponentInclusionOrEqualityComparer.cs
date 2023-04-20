﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentInclusionOrEqualityComparer<TData>
        : CrossContentTypesInstantBinaryAttributeComponentAcceptor<TData, NonFictionalAttributeComponent<TData>, bool>
    {
        public override bool Accept(NonFictionalAttributeComponent<TData> first, EmptyAttributeComponent<TData> second)
        {
            return true;
        }

        public override bool Accept(NonFictionalAttributeComponent<TData> first, NonFictionalAttributeComponent<TData> second)
        {
            return first.IncludesOrEqualsTo(second);
        }

        public override bool Accept(NonFictionalAttributeComponent<TData> first, FullAttributeComponent<TData> second)
        {
            return false;
        }
    }
}
