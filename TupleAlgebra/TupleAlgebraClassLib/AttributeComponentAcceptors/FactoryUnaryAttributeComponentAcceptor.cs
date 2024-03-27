﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Diagnostics;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class FactoryUnaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, TFactory, CTFactoryArgs, TOperationResult>
        : FactoryUnaryOperator<CTOperand, TFactory, TOperationResult>
        where CTOperand: NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    { }

    public abstract class AttributeComponentFactoryUnarySetOperator<TData, TIntermediateResult, CTOperand, TFactory, CTFactoryArgs>
        : FactoryUnaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, TFactory, CTFactoryArgs, IAttributeComponent<TData>>
        where CTOperand : NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    { }
}
