using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    public sealed class FullAttributeComponentSymmetricExceptionOperator<TData>
        : CrossContentTypesInstantBinaryAttributeComponentAcceptor<TData, FullAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public override AttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second)
        {
            return first;
        }

        public override AttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second)
        {
            return !second;
        }

        public override AttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return EmptyAttributeComponent<TData>.FictionalAttributeComponentFactory.CreateEmpty<TData>(new AttributeComponentFactoryArgs());
        }
    }
}
