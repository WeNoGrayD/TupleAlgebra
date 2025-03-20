using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.AttributeComponents;
using static UniversalClassLib.ReadOnlySequenceHelper;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Streaming
{
    using static AttributeComponentHelper;

    public class StreamingOrderedFiniteEnumerableAttributeComponent<TData>
        : OrderedFiniteEnumerableAttributeComponent<TData, ReadOnlySequence<TData>>,
          IOrderedFiniteEnumerableAttributeComponent<TData>
    {
        #region Instance properties

        public override int Count { get => Values.GetLength(); }

        #endregion

        #region Constructors

        static StreamingOrderedFiniteEnumerableAttributeComponent()
        {
            Helper.RegisterType<TData, StreamingOrderedFiniteEnumerableAttributeComponent<TData>>(
                acFactory: null,
                setOperations: (factory) => new StreamingOrderedFiniteEnumerableAttributeComponentOperationExecutorsContainer((IStreamingOrderedFiniteEnumerableAttributeComponentFactory<TData>)factory));

            return;
        }

        protected StreamingOrderedFiniteEnumerableAttributeComponent(
            AttributeComponentPower power,
            IQueryProvider queryProvider,
            Expression queryExpression = null)
            : base(power, queryProvider, queryExpression)
        { }

        public StreamingOrderedFiniteEnumerableAttributeComponent(
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

        protected override ReadOnlySequence<TData>
            ObtainAlreadyOrderedValues(
                IEnumerable<TData> values)
        {
            ReadOnlySequence<TData> domainUniverseSequence = 
                (Domain.Universe as IStreamingValuesProvider<TData>)!
                    .GetStream();

            return values.ToSubSequenceFromSingleSegmentSuperSequence(
                domainUniverseSequence, OrderingComparer);
        }

        protected override ReadOnlySequence<TData>
            ObtainNotYetOrderedValues(
                IEnumerable<TData> values)
        {
            List<TData> orderedValues = values.ToList();
            orderedValues.Sort();

            return ObtainAlreadyOrderedValues(orderedValues);
        }

        protected override IEnumerable<TData>
            GetFiniteEnumerableValues()
        {
            return Values.ToEnumerable();
        }

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(IEnumerable<TReproducedData> populatingData)
        {
            return new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TReproducedData>(
                values: populatingData,
                orderingComparer: _orderingComparer as IComparer<TReproducedData>,
                queryProvider: Provider);
        }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            return Values.ToEnumerable().GetEnumerator();
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой атрибута.
        /// </summary>
        protected class StreamingOrderedFiniteEnumerableAttributeComponentOperationExecutorsContainer
            : OrderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer<
                StreamingOrderedFiniteEnumerableAttributeComponent<TData>,
                IStreamingOrderedFiniteEnumerableAttributeComponentFactory<TData>,
                StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
        {
            #region Constructors

            public StreamingOrderedFiniteEnumerableAttributeComponentOperationExecutorsContainer(
                IStreamingOrderedFiniteEnumerableAttributeComponentFactory<TData> factory)
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
