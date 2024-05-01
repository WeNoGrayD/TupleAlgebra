using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered
{
    using static AttributeComponentHelper;

    public class BufferedOrderedFiniteEnumerableAttributeComponent<TData>
        : OrderedFiniteEnumerableAttributeComponent<TData, TData[]>,
          IOrderedFiniteEnumerableAttributeComponent<TData>,
          IStreamingValuesProvider<TData>
    {
        #region Instance properties

        public override int Count { get => Values.Length; }

        #endregion

        #region Constructors

        static BufferedOrderedFiniteEnumerableAttributeComponent()
        {
            Helper.RegisterType<TData, BufferedOrderedFiniteEnumerableAttributeComponent<TData>>(
                acFactory: null,
                setOperations: (factory) => new BufferedOrderedFiniteEnumerableAttributeComponentOperationExecutorsContainer((IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>)factory));

            return;
        }

        protected BufferedOrderedFiniteEnumerableAttributeComponent(
            AttributeComponentPower power,
            IQueryProvider queryProvider,
            Expression queryExpression = null)
            : base(power, queryProvider, queryExpression)
        { }

        public BufferedOrderedFiniteEnumerableAttributeComponent(
            AttributeComponentPower power,
            IEnumerable<TData> values,
            IComparer<TData> orderingComparer = null,
            bool valuesAreOrdered = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
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

        protected override TData[] ObtainAlreadyOrderedValues(
            IEnumerable<TData> values)
        {
            return values.ToArray();
        }

        protected override TData[] ObtainNotYetOrderedValues(
            IEnumerable<TData> values)
        {
            List<TData> orderedValues = new List<TData>(values);
            orderedValues.Sort(_orderingComparer);

            return orderedValues.ToArray();
        }

        protected override IEnumerable<TData>
            GetFiniteEnumerableValues()
        {
            return Values;
        }

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(IEnumerable<TReproducedData> populatingData)
        {
            return new BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TReproducedData>(
                values: populatingData,
                orderingComparer: _orderingComparer as IComparer<TReproducedData>,
                queryProvider: Provider);
        }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            IEnumerable<TData> values = Values;

            return values.GetEnumerator();
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой атрибута.
        /// </summary>
        protected class BufferedOrderedFiniteEnumerableAttributeComponentOperationExecutorsContainer
            : OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer<
                BufferedOrderedFiniteEnumerableAttributeComponent<TData>,
                IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>,
                BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
        {
            #region Constructors

            public BufferedOrderedFiniteEnumerableAttributeComponentOperationExecutorsContainer(
                IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData> factory)
                : base(
                      factory,
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
