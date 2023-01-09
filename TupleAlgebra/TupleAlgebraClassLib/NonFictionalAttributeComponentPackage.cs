using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TupleAlgebraClassLib
{
    /// <summary>
    /// Тип непустой компоненты атрибута.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class NonFictionalAttributeComponent<TValue> : AttributeComponent<TValue>
        where TValue : IComparable<TValue>
    {
        private static Dictionary<string, SetOperationExecutersContainer> _nonFictionalSpecificSetOperations;

        public readonly AttributeDomain<TValue> Domain;

        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.NonFictional;

        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        protected abstract string NatureType { get; }

        static NonFictionalAttributeComponent()
        {
            AttributeComponent<TValue>.InitSetOperations(
                CONTENT_TYPE, new NonFictionalAttributeComponentOperationExecutersContainer());

            _nonFictionalSpecificSetOperations = new Dictionary<string, SetOperationExecutersContainer>();
        }

        public NonFictionalAttributeComponent(
            AttributeDomain<TValue> domain,
            NonFictionalAttributeComponentPower power)
            : base(power)
        {
            Domain = domain;
        }

        protected static void InitSetOperations(
            string natureType,
            SetOperationExecutersContainer setOperations)
        {
            _nonFictionalSpecificSetOperations[natureType] = setOperations;
        }

        internal AttributeComponent<TValue> IntersectWith(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].Intersect(this, second) as AttributeComponent<TValue>;
        }

        internal AttributeComponent<TValue> UnionWith(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].Union(this, second) as AttributeComponent<TValue>;
        }

        internal bool Includes(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].Include(this, second);
        }

        internal bool EqualsTo(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].Equal(this, second);
        }

        internal bool IncludesOrEqualsTo(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].IncludeOrEqual(this, second);
        }

        public abstract bool IsEmpty();

        public bool IsFull()
        {
            return this.Domain == this;
        }

        private class NonFictionalAttributeComponentOperationExecutersContainer : SetOperationExecutersContainer
        {
            public NonFictionalAttributeComponentOperationExecutersContainer() : base(
                new NonFictionalAttributeComponentIntersectionOperator<TValue>(),
                new NonFictionalAttributeComponentUnionOperator(),
                new NonFictionalAttributeComponentInclusionComparer(),
                new NonFictionalAttributeComponentEqualityComparer<TValue>(),
                new NonFictionalAttributeComponentInclusionOrEqualityComparer())
            { }
        }

        public abstract class NonFictionalAttributeComponentPower : AttributeComponentPower
        {
            internal override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

            protected abstract int CompareToSame(dynamic second);

            public override sealed int CompareTo(AttributeComponentPower second)
            {
                int comparisonResult = base.CompareTo(second);
                if (comparisonResult == 0)
                    comparisonResult = this.CompareToSame(second);

                return comparisonResult;
            }
        }
    }

        public class NonFictionalAttributeComponentIntersectionOperator<TValue> 
        : AttributeComponentAcceptor<AttributeComponent>, ICrossContentTypesAttributeComponentAcceptor<AttributeComponent>
        , IAttributeComponentAcceptor<EmptyAttributeComponent<TValue>, AttributeComponent>
            where TValue : IComparable<TValue>
    {
        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect(empty, first as NonFictionalAttributeComponent<TValue>);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return null;
        }

        public AttributeComponent Accept(AttributeComponent first, EmptyAttributeComponent<TValue> empty)
        {
            return EmptyAttributeComponent<TValue>.Instance;
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentIntersectionRules.Intersect(first as NonFictionalAttributeComponent<TValue>, full);
        }
    }

    public class NonFictionalAttributeComponentUnionOperator
        : AttributeComponentAcceptor<AttributeComponent>, ICrossContentTypesAttributeComponentAcceptor<AttributeComponent>
    {
        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union(empty, first as NonFictionalAttributeComponent<TValue>);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union(first as NonFictionalAttributeComponent<TValue>, nonFictional);
        }

        public AttributeComponent Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentUnionRules.Union(first as NonFictionalAttributeComponent<TValue>, full);
        }
    }

    public class NonFictionalAttributeComponentInclusionComparer 
        : AttributeComponentAcceptor<bool>, ICrossContentTypesAttributeComponentAcceptor<bool>
    {
        public bool Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionRules.Include(empty, first as NonFictionalAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionRules.Include(first as NonFictionalAttributeComponent<TValue>, nonFictional);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionRules.Include(first as NonFictionalAttributeComponent<TValue>, full);
        }
    }

    public class NonFictionalAttributeComponentEqualityComparer<TValue>
        : AttributeComponentAcceptor<bool>, ICrossContentTypesAttributeComponentAcceptor<bool>
        , IAttributeComponentAcceptor<NonFictionalAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public bool Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentEqualityRules.Equal(empty, first as NonFictionalAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return false;
            return AttributeComponentEqualityRules.Equal(first as NonFictionalAttributeComponent<TValue>, nonFictional);
        }

        public bool Accept(AttributeComponent first, NonFictionalAttributeComponent<TValue> nonFictional)
        {
            return AttributeComponentEqualityRules.Equal(first as NonFictionalAttributeComponent<TValue>, nonFictional);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentEqualityRules.Equal(first as NonFictionalAttributeComponent<TValue>, full);
        }
    }

    public class NonFictionalAttributeComponentInclusionOrEqualityComparer
        : AttributeComponentAcceptor<bool>, ICrossContentTypesAttributeComponentAcceptor<bool>
    {
        public bool Accept<TValue>(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> empty)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(empty, first as NonFictionalAttributeComponent<TValue>);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first as NonFictionalAttributeComponent<TValue>, nonFictional);
        }

        public bool Accept<TValue>(AttributeComponent<TValue> first, FullAttributeComponent<TValue> full)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first as NonFictionalAttributeComponent<TValue>, full);
        }
    }
}
