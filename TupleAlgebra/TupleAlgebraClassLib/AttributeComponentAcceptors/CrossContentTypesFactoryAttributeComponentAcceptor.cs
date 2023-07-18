using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class CrossContentTypesFactoryBinaryAttributeComponentAcceptor<TData, TOperand1, TOperationResult>
        : FactoryBinaryAttributeComponentAcceptor<TData, TOperand1, TOperationResult>,
          IFactoryBinaryAttributeComponentAcceptor<TData, TOperand1, EmptyAttributeComponent<TData>, TOperationResult>,
          IFactoryBinaryAttributeComponentAcceptor<TData, TOperand1, NonFictionalAttributeComponent<TData>, TOperationResult>,
          IFactoryBinaryAttributeComponentAcceptor<TData, TOperand1, FullAttributeComponent<TData>, TOperationResult>
        where TOperand1 : AttributeComponent<TData>
    {
        public abstract TOperationResult Accept(
            TOperand1 first,
            EmptyAttributeComponent<TData> second,
            AttributeComponentFactory factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            NonFictionalAttributeComponent<TData> second,
            AttributeComponentFactory factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            FullAttributeComponent<TData> second,
            AttributeComponentFactory factory);
    }
}
