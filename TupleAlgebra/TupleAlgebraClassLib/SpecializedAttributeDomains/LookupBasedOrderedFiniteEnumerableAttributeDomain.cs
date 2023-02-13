using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public class LookupBasedOrderedFiniteEnumerableAttributeDomain<TKey, TData>
        : OrderedFiniteEnumerableAttributeDomain<IGrouping<TKey, TData>>
        where TKey : IComparable<TKey>
    {
        public LookupBasedOrderedFiniteEnumerableAttributeDomain(ILookup<TKey, TData> universum)
            : base(universum)
        { }
    }
}
