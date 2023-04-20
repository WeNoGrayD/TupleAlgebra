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
          IInstantUnaryAttributeComponentAcceptor<TData, EmptyAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public AttributeComponent<TData> Accept(EmptyAttributeComponent<TData> first)
        {
            return AttributeComponent<TData>.FictionalAttributeComponentFactory.CreateFull<TData>(first.ZipInfo<TData>(null));
        }
    }
}
