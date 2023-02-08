using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable
{
    public class LookupBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TValue>
        : OrderedFiniteEnumerableNonFictionalAttributeComponent<IGrouping<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        public LookupBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            LookupBasedOrderedFiniteEnumerableAttributeDomain<TKey, TValue> domain,
            ILookup<TKey, TValue> values)
            : base(domain, values)
        { }

        protected override IComparer<IGrouping<TKey, TValue>> InitOrderingComparerImpl()
        {
            return new KeyValuePairComparer();
        }

        private class KeyValuePairComparer : IComparer<IGrouping<TKey, TValue>>
        {
            public int Compare(IGrouping<TKey, TValue> first, IGrouping<TKey, TValue> second)
            {
                return first.Key.CompareTo(second.Key);
            }
        }
    }
}
