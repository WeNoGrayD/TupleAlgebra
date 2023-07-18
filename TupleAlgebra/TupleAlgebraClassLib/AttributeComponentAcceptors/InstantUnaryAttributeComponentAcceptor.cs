using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class InstantUnaryAttributeComponentAcceptor<TData, TOperand, TOperationResult>
        : InstantUnaryOperator<TOperand, TOperationResult>
        where TOperand : AttributeComponent<TData>
    { }
}
