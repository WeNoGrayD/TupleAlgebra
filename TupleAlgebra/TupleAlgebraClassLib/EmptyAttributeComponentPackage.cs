using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib
{
    public sealed class EmptyAttributeComponent<TValue> : AttributeComponent<TValue>
        where TValue : IComparable<TValue>
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

        public override IEnumerator<TValue> GetEnumerator()
        {
            yield break;
        }

        private class EmptyAttributeComponentOperationExecutersContainer : SetOperationExecutersContainer
        {
            public EmptyAttributeComponentOperationExecutersContainer() : base(
                new EmptyAttributeComponentIntersectionOperator<TValue>(),
                new EmptyAttributeComponentUnionOperator<TValue>(),
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

    public sealed class EmptyAttributeComponentIntersectionOperator<TValue>
        : CrossContentTypesFactoryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            AttributeComponent first, 
            EmptyAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentIntersectionRules.Intersect(second, first as EmptyAttributeComponent<TValue>);
        }

        public override AttributeComponent<TValue> Accept(
            AttributeComponent first, 
            NonFictionalAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentIntersectionRules.Intersect(first as EmptyAttributeComponent<TValue>, second);
        }

        public override AttributeComponent<TValue> Accept(
            AttributeComponent first, 
            FullAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentIntersectionRules.Intersect(first as EmptyAttributeComponent<TValue>, second);
        }
    }

    public sealed class EmptyAttributeComponentUnionOperator<TValue>
        : CrossContentTypesFactoryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public AttributeComponent<TValue> Accept(
            AttributeComponent<TValue> first, 
            EmptyAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentUnionRules.Union(second, first as EmptyAttributeComponent<TValue>);
        }

        public AttributeComponent<TValue> Accept(
            AttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentUnionRules.Union(first as EmptyAttributeComponent<TValue>, second);
        }

        public AttributeComponent<TValue> Accept(
            AttributeComponent<TValue> first, 
            FullAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentIntersectionRules.Intersect(first as EmptyAttributeComponent<TValue>, second);
        }
    }

    public sealed class EmptyAttributeComponentInclusionComparer<TValue>
        : ICrossContentTypesAttributeComponentAcceptor<TValue, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(AttributeComponent first, EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionRules.Include(second, first as EmptyAttributeComponent<TValue>);
        }

        public override bool Accept(AttributeComponent first, NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionRules.Include(first as EmptyAttributeComponent<TValue>, second);
        }

        public override bool Accept(AttributeComponent first, FullAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionRules.Include(first as EmptyAttributeComponent<TValue>, second);
        }
    }

    public sealed class EmptyAttributeComponentEqualityComparer<TValue>
        : ICrossContentTypesAttributeComponentAcceptor<TValue, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(AttributeComponent first, EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentEqualityRules.Equal(second, first as EmptyAttributeComponent<TValue>);
        }

        public override bool Accept(AttributeComponent first, NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentEqualityRules.Equal(first as EmptyAttributeComponent<TValue>, second);
        }

        public override bool Accept(AttributeComponent first, FullAttributeComponent<TValue> second)
        {
            return AttributeComponentEqualityRules.Equal(first as EmptyAttributeComponent<TValue>, second);
        }
    }

    public sealed class EmptyAttributeComponentInclusionOrEqualityComparer<TValue>
        : ICrossContentTypesAttributeComponentAcceptor<TValue, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(AttributeComponent first, EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(second, first as EmptyAttributeComponent<TValue>);
        }

        public override bool Accept(AttributeComponent first, NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first as EmptyAttributeComponent<TValue>, second);
        }

        public override bool Accept(AttributeComponent first, FullAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first as EmptyAttributeComponent<TValue>, second);
        }
    }
}
