using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public abstract class InstantSetOperationExecutersContainer<TValue> : SetOperationExecutersContainer<TValue>
    {
        protected InstantUnaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _complementionOperator;
        protected InstantBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _intersectionOperator;
        protected InstantBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _unionOperator;
        protected InstantBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _differenceOperator;
        protected InstantBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _symmetricExceptionOperator;

        public InstantSetOperationExecutersContainer(
            InstantUnaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> complementionOperator,
            InstantBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> intersectionOperator,
            InstantBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> unionOperator,
            InstantBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> differenceOperator,
            InstantBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> symmetricExceptionOperator,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> inclusionComparer,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> equalityComparer,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> inclusionOrEquationComparer)
            : base(inclusionComparer,
                   equalityComparer,
                   inclusionOrEquationComparer)
        {
            _complementionOperator = complementionOperator;
            _intersectionOperator = intersectionOperator;
            _unionOperator = unionOperator;
            _differenceOperator = differenceOperator;
            _symmetricExceptionOperator = symmetricExceptionOperator;
        }

        public AttributeComponent<TValue> Complement(AttributeComponent<TValue> first)
        {
            return _complementionOperator.Accept(first);
        }

        public override AttributeComponent<TValue> Intersect(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _intersectionOperator.Accept(first, second);
        }

        public override AttributeComponent<TValue> Union(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _unionOperator.Accept(first, second);
        }

        public override AttributeComponent<TValue> Except(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _differenceOperator.Accept(first, second);
        }

        public override AttributeComponent<TValue> SymmetricExcept(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _symmetricExceptionOperator.Accept(first, second);
        }
    }
}
