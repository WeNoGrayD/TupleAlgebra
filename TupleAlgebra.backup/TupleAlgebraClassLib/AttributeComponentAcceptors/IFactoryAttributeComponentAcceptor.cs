﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public interface IFactoryAttributeComponentAcceptor<TData, in TOperand, out TOperationResult>
        where TOperand : AttributeComponent<TData>
    {
        TOperationResult Accept(TOperand first, AttributeComponentFactory<TData> factory);
    }

    public interface IInstantAttributeComponentAcceptor<TData, in TOperand1, in TOperand2, out TOperationResult>
        where TOperand1 : AttributeComponent<TData>
        where TOperand2 : AttributeComponent<TData>
    {
        TOperationResult Accept(TOperand1 first, TOperand2 second);
    }
}
