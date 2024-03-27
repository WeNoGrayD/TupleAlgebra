using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public abstract class NonFictionalAttributeComponentSetBinaryOperator<
        TData, 
        TIntermediateResult,
        CTOperand1,
        TFactory, 
        CTFactoryArgs>
        : NonFictionalAttributeComponentCrossTypeFactoryBinaryAcceptor<TData, TIntermediateResult, CTOperand1, TFactory, CTFactoryArgs, IAttributeComponent<TData>>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand1, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    { }

    public abstract class NonFictionalAttributeComponentBooleanBinaryOperator<
        TData, 
        CTOperand1>
        : NonFictionalAttributeComponentCrossTypeInstantBinaryAcceptor<TData, NonFictionalAttributeComponent<TData>, CTOperand1, bool>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
    { }
}
