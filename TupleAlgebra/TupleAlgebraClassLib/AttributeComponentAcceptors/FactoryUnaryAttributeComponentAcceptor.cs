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
    public abstract class FactoryUnaryAttributeComponentAcceptor<TData, CTOperand, TOperationResult>
        : FactoryUnaryOperator<CTOperand, AttributeComponentFactory, TOperationResult>,
          IFactoryUnaryAttributeComponentAcceptor<TData, CTOperand, TOperationResult>
        where CTOperand: AttributeComponent<TData>
    { }
}
