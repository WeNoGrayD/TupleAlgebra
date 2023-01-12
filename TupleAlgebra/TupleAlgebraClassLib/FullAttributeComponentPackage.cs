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

        private class FullAttributeComponentOperationExecutersContainer : InstantSetOperationExecutersContainer<TValue>
        {
            public FullAttributeComponentOperationExecutersContainer() : base(
                _baseFactory,
                new FullAttributeComponentComplementionOperator<TValue>(),
                new FullAttributeComponentIntersectionOperator<TValue>(),
                new FullAttributeComponentUnionOperator<TValue>(),
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

    public sealed class FullAttributeComponentComplementionOperator<TValue>
        : FactoryUnaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>,
          IFactoryAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, EmptyAttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public EmptyAttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentComplementionRules.Complement(first)(factory);
        }
    }

    public sealed class FullAttributeComponentIntersectionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first, 
            EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentIntersectionRules.Intersect(second, first as FullAttributeComponent<TValue>);
        }

        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first, 
            NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentIntersectionRules.Intersect(second, first as FullAttributeComponent<TValue>);
        }

        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first, 
            FullAttributeComponent<TValue> second)
        {
            return AttributeComponentIntersectionRules.Intersect(first as FullAttributeComponent<TValue>, second);
        }
    }

    public sealed class FullAttributeComponentUnionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentUnionRules.Union(second, first as FullAttributeComponent<TValue>);
        }

        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first, 
            NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentUnionRules.Union(second, first as FullAttributeComponent<TValue>);
        }

        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first, 
            FullAttributeComponent<TValue> second)
        {
            return AttributeComponentUnionRules.Union(first as FullAttributeComponent<TValue>, second);
        }
    }

    public sealed class FullAttributeComponentInclusionComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(FullAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionRules.Include(second, first as FullAttributeComponent<TValue>);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionRules.Include(second, first as FullAttributeComponent<TValue>);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionRules.Include(first as FullAttributeComponent<TValue>, second);
        }
    }

    public sealed class FullAttributeComponentEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(FullAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentEqualityRules.Equal(second, first as FullAttributeComponent<TValue>);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentEqualityRules.Equal(second, first as FullAttributeComponent<TValue>);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return AttributeComponentEqualityRules.Equal(first as FullAttributeComponent<TValue>, second);
        }
    }

    public sealed class FullAttributeComponentInclusionOrEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(FullAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(second, first as FullAttributeComponent<TValue>);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(second, first as FullAttributeComponent<TValue>);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first as FullAttributeComponent<TValue>, second);
        }
    }
}
