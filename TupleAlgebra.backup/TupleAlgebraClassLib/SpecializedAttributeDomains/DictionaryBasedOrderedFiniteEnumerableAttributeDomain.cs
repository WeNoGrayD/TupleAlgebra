using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public class DictionaryBasedOrderedFiniteEnumerableAttributeDomain<TKey, TData>
        : OrderedFiniteEnumerableAttributeDomain<KeyValuePair<TKey, TData>>
        where TKey : IComparable<TKey>
    {
        public DictionaryBasedOrderedFiniteEnumerableAttributeDomain(IDictionary<TKey, TData> universum)
            : base(universum)
        { }
    }
}
