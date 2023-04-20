using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentComplementionOperator<TData>
        : InstantUnaryAttributeComponentAcceptor<TData, AttributeComponent<TData>>,
          IInstantUnaryAttributeComponentAcceptor<TData, NonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public AttributeComponent<TData> Accept(NonFictionalAttributeComponent<TData> first)
        {
            return first.Domain ^ first;
        }
    }
}
