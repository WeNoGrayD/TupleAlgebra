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

        internal AttributeComponent<TValue> ExceptWith(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].Except(this, second) as AttributeComponent<TValue>;
        }

        internal AttributeComponent<TValue> SymmetricExceptWith(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].SymmetricExcept(this, second) as AttributeComponent<TValue>;
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

        private sealed class NonFictionalAttributeComponentOperationExecutersContainer : InstantSetOperationExecutersContainer<TValue>
        {
            public NonFictionalAttributeComponentOperationExecutersContainer() : base(
                new NonFictionalAttributeComponentComplementionOperator<TValue>(),
                new NonFictionalAttributeComponentIntersectionOperator<TValue>(),
                new NonFictionalAttributeComponentUnionOperator<TValue>(),
                new NonFictionalAttributeComponentExceptionOperator<TValue>(),
                new NonFictionalAttributeComponentSymmetricExceptionOperator<TValue>(),
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

    public sealed class NonFictionalAttributeComponentComplementionOperator<TValue>
        : InstantUnaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>,
          IInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public AttributeComponent<TValue> Accept(NonFictionalAttributeComponent<TValue> first)
        {
            return AttributeComponentComplementionRules.Complement(first);
        }
    }

    public sealed class NonFictionalAttributeComponentIntersectionOperator<TValue> 
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first, 
            EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentIntersectionRules.Intersect(second, first);
        }

        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first, 
            NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentIntersectionRules.Intersect(first, second);
        }

        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first, 
            FullAttributeComponent<TValue> second)
        {
            return AttributeComponentIntersectionRules.Intersect(first, second);
        }
    }

    public sealed class NonFictionalAttributeComponentUnionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first, 
            EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentUnionRules.Union(second, first);
        }

        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first, 
            NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentUnionRules.Union(first, second);
        }

        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first, 
            FullAttributeComponent<TValue> second)
        {
            return AttributeComponentUnionRules.Union(first, second);
        }
    }

    public sealed class NonFictionalAttributeComponentExceptionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentExceptRules.Except(first, second);
        }

        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentExceptRules.Except(first, second);
        }

        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
        {
            return AttributeComponentExceptRules.Except(first, second);
        }
    }

    public sealed class NonFictionalAttributeComponentSymmetricExceptionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentSymmetricExceptionRules.SymmetricExcept(second, first);
        }

        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentSymmetricExceptionRules.SymmetricExcept(second, first);
        }

        public override AttributeComponent<TValue> Accept(
            NonFictionalAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
        {
            return AttributeComponentSymmetricExceptionRules.SymmetricExcept(first, second);
        }
    }

    public sealed class NonFictionalAttributeComponentInclusionComparer<TValue> 
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(NonFictionalAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return true;
            //return !AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(second, first);
        }

        public override bool Accept(NonFictionalAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return first.Includes(second);
            //return AttributeComponentInclusionRules.Include(first, second);
        }

        public override bool Accept(NonFictionalAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return false;
            //return AttributeComponentInclusionRules.Include(first, second);
        }
    }

    public sealed class NonFictionalAttributeComponentEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(NonFictionalAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return false;
            //return AttributeComponentEqualityRules.Equal(second, first);
        }

        public override bool Accept(NonFictionalAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return first.EqualsTo(second);
            //return AttributeComponentEqualityRules.Equal(first, second);
        }

        public override bool Accept(NonFictionalAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return false;
            //return AttributeComponentEqualityRules.Equal(first, second);
        }
    }

    public sealed class NonFictionalAttributeComponentInclusionOrEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, NonFictionalAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(NonFictionalAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return true;
            //return !AttributeComponentInclusionRules.Include(second, first);
        }

        public override bool Accept(NonFictionalAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return first.IncludesOrEqualsTo(second);
            //return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first, second);
        }

        public override bool Accept(NonFictionalAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return false;
            //return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first, second);
        }
    }
}
