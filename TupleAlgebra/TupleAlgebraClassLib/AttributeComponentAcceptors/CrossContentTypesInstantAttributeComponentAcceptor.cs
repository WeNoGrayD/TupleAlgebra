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
    public abstract class CrossContentTypesInstantAttributeComponentAcceptor<TValue, TOperand1, TOperationResult>
        : InstantBinaryAttributeComponentAcceptor<TValue, TOperationResult>,
          IInstantAttributeComponentAcceptor<TValue, TOperand1, EmptyAttributeComponent<TValue>, TOperationResult>,
          IInstantAttributeComponentAcceptor<TValue, TOperand1, NonFictionalAttributeComponent<TValue>, TOperationResult>,
          IInstantAttributeComponentAcceptor<TValue, TOperand1, FullAttributeComponent<TValue>, TOperationResult>
        where TOperand1 : AttributeComponent<TValue>
    {
        public abstract TOperationResult Accept(
            TOperand1 first,
            EmptyAttributeComponent<TValue> second);

        public abstract TOperationResult Accept(
            TOperand1 first,
            NonFictionalAttributeComponent<TValue> second);

        public abstract TOperationResult Accept(
            TOperand1 first,
            FullAttributeComponent<TValue> second);
    }
}
