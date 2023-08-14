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
    using static AttributeComponentHelper;

    public sealed class EmptyAttributeComponent<TData> : AttributeComponent<TData, EmptyAttributeComponent<TData>>
    {
        #region Static fields

        private static Lazy<ISetOperationExecutersContainer<AttributeComponent<TData>, EmptyAttributeComponent<TData>>>
            _setOperations;

        #endregion

        #region Instance properties

        protected override ISetOperationExecutersContainer<AttributeComponent<TData>, EmptyAttributeComponent<TData>> SetOperations
        { get => _setOperations.Value; }

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static EmptyAttributeComponent()
        {
            RegisterType<TData>(
                typeof(EmptyAttributeComponent<TData>));

            return;
        }

        public EmptyAttributeComponent(
            EmptyAttributeComponentPower power,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(power, queryProvider, queryExpression)
        {
            _setOperations = new Lazy<ISetOperationExecutersContainer<AttributeComponent<TData>, EmptyAttributeComponent<TData>>>(
                () => new EmptyAttributeComponentOperationExecutersContainer());

            return;
        }

        #endregion

        #region Instance methods

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(IEnumerable<TReproducedData> populatingData)
        {
            return new EmptyAttributeComponentFactoryArgs();
        }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            yield break;
        }

        protected override AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return GetFactory(this).CreateEmpty<TReproducedData>(factoryArgs);//new EmptyAttributeComponent<TReproducedData>(
                //factoryArgs.QueryProvider,
                //factoryArgs.QueryExpression);
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

        private class EmptyAttributeComponentOperationExecutersContainer
            : InstantAttributeComponentOperationExecutersContainer<TData, EmptyAttributeComponent<TData>>
        {
            public EmptyAttributeComponentOperationExecutersContainer() : base(
                () => new EmptyAttributeComponentComplementionOperator<TData>(),
                () => new EmptyAttributeComponentIntersectionOperator<TData>(),
                () => new EmptyAttributeComponentUnionOperator<TData>(),
                () => new EmptyAttributeComponentExceptionOperator<TData>(),
                () => new EmptyAttributeComponentSymmetricExceptionOperator<TData>(),
                () => new EmptyAttributeComponentInclusionComparer<TData>(),
                () => new EmptyAttributeComponentEqualityComparer<TData>(),
                () => new EmptyAttributeComponentInclusionOrEqualityComparer<TData>())
            { }
        }

        #endregion
    }
}
