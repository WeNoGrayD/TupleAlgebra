using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    /// <summary>
    /// Интерфейс для операторов и компараторов, которые способны принимать
    /// две типизированных компоненты атрибута.
    /// Реализует паттерн, обратный паттерну посетителя - 
    /// паттерн "приниматель" (название временное).
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public abstract class NonFictionalAttributeComponentCrossTypeInstantBinaryAcceptor<
        TData, 
        TOperand1, 
        CTOperand1,
        TOperationResult>
        : InstantBinaryAttributeComponentAcceptor<TData, CTOperand1, TOperationResult>,
          IInstantBinaryAttributeComponentAcceptor<TData, TOperand1, EmptyAttributeComponent<TData>, TOperationResult>,
          IInstantBinaryAttributeComponentAcceptor<TData, TOperand1, FullAttributeComponent<TData>, TOperationResult>
        where TOperand1 : AttributeComponent<TData>
        where CTOperand1 : TOperand1
    {
        public abstract TOperationResult Accept(
            TOperand1 first,
            EmptyAttributeComponent<TData> second);

        public abstract TOperationResult Accept(
            TOperand1 first,
            FullAttributeComponent<TData> second);
    }

    public abstract class FictionalAttributeComponentCrossTypeInstantBinaryAcceptor<
        TData,
        TOperand1, 
        TOperationResult>
        : NonFictionalAttributeComponentCrossTypeInstantBinaryAcceptor<TData, TOperand1, TOperand1, TOperationResult>,
          IInstantBinaryAttributeComponentAcceptor<TData, TOperand1, NonFictionalAttributeComponent<TData>, TOperationResult>
        where TOperand1 : AttributeComponent<TData>
    {
        public abstract TOperationResult Accept(
            TOperand1 first,
            NonFictionalAttributeComponent<TData> second);
    }

    public abstract class NonFictionalAttributeComponentCrossTypeFactoryBinaryAcceptor<
        TData, 
        TIntermediateResult, 
        CTOperand1,
        TFactory, 
        CTFactoryArgs, 
        TOperationResult>
        : FactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand1, TFactory, CTFactoryArgs, TOperationResult>,
          IFactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, NonFictionalAttributeComponent<TData>, CTOperand1, EmptyAttributeComponent<TData>, TFactory, CTFactoryArgs, TOperationResult>,
          IFactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, NonFictionalAttributeComponent<TData>, CTOperand1, FullAttributeComponent<TData>, TFactory, CTFactoryArgs, TOperationResult>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand1, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
        public abstract TOperationResult Accept(
            NonFictionalAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second,
            TFactory factory);

        public abstract TOperationResult Accept(
            NonFictionalAttributeComponent<TData> first,
            FullAttributeComponent<TData> second,
            TFactory factory);
    }
}
