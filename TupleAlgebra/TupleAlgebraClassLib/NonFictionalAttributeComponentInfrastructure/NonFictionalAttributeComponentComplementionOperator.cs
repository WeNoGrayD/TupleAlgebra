using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentComplementionOperator<TData, CTOperand>
        : FactoryUnaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>>,
          IFactoryUnaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>>
        where CTOperand: NonFictionalAttributeComponent<TData>
    {
        public override AttributeComponent<TData> Accept(CTOperand first, AttributeComponentFactory factory)
        {
            return first.Domain ^ first;
        }
    }
}
