using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    public sealed class EmptyAttributeComponentEqualityComparer<TData>
        : CrossContentTypesInstantAttributeComponentAcceptor<TData, EmptyAttributeComponent<TData>, bool>
    {
        public override bool Accept(EmptyAttributeComponent<TData> first, EmptyAttributeComponent<TData> second)
        {
            return true;
        }

        public override bool Accept(EmptyAttributeComponent<TData> first, NonFictionalAttributeComponent<TData> second)
        {
            return false;
        }

        public override bool Accept(EmptyAttributeComponent<TData> first, FullAttributeComponent<TData> second)
        {
            return false;
        }
    }
}
