using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    public sealed class FullAttributeComponentEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, bool>
    {
        public override bool Accept(FullAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return false;
        }

        public override bool Accept(FullAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return false;
        }

        public override bool Accept(FullAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return true;
        }
    }
}
