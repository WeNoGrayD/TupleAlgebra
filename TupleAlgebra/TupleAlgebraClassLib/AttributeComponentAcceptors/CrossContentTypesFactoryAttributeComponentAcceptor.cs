using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class CrossContentTypesFactoryAttributeComponentAcceptor<TValue, TOperand1, TOperationResult>
        : FactoryBinaryAttributeComponentAcceptor<TValue, TOperationResult>,
          IFactoryAttributeComponentAcceptor<TValue, TOperand1, EmptyAttributeComponent<TValue>, TOperationResult>,
          IFactoryAttributeComponentAcceptor<TValue, TOperand1, NonFictionalAttributeComponent<TValue>, TOperationResult>,
          IFactoryAttributeComponentAcceptor<TValue, TOperand1, FullAttributeComponent<TValue>, TOperationResult>
        where TOperand1 : AttributeComponent<TValue>
    {
        public abstract TOperationResult Accept(
            TOperand1 first,
            EmptyAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            NonFictionalAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            FullAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory);
    }
}
