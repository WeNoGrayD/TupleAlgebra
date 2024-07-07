using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational
{
    public class IntersectionOperator<TKey, TData>
        : NonFictionalAttributeComponentIntersectionOperator<
            TData,
            IEnumerable<TData>,
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
            IUnorderedFiniteEnumerableAttributeComponentFactory<TData>,
            UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IUnorderedFiniteEnumerableAttributeComponentBinaryOperator<TData>,
          IFiniteEnumerableXFilteringIntersectionOperator<
              TData,
              UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
              IUnorderedFiniteEnumerableAttributeComponentFactory<TData>,
              UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    {
        public IAttributeComponent<TData> Visit(
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> first,
            IFiniteEnumerableAttributeComponent<TData> second,
            IUnorderedFiniteEnumerableAttributeComponentFactory<TData> factory)
        {
            HashSet<TData> intersectedElements = first.Values.Intersect(second.Values).ToHashSet();

            return factory.CreateNonFictional(first, intersectedElements);
        }
    }
}
