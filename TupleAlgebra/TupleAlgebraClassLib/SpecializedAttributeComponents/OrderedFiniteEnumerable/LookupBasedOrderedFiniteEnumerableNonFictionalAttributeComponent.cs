using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.SpecializedAttributeComponents.Factories;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable
{
    using static AttributeComponentHelper;

    public class LookupBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TData>
        : BufferedOrderedFiniteEnumerableAttributeComponent<IGrouping<TKey, TData>>
        where TKey : IComparable<TKey>
    {
        static LookupBasedOrderedFiniteEnumerableNonFictionalAttributeComponent()
        {
            /*
            Helper.RegisterType<
                IGrouping<TKey, TData>, 
                LookupBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TData>>(
                    acFactory: (domain) => new LookupBasedAttributeComponentFactory<TKey, TData>(domain));
            */

            return;
        }

        public LookupBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            AttributeComponentPower power,
            IEnumerable<IGrouping<TKey, TData>> values,
            IComparer<IGrouping<TKey, TData>> orderingComparer = null,
            bool valuesAreOrdered = false,
            IQueryProvider queryProvider = null,
            System.Linq.Expressions.Expression queryExpression = null)
            : base(
                  power,
                  values,
                  orderingComparer,
                  valuesAreOrdered,
                  queryProvider,
                  queryExpression)
        {
            return;
        }

        public LookupBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            AttributeComponentPower power,
            ILookup<TKey, TData> values,
            IComparer<IGrouping<TKey, TData>> orderingComparer = null,
            bool valuesAreOrdered = false,
            IQueryProvider queryProvider = null,
            System.Linq.Expressions.Expression queryExpression = null)
            : base(
                  power,
                  values,
                  orderingComparer,
                  valuesAreOrdered,
                  queryProvider,
                  queryExpression)
        {
            return;
        }

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

    /*
    internal class LookupBasedAttributeComponentFactory<TKey, TData>
        : AttributeComponentWithCompoundDataFactory<IGrouping<TKey, TData>, OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
            where TKey : IComparable<TKey>
    {
        public LookupBasedAttributeComponentFactory(
            AttributeDomain<IGrouping<TKey, TData>> domain)
            : base(nameof(CreateLookupBased))
        {
            return;
        }

        private NonFictionalAttributeComponent<IGrouping<TKey, TData>> CreateLookupBased(
            OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData> args)
        {
            return new LookupBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TData>(
                args.Power,
                args.Values as IEnumerable<IGrouping<TKey, TData>>,
                args.OrderingComparer as IComparer<IGrouping<TKey, TData>>,
                args.ValuesAreOrdered,
                args.QueryProvider,
                args.QueryExpression);
        }
    }
    */
}
