using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework;
using System.Linq.Expressions;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponents
{
    public sealed class EmptyAttributeComponent<TData> : AttributeComponent<TData>
    {
        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.Empty;

        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        private static ISetOperationExecutersContainer<AttributeComponent<TData>, EmptyAttributeComponent<TData>>
            _setOperations = new EmptyAttributeComponentOperationExecutersContainer();

        static EmptyAttributeComponent()
        {
            //AttributeComponent<TData>.InitSetOperations(
            //    CONTENT_TYPE, new EmptyAttributeComponentOperationExecutersContainer());
        }

        public EmptyAttributeComponent(
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(new EmptyAttributeComponentPower(), queryProvider, queryExpression)
        { }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            yield break;
        }

        #region Instance methods

        protected override AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return new EmptyAttributeComponent<TReproducedData>(
                factoryArgs.QueryProvider,
                factoryArgs.QueryExpression);
        }

        #endregion

        public override bool IsEmpty()
        {
            return true;
        }

        public override bool IsFull()
        {
            return false;
        }

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

        private class EmptyAttributeComponentOperationExecutersContainer
            : InstantAttributeComponentOperationExecutersContainer<TData, EmptyAttributeComponent<TData>>
        {
            public EmptyAttributeComponentOperationExecutersContainer() : base(
                new EmptyAttributeComponentComplementionOperator<TData>(),
                new EmptyAttributeComponentIntersectionOperator<TData>(),
                new EmptyAttributeComponentUnionOperator<TData>(),
                new EmptyAttributeComponentExceptionOperator<TData>(),
                new EmptyAttributeComponentSymmetricExceptionOperator<TData>(),
                new EmptyAttributeComponentInclusionComparer<TData>(),
                new EmptyAttributeComponentEqualityComparer<TData>(),
                new EmptyAttributeComponentInclusionOrEqualityComparer<TData>())
            { }
        }

        private class EmptyAttributeComponentPower : AttributeComponentPower
        {
            internal override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

            public override void InitAttributeComponent(AttributeComponent<TData> component)
            {
                return;
            }

            public override bool IsZero()
            {
                return true;
            }
        }
    }
}
