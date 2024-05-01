using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.LINQ2TAFramework;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    using static AttributeComponentHelper;

    public abstract class OrderedFiniteEnumerableAttributeComponent<
        TData,
        TValuesContainer>
        : SequenceBasedNonFictionalAttributeComponent<
            TData,
            TValuesContainer>,
          ICountableAttributeComponent<TData>
    {
        #region Static fields

        /// <summary>
        /// Компаратор для упорядочения значений компоненты.
        /// </summary>
        protected static IComparer<TData> _defaultOrderingComparer;

        #endregion

        #region Static properties

        public static IComparer<TData> DefaultOrderingComparer
        { get => _defaultOrderingComparer; }

        #endregion

        #region Instance fields

        /// <summary>
        /// Компаратор для упорядочения значений компоненты.
        /// </summary>
        protected IComparer<TData> _orderingComparer;

        protected Lazy<TValuesContainer> _values;

        #endregion

        #region Instance properties

        public override TValuesContainer Values { get => _values.Value; }

        /// <summary>
        /// Компаратор для упорядочения значений компоненты.
        /// </summary>
        public IComparer<TData> OrderingComparer
        {
            get => _orderingComparer ?? _defaultOrderingComparer;
        }

        IEnumerable<TData> 
            IFiniteEnumerableAttributeComponent<TData>.Values 
            { get => GetFiniteEnumerableValues(); }

        public abstract int Count { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static OrderedFiniteEnumerableAttributeComponent()
        {
            _defaultOrderingComparer = InitDefaultOrderingComparer();

            Helper.RegisterType<TData, OrderedFiniteEnumerableAttributeComponent<TData, TValuesContainer>>(
                acFactory: (domain) => new OrderedFiniteEnumerableAttributeComponentFactory<TData>(domain),
                setOperations: null);

            return;
        }

        protected OrderedFiniteEnumerableAttributeComponent(
            AttributeComponentPower power,
            IQueryProvider queryProvider,
            Expression queryExpression = null)
            : base(
                  power,
                  queryProvider ?? new OrderedFiniteEnumerableAttributeComponentQueryProvider(), 
                  queryExpression)
        { }

        protected OrderedFiniteEnumerableAttributeComponent(
            AttributeComponentPower power,
            IEnumerable<TData> values,
            IComparer<TData> orderingComparer = null,
            bool valuesAreOrdered = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : this(power, queryProvider, queryExpression)
        {
            Initialize(values, orderingComparer, valuesAreOrdered);

            return;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Инициализация компаратора для упорядочения специализированным значением по умолчанию.
        /// </summary>
        /// <returns></returns>
        private static IComparer<TData> InitDefaultOrderingComparer()
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

        private void Initialize(
            IEnumerable<TData> values,
            IComparer<TData> orderingComparer,
            bool valuesAreOrdered)
        {
            InitOrderingComparer(orderingComparer);
            if (!(values is null))
                InitValues(values, valuesAreOrdered);

            return;
        }

        /// <summary>
        /// Инициализация компаратора для упорядочения.
        /// </summary>
        private void InitOrderingComparer(IComparer<TData> orderingComparer)
        {
            _orderingComparer = orderingComparer ?? InitOrderingComparerImpl();
            //OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer
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

        /// <summary>
        /// Инициализация перечисления значений компоненты.
        /// </summary>
        /// <param name="values"></param>
        public void InitValues(
            IEnumerable<TData> values,
            bool valuesAreOrdered)
        {
            Func<TValuesContainer> valuesInitializer;

            if (valuesAreOrdered)
                valuesInitializer = () => ObtainAlreadyOrderedValues(values);
            else
                valuesInitializer = () => ObtainNotYetOrderedValues(values);

            _values = new Lazy<TValuesContainer>(valuesInitializer);

            return;
        }

        protected abstract TValuesContainer ObtainAlreadyOrderedValues(
            IEnumerable<TData> values);

        protected abstract TValuesContainer ObtainNotYetOrderedValues(
            IEnumerable<TData> values);

        protected abstract IEnumerable<TData>
            GetFiniteEnumerableValues();

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой атрибута.
        /// </summary>
        protected abstract class OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer<TAttributeComponent, TFactory, TFactoryArgs>
            : NonFictionalAttributeComponentOperationExecutorsContainer<
                TAttributeComponent,
                IEnumerable<TData>,
                TFactory,
                TFactoryArgs>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IOrderedFiniteEnumerableAttributeComponent<TData>
        where TFactoryArgs : OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
        where TFactory : IOrderedFiniteEnumerableAttributeComponentFactory<TData, TAttributeComponent, TFactoryArgs>
        {
            #region Constructors

            public OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer(
                TFactory factory,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, IEnumerable<TData>, TAttributeComponent, TFactory, TFactoryArgs, IAttributeComponent<TData>>>
                    intersectionOperator,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, IEnumerable<TData>, TAttributeComponent, TFactory, TFactoryArgs, IAttributeComponent<TData>>>
                    unionOperator,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, IEnumerable<TData>, TAttributeComponent, TFactory, TFactoryArgs, IAttributeComponent<TData>>>
                    differenceOperator,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, IEnumerable<TData>, TAttributeComponent, TFactory, TFactoryArgs, IAttributeComponent<TData>>>
                    symmetricExceptionOperator,
                Func<InstantBinaryAttributeComponentAcceptor<TData, TAttributeComponent, bool>>
                    inclusionComparer,
                Func<InstantBinaryAttributeComponentAcceptor<TData, TAttributeComponent, bool>>
                    equalityComparer,
                Func<InstantBinaryAttributeComponentAcceptor<TData, TAttributeComponent, bool>>
                    inclusionOrEquationComparer)
                : base(
                      factory,
                      intersectionOperator,
                      unionOperator,
                      differenceOperator,
                      symmetricExceptionOperator,
                      inclusionComparer,
                      equalityComparer,
                      inclusionOrEquationComparer)
            {
                return;
            }

            #endregion
        }

        #endregion
    }
}
