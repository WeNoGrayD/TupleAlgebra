using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public abstract class SetOperationExecutersContainer<TData>
    {
        protected InstantBinaryAttributeComponentAcceptor<TData, bool> _inclusionComparer;
        protected InstantBinaryAttributeComponentAcceptor<TData, bool> _equalityComparer;
        protected InstantBinaryAttributeComponentAcceptor<TData, bool> _inclusionOrEqualityComparer;

        public SetOperationExecutersContainer(
            InstantBinaryAttributeComponentAcceptor<TData, bool> inclusionComparer,
            InstantBinaryAttributeComponentAcceptor<TData, bool> equalityComparer,
            InstantBinaryAttributeComponentAcceptor<TData, bool> inclusionOrEquationComparer)
        {
            _inclusionComparer = inclusionComparer;
            _equalityComparer = equalityComparer;
            _inclusionOrEqualityComparer = inclusionOrEquationComparer;
        }

        public abstract AttributeComponent<TData> Intersect(AttributeComponent<TData> first, AttributeComponent<TData> second);

        public abstract AttributeComponent<TData> Union(AttributeComponent<TData> first, AttributeComponent<TData> second);

        public abstract AttributeComponent<TData> Except(AttributeComponent<TData> first, AttributeComponent<TData> second);

        public abstract AttributeComponent<TData> SymmetricExcept(AttributeComponent<TData> first, AttributeComponent<TData> second);

        public bool Include(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _inclusionComparer.Accept(first, second);
        }

        public bool Equal(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _equalityComparer.Accept(first, second);
        }

        public bool IncludeOrEqual(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _inclusionOrEqualityComparer.Accept(first, second);
        }
    }
}
