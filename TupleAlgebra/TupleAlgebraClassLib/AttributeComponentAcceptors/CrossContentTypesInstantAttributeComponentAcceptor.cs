using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    /// <summary>
    /// Интерфейс для операторов и компараторов, которые способны принимать
    /// две типизированных компоненты атрибута.
    /// Реализует паттерн, обратный паттерну посетителя - 
    /// паттерн "приниматель" (название временное).
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public abstract class CrossContentTypesInstantBinaryAttributeComponentAcceptor<TData, TOperand1, TOperationResult>
        : InstantBinaryAttributeComponentAcceptor<TData, TOperationResult>,
          IInstantBinaryAttributeComponentAcceptor<TData, TOperand1, EmptyAttributeComponent<TData>, TOperationResult>,
          IInstantBinaryAttributeComponentAcceptor<TData, TOperand1, NonFictionalAttributeComponent<TData>, TOperationResult>,
          IInstantBinaryAttributeComponentAcceptor<TData, TOperand1, FullAttributeComponent<TData>, TOperationResult>
        where TOperand1 : AttributeComponent<TData>
    {
        public abstract TOperationResult Accept(
            TOperand1 first,
            EmptyAttributeComponent<TData> second);

        public abstract TOperationResult Accept(
            TOperand1 first,
            NonFictionalAttributeComponent<TData> second);

        public abstract TOperationResult Accept(
            TOperand1 first,
            FullAttributeComponent<TData> second);
    }
}
