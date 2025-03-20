using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentUnionOperator<TData>
        : CrossContentTypesInstantAttributeComponentAcceptor<TData, NonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
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
            return first.UnionWith(second);
        }

        public override AttributeComponent<TData> Accept(
            NonFictionalAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return second;
        }
    }
}
