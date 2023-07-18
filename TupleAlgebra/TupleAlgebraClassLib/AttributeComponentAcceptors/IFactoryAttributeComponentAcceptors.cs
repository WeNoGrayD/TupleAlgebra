using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public interface IFactoryUnaryAttributeComponentAcceptor<TData, in TOperand, out TOperationResult>
        : IFactoryUnaryOperator<TOperand, AttributeComponentFactory, TOperationResult>
        where TOperand : AttributeComponent<TData>
    { }

    public interface IFactoryBinaryAttributeComponentAcceptor<TData, in TOperand1, in TOperand2, out TOperationResult>
        : IFactoryBinaryOperator<TOperand1, TOperand2, AttributeComponentFactory, TOperationResult>
        where TOperand1 : AttributeComponent<TData>
        where TOperand2 : AttributeComponent<TData>
    { }
}
