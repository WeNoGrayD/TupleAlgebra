using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;

namespace TupleAlgebraClassLib.AttributeComponentVisitors
{
    public interface IInstantUnaryAttributeComponentVisitor<TData, in TOperand, out TOperationResult>
        : IInstantUnaryOperator<TOperand, TOperationResult>
        where TOperand : IAttributeComponent<TData>
    { }

    public interface IInstantBinaryAttributeComponentVisitor<TData, in TOperand1, in TOperand2, out TOperationResult>
        : IInstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
        where TOperand1 : IAttributeComponent<TData>
        where TOperand2 : IAttributeComponent<TData>
    { }
}
