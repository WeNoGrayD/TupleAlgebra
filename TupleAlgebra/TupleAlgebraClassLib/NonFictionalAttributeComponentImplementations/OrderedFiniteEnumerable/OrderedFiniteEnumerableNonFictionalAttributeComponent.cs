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

        #endregion

        #region Static properties

        public static IComparer<TData> DefaultOrderingComparer { get => _defaultOrderingComparer; }

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

            Helper.RegisterType<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>>(
                acFactory: new OrderedFiniteEnumerableAttributeComponentFactory(),
                setOperations: new OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer());

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

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой аттрибута.
        /// </summary>
        private class OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer 
            : NonFictionalAttributeComponentOperationExecutorsContainer<OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>>
        {
            #region Constructors

            public OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer() : base(
                () => new IntersectionOperator<TData>(),
                () => new UnionOperator<TData>(),
                () => new ExceptionOperator<TData>(),
                () => new SymmetricExceptionOperator<TData>(),
                () => new InclusionComparer<TData>(),
                () => new EqualityComparer<TData>(),
                () => new InclusionOrEqualityComparer<TData>())
            {
                return;
            }

            #endregion
        }

        #endregion
    }
}
