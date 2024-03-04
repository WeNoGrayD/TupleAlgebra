using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public interface IFactoryUnaryAttributeComponentAcceptor<TData, in CTOperand, in TFactory, in TFactoryArgs, out TOperationResult>
        : IFactoryUnaryOperator<CTOperand, TFactory, TOperationResult>
        where CTOperand : NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, CTOperand, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    { }

    public interface IFactoryBinaryAttributeComponentAcceptor<TData, in TOperand1, in CTOperand1, in TOperand2, in TFactory, in TFactoryArgs, out TOperationResult>
        : IFactoryBinaryOperator<TOperand1, TOperand2, TFactory, TOperationResult>
        where TOperand1 : NonFictionalAttributeComponent<TData>
        where CTOperand1 : TOperand1
        where TOperand2 : IAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, CTOperand1, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    { }

    public interface IFactoryBinaryAttributeComponentAcceptor<TData, in CTOperand1, in TOperand2, in TFactory, in TFactoryArgs, out TOperationResult>
        : IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand1, CTOperand1, TOperand2, TFactory, TFactoryArgs, TOperationResult>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
        where TOperand2 : IAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, CTOperand1, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    { }
}
