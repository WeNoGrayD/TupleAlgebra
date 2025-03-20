using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.SetOperationExecutorsContainers
{
    public abstract class SetOperationExecutorsContainer<
        BTOperand, 
        CTOperand,
        TOperationResultFactory>
        : ISetOperationExecutorsContainer<BTOperand>
        where CTOperand: class, BTOperand
    {
        #region Instance fields

        private Lazy<FactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand>> _complementationOperator;
        
        private Lazy<InstantBinaryOperator<CTOperand, BTOperand, bool>> _inclusionComparer;

        private Lazy<InstantBinaryOperator<CTOperand, BTOperand, bool>> _equalityComparer;

        private Lazy<InstantBinaryOperator<CTOperand, BTOperand, bool>> _inclusionOrEqualityComparer;

        #endregion

        #region Instance properties

        protected TOperationResultFactory Factory { get; private set; }

        protected FactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand> ComplementationOperator
        { get => _complementationOperator.Value; }

        protected InstantBinaryOperator<CTOperand, BTOperand, bool> InclusionComparer
        { get => _inclusionComparer.Value; }

        protected InstantBinaryOperator<CTOperand, BTOperand, bool> EqualityComparer
        { get => _equalityComparer.Value; }

        protected InstantBinaryOperator<CTOperand, BTOperand, bool> InclusionOrEqualityComparer
        { get => _inclusionOrEqualityComparer.Value; }

        #endregion

        #region Constructors

        public SetOperationExecutorsContainer(
            TOperationResultFactory factory,
            Func<FactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand>> complementionOperator,
            Func<InstantBinaryOperator<CTOperand, BTOperand, bool>> inclusionComparer,
            Func<InstantBinaryOperator<CTOperand, BTOperand, bool>> equalityComparer,
            Func<InstantBinaryOperator<CTOperand, BTOperand, bool>> inclusionOrEquationComparer)
        {
            Factory = factory;

            _complementationOperator = new Lazy<FactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand>>(
                complementionOperator);
            _inclusionComparer = new Lazy<InstantBinaryOperator<CTOperand, BTOperand, bool>>(
                inclusionComparer);
            _equalityComparer = new Lazy<InstantBinaryOperator<CTOperand, BTOperand, bool>>(
                equalityComparer);
            _inclusionOrEqualityComparer = new Lazy<InstantBinaryOperator<CTOperand, BTOperand, bool>>(
                inclusionOrEquationComparer);

            return;
        }

        #endregion

        #region Instance methods

        public BTOperand Complement(BTOperand first)
        {
            return ComplementationOperator.Visit((first as CTOperand)!, Factory);
        }

        public abstract BTOperand Intersect(BTOperand first, BTOperand second);

        public abstract BTOperand Union(BTOperand first, BTOperand second);

        public abstract BTOperand Except(BTOperand first, BTOperand second);

        public abstract BTOperand SymmetricExcept(BTOperand first, BTOperand second);

        public bool Include(BTOperand first, BTOperand second)
        {
            return InclusionComparer.Visit((first as CTOperand)!, second);
        }

        public bool Equal(BTOperand first, BTOperand second)
        {
            return EqualityComparer.Visit((first as CTOperand)!, second);
        }

        public bool IncludeOrEqual(BTOperand first, BTOperand second)
        {
            return InclusionOrEqualityComparer.Visit((first as CTOperand)!, second);
        }

        #endregion
    }
}
