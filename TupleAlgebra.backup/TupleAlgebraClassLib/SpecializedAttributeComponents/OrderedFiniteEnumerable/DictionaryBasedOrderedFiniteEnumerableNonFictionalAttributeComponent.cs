using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable
{
    public class DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TData>
        : OrderedFiniteEnumerableNonFictionalAttributeComponent<KeyValuePair<TKey, TData>>
        where TKey : IComparable<TKey>
    {
        public DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            DictionaryBasedOrderedFiniteEnumerableAttributeDomain<TKey, TData> domain,
            IDictionary<TKey, TData> values)
            : base(domain, values)
        { }

        protected override IComparer<KeyValuePair<TKey, TData>> InitOrderingComparerImpl()
        {
            return new KeyValuePairComparer();
        }

        private class KeyValuePairComparer : IComparer<KeyValuePair<TKey, TData>>
        {
            public int Compare(KeyValuePair<TKey, TData> first, KeyValuePair<TKey, TData> second)
            {
                return first.Key.CompareTo(second.Key);
            }
        }
    }
}
