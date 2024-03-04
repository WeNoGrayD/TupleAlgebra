using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    public sealed class FullAttributeComponentInclusionComparer<TData>
        : FictionalAttributeComponentCrossTypeInstantBinaryAcceptor<TData, FullAttributeComponent<TData>, bool>
    {
        public override bool Accept(FullAttributeComponent<TData> first, EmptyAttributeComponent<TData> second)
        {
            return true;
        }

        public override bool Accept(FullAttributeComponent<TData> first, NonFictionalAttributeComponent<TData> second)
        {
            return true;
        }

        public override bool Accept(FullAttributeComponent<TData> first, FullAttributeComponent<TData> second)
        {
            return false;
        }
    }
}
