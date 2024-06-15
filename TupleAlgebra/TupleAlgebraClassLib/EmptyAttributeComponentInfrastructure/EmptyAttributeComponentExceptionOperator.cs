using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    public sealed class EmptyAttributeComponentExceptionOperator<TData>
        : FictionalAttributeComponentCrossTypeInstantBinaryVisitor<
            TData, 
            EmptyAttributeComponent<TData>, 
            IAttributeComponent<TData>>
    {
        public override IAttributeComponent<TData> Visit(
            EmptyAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second)
        {
            return first;
        }

        public override IAttributeComponent<TData> Visit(
            EmptyAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second)
        {
            return first;
        }

        public override IAttributeComponent<TData> Visit(
            EmptyAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return first;
        }
    }
}
