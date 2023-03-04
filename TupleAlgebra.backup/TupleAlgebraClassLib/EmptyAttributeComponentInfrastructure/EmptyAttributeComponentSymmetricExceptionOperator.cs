using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    public sealed class EmptyAttributeComponentSymmetricExceptionOperator<TData>
        : CrossContentTypesInstantAttributeComponentAcceptor<TData, EmptyAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public override AttributeComponent<TData> Accept(
            EmptyAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second)
        {
            return first;
        }

        public override AttributeComponent<TData> Accept(
            EmptyAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second)
        {
            return second;
        }

        public override AttributeComponent<TData> Accept(
            EmptyAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return second;
        }
    }
}
