using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    public sealed class EmptyAttributeComponentComplementionOperator<TData>
        : InstantUnaryAttributeComponentAcceptor<TData, AttributeComponent<TData>>,
          IInstantAttributeComponentAcceptor<TData, EmptyAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public AttributeComponent<TData> Accept(EmptyAttributeComponent<TData> first)
        {
            return FullAttributeComponent<TData>.FictionalAttributeComponentFactory.CreateFull
                (new AttributeComponentFactoryArgs<TData>(first.Domain));
        }
    }
}
