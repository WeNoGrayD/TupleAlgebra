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
    using static AttributeComponentHelper;

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
        protected internal IEnumerable<TData> Values { get; protected set; }

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
            _defaultOrderingComparer = InitDefaultOrderingComparer();

            RegisterType<TData>(
                typeof(OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>),
                acFactory: new OrderedFiniteEnumerableAttributeComponentFactory());

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="values"></param>
        /// <param name="queryExpression"></param>
        public OrderedFiniteEnumerableNonFictionalAttributeComponent(
            AttributeComponentPower power,
            IEnumerable<TData> values,
            IComparer<TData> orderingComparer = null,
            bool valuesAreOrdered = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(power,
                   queryProvider ?? new OrderedFiniteEnumerableAttributeComponentQueryProvider(),
                   queryExpression)
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
        public virtual void InitValues(
            IEnumerable<TData> values,
            bool valuesAreOrdered)
        {
            if (valuesAreOrdered)
                Values = values.ToArray();
            else
            {
                List<TData> orderedValues = new List<TData>(values);
                orderedValues.Sort(_orderingComparer);
                TData[] orderedValuesArr = new TData[orderedValues.Count];
                orderedValues.CopyTo(orderedValuesArr);
                Values = orderedValuesArr;
            }

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

        /*
        protected override sealed AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return _setOperations.Produce<TReproducedData>(factoryArgs);
        }
        */

        /// <summary>
        /// Перечисление значений компоненты.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            if (Values is null)
                return default(IEnumerator<TData>);
            else
                return Values.GetEnumerator();
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
                () => new OrderedFiniteEnumerableAttributeComponentFactory(),
                () => new OrderedFiniteEnumerableNonFictionalAttributeComponentIntersectionOperator<TData>(),
                () => new OrderedFiniteEnumerableNonFictionalAttributeComponentUnionOperator<TData>(),
                () => new OrderedFiniteEnumerableNonFictionalAttributeComponentExceptionOperator<TData>(),
                () => new OrderedFiniteEnumerableNonFictionalAttributeComponentSymmetricExceptionOperator<TData>(),
                () => new OrderedFiniteEnumerableNonFictionalAttributeComponentInclusionComparer<TData>(),
                () => new OrderedFiniteEnumerableNonFictionalAttributeComponentEqualityComparer<TData>(),
                () => new OrderedFiniteEnumerableNonFictionalAttributeComponentInclusionOrEqualityComparer<TData>())
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

        #endregion
    }
}
