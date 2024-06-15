using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    public sealed class FullAttributeComponentInclusionOrEqualityComparer<TData>
        : FictionalAttributeComponentCrossTypeInstantBinaryVisitor<TData, FullAttributeComponent<TData>, bool>
    {
        public override bool Visit(FullAttributeComponent<TData> first, EmptyAttributeComponent<TData> second)
        {
            return true;
        }

        public override bool Visit(FullAttributeComponent<TData> first, NonFictionalAttributeComponent<TData> second)
        {
            return true;
        }

        public override bool Visit(FullAttributeComponent<TData> first, FullAttributeComponent<TData> second)
        {
            return true;
        }
    }
}
