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

        public override IEnumerator<TValue> GetEnumerator()
        {
            yield break;
            //return Domain.GetEnumerator();
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

    public sealed class FullAttributeComponentComplementionOperator<TValue>
        : InstantUnaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>,
          IInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public AttributeComponent<TValue> Accept(FullAttributeComponent<TValue> first)
        {
            return AttributeComponentComplementionRules.Complement(first);
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

    public sealed class FullAttributeComponentExceptionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentExceptRules.Except(first, second);
        }

        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentExceptRules.Except(first, second);
        }

        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
        {
            return AttributeComponentExceptRules.Except(first, second);
        }
    }

    public sealed class FullAttributeComponentSymmetricExceptionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
        {
            return AttributeComponentSymmetricExceptionRules.SymmetricExcept(second, first);
        }

        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
        {
            return AttributeComponentSymmetricExceptionRules.SymmetricExcept(second, first);
        }

        public override AttributeComponent<TValue> Accept(
            FullAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
        {
            return AttributeComponentSymmetricExceptionRules.SymmetricExcept(first, second);
        }
    }

    public sealed class FullAttributeComponentInclusionComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(FullAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return true;
            //return AttributeComponentInclusionRules.Include(second, first as FullAttributeComponent<TValue>);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return true;
            //return AttributeComponentInclusionRules.Include(second, first as FullAttributeComponent<TValue>);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return false;
            //return AttributeComponentInclusionRules.Include(first as FullAttributeComponent<TValue>, second);
        }
    }

    public sealed class FullAttributeComponentEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(FullAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return false;
            //return AttributeComponentEqualityRules.Equal(second, first);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return false;
            //return AttributeComponentEqualityRules.Equal(second, first);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return true;
            //return AttributeComponentEqualityRules.Equal(first, second);
        }
    }

    public sealed class FullAttributeComponentInclusionOrEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, FullAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(FullAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return true;
            //return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(second, first);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return true;
            //return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(second, first);
        }

        public override bool Accept(FullAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return true;
            //return AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(first, second);
        }
    }
}
