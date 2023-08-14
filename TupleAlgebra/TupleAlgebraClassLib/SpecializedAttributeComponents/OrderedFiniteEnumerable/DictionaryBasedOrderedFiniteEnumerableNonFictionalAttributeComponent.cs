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
    /// Упорядоченная конечная перечислимая компонента атрибута, основанная на контейнере данных, 
    /// который поддерживает интерфейс IDIctionary<TKey, TData>.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TData"></typeparam>
    public class DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<TKey, TData>
        : OrderedFiniteEnumerableNonFictionalAttributeComponent<KeyValuePair<TKey, TData>>
        where TKey : IComparable<TKey>
    {
        #region Constructors

        /// <summary>
        /// труктор экземпляра.
        /// </summary>
        public DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent() : base(null, null)
        { 
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="values"></param>
        public DictionaryBasedOrderedFiniteEnumerableNonFictionalAttributeComponent(
            IDictionary<TKey, TData> values)
            : base(null, values)
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
}
