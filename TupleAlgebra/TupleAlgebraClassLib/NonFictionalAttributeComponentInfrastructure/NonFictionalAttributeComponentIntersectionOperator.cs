using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentIntersectionOperator<TData>
        : CrossContentTypesInstantBinaryAttributeComponentAcceptor<TData, NonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public override AttributeComponent<TData> Accept(
            NonFictionalAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second)
        {
            return second;
        }

        public override AttributeComponent<TData> Accept(
            NonFictionalAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second)
        {
            return first.IntersectWith(second);
        }

        public override AttributeComponent<TData> Accept(
            NonFictionalAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return first;
        }
    }
}
