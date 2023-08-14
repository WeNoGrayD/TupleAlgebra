using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    using static AttributeComponentHelper;

    public sealed class EmptyAttributeComponentComplementionOperator<TData>
        : InstantUnaryAttributeComponentAcceptor<TData, EmptyAttributeComponent<TData>, AttributeComponent<TData>>,
          IInstantUnaryAttributeComponentAcceptor<TData, EmptyAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public override AttributeComponent<TData> Accept(EmptyAttributeComponent<TData> first)
        {
            return GetFactory(first).CreateFull<TData>(first.ZipInfo<TData>(null));
        }
    }
}
