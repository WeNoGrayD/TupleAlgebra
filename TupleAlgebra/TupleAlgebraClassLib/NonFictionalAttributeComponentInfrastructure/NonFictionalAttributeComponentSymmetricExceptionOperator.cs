using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentSymmetricExceptionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>
    {
        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
        {
            return first;
        }

        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
        {
            return first.SymmetricExceptWith(second);
        }

        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
        {
            return !first;
        }
    }
}
