using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public abstract class NonFictionalAttributeComponentInclusionOrEqualityComparer<TData, CTOperand1>
        : InstantBinaryAttributeComponentAcceptor<TData, CTOperand1, bool>,
          IInstantBinaryAttributeComponentAcceptor<TData, NonFictionalAttributeComponent<TData>, EmptyAttributeComponent<TData>, bool>,
          IInstantBinaryAttributeComponentAcceptor<TData, CTOperand1, NonFictionalAttributeComponent<TData>, bool>,
          IInstantBinaryAttributeComponentAcceptor<TData, NonFictionalAttributeComponent<TData>, FullAttributeComponent<TData>, bool>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
    {
        public bool Accept(NonFictionalAttributeComponent<TData> first, EmptyAttributeComponent<TData> second)
        {
            return true;
        }

        public bool Accept(CTOperand1 first, NonFictionalAttributeComponent<TData> second)
        {
            throw new NotImplementedException();
        }

        public bool Accept(NonFictionalAttributeComponent<TData> first, FullAttributeComponent<TData> second)
        {
            return false;
        }
    }
}
