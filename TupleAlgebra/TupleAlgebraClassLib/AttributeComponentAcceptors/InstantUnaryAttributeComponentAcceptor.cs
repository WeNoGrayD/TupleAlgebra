using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectAcceptors;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class InstantUnaryAttributeComponentAcceptor<TData, TOperand, TOperationResult>
        : InstantUnaryOperator<TOperand, TOperationResult>,
          IInstantUnaryAttributeComponentAcceptor<TData, TOperand, TOperationResult>
        where TOperand : IAttributeComponent<TData>
    {
    }
}
