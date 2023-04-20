using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentExceptionOperator<TData>
        : CrossContentTypesInstantBinaryAttributeComponentAcceptor<TData, NonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public override AttributeComponent<TData> Accept(
            NonFictionalAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second)
        {
            return first;
        }

        public override AttributeComponent<TData> Accept(
            NonFictionalAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second)
        {
            return first.ExceptWith(second);
        }

        public override AttributeComponent<TData> Accept(
            NonFictionalAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return EmptyAttributeComponent<TData>.FictionalAttributeComponentFactory.CreateEmpty<TData>(new AttributeComponentFactoryArgs());
        }
    }
}
