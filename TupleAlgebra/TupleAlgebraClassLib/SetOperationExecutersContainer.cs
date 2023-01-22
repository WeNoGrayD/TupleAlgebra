using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    public abstract class SetOperationExecutersContainer<TValue>
        where TValue : IComparable<TValue>
    {
        protected InstantBinaryAttributeComponentAcceptor<TValue, bool> _inclusionComparer;
        protected InstantBinaryAttributeComponentAcceptor<TValue, bool> _equalityComparer;
        protected InstantBinaryAttributeComponentAcceptor<TValue, bool> _inclusionOrEqualityComparer;

        public SetOperationExecutersContainer(
            InstantBinaryAttributeComponentAcceptor<TValue, bool> inclusionComparer,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> equalityComparer,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> inclusionOrEquationComparer)
        {
            _inclusionComparer = inclusionComparer;
            _equalityComparer = equalityComparer;
            _inclusionOrEqualityComparer = inclusionOrEquationComparer;
        }

        public abstract AttributeComponent<TValue> Intersect(AttributeComponent<TValue> first, AttributeComponent<TValue> second);

        public abstract AttributeComponent<TValue> Union(AttributeComponent<TValue> first, AttributeComponent<TValue> second);

        public abstract AttributeComponent<TValue> Except(AttributeComponent<TValue> first, AttributeComponent<TValue> second);

        public abstract AttributeComponent<TValue> SymmetricExcept(AttributeComponent<TValue> first, AttributeComponent<TValue> second);

        public bool Include(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _inclusionComparer.Accept(first, second);
        }

        public bool Equal(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _equalityComparer.Accept(first, second);
        }

        public bool IncludeOrEqual(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _inclusionOrEqualityComparer.Accept(first, second);
        }
    }

    public abstract class InstantSetOperationExecutersContainer<TValue> : SetOperationExecutersContainer<TValue>
        where TValue : IComparable<TValue>
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

    public abstract class FactorySetOperationExecutersContainer<TValue> : SetOperationExecutersContainer<TValue>
        where TValue : IComparable<TValue>
    {
        protected AttributeComponentFactory<TValue> _componentFactory;

        protected FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _intersectionOperator;
        protected FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _unionOperator;
        protected FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _differenceOperator;
        protected FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _symmetricExceptionOperator;

        public FactorySetOperationExecutersContainer(
            AttributeComponentFactory<TValue> componentFactory,
            FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> intersectionOperator,
            FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> unionOperator,
            FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> differenceOperator,
            FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> symmetricExceptionOperator,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> inclusionComparer,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> equalityComparer,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> inclusionOrEquationComparer)
            : base(inclusionComparer, 
                   equalityComparer, 
                   inclusionOrEquationComparer)
        {
            _componentFactory = componentFactory;
            
            _intersectionOperator = intersectionOperator;
            _unionOperator = unionOperator;
            _differenceOperator = differenceOperator;
            _symmetricExceptionOperator = symmetricExceptionOperator;
        }

        public override AttributeComponent<TValue> Intersect(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _intersectionOperator.Accept(first, second, _componentFactory);
        }

        public override AttributeComponent<TValue> Union(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _unionOperator.Accept(first, second, _componentFactory);
        }

        public override AttributeComponent<TValue> Except(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _differenceOperator.Accept(first, second, _componentFactory);
        }

        public override AttributeComponent<TValue> SymmetricExcept(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _symmetricExceptionOperator.Accept(first, second, _componentFactory);
        }
    }
}
