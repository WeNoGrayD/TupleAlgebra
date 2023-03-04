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

namespace TupleAlgebraClassLib
{
    public sealed class EmptyAttributeComponent<TData> : AttributeComponent<TData>
    {
        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.Empty;

        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        static EmptyAttributeComponent()
        {
            AttributeComponent<TData>.InitSetOperations(
                CONTENT_TYPE, new EmptyAttributeComponentOperationExecutersContainer());
        }

        public EmptyAttributeComponent(
            AttributeDomain<TData> domain,
            QueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(domain, new EmptyAttributeComponentPower(), queryProvider, queryExpression)
        { }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            yield break;
        }

        #region Instance methods

        protected override AttributeComponent<TData> ReproduceImpl(
            AttributeComponentFactoryArgs<TData> factoryArgs)
        {
            return new EmptyAttributeComponent<TData>(
                factoryArgs.Domain,
                factoryArgs.QueryProvider,
                factoryArgs.QueryExpression);
        }

        #endregion

        private class EmptyAttributeComponentOperationExecutersContainer : InstantSetOperationExecutersContainer<TData>
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
        }
    }
}
