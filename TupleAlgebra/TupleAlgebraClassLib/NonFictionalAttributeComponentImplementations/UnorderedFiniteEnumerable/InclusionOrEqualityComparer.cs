using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    public class InclusionOrEqualityComparer<TData>
        : NonFictionalAttributeComponentInclusionComparer<TData, UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>>,
          IInstantBinaryAttributeComponentAcceptor<TData, UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>, IFiniteEnumerableAttributeComponent<TData>, bool>
    {
        public bool Accept(
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> greater,
            IFiniteEnumerableAttributeComponent<TData> lesser)
        {
            return greater.Values.IsSupersetOf(lesser.Values);
        }
    }
}
