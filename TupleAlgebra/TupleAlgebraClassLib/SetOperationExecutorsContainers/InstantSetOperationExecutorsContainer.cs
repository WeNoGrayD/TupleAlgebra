using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.SetOperationExecutorsContainers
{
    public abstract class InstantSetOperationExecutorsContainer<
        BTOperand, 
        CTOperand>
        : SetOperationExecutorsContainer<BTOperand, CTOperand>
        where CTOperand : class, BTOperand
    {
        #region Instance fields

        private Lazy<IInstantUnaryOperator<CTOperand, BTOperand>> _complementationOperator;

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _intersectionOperator;

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _unionOperator;

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _differenceOperator;

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _symmetricExceptionOperator;

        #endregion

        #region Instance properties

        protected IInstantUnaryOperator<CTOperand, BTOperand> ComplementationOperator
        { get => _complementationOperator.Value; }

        protected IInstantBinaryOperator<CTOperand, BTOperand, BTOperand> IntersectionOperator
        { get => _intersectionOperator.Value; }

        protected IInstantBinaryOperator<CTOperand, BTOperand, BTOperand> UnionOperator
        { get => _unionOperator.Value; }

        protected IInstantBinaryOperator<CTOperand, BTOperand, BTOperand> DifferenceOperator
        { get => _differenceOperator.Value; }

        protected IInstantBinaryOperator<CTOperand, BTOperand, BTOperand> SymmetricExceptionOperator
        { get => _symmetricExceptionOperator.Value; }

        #endregion

        #region Constructors

        public InstantSetOperationExecutorsContainer(
            Func<IInstantUnaryOperator<CTOperand, BTOperand>> complementationOperator,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> intersectionOperator,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> unionOperator,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> differenceOperator,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> symmetricExceptionOperator,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, bool>> inclusionComparer,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, bool>> equalityComparer,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, bool>> inclusionOrEquationComparer)
            : base(inclusionComparer,
                   equalityComparer,
                   inclusionOrEquationComparer)
        {
            _complementationOperator = new Lazy<IInstantUnaryOperator<CTOperand, BTOperand>>(
                complementationOperator);
            _intersectionOperator = new Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>>(
                intersectionOperator);
            _unionOperator = new Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>>(
                unionOperator);
            _differenceOperator = new Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>>(
                differenceOperator);
            _symmetricExceptionOperator = new Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>>(
                symmetricExceptionOperator);

            return;
        }

        #endregion

        #region Instance methods

        public override BTOperand Complement(BTOperand first)
        {
            return ComplementationOperator.Accept((first as CTOperand)!);
        }

        public override BTOperand Intersect(BTOperand first, BTOperand second)
        {
            return IntersectionOperator.Accept((first as CTOperand)!, second);
        }

        public override BTOperand Union(BTOperand first, BTOperand second)
        {
            return UnionOperator.Accept((first as CTOperand)!, second);
        }

        public override BTOperand Except(BTOperand first, BTOperand second)
        {
            return DifferenceOperator.Accept((first as CTOperand)!, second);
        }

        public override BTOperand SymmetricExcept(BTOperand first, BTOperand second)
        {
            return SymmetricExceptionOperator.Accept((first as CTOperand)!, second);
        }

        #endregion
    }
}
