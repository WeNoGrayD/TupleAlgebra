using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public class LookupBasedOrderedFiniteEnumerableAttributeDomain<TKey, TValue>
        : OrderedFiniteEnumerableAttributeDomain<IGrouping<TKey, TValue>>
        where TKey : IComparable<TKey>
    {
        public LookupBasedOrderedFiniteEnumerableAttributeDomain(ILookup<TKey, TValue> universum)
            : base(universum)
        { }
    }
}
