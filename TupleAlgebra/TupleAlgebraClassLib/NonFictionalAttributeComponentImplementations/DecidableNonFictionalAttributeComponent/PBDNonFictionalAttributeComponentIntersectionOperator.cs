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
    public class PredicateBasedDecidableNonFictionalAttributeComponentIntersectionOperator<TValue>
        : FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>,
          IFactoryAttributeComponentAcceptor<TValue, PredicateBasedDecidableNonFictionalAttributeComponent<TValue>, PredicateBasedDecidableNonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>
    {
        public AttributeComponent<TValue> Accept(
            PredicateBasedDecidableNonFictionalAttributeComponent<TValue> first,
            PredicateBasedDecidableNonFictionalAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            IEnumerable<Expression<Predicate<TValue>>> exprs1 = first.Rules.Cast<Expression<Predicate<TValue>>>(),
                                                       exprs2 = second.Rules.Cast<Expression<Predicate<TValue>>>();

            return null;
        }
    }
}
