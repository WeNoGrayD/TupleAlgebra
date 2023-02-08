using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure
{
    /// <summary>
    /// Упорядоченная конечная перечислимая нефиктивная компонента.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue> 
        : NonFictionalAttributeComponent<TValue>
    {
        #region Constants

        /// <summary>
        /// Константнтое значение строкового представления математического типа нефиктивной компоненты -
        /// упорядоченный конечный перечислимый.
        /// </summary>
        private const string NATURE_TYPE = "OrderedFiniteEnumerable";

        #endregion

        #region Instance properties

        /// <summary>
        /// Строковое представление математического типа нефиктивной компоненты.
        /// </summary>
        protected override string NatureType { get => NATURE_TYPE; }

        /// <summary>
        /// Компаратор для упорядочения значений компоненты.
        /// </summary>
        protected IComparer<TValue> OrderingComparer { get => _orderingComparer; }

        #endregion

        #region Static fields

        /// <summary>
        /// Компаратор для упорядочения значений компоненты.
        /// </summary>
        protected static IComparer<TValue> _orderingComparer;

        #endregion

        #region Instance fields

        /// <summary>
        /// Перечисление значений компоненты.
        /// </summary>
        private IEnumerable<TValue> _values;

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static OrderedFiniteEnumerableNonFictionalAttributeComponent()
        {
            NonFictionalAttributeComponent<TValue>.InitSetOperations(
                NATURE_TYPE,
                new OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer());
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="values"></param>
        /// <param name="queryExpression"></param>
        public OrderedFiniteEnumerableNonFictionalAttributeComponent(
            AttributeDomain<TValue> domain,
            IEnumerable<TValue> values,
            NonFictionalAttributeComponentQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(domain,
                   new OrderedFiniteEnumerableNonFictionalAttributeComponentPower(values),
                   queryProvider ?? new OrderedFiniteEnumerableNonFictionalAttributeComponentQueryProvider(),
                   queryExpression)
        {
            if (_orderingComparer is null)
                InitOrderingComparer();
            if (!(values is null))
                InitValues(values);
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="setDomainCallback"></param>
        /// <param name="queryExpression"></param>
        public OrderedFiniteEnumerableNonFictionalAttributeComponent(
            IEnumerable<TValue> values,
            out Action<AttributeDomain<TValue>> setDomainCallback,
            NonFictionalAttributeComponentQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(new OrderedFiniteEnumerableNonFictionalAttributeComponentPower(values),
                   out setDomainCallback,
                   queryProvider ?? new OrderedFiniteEnumerableNonFictionalAttributeComponentQueryProvider(),
                   queryExpression)
        {
            if (_orderingComparer is null)
                InitOrderingComparer();
            if (!(values is null))
                InitValues(values);
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Создание компаратора значений компоненты по умолчанию.
        /// Работает при помощи хэш-кодов значений.
        /// </summary>
        /// <returns></returns>
        private static IComparer<TValue> ConstructDefaultHashCodeComparer()
        {
            Comparison<TValue> hashCodeComparison = (first, second) =>
            {
                int firstHC = first.GetHashCode(),
                    secondHC = second.GetHashCode();
                return firstHC.CompareTo(secondHC);
            };

            return Comparer<TValue>.Create(hashCodeComparison);
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Инициализация перечисления значений компоненты.
        /// </summary>
        /// <param name="values"></param>
        protected virtual void InitValues(IEnumerable<TValue> values)
        {
            List<TValue> orderedValues = new List<TValue>(values);
            orderedValues.Sort(OrderingComparer);
            TValue[] orderedValuesArr = new TValue[orderedValues.Count];
            orderedValues.CopyTo(orderedValuesArr);
            _values = orderedValuesArr;

            return;
        }

        /// <summary>
        /// Инициализация компаратора для упорядочения.
        /// </summary>
        private void InitOrderingComparer()
        {
            _orderingComparer = InitOrderingComparerImpl();
            OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer
                .InitAcceptorsComparer(_orderingComparer);

            return;
        }

        /// <summary>
        /// Инициализация компаратора для упорядочения специализированным значением.
        /// В производных классах с более конкретным TValue можно переопределить этот метод,
        /// дабы предоставить более подходящий компаратор.
        /// </summary>
        /// <returns></returns>
        protected virtual IComparer<TValue> InitOrderingComparerImpl()
        {
            Type valueType = typeof(TValue);
            if (!(valueType.GetInterface(nameof(IComparable<TValue>)) is null))
                return Comparer<TValue>.Default;
            //else if (!(valueType.GetInterface(nameof(IComparable)) is null))
            //    return Comparer.Default;
            else
                return ConstructDefaultHashCodeComparer();
        }

        /// <summary>
        /// Проверка компоненты на пустоту.
        /// </summary>
        /// <returns></returns>
        public override bool IsEmpty()
        {
            if (_values is null)
                return false;
            else
                return _values.Count() == 0;
        }

        /// <summary>
        /// Проверка компоненты на полноту.
        /// </summary>
        /// <returns></returns>
        public override bool IsFull()
        {
            if (_values is null)
                return false;
            else
                return this.Domain == this;
        }

        /// <summary>
        /// Перечисление значений компоненты.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<TValue> GetEnumeratorImpl()
        {
            if (_values is null)
                return default(IEnumerator<TValue>);
            else
                return _values.GetEnumerator();
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой аттрибута.
        /// </summary>
        private class OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer : FactorySetOperationExecutersContainer<TValue>
        {
            public OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer() : base(
                new OrderedFiniteEnumerableNonFictionalAttributeComponentFactory<TValue>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentIntersectionOperator<TValue>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentUnionOperator<TValue>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentExceptionOperator<TValue>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentSymmetricExceptionOperator<TValue>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentInclusionComparer<TValue>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentEqualityComparer<TValue>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentInclusionOrEqualityComparer<TValue>())
            { }

            public static void InitAcceptorsComparer(IComparer<TValue> orderingComparer)
            {
                FactoryBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TValue>
                    .InitOrderingComparer(orderingComparer);
                InstantBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TValue>
                    .InitOrderingComparer(orderingComparer);
            }
        }

        /// <summary>
        /// Мощность упорядоченной конечной перечислимой компонентой аттрибута.
        /// </summary>
        public class OrderedFiniteEnumerableNonFictionalAttributeComponentPower : NonFictionalAttributeComponentPower
        {
            private IEnumerable<TValue> _componentValues;

            public int Value { get => _componentValues.Count(); }

            public OrderedFiniteEnumerableNonFictionalAttributeComponentPower(IEnumerable<TValue> componentValues)
            {
                _componentValues = componentValues;
            }

            protected override int CompareToSame(dynamic second)
            {
                if (second is OrderedFiniteEnumerableNonFictionalAttributeComponentPower second2)
                    return this.CompareToSame(second);
                else
                    throw new InvalidCastException("Непустая компонента с конечным перечислимым содержимым сравнивается с непустой компонентой другого вида, в данный момент эта операция не поддерживается.");
            }

            protected int CompareToSame(OrderedFiniteEnumerableNonFictionalAttributeComponentPower second)
            {
                return this.Value.CompareTo(second.Value);
            }
        }

        #endregion
    }
}
