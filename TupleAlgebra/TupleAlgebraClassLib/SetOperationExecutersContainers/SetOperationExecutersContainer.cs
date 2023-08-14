using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public abstract class SetOperationExecutersContainer<BTOperand, CTOperand>
        : ISetOperationExecutersContainer<BTOperand, CTOperand>
        where CTOperand: BTOperand
    {
        #region Instance fields

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, bool>> _inclusionComparer;

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, bool>> _equalityComparer;

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, bool>> _inclusionOrEqualityComparer;

        #endregion

        #region Instance properties

        protected IInstantBinaryOperator<CTOperand, BTOperand, bool> InclusionComparer
        { get => _inclusionComparer.Value; }

        protected IInstantBinaryOperator<CTOperand, BTOperand, bool> EqualityComparer
        { get => _equalityComparer.Value; }

        protected IInstantBinaryOperator<CTOperand, BTOperand, bool> InclusionOrEqualityComparer
        { get => _inclusionOrEqualityComparer.Value; }

        #endregion

        #region Constructors

        public SetOperationExecutersContainer(
            Func<IInstantBinaryOperator<CTOperand, BTOperand, bool>> inclusionComparer,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, bool>> equalityComparer,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, bool>> inclusionOrEquationComparer)
        {
            _inclusionComparer = new Lazy<IInstantBinaryOperator<CTOperand, BTOperand, bool>>(
                inclusionComparer);
            _equalityComparer = new Lazy<IInstantBinaryOperator<CTOperand, BTOperand, bool>>(
                equalityComparer);
            _inclusionOrEqualityComparer = new Lazy<IInstantBinaryOperator<CTOperand, BTOperand, bool>>(
                inclusionOrEquationComparer);

            return;
        }

        #endregion

        #region Instance methods

        public abstract BTOperand Complement(CTOperand first);

        public abstract BTOperand Intersect(CTOperand first, BTOperand second);

        public abstract BTOperand Union(CTOperand first, BTOperand second);

        public abstract BTOperand Except(CTOperand first, BTOperand second);

        public abstract BTOperand SymmetricExcept(CTOperand first, BTOperand second);

        public bool Include(CTOperand first, BTOperand second)
        {
            return InclusionComparer.Accept(first, second);
        }

        public bool Equal(CTOperand first, BTOperand second)
        {
            return EqualityComparer.Accept(first, second);
        }

        public bool IncludeOrEqual(CTOperand first, BTOperand second)
        {
            return InclusionOrEqualityComparer.Accept(first, second);
        }

        #endregion
    }
}
