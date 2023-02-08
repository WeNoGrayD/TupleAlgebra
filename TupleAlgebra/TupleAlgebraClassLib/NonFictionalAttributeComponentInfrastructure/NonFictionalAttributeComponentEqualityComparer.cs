using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, bool>
    {
        public override bool Accept(NonFictionalAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return false;
        }

        public override bool Accept(NonFictionalAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return first.EqualsTo(second);
        }

        public override bool Accept(NonFictionalAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return false;
        }
    }
}
