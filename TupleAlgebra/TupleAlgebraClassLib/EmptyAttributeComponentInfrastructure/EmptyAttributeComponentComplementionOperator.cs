﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    using static AttributeComponentHelper;

    public sealed class EmptyAttributeComponentComplementationOperator<TData>
        : InstantUnaryAttributeComponentAcceptor<TData, EmptyAttributeComponent<TData>, IAttributeComponent<TData>>
    {
        public override IAttributeComponent<TData> Accept(
            EmptyAttributeComponent<TData> first)
        {
            return first.Factory.CreateFull();
        }
    }
}
