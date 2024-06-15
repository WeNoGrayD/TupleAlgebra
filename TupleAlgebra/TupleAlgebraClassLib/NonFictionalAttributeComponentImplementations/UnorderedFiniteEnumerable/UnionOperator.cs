using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    public class UnionOperator<TData>
        : NonFictionalAttributeComponentUnionOperator<
            TData, 
            IEnumerable<TData>, 
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
            IUnorderedFiniteEnumerableAttributeComponentFactory<TData>,
            UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IUnorderedFiniteEnumerableAttributeComponentBinaryOperator<TData>,
          IFiniteEnumerableXFilteringUnionOperator<
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
            IEnumerable<TData> unitedElements = first.Values.Union(second.Values);

            return factory.CreateNonFictional(first, unitedElements);
        }
    }
}
