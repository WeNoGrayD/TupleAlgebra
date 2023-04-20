using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public interface IInstantUnaryOperator<in TOperand, out TOperationResult>
    {
        TOperationResult Accept(TOperand first);
    }

    public interface IInstantUnaryAttributeComponentAcceptor<TData, in TOperand, out TOperationResult>
        : IInstantUnaryOperator<TOperand, TOperationResult>
        where TOperand : AttributeComponent<TData>
    { }

    public interface IInstantBinaryOperator<in TOperand1, in TOperand2, out TOperationResult>
    {
        TOperationResult Accept(TOperand1 first, TOperand2 second);
    }

    public interface IInstantBinaryAttributeComponentAcceptor<TData, in TOperand1, in TOperand2, out TOperationResult>
        : IInstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
        where TOperand1 : AttributeComponent<TData>
        where TOperand2 : AttributeComponent<TData>
    { }
}
