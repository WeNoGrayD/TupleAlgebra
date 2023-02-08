using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable
{
    public class DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TValue>
        : OrderedFiniteEnumerableNonFictionalAttributeComponent<KeyValuePair<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        public DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            DictionaryBasedOrderedFiniteEnumerableAttributeDomain<TKey, TValue> domain,
            IDictionary<TKey, TValue> values)
            : base(domain, values)
        { }

        protected override IComparer<KeyValuePair<TKey, TValue>> InitOrderingComparerImpl()
        {
            return new KeyValuePairComparer();
        }

        private class KeyValuePairComparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            public int Compare(KeyValuePair<TKey, TValue> first, KeyValuePair<TKey, TValue> second)
            {
                return first.Key.CompareTo(second.Key);
            }
        }
    }
}
