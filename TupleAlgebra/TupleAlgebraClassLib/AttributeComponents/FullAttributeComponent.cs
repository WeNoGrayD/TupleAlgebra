using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.FullAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework;
using System.Linq.Expressions;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponents
{
    using static AttributeComponentHelper;
    
    /// <summary>
    /// Полная фиктивная компонента атрибута.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public sealed class FullAttributeComponent<TData> 
        : AttributeComponent<TData>
    {
        #region Instance fields

        private Func<AttributeDomain<TData>> _getDomain;

        #endregion

        #region Instance properties

        /*
        public override AttributeDomain<TData> Domain 
        {
            get => base.Domain;
            set
            {
                _domain = value;
                (Power as FullAttributeComponentPower).UniversePower =
                    value.Universe.Power;
            }
        }
        */

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static FullAttributeComponent()
        {
            Helper.RegisterType<TData, FullAttributeComponent<TData>>(
                setOperations: (factory) => new FullAttributeComponentOperationExecutorsContainer(factory));

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="queryProvider"></param>
        /// <param name="queryExpression"></param>
        public FullAttributeComponent(
            FullAttributeComponentPower power,
            IQueryProvider queryProvider,
            Expression queryExpression = null)
            : base(power, queryProvider, queryExpression)
        {
            return;
        }

        #endregion

        #region Instance methods

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(IEnumerable<TReproducedData> populatingData)
        {
            return new FullAttributeComponentFactoryArgs();
        }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            return Domain.Universe.GetEnumerator();
        }

        protected override AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return null;
            //return Factory.CreateFull<TReproducedData>(factoryArgs);
        }

        #endregion

        #region Nested types

        private class FullAttributeComponentOperationExecutorsContainer
            : InstantAttributeComponentOperationExecutorsContainer<FullAttributeComponent<TData>>
        {
            public FullAttributeComponentOperationExecutorsContainer(
                IAttributeComponentFactory<TData> factory) : base(
                    factory,
                    () => new FullAttributeComponentComplementationOperator<TData>(),
                    () => new FullAttributeComponentIntersectionOperator<TData>(),
                    () => new FullAttributeComponentUnionOperator<TData>(),
                    () => new FullAttributeComponentExceptionOperator<TData>(),
                    () => new FullAttributeComponentExceptionOperator<TData>(),
                    () => new FullAttributeComponentInclusionComparer<TData>(),
                    () => new FullAttributeComponentEqualityComparer<TData>(),
                    () => new FullAttributeComponentInclusionOrEqualityComparer<TData>())
            { }
        }

        #endregion
    }
}
