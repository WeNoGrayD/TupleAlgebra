using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.FullAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutersContainers;

namespace TupleAlgebraClassLib
{
    public sealed class FullAttributeComponent<TValue> : AttributeComponent<TValue>
    {
        public static FullAttributeComponent<TValue> Instance { get; } =
            new FullAttributeComponent<TValue>();

        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.Full;

        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        static FullAttributeComponent()
        {
            AttributeComponent<TValue>.InitSetOperations(
                CONTENT_TYPE, new FullAttributeComponentOperationExecutersContainer());
        }

        private FullAttributeComponent() 
            : base(new FullAttributeComponentPower())
        { }

        public override IEnumerator<TValue> GetEnumeratorImpl()
        {
            yield break;
        }

        private class FullAttributeComponentOperationExecutersContainer : InstantSetOperationExecutersContainer<TValue>
        {
            public FullAttributeComponentOperationExecutersContainer() : base(
                new FullAttributeComponentComplementionOperator<TValue>(),
                new FullAttributeComponentIntersectionOperator<TValue>(),
                new FullAttributeComponentUnionOperator<TValue>(),
                new FullAttributeComponentExceptionOperator<TValue>(),
                new FullAttributeComponentSymmetricExceptionOperator<TValue>(),
                new FullAttributeComponentInclusionComparer<TValue>(),
                new FullAttributeComponentEqualityComparer<TValue>(),
                new FullAttributeComponentInclusionOrEqualityComparer<TValue>())
            { }
        }

        private class FullAttributeComponentPower : AttributeComponentPower
        {
            internal override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }
        }
    }
}
