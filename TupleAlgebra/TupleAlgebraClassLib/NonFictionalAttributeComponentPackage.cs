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
        private static Dictionary<string, SetOperationExecutersContainer<TValue>> _nonFictionalSpecificSetOperations;

        public readonly AttributeDomain<TValue> Domain;

        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.NonFictional;

        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        protected abstract string NatureType { get; }

        static NonFictionalAttributeComponent()
        {
            AttributeComponent<TValue>.InitSetOperations(
                CONTENT_TYPE, new NonFictionalAttributeComponentOperationExecutersContainer());

            _nonFictionalSpecificSetOperations = new Dictionary<string, SetOperationExecutersContainer<TValue>>();
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
            SetOperationExecutersContainer<TValue> setOperations)
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

        private sealed class NonFictionalAttributeComponentOperationExecutersContainer : SetOperationExecutersContainer<TValue>
        {
            public NonFictionalAttributeComponentOperationExecutersContainer() : base(
                new NonFictionalAttributeComponentIntersectionOperator<TValue>(),
                new NonFictionalAttributeComponentUnionOperator<TValue>(),
                new NonFictionalAttributeComponentInclusionComparer<TValue>(),
                new NonFictionalAttributeComponentEqualityComparer<TValue>(),
                new NonFictionalAttributeComponentInclusionOrEqualityComparer<TValue>())
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
        : CrossContentTypesFactoryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            AttributeComponent<TValue> first, 
            EmptyAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentIntersectionRules.Intersect(second, first as NonFictionalAttributeComponent<TValue>);
        }

        public override AttributeComponent<TValue> Accept(
            AttributeComponent<TValue> first, 
            NonFictionalAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentIntersectionRules.Intersect(first as NonFictionalAttributeComponent<TValue>, second);
        }

        public override AttributeComponent<TValue> Accept(
            AttributeComponent<TValue> first, 
            FullAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentIntersectionRules.Intersect(first as NonFictionalAttributeComponent<TValue>, second);
        }
    }

    public class NonFictionalAttributeComponentUnionOperator<TValue>
        //: CrossContentTypesFactoryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>
          : FactoryAttributeComponentAcceptor<AttributeComponent<TValue>>,
          IFactoryAttributeComponentAcceptor<EmptyAttributeComponent<TValue>, AttributeComponent<TValue>>,
          IFactoryAttributeComponentAcceptor<NonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>,
          IFactoryAttributeComponentAcceptor<FullAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        AttributeComponent<TValue> IFactoryAttributeComponentAcceptor<EmptyAttributeComponent<TValue>, AttributeComponent<TValue>>.Accept<T>(
            AttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentUnionRules.Union(second, first as NonFictionalAttributeComponent<TValue>);
        }
        /*
        public override AttributeComponent<TValue> Accept(
            AttributeComponent<TValue> first, 
            EmptyAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentUnionRules.Union(second, first as NonFictionalAttributeComponent<TValue>);
        }

        public override AttributeComponent<TValue> Accept(
            AttributeComponent<TValue> first, 
            NonFictionalAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentUnionRules.Union(first as NonFictionalAttributeComponent<TValue>, second);
        }

        public override AttributeComponent<TValue> Accept(
            AttributeComponent<TValue> first, 
            FullAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            return AttributeComponentUnionRules.Union(first as NonFictionalAttributeComponent<TValue>, second);
        }
        */
    }

    public class NonFictionalAttributeComponentInclusionComparer<TValue> 
        : CrossContentTypesAttributeComponentAcceptor<TValue, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionRules.Include(second, first as NonFictionalAttributeComponent<TValue>);
        }

        public override bool Accept(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionRules.Include(first as NonFictionalAttributeComponent<TValue>, second);
        }

        public override bool Accept(AttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionRules.Include(first as NonFictionalAttributeComponent<TValue>, second);
        }
    }

    public class NonFictionalAttributeComponentEqualityComparer<TValue>
        : CrossContentTypesAttributeComponentAcceptor<TValue, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentEqualityRules.Equal(second, first as NonFictionalAttributeComponent<TValue>);
        }

        public override bool Accept(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentEqualityRules.Equal(first as NonFictionalAttributeComponent<TValue>, second);
        }

        public override bool Accept(AttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return AttributeComponentEqualityRules.Equal(first as NonFictionalAttributeComponent<TValue>, second);
        }
    }

    public class NonFictionalAttributeComponentInclusionOrEqualityComparer<TValue>
        : CrossContentTypesAttributeComponentAcceptor<TValue, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(second, first as NonFictionalAttributeComponent<TValue>);
        }

        public override bool Accept(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first as NonFictionalAttributeComponent<TValue>, second);
        }

        public override bool Accept(AttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first as NonFictionalAttributeComponent<TValue>, second);
        }
    }
}
