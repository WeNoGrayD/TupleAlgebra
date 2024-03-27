using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Diagnostics;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class FactoryBinaryAttributeComponentAcceptor<
        TData, 
        TIntermediateResult, 
        CTOperand1, 
        CTFactory, 
        TFactoryArgs, 
        TOperationResult>
        : FactoryBinaryOperator<CTOperand1, IAttributeComponent<TData>, CTFactory, TOperationResult>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
        where CTFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand1, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    { }
}
