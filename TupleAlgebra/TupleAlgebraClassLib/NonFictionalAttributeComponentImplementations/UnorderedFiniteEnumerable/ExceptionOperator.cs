using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    public sealed class ExceptionOperator<TData>
        : NonFictionalAttributeComponentExceptionOperator<
            TData,
            IEnumerable<TData>,
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
            IUnorderedFiniteEnumerableAttributeComponentFactory<TData>, 
            UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IUnorderedFiniteEnumerableAttributeComponentBinaryOperator<TData>,
          IFiniteEnumerableXFilteringExceptionOperator<
              TData,
              UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
              IUnorderedFiniteEnumerableAttributeComponentFactory<TData>,
              UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    {
        public IAttributeComponent<TData> Accept(
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> first,
            IFiniteEnumerableAttributeComponent<TData> second,
            IUnorderedFiniteEnumerableAttributeComponentFactory<TData> factory)
        {
            HashSet<TData> remainedElements = first.Values.Except(second.Values).ToHashSet();

            return factory.CreateNonFictional(first, remainedElements);
        }
    }
}
