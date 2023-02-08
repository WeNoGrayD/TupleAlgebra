using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    public sealed class EmptyAttributeComponentEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, EmptyAttributeComponent<TValue>, bool>
    {
        public override bool Accept(EmptyAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return true;
        }

        public override bool Accept(EmptyAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return false;
        }

        public override bool Accept(EmptyAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return false;
        }
    }
}
