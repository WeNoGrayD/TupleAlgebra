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
    public class PredicateBasedDecidableNonFictionalAttributeComponentIntersectionOperator<TData>
        : FactoryBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>>,
          IFactoryAttributeComponentAcceptor<TData, PredicateBasedDecidableNonFictionalAttributeComponent<TData>, PredicateBasedDecidableNonFictionalAttributeComponent<TData>, AttributeComponent<TData>>
    {
        public AttributeComponent<TData> Accept(
            PredicateBasedDecidableNonFictionalAttributeComponent<TData> first,
            PredicateBasedDecidableNonFictionalAttributeComponent<TData> second,
            AttributeComponentFactory<TData> factory)
        {
            IEnumerable<Expression<Predicate<TData>>> exprs1 = first.Rules.Cast<Expression<Predicate<TData>>>(),
                                                       exprs2 = second.Rules.Cast<Expression<Predicate<TData>>>();

            return null;
        }
    }
}
