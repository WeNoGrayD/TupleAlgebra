using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Diagnostics;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class FactoryBinaryAttributeComponentAcceptor<TData, TOperand1, TOperationResult>
        : FactoryBinaryOperator<TOperand1, AttributeComponent<TData>, AttributeComponentFactory, TOperationResult>,
          IFactoryBinaryAttributeComponentAcceptor<TData, TOperand1, AttributeComponent<TData>, TOperationResult>
        where TOperand1 : AttributeComponent<TData>
    { }
}
