﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    /// <summary>
    /// Дополнение нефиктивной компоненты, стандартная реализация.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="CTOperand"></typeparam>
    /// <typeparam name="CTFactoryArgs"></typeparam>
    public class NonFictionalAttributeComponentComplementationOperator<
        TData, 
        TIntermediateResult, 
        CTOperand,
        TFactory, 
        CTFactoryArgs>
        : FactoryUnaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, TFactory, CTFactoryArgs, IAttributeComponent<TData>>
        where CTOperand: NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
        public override IAttributeComponent<TData> Accept(
            CTOperand first, 
            TFactory factory)
        {
            return first.Domain ^ first;
        }
    }
}
