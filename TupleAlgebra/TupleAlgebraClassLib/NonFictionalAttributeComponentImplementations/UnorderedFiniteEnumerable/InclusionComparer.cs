using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    public sealed class InclusionComparer<TData>
        : NonFictionalAttributeComponentInclusionComparer<
            TData, 
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>>,
          IInstantBinaryAttributeComponentAcceptor<
              TData, 
              UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
              IFiniteEnumerableAttributeComponent<TData>, 
              bool>,
          IFiniteEnumerableXFilteringInclusionComparer<TData>
    {
        public bool Accept(UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> greater,
                           IFiniteEnumerableAttributeComponent<TData> lesser)
        {
            return greater.Values.IsProperSupersetOf(lesser.Values);
        }
    }
}
