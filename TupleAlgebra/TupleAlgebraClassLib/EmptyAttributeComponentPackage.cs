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
                new EmptyAttributeComponentIntersectionOperator(),
                new EmptyAttributeComponentUnionOperator(),
                new EmptyAttributeComponentInclusionComparer(),
                new EmptyAttributeComponentEqualityComparer(),
                new EmptyAttributeComponentInclusionOrEqualityComparer())
            { }
        }

        private class EmptyAttributeComponentPower : AttributeComponentPower
        {
            internal override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }
        }
    }

    public sealed class EmptyAttributeComponentIntersectionOperator
        : AttributeComponentAcceptor<AttributeComponent>, ICrossContentTypesAttributeComponentAcceptor<AttributeComponent>
    {
        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect(empty, first as EmptyAttributeComponent<TValue>);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect(first as EmptyAttributeComponent<TValue>, nonFictional);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect(first as EmptyAttributeComponent<TValue>, full);
        }
    }

    public sealed class EmptyAttributeComponentUnionOperator
        : AttributeComponentAcceptor<AttributeComponent>, ICrossContentTypesAttributeComponentAcceptor<AttributeComponent>
    {
        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union(empty, first as EmptyAttributeComponent<TValue>);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union(first as EmptyAttributeComponent<TValue>, nonFictional);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union(first as EmptyAttributeComponent<TValue>, full);
        }
    }

    public sealed class EmptyAttributeComponentInclusionComparer
        : AttributeComponentAcceptor<bool>, ICrossContentTypesAttributeComponentAcceptor<bool>
    {
        public bool Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionRules.Include(empty, first as EmptyAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionRules.Include(first as EmptyAttributeComponent<TValue>, nonFictional);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionRules.Include(first as EmptyAttributeComponent<TValue>, full);
        }
    }

    public sealed class EmptyAttributeComponentEqualityComparer
        : AttributeComponentAcceptor<bool>, ICrossContentTypesAttributeComponentAcceptor<bool>
    {
        public bool Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentEqualityRules.Equal(empty, first as EmptyAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentEqualityRules.Equal(first as EmptyAttributeComponent<TValue>, nonFictional);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentEqualityRules.Equal(first as EmptyAttributeComponent<TValue>, full);
        }
    }

    public sealed class EmptyAttributeComponentInclusionOrEqualityComparer
        : AttributeComponentAcceptor<bool>, ICrossContentTypesAttributeComponentAcceptor<bool>
    {
        public bool Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(empty, first as EmptyAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first as EmptyAttributeComponent<TValue>, nonFictional);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first as EmptyAttributeComponent<TValue>, full);
        }
    }
}
