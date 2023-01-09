using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib
{
    public sealed class FullAttributeComponent<TValue> : AttributeComponent<TValue>
        where TValue : IComparable<TValue>
    {
        public readonly AttributeDomain<TValue> Domain;

        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.Full;

        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        static FullAttributeComponent()
        {
            AttributeComponent<TValue>.InitSetOperations(
                CONTENT_TYPE, new FullAttributeComponentOperationExecutersContainer());
        }

        public FullAttributeComponent(AttributeDomain<TValue> domain) 
            : base(new FullAttributeComponentPower())
        {
            Domain = domain;
        }

        public override IEnumerator<TValue> GetEnumerator()
        {
            return Domain.GetEnumerator();
        }

        private class FullAttributeComponentOperationExecutersContainer : SetOperationExecutersContainer
        {
            public FullAttributeComponentOperationExecutersContainer() : base(
                new FullAttributeComponentIntersectionOperator(),
                new FullAttributeComponentUnionOperator(),
                new FullAttributeComponentInclusionComparer(),
                new FullAttributeComponentEqualityComparer(),
                new FullAttributeComponentInclusionOrEqualityComparer())
            { }
        }

        private class FullAttributeComponentPower : AttributeComponentPower
        {
            internal override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }
        }
    }

    public sealed class FullAttributeComponentIntersectionOperator 
        : AttributeComponentAcceptor<AttributeComponent>, ICrossContentTypesAttributeComponentAcceptor<AttributeComponent>
    {
        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect(empty, first as FullAttributeComponent<TValue>);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect(nonFictional, first as FullAttributeComponent<TValue>);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect(first as FullAttributeComponent<TValue>, full);
        }
    }

    public sealed class FullAttributeComponentUnionOperator
        : AttributeComponentAcceptor<AttributeComponent>, ICrossContentTypesAttributeComponentAcceptor<AttributeComponent>
    {
        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union(empty, first as FullAttributeComponent<TValue>);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union(nonFictional, first as FullAttributeComponent<TValue>);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union(first as FullAttributeComponent<TValue>, full);
        }
    }

    public sealed class FullAttributeComponentInclusionComparer
        : AttributeComponentAcceptor<bool>, ICrossContentTypesAttributeComponentAcceptor<bool>
    {
        public bool Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionRules.Include(empty, first as FullAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionRules.Include(nonFictional, first as FullAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionRules.Include(first as FullAttributeComponent<TValue>, full);
        }
    }

    public sealed class FullAttributeComponentEqualityComparer
        : AttributeComponentAcceptor<bool>, ICrossContentTypesAttributeComponentAcceptor<bool>
    {
        public bool Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentEqualityRules.Equal(empty, first as FullAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentEqualityRules.Equal(nonFictional, first as FullAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentEqualityRules.Equal(first as FullAttributeComponent<TValue>, full);
        }
    }

    public sealed class FullAttributeComponentInclusionOrEqualityComparer
        : AttributeComponentAcceptor<bool>, ICrossContentTypesAttributeComponentAcceptor<bool>
    {
        public bool Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(empty, first as FullAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(nonFictional, first as FullAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first as FullAttributeComponent<TValue>, full);
        }
    }
}
