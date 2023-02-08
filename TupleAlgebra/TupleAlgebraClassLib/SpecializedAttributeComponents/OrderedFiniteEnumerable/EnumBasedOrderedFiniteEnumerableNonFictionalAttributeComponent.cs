using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable
{
    public class EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>
        : OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>
        where TValue : struct, IComparable
    {
        public EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            EnumBasedOrderedFiniteEnumerableAttributeDomain<TValue> domain,
            IEnumerable<TValue> values)
            : base(domain, values)
        { }

        protected override IComparer<TValue> InitOrderingComparerImpl()
        {
            return new EnumComparer();
        }

        private class EnumComparer : IComparer<TValue>
        {
            public int Compare(TValue first, TValue second)
            {
                return first.CompareTo(second);
            }
        }
    }
}
