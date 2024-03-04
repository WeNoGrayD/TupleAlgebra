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
    public abstract class FactoryBinaryAttributeComponentAcceptor<TData, CTOperand1, CTFactory, TFactoryArgs, TOperationResult>
        : FactoryBinaryOperator<CTOperand1, AttributeComponent<TData>, CTFactory, TOperationResult>,
          IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand1, AttributeComponent<TData>, CTFactory, TFactoryArgs, TOperationResult>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
        where CTFactory : INonFictionalAttributeComponentFactory<TData, CTOperand1, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    { }
}
