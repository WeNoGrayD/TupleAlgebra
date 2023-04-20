﻿using System;
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
