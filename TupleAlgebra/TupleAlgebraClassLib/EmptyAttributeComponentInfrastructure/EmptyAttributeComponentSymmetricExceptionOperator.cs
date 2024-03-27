using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    public sealed class EmptyAttributeComponentSymmetricExceptionOperator<TData>
        : FictionalAttributeComponentCrossTypeInstantBinaryAcceptor<
            TData, 
            EmptyAttributeComponent<TData>, 
            IAttributeComponent<TData>>
    {
        public override IAttributeComponent<TData> Accept(
            EmptyAttributeComponent<TData> first,
            EmptyAttributeComponent<TData> second)
        {
            return first;
        }

        public override IAttributeComponent<TData> Accept(
            EmptyAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second)
        {
            return second;
        }

        public override IAttributeComponent<TData> Accept(
            EmptyAttributeComponent<TData> first,
            FullAttributeComponent<TData> second)
        {
            return second;
        }
    }
}
