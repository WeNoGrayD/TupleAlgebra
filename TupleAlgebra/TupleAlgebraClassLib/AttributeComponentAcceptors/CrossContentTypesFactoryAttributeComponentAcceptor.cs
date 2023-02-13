using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class CrossContentTypesFactoryAttributeComponentAcceptor<TData, TOperand1, TOperationResult>
        : FactoryBinaryAttributeComponentAcceptor<TData, TOperationResult>,
          IFactoryAttributeComponentAcceptor<TData, TOperand1, EmptyAttributeComponent<TData>, TOperationResult>,
          IFactoryAttributeComponentAcceptor<TData, TOperand1, NonFictionalAttributeComponent<TData>, TOperationResult>,
          IFactoryAttributeComponentAcceptor<TData, TOperand1, FullAttributeComponent<TData>, TOperationResult>
        where TOperand1 : AttributeComponent<TData>
    {
        public abstract TOperationResult Accept(
            TOperand1 first,
            EmptyAttributeComponent<TData> second,
            AttributeComponentFactory<TData> factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            NonFictionalAttributeComponent<TData> second,
            AttributeComponentFactory<TData> factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            FullAttributeComponent<TData> second,
            AttributeComponentFactory<TData> factory);
    }
}
