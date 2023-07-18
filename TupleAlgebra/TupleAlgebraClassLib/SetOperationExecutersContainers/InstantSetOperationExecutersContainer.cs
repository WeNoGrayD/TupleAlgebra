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
        protected InstantUnaryOperator<CTOperand, BTOperand> _complementionOperator;
        protected InstantBinaryOperator<CTOperand, BTOperand, BTOperand> _intersectionOperator;
        protected InstantBinaryOperator<CTOperand, BTOperand, BTOperand> _unionOperator;
        protected InstantBinaryOperator<CTOperand, BTOperand, BTOperand> _differenceOperator;
        protected InstantBinaryOperator<CTOperand, BTOperand, BTOperand> _symmetricExceptionOperator;

        public InstantSetOperationExecutersContainer(
            InstantUnaryOperator<CTOperand, BTOperand> complementionOperator,
            InstantBinaryOperator<CTOperand, BTOperand, BTOperand> intersectionOperator,
            InstantBinaryOperator<CTOperand, BTOperand, BTOperand> unionOperator,
            InstantBinaryOperator<CTOperand, BTOperand, BTOperand> differenceOperator,
            InstantBinaryOperator<CTOperand, BTOperand, BTOperand> symmetricExceptionOperator,
            InstantBinaryOperator<CTOperand, BTOperand, bool> inclusionComparer,
            InstantBinaryOperator<CTOperand, BTOperand, bool> equalityComparer,
            InstantBinaryOperator<CTOperand, BTOperand, bool> inclusionOrEquationComparer)
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

        public override BTOperand Complement(CTOperand first)
        {
            return _complementionOperator.Accept(first);
        }

        public override BTOperand Intersect(CTOperand first, BTOperand second)
        {
            return _intersectionOperator.Accept(first, second);
        }

        public override BTOperand Union(CTOperand first, BTOperand second)
        {
            return _unionOperator.Accept(first, second);
        }

        public override BTOperand Except(CTOperand first, BTOperand second)
        {
            return _differenceOperator.Accept(first, second);
        }

        public override BTOperand SymmetricExcept(CTOperand first, BTOperand second)
        {
            return _symmetricExceptionOperator.Accept(first, second);
        }
    }

    public abstract class InstantAttributeComponentOperationExecutersContainer<TData, CTOperand>
        : InstantSetOperationExecutersContainer<AttributeComponent<TData>, CTOperand>
        where CTOperand : AttributeComponent<TData>
    {
        public InstantAttributeComponentOperationExecutersContainer(
            InstantUnaryOperator<CTOperand, AttributeComponent<TData>> complementionOperator,
            InstantBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponent<TData>> intersectionOperator,
            InstantBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponent<TData>> unionOperator,
            InstantBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponent<TData>> differenceOperator,
            InstantBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponent<TData>> symmetricExceptionOperator,
            InstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool> inclusionComparer,
            InstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool> equalityComparer,
            InstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool> inclusionOrEquationComparer)
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
