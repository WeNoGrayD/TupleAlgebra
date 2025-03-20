using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    public sealed class EqualityComparer<TData>
        : NonFictionalAttributeComponentEqualityComparer<
            TData,
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>>,
          IInstantBinaryAttributeComponentVisitor<
              TData, 
              UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
              IFiniteEnumerableAttributeComponent<TData>,
              bool>,
          IFiniteEnumerableXFilteringEqualityComparer<TData>
    {
        public bool Visit(
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> first, 
            IFiniteEnumerableAttributeComponent<TData> second)
        {
            return first.Values.SetEquals(second.Values);
        }
    }
}
