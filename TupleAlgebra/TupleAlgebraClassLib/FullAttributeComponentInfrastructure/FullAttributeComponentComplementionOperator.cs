using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    public sealed class FullAttributeComponentComplementionOperator<TData>
        : InstantUnaryAttributeComponentAcceptor<TData, FullAttributeComponent<TData>, AttributeComponent<TData>>,
          IInstantUnaryAttributeComponentAcceptor<TData, FullAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public override AttributeComponent<TData> Accept(FullAttributeComponent<TData> first)
        {
            return AttributeComponent<TData>.FictionalAttributeComponentFactory.CreateEmpty<TData>(first.ZipInfo<TData>(null));
        }
    }
}
