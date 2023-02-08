using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentComplementionOperator<TValue>
        : InstantUnaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>,
          IInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>
    {
        public AttributeComponent<TValue> Accept(NonFictionalAttributeComponent<TValue> first)
        {
            return first.Domain ^ first;
        }
    }
}
