using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public abstract class NonFictionalAttributeComponentSymmetricExceptionOperator<TData, CTOperand1>
        : FactoryBinaryAttributeComponentAcceptor<TData, CTOperand1, AttributeComponent<TData>>,
          IFactoryBinaryAttributeComponentAcceptor<TData, NonFictionalAttributeComponent<TData>, EmptyAttributeComponent<TData>, AttributeComponent<TData>>,
          IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand1, NonFictionalAttributeComponent<TData>, AttributeComponent<TData>>,
          IFactoryBinaryAttributeComponentAcceptor<TData, NonFictionalAttributeComponent<TData>, FullAttributeComponent<TData>, AttributeComponent<TData>>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
    {
        public AttributeComponent<TData> Accept(
            NonFictionalAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second,
            AttributeComponentFactory factory)
        {
            return first;
        }

        public AttributeComponent<TData> Accept(
            CTOperand1 first,
            NonFictionalAttributeComponent<TData> second,
            AttributeComponentFactory factory)
        {
            throw new NotImplementedException();
        }

        public AttributeComponent<TData> Accept(
            NonFictionalAttributeComponent<TData> first,
            FullAttributeComponent<TData> second,
            AttributeComponentFactory factory)
        {
            return ~first;
        }
    }
}
