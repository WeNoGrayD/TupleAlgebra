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

    public sealed class EmptyAttributeComponentComplementionOperator<TValue>
        : InstantUnaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>>,
          IInstantAttributeComponentAcceptor<TValue, EmptyAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public AttributeComponent<TValue> Accept(EmptyAttributeComponent<TValue> first)
        {
            return AttributeComponentComplementionRules.Complement(first);
        }
    }

    public sealed class EmptyAttributeComponentIntersectionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, EmptyAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first, 
            EmptyAttributeComponent<TValue> second)
        {
            return first;
        }

        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first, 
            NonFictionalAttributeComponent<TValue> second)
        {
            return first;
        }

        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first, 
            FullAttributeComponent<TValue> second)
        {
            return first;
        }
    }

    public sealed class EmptyAttributeComponentUnionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, EmptyAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first, 
            EmptyAttributeComponent<TValue> second)
        {
            return first;
        }

        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
        {
            return second;
        }

        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first, 
            FullAttributeComponent<TValue> second)
        {
            return second;
        }
    }

    public sealed class EmptyAttributeComponentExceptionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, EmptyAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
        {
            return first;
        }

        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
        {
            return first;
        }

        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
        {
            return first;
        }
    }

    public sealed class EmptyAttributeComponentSymmetricExceptionOperator<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, EmptyAttributeComponent<TValue>, AttributeComponent<TValue>>
        where TValue : IComparable<TValue>
    {
        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
        {
            return first;
        }

        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
        {
            return second;
        }

        public override AttributeComponent<TValue> Accept(
            EmptyAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
        {
            return second;
        }
    }

    public sealed class EmptyAttributeComponentInclusionComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, EmptyAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(EmptyAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return false;
        }

        public override bool Accept(EmptyAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return false;
        }

        public override bool Accept(EmptyAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return false;
        }
    }

    public sealed class EmptyAttributeComponentEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, EmptyAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(EmptyAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return true;
        }

        public override bool Accept(EmptyAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return false;
        }

        public override bool Accept(EmptyAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return false;
        }
    }

    public sealed class EmptyAttributeComponentInclusionOrEqualityComparer<TValue>
        : CrossContentTypesInstantAttributeComponentAcceptor<TValue, EmptyAttributeComponent<TValue>, bool>
        where TValue : IComparable<TValue>
    {
        public override bool Accept(EmptyAttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second)
        {
            return true;
        }

        public override bool Accept(EmptyAttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second)
        {
            return false;
        }

        public override bool Accept(EmptyAttributeComponent<TValue> first, FullAttributeComponent<TValue> second)
        {
            return false;
        }
    }
}
