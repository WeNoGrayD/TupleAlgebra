using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public abstract class FactorySetOperationExecutersContainer<
        BTOperand, 
        CTOperand, 
        TOperationResultFactory, 
        TOperationResultFactoryArgs>
        : SetOperationExecutersContainer<BTOperand, CTOperand>
        where CTOperand : BTOperand
    {
        #region Instance fields

        private Lazy<TOperationResultFactory> _componentFactory;

        private Lazy<IFactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand>> _complementionOperator;

        private Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>> _intersectionOperator;

        private Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>> _unionOperator;

        private Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>> _differenceOperator;

        private Lazy<IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand>> _symmetricExceptionOperator;

        #endregion

        #region Instance properties

        protected TOperationResultFactory ComponentFactory
        { get => _componentFactory.Value; }

        protected IFactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand> ComplementionOperator
        { get => _complementionOperator.Value; }

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

        public FactorySetOperationExecutersContainer(
            Func<TOperationResultFactory> factory,
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
            : base(inclusionComparer,
                   equalityComparer,
                   inclusionOrEquationComparer)
        {
            _componentFactory = new Lazy<TOperationResultFactory>(factory);

            _complementionOperator = new Lazy<IFactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand>>(
                complementionOperator);
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

        public override BTOperand Complement(CTOperand first)
        {
            return ComplementionOperator.Accept(first, ComponentFactory);
        }

        public override BTOperand Intersect(CTOperand first, BTOperand second)
        {
            return IntersectionOperator.Accept(first, second, ComponentFactory);
        }

        public override BTOperand Union(CTOperand first, BTOperand second)
        {
            return UnionOperator.Accept(first, second, ComponentFactory);
        }

        public override BTOperand Except(CTOperand first, BTOperand second)
        {
            return DifferenceOperator.Accept(first, second, ComponentFactory);
        }

        public override BTOperand SymmetricExcept(CTOperand first, BTOperand second)
        {
            return SymmetricExceptionOperator.Accept(first, second, ComponentFactory);
        }

        #endregion
    }

    public abstract class FactoryAttributeComponentOperationExecutersContainer<TData, CTOperand>
        : FactorySetOperationExecutersContainer<
            AttributeComponent<TData>, 
            CTOperand, 
            AttributeComponentFactory, AttributeComponentFactoryArgs>,
          IFactoryAttributeComponentOperationExecutersContainer<TData, CTOperand>
        where CTOperand : AttributeComponent<TData>
    {
        #region Constructors

        public FactoryAttributeComponentOperationExecutersContainer(
            Func<AttributeComponentFactory> componentFactory,
            Func<IFactoryUnaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>>>
                complementionOperator,
            Func<IFactoryBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>>> 
                intersectionOperator,
            Func<IFactoryBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>>> 
                unionOperator,
            Func<IFactoryBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>>> 
                differenceOperator,
            Func<IFactoryBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>>>
                symmetricExceptionOperator,
            Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool>> 
                inclusionComparer,
            Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool>>
                equalityComparer,
            Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool>> 
                inclusionOrEquationComparer)
            : base(componentFactory,
                   complementionOperator,
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

        #endregion

        #region Instance methods

        public AttributeComponent<TProducedData> Produce<TProducedData>(AttributeComponentFactoryArgs factoryArgs)
        {
            return ComponentFactory.CreateNonFictional<TProducedData>(factoryArgs);
        }

        #endregion
    }
}
