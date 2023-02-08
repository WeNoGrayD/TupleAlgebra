using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    public sealed class EmptyAttributeComponentComplementionOperator<TValue>
        : InstantUnaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>,
          IInstantAttributeComponentAcceptor<TValue, EmptyAttributeComponent<TValue>, AttributeComponent<TValue>>
    {
        public AttributeComponent<TValue> Accept(EmptyAttributeComponent<TValue> first)
        {
            return FullAttributeComponent<TValue>.Instance;
        }
    }
}
