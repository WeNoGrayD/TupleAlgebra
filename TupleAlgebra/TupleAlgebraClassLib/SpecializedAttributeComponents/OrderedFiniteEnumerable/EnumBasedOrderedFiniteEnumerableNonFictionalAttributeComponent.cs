using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable
{
    /// <summary>
    /// Упорядоченная конечная перечислимая компонента атрибута, тип данных которого является перечислением в C# (Enum).
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TData>
        : OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>
        where TData : Enum
    {
        public EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent()
            : base()
        {
            return;
        }

        public EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            EnumBasedOrderedFiniteEnumerableAttributeDomain<TData> domain,
            IEnumerable<TData> values)
            : base(domain, values)
        {
            return;
        }

        /*
        protected override IComparer<TData> InitOrderingComparerImpl()
        {
            return Comparer<TData>.Default;// new EnumComparer();
        }

        private class EnumComparer : IComparer<TData>
        {
            public int Compare(TData first, TData second)
            {
                return first.CompareTo(second);
            }
        }
        */
    }
}
