using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    public sealed class FullAttributeComponentExceptionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, AttributeComponent<TValue>>
    {
        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
        {
            return first;
        }

        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
        {
            return !second;
        }

        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
        {
            return EmptyAttributeComponent<TValue>.Instance;
        }
    }
}
