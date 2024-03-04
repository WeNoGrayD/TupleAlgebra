using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.SetOperationExecutorsContainers
{
    public abstract class SetOperationExecutorsContainer<BTOperand, CTOperand>
        : ISetOperationExecutorsContainer<BTOperand>
        where CTOperand: class, BTOperand
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

        public SetOperationExecutorsContainer(
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

        public abstract BTOperand Complement(BTOperand first);

        public abstract BTOperand Intersect(BTOperand first, BTOperand second);

        public abstract BTOperand Union(BTOperand first, BTOperand second);

        public abstract BTOperand Except(BTOperand first, BTOperand second);

        public abstract BTOperand SymmetricExcept(BTOperand first, BTOperand second);

        public bool Include(BTOperand first, BTOperand second)
        {
            return InclusionComparer.Accept((first as CTOperand)!, second);
        }

        public bool Equal(BTOperand first, BTOperand second)
        {
            return EqualityComparer.Accept((first as CTOperand)!, second);
        }

        public bool IncludeOrEqual(BTOperand first, BTOperand second)
        {
            return InclusionOrEqualityComparer.Accept((first as CTOperand)!, second);
        }

        #endregion
    }
}
