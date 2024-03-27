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
    using static AttributeComponentHelper;

    public sealed class FullAttributeComponentExceptionOperator<TData>
        : FictionalAttributeComponentCrossTypeInstantBinaryAcceptor<
            TData, 
            FullAttributeComponent<TData>, 
            IAttributeComponent<TData>>
    {
        public override IAttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second)
        {
            return first;
        }

        public override IAttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second)
        {
            return ~second;
        }

        public override IAttributeComponent<TData> Accept(
            FullAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return first.Factory.CreateEmpty();
        }
    }
}
