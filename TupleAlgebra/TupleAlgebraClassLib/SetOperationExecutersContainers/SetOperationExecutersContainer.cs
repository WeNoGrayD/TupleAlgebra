using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public interface ISetOperationExecutersContainer<BTOperand, CTOperand>
        where CTOperand : BTOperand
    {
        public BTOperand Complement(CTOperand first);

        public BTOperand Intersect(CTOperand first, BTOperand second);

        public BTOperand Union(CTOperand first, BTOperand second);

        public BTOperand Except(CTOperand first, BTOperand second);

        public BTOperand SymmetricExcept(CTOperand first, BTOperand second);

        public bool Include(CTOperand first, BTOperand second);

        public bool Equal(CTOperand first, BTOperand second);

        public bool IncludeOrEqual(CTOperand first, BTOperand second);
    }

    public abstract class SetOperationExecutersContainer<BTOperand, CTOperand>
        : ISetOperationExecutersContainer<BTOperand, CTOperand>
        where CTOperand: BTOperand
    {
        protected IInstantBinaryOperator<CTOperand, BTOperand, bool> _inclusionComparer;
        protected IInstantBinaryOperator<CTOperand, BTOperand, bool> _equalityComparer;
        protected IInstantBinaryOperator<CTOperand, BTOperand, bool> _inclusionOrEqualityComparer;

        public SetOperationExecutersContainer(
            IInstantBinaryOperator<CTOperand, BTOperand, bool> inclusionComparer,
            IInstantBinaryOperator<CTOperand, BTOperand, bool> equalityComparer,
            IInstantBinaryOperator<CTOperand, BTOperand, bool> inclusionOrEquationComparer)
        {
            _inclusionComparer = inclusionComparer;
            _equalityComparer = equalityComparer;
            _inclusionOrEqualityComparer = inclusionOrEquationComparer;
        }

        public abstract BTOperand Complement(CTOperand first);

        public abstract BTOperand Intersect(CTOperand first, BTOperand second);

        public abstract BTOperand Union(CTOperand first, BTOperand second);

        public abstract BTOperand Except(CTOperand first, BTOperand second);

        public abstract BTOperand SymmetricExcept(CTOperand first, BTOperand second);

        public bool Include(CTOperand first, BTOperand second)
        {
            return _inclusionComparer.Accept(first, second);
        }

        public bool Equal(CTOperand first, BTOperand second)
        {
            return _equalityComparer.Accept(first, second);
        }

        public bool IncludeOrEqual(CTOperand first, BTOperand second)
        {
            return _inclusionOrEqualityComparer.Accept(first, second);
        }
    }
}
