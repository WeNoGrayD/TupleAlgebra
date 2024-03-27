using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    public sealed class FullAttributeComponentIntersectionOperator<TData>
        : FictionalAttributeComponentCrossTypeInstantBinaryAcceptor<
            TData, 
            FullAttributeComponent<TData>, 
            IAttributeComponent<TData>>
    {
        public override IAttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second)
        {
            return second;
        }

        public override IAttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second)
        {
            return second;
        }

        public override IAttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return first;
        }
    }
}
