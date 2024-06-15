using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    using static AttributeComponentHelper;

    public sealed class FullAttributeComponentComplementationOperator<TData>
        : AttributeComponentFactoryUnarySetOperator<
            TData,
            FullAttributeComponent<TData>>
    {
        public override IAttributeComponent<TData> Visit(
            FullAttributeComponent<TData> first,
            IAttributeComponentFactory<TData> factory)
        {
            return factory.CreateEmpty();
        }
    }
}
