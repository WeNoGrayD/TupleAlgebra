using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    public sealed class EqualityComparer<TData>
        : NonFictionalAttributeComponentEqualityComparer<
            TData,
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>>,
          IInstantBinaryAttributeComponentAcceptor<
              TData, 
              UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
              IFiniteEnumerableAttributeComponent<TData>,
              bool>,
          IFiniteEnumerableXFilteringEqualityComparer<TData>
    {
        public bool Accept(
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> first, 
            IFiniteEnumerableAttributeComponent<TData> second)
        {
            return first.Values.SetEquals(second.Values);
        }
    }
}
