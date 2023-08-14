using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.FullAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
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
    public sealed class FullAttributeComponent<TData> : AttributeComponent<TData, FullAttributeComponent<TData>>
    {
        #region Static fields

        private static Lazy<ISetOperationExecutersContainer<AttributeComponent<TData>, FullAttributeComponent<TData>>>
            _setOperations;

        #endregion

        #region Instance properties

        protected override ISetOperationExecutersContainer<AttributeComponent<TData>, FullAttributeComponent<TData>> SetOperations
        { get => _setOperations.Value; }

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static FullAttributeComponent()
        {
            RegisterType<TData>(
                typeof(FullAttributeComponent<TData>));

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
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(power, queryProvider, queryExpression)
        {
            _setOperations = new Lazy<ISetOperationExecutersContainer<AttributeComponent<TData>, FullAttributeComponent<TData>>>(
                () => new FullAttributeComponentOperationExecutersContainer());

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
            return Domain.Universum.GetEnumerator();
        }

        protected override AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return GetFactory(this).CreateFull<TReproducedData>(factoryArgs);
        }

        #endregion

        #region Operators

        protected override AttributeComponent<TData> ComplementThe()
        {
            return SetOperations.Complement(this);
        }

        protected override AttributeComponent<TData> IntersectWith(AttributeComponent<TData> second)
        {
            return SetOperations.Intersect(this, second);
        }

        protected override AttributeComponent<TData> UnionWith(AttributeComponent<TData> second)
        {
            return SetOperations.Union(this, second);
        }

        protected override AttributeComponent<TData> ExceptWith(AttributeComponent<TData> second)
        {
            return SetOperations.Except(this, second);
        }

        protected override AttributeComponent<TData> SymmetricExceptWith(AttributeComponent<TData> second)
        {
            return SetOperations.SymmetricExcept(this, second);
        }

        protected override bool Includes(AttributeComponent<TData> second)
        {
            return SetOperations.Include(this, second);
        }

        protected override bool EqualsTo(AttributeComponent<TData> second)
        {
            return SetOperations.Equal(this, second);
        }

        protected override bool IncludesOrEqualsTo(AttributeComponent<TData> second)
        {
            return SetOperations.IncludeOrEqual(this, second);
        }

        #endregion

        #region Nested types

        private class FullAttributeComponentOperationExecutersContainer
            : InstantAttributeComponentOperationExecutersContainer<TData, FullAttributeComponent<TData>>
        {
            public FullAttributeComponentOperationExecutersContainer() : base(
                () => new FullAttributeComponentComplementionOperator<TData>(),
                () => new FullAttributeComponentIntersectionOperator<TData>(),
                () => new FullAttributeComponentUnionOperator<TData>(),
                () => new FullAttributeComponentExceptionOperator<TData>(),
                () => new FullAttributeComponentSymmetricExceptionOperator<TData>(),
                () => new FullAttributeComponentInclusionComparer<TData>(),
                () => new FullAttributeComponentEqualityComparer<TData>(),
                () => new FullAttributeComponentInclusionOrEqualityComparer<TData>())
            { }
        }

        #endregion
    }
}
