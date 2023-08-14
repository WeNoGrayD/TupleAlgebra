using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public abstract class InstantSetOperationExecutersContainer<BTOperand, CTOperand>
        : SetOperationExecutersContainer<BTOperand, CTOperand>
        where CTOperand : BTOperand
    {
        #region Instance fields

        private Lazy<IInstantUnaryOperator<CTOperand, BTOperand>> _complementionOperator;

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _intersectionOperator;

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _unionOperator;

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _differenceOperator;

        private Lazy<IInstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _symmetricExceptionOperator;

        #endregion

        #region Instance properties

        protected IInstantUnaryOperator<CTOperand, BTOperand> ComplementionOperator
        { get => _complementionOperator.Value; }

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

        public InstantSetOperationExecutersContainer(
            Func<IInstantUnaryOperator<CTOperand, BTOperand>> complementionOperator,
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
            _complementionOperator = new Lazy<IInstantUnaryOperator<CTOperand, BTOperand>>(
                complementionOperator);
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

        public override BTOperand Complement(CTOperand first)
        {
            return ComplementionOperator.Accept(first);
        }

        public override BTOperand Intersect(CTOperand first, BTOperand second)
        {
            return IntersectionOperator.Accept(first, second);
        }

        public override BTOperand Union(CTOperand first, BTOperand second)
        {
            return UnionOperator.Accept(first, second);
        }

        public override BTOperand Except(CTOperand first, BTOperand second)
        {
            return DifferenceOperator.Accept(first, second);
        }

        public override BTOperand SymmetricExcept(CTOperand first, BTOperand second)
        {
            return SymmetricExceptionOperator.Accept(first, second);
        }

        #endregion
    }

    public abstract class InstantAttributeComponentOperationExecutersContainer<TData, CTOperand>
        : InstantSetOperationExecutersContainer<AttributeComponent<TData>, CTOperand>
        where CTOperand : AttributeComponent<TData>
    {
        public InstantAttributeComponentOperationExecutersContainer(
            Func<IInstantUnaryOperator<CTOperand, AttributeComponent<TData>>> 
                complementionOperator,
            Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                intersectionOperator,
            Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>> 
                unionOperator,
            Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>> 
                differenceOperator,
            Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>> 
                symmetricExceptionOperator,
            Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool>> 
                inclusionComparer,
            Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool>> 
                equalityComparer,
            Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool>> 
                inclusionOrEquationComparer)
            : base(complementionOperator,
                   intersectionOperator,
                   unionOperator,
                   differenceOperator,
                   symmetricExceptionOperator,
                   inclusionComparer,
                   equalityComparer,
                   inclusionOrEquationComparer)
        {
            return;
        }
    }
}
