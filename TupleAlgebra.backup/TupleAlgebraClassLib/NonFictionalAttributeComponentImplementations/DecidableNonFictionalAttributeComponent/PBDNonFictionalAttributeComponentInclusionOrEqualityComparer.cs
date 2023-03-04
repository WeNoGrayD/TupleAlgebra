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
    public class PredicateBasedDecidableNonFictionalAttributeComponentInclusionOrEqualityComparer<TData>
        : InstantBinaryAttributeComponentAcceptor<TData, bool>,
          IInstantAttributeComponentAcceptor<TData, PredicateBasedDecidableNonFictionalAttributeComponent<TData>, PredicateBasedDecidableNonFictionalAttributeComponent<TData>, bool>
    {
        public bool Accept(
            PredicateBasedDecidableNonFictionalAttributeComponent<TData> greater,
            PredicateBasedDecidableNonFictionalAttributeComponent<TData> lesser)
        {
            return false;
        }
    }
}
