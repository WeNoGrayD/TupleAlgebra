using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentSymmetricExceptionOperator<TData>
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
            return first.SymmetricExceptWith(second);
        }

        public override AttributeComponent<TData> Accept(
            NonFictionalAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return !first;
        }
    }
}
