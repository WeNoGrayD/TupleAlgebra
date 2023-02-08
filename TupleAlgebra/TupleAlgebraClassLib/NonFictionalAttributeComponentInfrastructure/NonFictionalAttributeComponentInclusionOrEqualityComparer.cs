using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentInclusionOrEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, bool>
    {
        public override bool Accept(NonFictionalAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return true;
        }

        public override bool Accept(NonFictionalAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return first.IncludesOrEqualsTo(second);
        }

        public override bool Accept(NonFictionalAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return false;
        }
    }
}
