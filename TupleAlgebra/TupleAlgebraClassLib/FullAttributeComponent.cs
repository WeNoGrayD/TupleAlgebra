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
    public sealed class FullAttributeComponent<TData> : AttributeComponent<TData>
    {
        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.Full;

        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        static FullAttributeComponent()
        {
            AttributeComponent<TData>.InitSetOperations(
                CONTENT_TYPE, new FullAttributeComponentOperationExecutersContainer());
        }

        public FullAttributeComponent(
            AttributeDomain<TData> domain,
            QueryProvider queryProvider = null,
            Expression queryExpression = null) 
            : base(domain, new FullAttributeComponentPower(), queryProvider, queryExpression)
        { }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            return Domain.Universum.GetEnumerator();
        }

        #region Instance methods

        protected override AttributeComponent<TData> ReproduceImpl(
            AttributeComponentFactoryArgs<TData> factoryArgs)
        {
            return new FullAttributeComponent<TData>(
                factoryArgs.Domain,
                factoryArgs.QueryProvider,
                factoryArgs.QueryExpression);
        }

        #endregion

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

        private class FullAttributeComponentPower : AttributeComponentPower
        {
            internal override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }
        }
    }
}
