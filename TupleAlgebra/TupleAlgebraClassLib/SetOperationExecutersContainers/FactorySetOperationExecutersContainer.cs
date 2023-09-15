using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.SetOperationExecutorsContainers
{
    public abstract class FactorySetOperationExecutorsContainer<
        BTOperand, 
        CTOperand, 
        TOperationResultFactory>
        : SetOperationExecutorsContainer<BTOperand, CTOperand, TOperationResultFactory>
        where CTOperand : class, BTOperand
    {
        #region Instance fields

        private Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>> _intersectionOperator;

        private Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>> _unionOperator;

        private Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>> _differenceOperator;

        private Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>> _symmetricExceptionOperator;

        #endregion

        #region Instance properties

        protected IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> IntersectionOperator
        { get => _intersectionOperator.Value; }

        protected IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> UnionOperator
        { get => _unionOperator.Value; }

        protected IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> DifferenceOperator
        { get => _differenceOperator.Value; }

        protected IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> SymmetricExceptionOperator
        { get => _symmetricExceptionOperator.Value; }

        #endregion

        #region Constructors

        public FactorySetOperationExecutorsContainer(
            Func<IFactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand>>
                complementionOperator,
            Func<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>>
                intersectionOperator,
            Func<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>>
                unionOperator,
            Func<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>>
                differenceOperator,
            Func<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>>
                symmetricExceptionOperator,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, bool>>
                inclusionComparer,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, bool>> 
                equalityComparer,
            Func<IInstantBinaryOperator<CTOperand, BTOperand, bool>> 
                inclusionOrEquationComparer)
            : base(complementionOperator,
                   inclusionComparer,
                   equalityComparer,
                   inclusionOrEquationComparer)
        {
            _intersectionOperator = new Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>>(
                intersectionOperator);
            _unionOperator = new Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>>(
                unionOperator);
            _differenceOperator = new Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>>(
                differenceOperator);
            _symmetricExceptionOperator = new Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>>(
                symmetricExceptionOperator);

            return;
        }

        #endregion

        #region Instance methods

        public override BTOperand Intersect(BTOperand first, BTOperand second)
        {
            return IntersectionOperator.Accept((first as CTOperand)!, second, Factory);
        }

        public override BTOperand Union(BTOperand first, BTOperand second)
        {
            return UnionOperator.Accept((first as CTOperand)!, second, Factory);
        }

        public override BTOperand Except(BTOperand first, BTOperand second)
        {
            return DifferenceOperator.Accept((first as CTOperand)!, second, Factory);
        }

        public override BTOperand SymmetricExcept(BTOperand first, BTOperand second)
        {
            return SymmetricExceptionOperator.Accept((first as CTOperand)!, second, Factory);
        }

        #endregion
    }
}
