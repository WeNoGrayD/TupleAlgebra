using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable
{
    public class EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TData>
        : OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>
        where TData : struct, IComparable
    {
        public EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            EnumBasedOrderedFiniteEnumerableAttributeDomain<TData> domain,
            IEnumerable<TData> values)
            : base(domain, values)
        { }

        protected override IComparer<TData> InitOrderingComparerImpl()
        {
            return new EnumComparer();
        }

        private class EnumComparer : IComparer<TData>
        {
            public int Compare(TData first, TData second)
            {
                return first.CompareTo(second);
            }
        }
    }
}
