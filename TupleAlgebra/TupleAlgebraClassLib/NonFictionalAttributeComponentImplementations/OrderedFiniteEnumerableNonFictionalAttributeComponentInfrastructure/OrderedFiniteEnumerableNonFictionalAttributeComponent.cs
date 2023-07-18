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
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    /// <summary>
    /// Упорядоченная конечная перечислимая нефиктивная компонента.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> 
        : NonFictionalAttributeComponent<TData>
    {
        #region Static fields

        /// <summary>
        /// Компаратор для упорядочения значений компоненты.
        /// </summary>
        protected static IComparer<TData> _defaultOrderingComparer;

        private static IFactoryAttributeComponentOperationExecutersContainer<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>>
            _setOperations = new OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer();

        #endregion

        #region Instance fields

        /// <summary>
        /// Перечисление значений компоненты.
        /// </summary>
        private IEnumerable<TData> _values;

        /// <summary>
        /// Компаратор для упорядочения значений компоненты.
        /// </summary>
        protected IComparer<TData> _orderingComparer;

        #endregion

        #region Instance properties

        /// <summary>
        /// Компаратор для упорядочения значений компоненты.
        /// </summary>
        public IComparer<TData> OrderingComparer
        {
            get => _orderingComparer ?? _defaultOrderingComparer;
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static OrderedFiniteEnumerableNonFictionalAttributeComponent()
        {
            //NonFictionalAttributeComponent<TData>.InitSetOperations(
            //    NATURE_TYPE,
            //    new OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer());

            _defaultOrderingComparer = InitDefaultOrderingComparer();
        }

        public OrderedFiniteEnumerableNonFictionalAttributeComponent() 
            : this(Enumerable.Empty<TData>())
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
            IEnumerable<TData> values,
            IComparer<TData> orderingComparer = null,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(new OrderedFiniteEnumerableNonFictionalAttributeComponentPower(),
                   queryProvider ?? new OrderedFiniteEnumerableAttributeComponentQueryProvider(),
                   queryExpression)
        {
            Initialize(values, orderingComparer);

            return;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Инициализация компаратора для упорядочения специализированным значением по умолчанию.
        /// </summary>
        /// <returns></returns>
        protected static IComparer<TData> InitDefaultOrderingComparer()
        {
            Type valueType = typeof(TData);
            if (valueType.GetInterface(nameof(IComparable<TData>)) is not null)
                return Comparer<TData>.Default;
            else
                return ConstructDefaultHashCodeComparer();
        }

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

        private void Initialize(IEnumerable<TData> values, IComparer<TData> orderingComparer)
        {
            InitOrderingComparer(orderingComparer);
            if (!(values is null))
                InitValues(values);

            return;
        }

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(IEnumerable<TReproducedData> populatingData)
        {
            return OrderedFiniteEnumerableAttributeComponentFactoryArgs.Construct(
                typeof(TData).Equals(typeof(TReproducedData)) ? this._orderingComparer as IComparer<TReproducedData> : null,
                populatingData);//,
                //this.Provider as OrderedFiniteEnumerableAttributeComponentQueryProvider);
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
        private void InitOrderingComparer(IComparer<TData> orderingComparer)
        {
            _orderingComparer = orderingComparer ?? InitOrderingComparerImpl();
            //OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer
            //    .InitAcceptorsComparer(_orderingComparer);

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
            return _defaultOrderingComparer;
        }

        protected override sealed AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return _setOperations.Produce<TReproducedData>(factoryArgs);
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
                return base.IsEmpty();
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
                return base.IsFull();
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

        #region Operators

        protected override AttributeComponent<TData> ComplementThe()
        {
            return _setOperations.Complement(this);
        }

        protected override AttributeComponent<TData> IntersectWith(AttributeComponent<TData> second)
        {
            return _setOperations.Intersect(this, second);
        }

        protected override AttributeComponent<TData> UnionWith(AttributeComponent<TData> second)
        {
            return _setOperations.Union(this, second);
        }

        protected override AttributeComponent<TData> ExceptWith(AttributeComponent<TData> second)
        {
            return _setOperations.Except(this, second);
        }

        protected override AttributeComponent<TData> SymmetricExceptWith(AttributeComponent<TData> second)
        {
            return _setOperations.SymmetricExcept(this, second);
        }

        protected override bool Includes(AttributeComponent<TData> second)
        {
            return _setOperations.Include(this, second);
        }

        protected override bool EqualsTo(AttributeComponent<TData> second)
        {
            return _setOperations.Equal(this, second);
        }

        protected override bool IncludesOrEqualsTo(AttributeComponent<TData> second)
        {
            return _setOperations.IncludeOrEqual(this, second);
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой аттрибута.
        /// </summary>
        private class OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer 
            : NonFictionalAttributeComponentOperationExecutersContainer<OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>>
        {
            public OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutersContainer() : base(
                new OrderedFiniteEnumerableAttributeComponentFactory(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentIntersectionOperator<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentUnionOperator<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentExceptionOperator<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentSymmetricExceptionOperator<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentInclusionComparer<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentEqualityComparer<TData>(),
                new OrderedFiniteEnumerableNonFictionalAttributeComponentInclusionOrEqualityComparer<TData>())
            { }

            /*
            public static void InitAcceptorsComparer(IComparer<TData> orderingComparer)
            {
                FactoryBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TData>
                    .InitOrderingComparer(orderingComparer);
                InstantBinaryOrderedFiniteEnumerableNonFictionalAttributeComponentAcceptor<TData>
                    .InitOrderingComparer(orderingComparer);
            }
            */
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
