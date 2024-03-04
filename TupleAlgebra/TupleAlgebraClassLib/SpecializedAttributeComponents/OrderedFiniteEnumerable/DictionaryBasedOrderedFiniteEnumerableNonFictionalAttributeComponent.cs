using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.SpecializedAttributeComponents.Factories;

namespace TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable
{
    using static AttributeComponentHelper;

    /// <summary>
    /// Упорядоченная конечная перечислимая компонента атрибута, основанная на контейнере данных, 
    /// который поддерживает интерфейс IDIctionary<TKey, TData>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TData>
        : BufferedOrderedFiniteEnumerableAttributeComponent<KeyValuePair<TKey, TData>>
        where TKey : IComparable<TKey>
    {
        #region Constructors

        static DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent()
        {
            Helper.RegisterType<
                KeyValuePair<TKey, TData>,
                DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TData>>(
                    acFactory: (domain) => new DictionaryBasedAttributeComponentFactory<TKey, TData>(domain));

            return;
        }
        public DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            AttributeComponentPower power,
            IEnumerable<KeyValuePair<TKey, TData>> values,
            IComparer<KeyValuePair<TKey, TData>> orderingComparer = null,
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

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="power"></param>
        /// <param name="values"></param>
        /// <param name="orderingComparer"></param>
        /// <param name="valuesAreOrdered"></param>
        /// <param name="queryProvider"></param>
        /// <param name="queryExpression"></param>
        public DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            AttributeComponentPower power,
            IDictionary<TKey, TData> values,
            IComparer<KeyValuePair<TKey, TData>> orderingComparer = null,
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

        #endregion

        #region Instance methods

        /// <summary>
        /// Инициализация компаратора для упорядочения внутренних данных.
        /// </summary>
        /// <returns></returns>
        protected override IComparer<KeyValuePair<TKey, TData>> InitOrderingComparerImpl()
        {
            return new KeyValuePairComparer();
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Класс компаратора пар "ключ-значение". Поддерживает сравнение по ключу.
        /// </summary>
        private class KeyValuePairComparer : IComparer<KeyValuePair<TKey, TData>>
        {
            public int Compare(KeyValuePair<TKey, TData> first, KeyValuePair<TKey, TData> second)
            {
                return first.Key.CompareTo(second.Key);
            }
        }

        #endregion
    }

    internal class DictionaryBasedAttributeComponentFactory<TKey, TData>
        : AttributeComponentWithCompoundDataFactory<KeyValuePair<TKey, TData>, OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
            where TKey : IComparable<TKey>
    {
        public DictionaryBasedAttributeComponentFactory(
            AttributeDomain<KeyValuePair<TKey, TData>> domain)
            : base(nameof(CreateDictionaryBased))
        {
            return;
        }

        private NonFictionalAttributeComponent<KeyValuePair<TKey, TData>> CreateDictionaryBased(
            OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData> args)
        {
            return new DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TData>(
                args.Power,
                args.Values as IEnumerable<KeyValuePair<TKey, TData>>,
                args.OrderingComparer as IComparer<KeyValuePair<TKey, TData>>,
                args.ValuesAreOrdered,
                args.QueryProvider,
                args.QueryExpression);
        }
    }
}
