using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    public sealed class FullAttributeComponentEqualityComparer<TData>
        : CrossContentTypesInstantBinaryAttributeComponentAcceptor<TData, FullAttributeComponent<TData>, bool>
    {
        public override bool Accept(FullAttributeComponent<TData> first, EmptyAttributeComponent<TData> second)
        {
            return false;
        }

        public override bool Accept(FullAttributeComponent<TData> first, NonFictionalAttributeComponent<TData> second)
        {
            return false;
        }

        public override bool Accept(FullAttributeComponent<TData> first, FullAttributeComponent<TData> second)
        {
            return true;
        }
    }
}
