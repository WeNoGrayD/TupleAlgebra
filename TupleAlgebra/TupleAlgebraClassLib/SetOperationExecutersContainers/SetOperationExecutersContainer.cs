using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public abstract class SetOperationExecutersContainer<TValue>
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
}
