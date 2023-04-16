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

namespace TupleAlgebraClassLib
{
    /// <summary>
    /// Полная фиктивная компонента атрибута.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public sealed class FullAttributeComponent<TData> : AttributeComponent<TData>
    {
        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.Full;

        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static FullAttributeComponent()
        {
            AttributeComponent<TData>.InitSetOperations(
                CONTENT_TYPE, new FullAttributeComponentOperationExecutersContainer());
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="queryProvider"></param>
        /// <param name="queryExpression"></param>
        public FullAttributeComponent(
            AttributeDomain<TData> domain,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null) 
            : base(domain, new FullAttributeComponentPower(), queryProvider, queryExpression)
        { }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            return Domain.Universum.GetEnumerator();
        }

        #region Instance methods

        protected override AttributeComponent<TData> ReproduceImpl(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return new FullAttributeComponent<TData>(
                factoryArgs.Domain as AttributeDomain<TData>,
                factoryArgs.QueryProvider,
                factoryArgs.QueryExpression);
        }

        #endregion

        #region Inner classes

        private class FullAttributeComponentOperationExecutersContainer : InstantSetOperationExecutersContainer<TData>
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
