using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.LINQ2TAFramework;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    /// <summary>
    /// Упорядоченная конечная перечислимая нефиктивная компонента.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> 
        : NonFictionalAttributeComponent<TData>
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
        protected IComparer<TData> OrderingComparer { get => _orderingComparer; }

        #endregion

        #region Static fields

        /// <summary>
        /// Компаратор для упорядочения значений компоненты.
        /// </summary>
        protected static IComparer<TData> _orderingComparer;

        #endregion

        #region Instance fields

        /// <summary>
        /// Перечисление значений компоненты.
        /// </summary>
        private IEnumerable<TData> _values;

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static OrderedFiniteEnumerableNonFictionalAttributeComponent()
        {
            NonFictionalAttributeComponent<TData>.InitSetOperations(
                NATURE_TYPE,
                new OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer());
        }

        public OrderedFiniteEnumerableNonFictionalAttributeComponent() 
            : this(
                  null,
                  Enumerable.Empty<TData>())
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="values"></param>
        /// <param name="queryExpression"></param>
        public OrderedFiniteEnumerableNonFictionalAttributeComponent(
            AttributeDomain<TData> domain,
            IEnumerable<TData> values,
            QueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(domain,
                   new OrderedFiniteEnumerableNonFictionalAttributeComponentPower(),
                   queryProvider ?? new OrderedFiniteEnumerableAttributeComponentQueryProvider(),
                   queryExpression)
        {
            Initialize(values);

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="setDomainCallback"></param>
        /// <param name="queryExpression"></param>
        public OrderedFiniteEnumerableNonFictionalAttributeComponent(
            IEnumerable<TData> values,
            out Action<AttributeDomain<TData>> setDomainCallback,
            QueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(new OrderedFiniteEnumerableNonFictionalAttributeComponentPower(),
                   out setDomainCallback,
                   queryProvider ?? new OrderedFiniteEnumerableAttributeComponentQueryProvider(),
                   queryExpression)
        {
            Initialize(values);

            return;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Создание компаратора значений компоненты по умолчанию.
        /// Работает при помощи хэш-кодов значений.
        /// </summary>
        /// <returns></returns>
        private static IComparer<TData> ConstructDefaultHashCodeComparer()
        {
            Comparison<TData> hashCodeComparison = (first, second) =>
            {
                int firstHC = first.GetHashCode(),
                    secondHC = second.GetHashCode();
                return firstHC.CompareTo(secondHC);
            };

            return Comparer<TData>.Create(hashCodeComparison);
        }

        #endregion

        #region Instance methods

        private void Initialize(IEnumerable<TData> values)
        {
            if (_orderingComparer is null)
                InitOrderingComparer();
            if (!(values is null))
                InitValues(values);

            return;
        }

        public override AttributeComponentFactoryArgs<TData> ZipInfo(
            IEnumerable<TData> populatingData)
        {
            return new OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(
                this.Domain,
                populatingData,
                this.Provider as OrderedFiniteEnumerableAttributeComponentQueryProvider);
        }

        /// <summary>
        /// Инициализация перечисления значений компоненты.
        /// </summary>
        /// <param name="values"></param>
        public virtual void InitValues(IEnumerable<TData> values)
        {
            List<TData> orderedValues = new List<TData>(values);
            orderedValues.Sort(_orderingComparer);
            TData[] orderedValuesArr = new TData[orderedValues.Count];
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
        /// В производных классах с более конкретным TData можно переопределить этот метод,
        /// дабы предоставить более подходящий компаратор.
        /// </summary>
        /// <returns></returns>
        protected virtual IComparer<TData> InitOrderingComparerImpl()
        {
            Type valueType = typeof(TData);
            if (valueType.GetInterface(nameof(IComparable<TData>)) is not null)
                return Comparer<TData>.Default;
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
                return this.Power.IsZero();
            //_values.Count() == 0;
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
        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            if (_values is null)
                return default(IEnumerator<TData>);
            else
                return _values.GetEnumerator();
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой аттрибута.
        /// </summary>
        private class OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer : FactorySetOperationExecutersContainer<TData>
        {
            public OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer() : base(
                new OrderedFiniteEnumerableAttributeComponentFactory<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentIntersectionOperator<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentUnionOperator<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentExceptionOperator<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentSymmetricExceptionOperator<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentInclusionComparer<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentEqualityComparer<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentInclusionOrEqualityComparer<TData>())
            { }

            public static void InitAcceptorsComparer(IComparer<TData> orderingComparer)
            {
                FactoryBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TData>
                    .InitOrderingComparer(orderingComparer);
                InstantBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TData>
                    .InitOrderingComparer(orderingComparer);
            }
        }

        /// <summary>
        /// Мощность упорядоченной конечной перечислимой компонентой аттрибута.
        /// </summary>
        public class OrderedFiniteEnumerableNonFictionalAttributeComponentPower : NonFictionalAttributeComponentPower
        {
            private OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> _component;

            public int Value { get => _component._values.Count(); }

            //public static readonly OrderedFiniteEnumerableNonFictionalAttributeComponentPower Empty =
            //    new OrderedFiniteEnumerableNonFictionalAttributeComponentPower(Enumerable.Empty<TData>());

            public override void InitAttributeComponent(AttributeComponent<TData> component)
            {
                _component = component as OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>;
            }

            public override bool IsZero()
            {
                return Value == 0;
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
