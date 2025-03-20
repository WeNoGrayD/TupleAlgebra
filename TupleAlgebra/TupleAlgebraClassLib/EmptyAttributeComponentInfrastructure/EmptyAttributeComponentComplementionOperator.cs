using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    using static AttributeComponentHelper;

    public sealed class EmptyAttributeComponentComplementationOperator<TData>
        : AttributeComponentFactoryUnarySetOperator<
            TData, 
            EmptyAttributeComponent<TData>>
    {
        public override IAttributeComponent<TData> Visit(
            EmptyAttributeComponent<TData> first,
            IAttributeComponentFactory<TData> factory)
        {
            return factory.CreateFull();
        }
    }
}
