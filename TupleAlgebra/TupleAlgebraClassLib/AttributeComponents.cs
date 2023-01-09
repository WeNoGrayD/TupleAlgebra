using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib
{
    /// <summary>
    /// Абстрактный тип компоненты атрибута.
    /// Определяет свойство и тип мощности компоненты, 
    /// свойство и тип перечисления содержимого компоненты (пуста, непуста, полна).
    /// <para>Также является некоторого рода интерфейсом-маркером необобщённого типа компоненты атрибута.</para>
    /// </summary>
    public abstract class AttributeComponent
    {
        protected abstract AttributeComponentContentType ContentType { get; }

        public readonly AttributeComponentPower Power;

        #region Constructors

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="power"></param>
        public AttributeComponent(AttributeComponentPower power)
        {
            Power = power;
        }

        #endregion

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

    /// <summary>
    /// Параметризированная компонента атрибута.
    /// Определяет операторы и методы для операций пересечения, объединения,
    /// дополнения, сравнения на равенство и включения.
    /// </summary>
    /// <typeparam name="TValue">Тип значения, которое может принимать атрибут.</typeparam>
    public abstract class AttributeComponent<TValue> : AttributeComponent, IEnumerable<TValue>
        where TValue : IComparable<TValue>
    {
        private static Dictionary<AttributeComponentContentType, SetOperationExecutersContainer> _setOperations;

        static AttributeComponent()
        {
            _setOperations = new Dictionary<AttributeComponentContentType, SetOperationExecutersContainer>();
        }

        public AttributeComponent(AttributeComponentPower power) 
            : base(power)
        { }

        protected static void InitSetOperations(
            AttributeComponentContentType contentType,
            SetOperationExecutersContainer setOperations)
        {
            _setOperations[contentType] = setOperations;
        }

        protected AttributeComponent<TValue> IntersectWith(AttributeComponent<TValue> second)
        {
            return _setOperations[ContentType].Intersect(this, second) as AttributeComponent<TValue>;
        }

        protected AttributeComponent<TValue> UnionWith(AttributeComponent<TValue> second)
        {
            return _setOperations[ContentType].Union(this, second) as AttributeComponent<TValue>;
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

        public abstract IEnumerator<TValue> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
    }
}
