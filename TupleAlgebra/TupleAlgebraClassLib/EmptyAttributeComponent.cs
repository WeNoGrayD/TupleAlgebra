using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutersContainers;

namespace TupleAlgebraClassLib
{
    public sealed class EmptyAttributeComponent<TValue> : AttributeComponent<TValue>
    {
        public static EmptyAttributeComponent<TValue> Instance { get; } =
            new EmptyAttributeComponent<TValue>();

        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.Empty;

        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        static EmptyAttributeComponent()
        {
            AttributeComponent<TValue>.InitSetOperations(
                CONTENT_TYPE, new EmptyAttributeComponentOperationExecutersContainer());
        }

        private EmptyAttributeComponent()
            : base(new EmptyAttributeComponentPower())
        { }

        public override IEnumerator<TValue> GetEnumeratorImpl()
        {
            yield break;
        }

        private class EmptyAttributeComponentOperationExecutersContainer : InstantSetOperationExecutersContainer<TValue>
        {
            public EmptyAttributeComponentOperationExecutersContainer() : base(
                new EmptyAttributeComponentComplementionOperator<TValue>(),
                new EmptyAttributeComponentIntersectionOperator<TValue>(),
                new EmptyAttributeComponentUnionOperator<TValue>(),
                new EmptyAttributeComponentExceptionOperator<TValue>(),
                new EmptyAttributeComponentSymmetricExceptionOperator<TValue>(),
                new EmptyAttributeComponentInclusionComparer<TValue>(),
                new EmptyAttributeComponentEqualityComparer<TValue>(),
                new EmptyAttributeComponentInclusionOrEqualityComparer<TValue>())
            { }
        }

        private class EmptyAttributeComponentPower : AttributeComponentPower
        {
            internal override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }
        }
    }
}
