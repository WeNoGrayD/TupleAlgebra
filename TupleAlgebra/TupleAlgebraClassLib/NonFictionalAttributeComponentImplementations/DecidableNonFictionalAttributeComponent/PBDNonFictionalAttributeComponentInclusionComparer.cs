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
    public sealed class PredicateBasedDecidableNonFictionalAttributeComponentInclusionComparer<TData>
        : InstantBinaryAttributeComponentAcceptor<TData, bool>,
          IInstantBinaryAttributeComponentAcceptor<TData, PredicateBasedDecidableNonFictionalAttributeComponent<TData>, PredicateBasedDecidableNonFictionalAttributeComponent<TData>, bool>
    {
        public bool Accept(PredicateBasedDecidableNonFictionalAttributeComponent<TData> greater,
                           PredicateBasedDecidableNonFictionalAttributeComponent<TData> lesser)
        {
            return false;
        }
    }
}
