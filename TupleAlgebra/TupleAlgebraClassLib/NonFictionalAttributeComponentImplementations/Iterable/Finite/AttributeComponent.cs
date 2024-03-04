using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Iterable.Finite
{
    using static AttributeComponentHelper;

    public class FiniteIterableAttributeComponent<TData>
        : IterableAttributeComponent<TData>,
          IFiniteEnumerableAttributeComponent<TData>
    {
        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static FiniteIterableAttributeComponent()
        {
            Helper.RegisterType<TData, IterableAttributeComponent<TData>>(
                acFactory: (domain) => new FiniteIterableAttributeComponentFactory<TData>(domain),
                setOperations: (factory) => new FiniteIterableNonFictionalAttributeComponentOperationExecutorsContainer((IFiniteIterableAttributeComponentFactory<TData>)factory));

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="values"></param>
        /// <param name="queryExpression"></param>
        public FiniteIterableAttributeComponent(
            AttributeComponentPower power,
            IEnumerable<TData> values,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(power,
                   values,
                   queryProvider ?? new DefaultAttributeComponentQueryProvider(),
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
            return new FiniteIterableAttributeComponentFactoryArgs<TReproducedData>(
                populatingData as IEnumerable<TReproducedData>);//,
                                                            //this.Provider as OrderedFiniteEnumerableAttributeComponentQueryProvider);
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой аттрибута.
        /// </summary>
        private class FiniteIterableNonFictionalAttributeComponentOperationExecutorsContainer
            : NonFictionalAttributeComponentOperationExecutorsContainer<
                FiniteIterableAttributeComponent<TData>,
                IFiniteIterableAttributeComponentFactory<TData>,
                FiniteIterableAttributeComponentFactoryArgs<TData>>
        {
            #region Constructors

            public FiniteIterableNonFictionalAttributeComponentOperationExecutorsContainer(
                IFiniteIterableAttributeComponentFactory<TData> factory)
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
