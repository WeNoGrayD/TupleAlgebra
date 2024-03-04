using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public interface IInstantUnaryAttributeComponentAcceptor<TData, in TOperand, out TOperationResult>
        : IInstantUnaryOperator<TOperand, TOperationResult>
        where TOperand : IAttributeComponent<TData>
    { }

    public interface IInstantBinaryAttributeComponentAcceptor<TData, in TOperand1, in TOperand2, out TOperationResult>
        : IInstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
        where TOperand1 : IAttributeComponent<TData>
        where TOperand2 : IAttributeComponent<TData>
    { }
}
