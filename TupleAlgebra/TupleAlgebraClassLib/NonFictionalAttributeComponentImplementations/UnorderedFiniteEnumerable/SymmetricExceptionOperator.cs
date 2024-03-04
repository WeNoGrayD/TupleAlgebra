using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    public sealed class SymmetricExceptionOperator<TData>
        : NonFictionalAttributeComponentSymmetricExceptionOperator<TData, UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>, IUnorderedFiniteEnumerableAttributeComponentFactory<TData>, UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IUnorderedFiniteEnumerableAttributeComponentBinaryOperator<TData>
    {
        public AttributeComponent<TData> Accept(
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> first,
            IFiniteEnumerableAttributeComponent<TData> second,
            IUnorderedFiniteEnumerableAttributeComponentFactory<TData> factory)
        {
            HashSet<TData> remainedElements = first.Values.ToHashSet(first.Values.Comparer);
            /* 
             * Если параметр second является коллекцией HashSet<T> с тем же EqualityComparer, 
             * что и текущий HashSet<T> объект, этот метод является операцией O(n). 
             * В противном случае этот метод является операцией O(n + m), где 
             * n количество элементов в other и m равно .Count*/
            remainedElements.SymmetricExceptWith(second.Values);

            return factory.CreateNonFictional(first, remainedElements);
        }
    }
}
