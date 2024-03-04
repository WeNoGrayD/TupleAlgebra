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
    public abstract class FactoryUnaryAttributeComponentAcceptor<TData, CTOperand, TFactory, CTFactoryArgs, TOperationResult>
        : FactoryUnaryOperator<CTOperand, TFactory, TOperationResult>,
          IFactoryUnaryAttributeComponentAcceptor<TData, CTOperand, TFactory, CTFactoryArgs, TOperationResult>
        where CTOperand: NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, CTOperand, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    { }
}
