using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public sealed class NonFictionalAttributeComponentInclusionComparer<TData>
        : CrossContentTypesInstantAttributeComponentAcceptor<TData, NonFictionalAttributeComponent<TData>, bool>
    {
        public override bool Accept(NonFictionalAttributeComponent<TData> first, EmptyAttributeComponent<TData> second)
        {
            return true;
        }

        public override bool Accept(NonFictionalAttributeComponent<TData> first, NonFictionalAttributeComponent<TData> second)
        {
            return first.Includes(second);
        }

        public override bool Accept(NonFictionalAttributeComponent<TData> first, FullAttributeComponent<TData> second)
        {
            return false;
        }
    }
}
