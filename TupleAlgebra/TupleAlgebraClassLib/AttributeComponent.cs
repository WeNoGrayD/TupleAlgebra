using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using System.Linq.Expressions;

namespace TupleAlgebraClassLib
{
    /// <summary>
    /// Параметризированная компонента атрибута.
    /// Определяет операторы и методы для операций пересечения, объединения,
    /// дополнения, сравнения на равенство и включения.
    /// </summary>
    /// <typeparam name="TValue">Тип значения, которое может принимать атрибут.</typeparam>
    public abstract class AttributeComponent<TValue> : IEnumerable, IEnumerable<TValue>, IQueryable<TValue>
    {
        public virtual Expression Expression { get; protected set; }

        public virtual Type ElementType { get => typeof(TValue); }

        public virtual IQueryProvider Provider { get; protected set; }

        protected abstract AttributeComponentContentType ContentType { get; }

        public readonly AttributeComponentPower Power;

        private static Dictionary<AttributeComponentContentType, InstantSetOperationExecutersContainer<TValue>> _setOperations;

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static AttributeComponent()
        {
            _setOperations = new Dictionary<AttributeComponentContentType, InstantSetOperationExecutersContainer<TValue>>();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="power"></param>
        public AttributeComponent(AttributeComponentPower power)
        {
            Power = power;
        }

        #endregion

        protected static void InitSetOperations(
            AttributeComponentContentType contentType,
            InstantSetOperationExecutersContainer<TValue> setOperations)
        {
            _setOperations[contentType] = setOperations;
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            return Provider.Execute<AttributeComponent<TValue>>(Expression).GetEnumeratorImpl();
        }

        public abstract IEnumerator<TValue> GetEnumeratorImpl();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected AttributeComponent<TValue> ComplementThe()
        {
            return _setOperations[ContentType].Complement(this);
        }

        protected AttributeComponent<TValue> IntersectWith(AttributeComponent<TValue> second)
        {
            return _setOperations[ContentType].Intersect(this, second);
        }

        protected AttributeComponent<TValue> UnionWith(AttributeComponent<TValue> second)
        {
            return _setOperations[ContentType].Union(this, second);
        }

        protected AttributeComponent<TValue> ExceptWith(AttributeComponent<TValue> second)
        {
            return _setOperations[ContentType].Except(this, second);
        }

        protected AttributeComponent<TValue> SymmetricExceptWith(AttributeComponent<TValue> second)
        {
            return _setOperations[ContentType].SymmetricExcept(this, second);
        }

        protected bool Includes(AttributeComponent<TValue> second)
        {
            return _setOperations[ContentType].Include(this, second);
        }

        protected bool EqualsTo(AttributeComponent<TValue> second)
        {
            return _setOperations[ContentType].Equal(this, second);
        }

        protected bool IncludesOrEqualsTo(AttributeComponent<TValue> second)
        {
            return _setOperations[ContentType].IncludeOrEqual(this, second);
        }

        public static AttributeComponent<TValue> operator !(AttributeComponent<TValue> first)
        {
            return first.ComplementThe();
        }

        public static AttributeComponent<TValue> operator &(
            AttributeComponent<TValue> first,
            AttributeComponent<TValue> second)
        {
            return first.IntersectWith(second);
        }

        public static AttributeComponent<TValue> operator |(
            AttributeComponent<TValue> first,
            AttributeComponent<TValue> second)
        {
            return first.UnionWith(second);
        }

        public static AttributeComponent<TValue> operator /(
            AttributeComponent<TValue> first,
            AttributeComponent<TValue> second)
        {
            return first.ExceptWith(second);
        }

        public static AttributeComponent<TValue> operator ^(
            AttributeComponent<TValue> first,
            AttributeComponent<TValue> second)
        {
            return first.SymmetricExceptWith(second);
        }

        public static bool operator ==(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return first.EqualsTo(second);
        }

        public static bool operator !=(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return !(first == second);
        }

        public static bool operator >(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return first.Includes(second);
        }

        public static bool operator >=(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return first.IncludesOrEqualsTo(second);
        }

        public static bool operator <(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return second.Includes(first);
        }

        public static bool operator <=(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return second.IncludesOrEqualsTo(first);
        }

        #region InnerTypes

        /// <summary>
        /// Перечисление типов содержимого компоненты атрибута.
        /// </summary>
        protected internal enum AttributeComponentContentType : byte
        {
            /// <summary>
            /// Пустая фиктивная компонента.
            /// </summary>
            Empty = 0,
            /// <summary>
            /// Непустая компонента.
            /// </summary>
            NonFictional = 1,
            /// <summary>
            /// Полная фиктивная компонента.
            /// </summary>
            Full = 2
        }

        /// <summary>
        /// Абстрактный тип мощности компоненты атрибута.
        /// </summary>
        public abstract class AttributeComponentPower : IComparable<AttributeComponentPower>
        {
            internal abstract AttributeComponentContentType ContentType { get; }

            public virtual int CompareTo(AttributeComponentPower second)
            {
                return this.ContentType.CompareTo(second.ContentType);
            }

            #region Operators

            /// <summary>
            /// Оператор сравнения на равенство двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator ==(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) == 0;
            }

            /// <summary>
            /// Оператор сравнения на неравенство двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator !=(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) != 0;
            }

            /// <summary>
            /// Оператор сравнения "больше" двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator >(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) == 1;
            }

            /// <summary>
            /// Оператор сравнения "больше или равно" двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator >=(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) > -1;
            }

            /// <summary>
            /// Оператор сравнения "меньше" двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator <(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) == -1;
            }

            /// <summary>
            /// Оператор сравнения "меньше или равно" двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator <=(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) < 1;
            }

            #endregion
        }

        #endregion
    }
}
