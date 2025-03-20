using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    public sealed class EmptyAttributeComponentEqualityComparer<TData>
        : FictionalAttributeComponentCrossTypeInstantBinaryVisitor<TData, EmptyAttributeComponent<TData>, bool>
    {
        public override bool Visit(EmptyAttributeComponent<TData> first, EmptyAttributeComponent<TData> second)
        {
            return true;
        }

        public override bool Visit(EmptyAttributeComponent<TData> first, NonFictionalAttributeComponent<TData> second)
        {
            return false;
        }

        public override bool Visit(EmptyAttributeComponent<TData> first, FullAttributeComponent<TData> second)
        {
            return false;
        }
    }
}
