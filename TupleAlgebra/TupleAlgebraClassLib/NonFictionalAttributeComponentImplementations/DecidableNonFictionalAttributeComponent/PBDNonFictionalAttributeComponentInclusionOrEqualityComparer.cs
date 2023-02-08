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
    public class PredicateBasedDecidableNonFictionalAttributeComponentInclusionOrEqualityComparer<TValue>
        : InstantBinaryAttributeComponentAcceptor<TValue, bool>,
          IInstantAttributeComponentAcceptor<TValue, PredicateBasedDecidableNonFictionalAttributeComponent<TValue>, PredicateBasedDecidableNonFictionalAttributeComponent<TValue>, bool>
    {
        public bool Accept(
            PredicateBasedDecidableNonFictionalAttributeComponent<TValue> greater,
            PredicateBasedDecidableNonFictionalAttributeComponent<TValue> lesser)
        {
            return false;
        }
    }
}
