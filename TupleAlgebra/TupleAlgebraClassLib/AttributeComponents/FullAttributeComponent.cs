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
    /// <summary>
    /// Полная фиктивная компонента атрибута.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public sealed class FullAttributeComponent<TData> : AttributeComponent<TData>
    {
        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.Full;

        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        private static ISetOperationExecutersContainer<AttributeComponent<TData>, FullAttributeComponent<TData>>
            _setOperations = new FullAttributeComponentOperationExecutersContainer();

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static FullAttributeComponent()
        {
            //AttributeComponent<TData>.InitSetOperations(
            //    CONTENT_TYPE, new FullAttributeComponentOperationExecutersContainer());
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="queryProvider"></param>
        /// <param name="queryExpression"></param>
        public FullAttributeComponent(
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(new FullAttributeComponentPower(), queryProvider, queryExpression)
        { }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            return Domain.Universum.GetEnumerator();
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override bool IsFull()
        {
            return true;
        }

        #region Instance methods

        protected override AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return new FullAttributeComponent<TReproducedData>(factoryArgs.QueryProvider, factoryArgs.QueryExpression);
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

        #region Inner classes

        private class FullAttributeComponentOperationExecutersContainer
            : InstantAttributeComponentOperationExecutersContainer<TData, FullAttributeComponent<TData>>
        {
            public FullAttributeComponentOperationExecutersContainer() : base(
                new FullAttributeComponentComplementionOperator<TData>(),
                new FullAttributeComponentIntersectionOperator<TData>(),
                new FullAttributeComponentUnionOperator<TData>(),
                new FullAttributeComponentExceptionOperator<TData>(),
                new FullAttributeComponentSymmetricExceptionOperator<TData>(),
                new FullAttributeComponentInclusionComparer<TData>(),
                new FullAttributeComponentEqualityComparer<TData>(),
                new FullAttributeComponentInclusionOrEqualityComparer<TData>())
            { }
        }

        /// <summary>
        /// Мощность полной фиктивной компоненты атрибута.
        /// </summary>
        private class FullAttributeComponentPower : AttributeComponentPower
        {
            internal override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

            public override void InitAttributeComponent(AttributeComponent<TData> component)
            {
                return;
            }

            public override bool IsZero()
            {
                return false;
            }
        }

        #endregion
    }
}
