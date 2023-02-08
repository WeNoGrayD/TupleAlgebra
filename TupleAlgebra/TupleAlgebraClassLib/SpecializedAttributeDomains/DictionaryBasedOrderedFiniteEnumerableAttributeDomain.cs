using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public class DictionaryBasedOrderedFiniteEnumerableAttributeDomain<TKey, TValue>
        : OrderedFiniteEnumerableAttributeDomain<KeyValuePair<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        public DictionaryBasedOrderedFiniteEnumerableAttributeDomain(IDictionary<TKey, TValue> universum)
            : base(universum)
        { }
    }
}
