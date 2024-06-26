﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponents;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.SetOperationExecutorsContainers
{
    public abstract class InstantSetOperationExecutorsContainer<
        BTOperand, 
        CTOperand,
        TOperationResultFactory>
        : SetOperationExecutorsContainer<BTOperand, CTOperand, TOperationResultFactory>
        where CTOperand : class, BTOperand
    {
        #region Instance fields

        private Lazy<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _intersectionOperator;

        private Lazy<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _unionOperator;

        private Lazy<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _differenceOperator;

        private Lazy<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>> _symmetricExceptionOperator;

        #endregion

        #region Instance properties

        protected InstantBinaryOperator<CTOperand, BTOperand, BTOperand> IntersectionOperator
        { get => _intersectionOperator.Value; }

        protected InstantBinaryOperator<CTOperand, BTOperand, BTOperand> UnionOperator
        { get => _unionOperator.Value; }

        protected InstantBinaryOperator<CTOperand, BTOperand, BTOperand> DifferenceOperator
        { get => _differenceOperator.Value; }

        protected InstantBinaryOperator<CTOperand, BTOperand, BTOperand> SymmetricExceptionOperator
        { get => _symmetricExceptionOperator.Value; }

        #endregion

        #region Constructors

        public InstantSetOperationExecutorsContainer(
            TOperationResultFactory factory,
            Func<FactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand>> complementionOperator,
            Func<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>> intersectionOperator,
            Func<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>> unionOperator,
            Func<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>> differenceOperator,
            Func<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>> symmetricExceptionOperator,
            Func<InstantBinaryOperator<CTOperand, BTOperand, bool>> inclusionComparer,
            Func<InstantBinaryOperator<CTOperand, BTOperand, bool>> equalityComparer,
            Func<InstantBinaryOperator<CTOperand, BTOperand, bool>> inclusionOrEquationComparer)
            : base(factory,
                   complementionOperator,
                   inclusionComparer,
                   equalityComparer,
                   inclusionOrEquationComparer)
        {
            _intersectionOperator = new Lazy<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>>(
                intersectionOperator);
            _unionOperator = new Lazy<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>>(
                unionOperator);
            _differenceOperator = new Lazy<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>>(
                differenceOperator);
            _symmetricExceptionOperator = new Lazy<InstantBinaryOperator<CTOperand, BTOperand, BTOperand>>(
                symmetricExceptionOperator);

            return;
        }

        #endregion

        #region Instance methods

        public override BTOperand Intersect(BTOperand first, BTOperand second)
        {
            return IntersectionOperator.Visit((first as CTOperand)!, second);
        }

        public override BTOperand Union(BTOperand first, BTOperand second)
        {
            return UnionOperator.Visit((first as CTOperand)!, second);
        }

        public override BTOperand Except(BTOperand first, BTOperand second)
        {
            return DifferenceOperator.Visit((first as CTOperand)!, second);
        }

        public override BTOperand SymmetricExcept(BTOperand first, BTOperand second)
        {
            return SymmetricExceptionOperator.Visit((first as CTOperand)!, second);
        }

        #endregion
    }
}
