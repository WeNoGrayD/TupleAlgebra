using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public abstract class InstantSetOperationExecutersContainer<TData> : SetOperationExecutersContainer<TData>
    {
        protected InstantUnaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> _complementionOperator;
        protected InstantBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> _intersectionOperator;
        protected InstantBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> _unionOperator;
        protected InstantBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> _differenceOperator;
        protected InstantBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> _symmetricExceptionOperator;

        public InstantSetOperationExecutersContainer(
            InstantUnaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> complementionOperator,
            InstantBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> intersectionOperator,
            InstantBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> unionOperator,
            InstantBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> differenceOperator,
            InstantBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> symmetricExceptionOperator,
            InstantBinaryAttributeComponentAcceptor<TData, bool> inclusionComparer,
            InstantBinaryAttributeComponentAcceptor<TData, bool> equalityComparer,
            InstantBinaryAttributeComponentAcceptor<TData, bool> inclusionOrEquationComparer)
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

        public AttributeComponent<TData> Complement(AttributeComponent<TData> first)
        {
            return _complementionOperator.Accept(first);
        }

        public override AttributeComponent<TData> Intersect(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _intersectionOperator.Accept(first, second);
        }

        public override AttributeComponent<TData> Union(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _unionOperator.Accept(first, second);
        }

        public override AttributeComponent<TData> Except(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _differenceOperator.Accept(first, second);
        }

        public override AttributeComponent<TData> SymmetricExcept(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _symmetricExceptionOperator.Accept(first, second);
        }
    }
}
