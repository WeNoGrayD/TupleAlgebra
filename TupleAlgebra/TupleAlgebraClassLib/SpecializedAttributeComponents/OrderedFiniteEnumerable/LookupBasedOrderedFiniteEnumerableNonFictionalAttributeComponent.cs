using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable
{
    public class LookupBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TData>
        : OrderedFiniteEnumerableNonFictionalAttributeComponent<IGrouping<TKey, TData>>
        where TKey : IComparable<TKey>
    {
        public LookupBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            LookupBasedOrderedFiniteEnumerableAttributeDomain<TKey, TData> domain,
            ILookup<TKey, TData> values)
            : base(domain, values)
        { }

        protected override IComparer<IGrouping<TKey, TData>> InitOrderingComparerImpl()
        {
            return new KeyValuePairComparer();
        }

        private class KeyValuePairComparer : IComparer<IGrouping<TKey, TData>>
        {
            public int Compare(IGrouping<TKey, TData> first, IGrouping<TKey, TData> second)
            {
                return first.Key.CompareTo(second.Key);
            }
        }
    }
}
