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
    public interface IFactoryUnaryAttributeComponentVisitor<TData, TIntermediateResult, in CTOperand, in TFactory, in TFactoryArgs, out TOperationResult>
        : IFactoryUnaryOperator<CTOperand, TFactory, TOperationResult>
        where CTOperand : NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    { }

    public interface IFactoryBinaryAttributeComponentVisitor<TData, TIntermediateResult, in TOperand1, in CTOperand1, in TOperand2, in TFactory, in TFactoryArgs, out TOperationResult>
        : IFactoryBinaryOperator<TOperand1, TOperand2, TFactory, TOperationResult>
        where TOperand1 : NonFictionalAttributeComponent<TData>
        where CTOperand1 : TOperand1
        where TOperand2 : IAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    { }

    public interface IFactoryBinaryAttributeComponentVisitor<TData, TIntermediateResult, in CTOperand1, in TOperand2, in TFactory, in TFactoryArgs, out TOperationResult>
        : IFactoryBinaryAttributeComponentVisitor<TData, TIntermediateResult, CTOperand1, CTOperand1, TOperand2, TFactory, TFactoryArgs, TOperationResult>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
        where TOperand2 : IAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    { }
}
