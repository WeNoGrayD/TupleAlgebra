using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public sealed class PredicateBasedDecidableNonFictionalAttributeComponentExceptionOperator<TData>
        : FactoryBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>>,
          IFactoryBinaryAttributeComponentAcceptor<TData, PredicateBasedDecidableNonFictionalAttributeComponent<TData>, PredicateBasedDecidableNonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public AttributeComponent<TData> Accept(
            PredicateBasedDecidableNonFictionalAttributeComponent<TData> first,
            PredicateBasedDecidableNonFictionalAttributeComponent<TData> second,
            AttributeComponentFactory factory)
        {
            return null;
        }
    }
}
