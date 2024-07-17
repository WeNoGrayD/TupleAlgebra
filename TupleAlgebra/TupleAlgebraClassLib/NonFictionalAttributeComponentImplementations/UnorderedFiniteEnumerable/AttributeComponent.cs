using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.UnorderedFiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    using static AttributeComponentHelper;

    /// <summary>
    /// Неупорядоченная конечная перечислимая нефиктивная компонента.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> 
        : SequenceBasedNonFictionalAttributeComponent<TData, HashSet<TData>>,
          ICountableAttributeComponent<TData>
    {
        #region Instance properties

        /// <summary>
        /// Перечисление значений компоненты.
        /// </summary>
        IEnumerable<TData> IFiniteEnumerableAttributeComponent<TData>.Values { get => Values; }

        int ICountableAttributeComponent<TData>.Count 
            { get => Values.Count; }

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static UnorderedFiniteEnumerableNonFictionalAttributeComponent()
        {
            Helper.RegisterType<TData, UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>>(
                acFactory: (domain) => new UnorderedFiniteEnumerableAttributeComponentFactory<TData>(domain),
                setOperations: (factory) => new UnorderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer((IUnorderedFiniteEnumerableAttributeComponentFactory<TData>)factory));

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="values"></param>
        /// <param name="queryExpression"></param>
        public UnorderedFiniteEnumerableNonFictionalAttributeComponent(
            AttributeComponentPower power,
            HashSet<TData> values,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(power,
                   queryProvider ?? new UnorderedFiniteEnumerableAttributeComponentQueryProvider(),
                   queryExpression)
        {
            Values = values;

            return;
        }

        #endregion

        #region Instance methods

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(
            IEnumerable<TReproducedData> populatingData)
        {
            return new UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TReproducedData>(
                populatingData.ToHashSet());//,
                //this.Provider as OrderedFiniteEnumerableAttributeComponentQueryProvider);
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
            return Values.GetEnumerator();//Values is not null ? Values.GetEnumerator() : Enumerable.Empty<TData>().GetEnumerator();
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой аттрибута.
        /// </summary>
        private class UnorderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer 
            : NonFictionalAttributeComponentOperationExecutorsContainer<
                UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
                IEnumerable<TData>,
                IUnorderedFiniteEnumerableAttributeComponentFactory<TData>,
                UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
        {
            #region Constructors

            public UnorderedFiniteEnumerableNonFictionalAttributeComponentOperationExecutorsContainer(
                IUnorderedFiniteEnumerableAttributeComponentFactory<TData> factory) 
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
