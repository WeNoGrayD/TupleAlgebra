using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponentVisitors
{
    /// <summary>
    /// Интерфейс для операторов и компараторов, которые способны принимать
    /// две типизированных компоненты атрибута.
    /// Реализует паттерн, обратный паттерну посетителя - 
    /// паттерн "приниматель" (название временное).
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public abstract class NonFictionalAttributeComponentCrossTypeInstantBinaryVisitor<
        TData, 
        TOperand1, 
        CTOperand1,
        TOperationResult>
        : InstantBinaryAttributeComponentVisitor<TData, CTOperand1, TOperationResult>,
          IInstantBinaryAttributeComponentVisitor<TData, TOperand1, EmptyAttributeComponent<TData>, TOperationResult>,
          IInstantBinaryAttributeComponentVisitor<TData, TOperand1, FullAttributeComponent<TData>, TOperationResult>
        where TOperand1 : AttributeComponent<TData>
        where CTOperand1 : TOperand1
    {
        public abstract TOperationResult Visit(
            TOperand1 first,
            EmptyAttributeComponent<TData> second);

        public abstract TOperationResult Visit(
            TOperand1 first,
            FullAttributeComponent<TData> second);
    }

    public abstract class FictionalAttributeComponentCrossTypeInstantBinaryVisitor<
        TData,
        TOperand1, 
        TOperationResult>
        : NonFictionalAttributeComponentCrossTypeInstantBinaryVisitor<TData, TOperand1, TOperand1, TOperationResult>,
          IInstantBinaryAttributeComponentVisitor<TData, TOperand1, NonFictionalAttributeComponent<TData>, TOperationResult>
        where TOperand1 : AttributeComponent<TData>
    {
        public abstract TOperationResult Visit(
            TOperand1 first,
            NonFictionalAttributeComponent<TData> second);
    }

    public abstract class NonFictionalAttributeComponentCrossTypeFactoryBinaryVisitor<
        TData, 
        TIntermediateResult, 
        CTOperand1,
        TFactory, 
        CTFactoryArgs, 
        TOperationResult>
        : FactoryBinaryAttributeComponentVisitor<TData, TIntermediateResult, CTOperand1, TFactory, CTFactoryArgs, TOperationResult>,
          IFactoryBinaryAttributeComponentVisitor<TData, TIntermediateResult, NonFictionalAttributeComponent<TData>, CTOperand1, EmptyAttributeComponent<TData>, TFactory, CTFactoryArgs, TOperationResult>,
          IFactoryBinaryAttributeComponentVisitor<TData, TIntermediateResult, NonFictionalAttributeComponent<TData>, CTOperand1, FullAttributeComponent<TData>, TFactory, CTFactoryArgs, TOperationResult>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand1, CTFactoryArgs>
        where CTFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
    {
        public abstract TOperationResult Visit(
            NonFictionalAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second,
            TFactory factory);

        public abstract TOperationResult Visit(
            NonFictionalAttributeComponent<TData> first,
            FullAttributeComponent<TData> second,
            TFactory factory);
    }
}
