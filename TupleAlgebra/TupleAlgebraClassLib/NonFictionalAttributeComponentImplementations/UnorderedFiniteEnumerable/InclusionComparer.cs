using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    public sealed class InclusionComparer<TData>
        : NonFictionalAttributeComponentInclusionComparer<
            TData, 
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>>,
          IInstantBinaryAttributeComponentVisitor<
              TData, 
              UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
              IFiniteEnumerableAttributeComponent<TData>, 
              bool>,
          IFiniteEnumerableXFilteringInclusionComparer<TData>
    {
        public bool Visit(UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> greater,
                           IFiniteEnumerableAttributeComponent<TData> lesser)
        {
            return greater.Values.IsProperSupersetOf(lesser.Values);
        }
    }
}
