using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public abstract class FactorySetOperationExecutersContainer<BTOperand, CTOperand, TOperationResultFactory, TOperationResultFactoryArgs>
        : SetOperationExecutersContainer<BTOperand, CTOperand>
        where CTOperand : BTOperand
    {
        protected TOperationResultFactory _componentFactory;

        protected IFactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand> _complementionOperator;
        protected IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> _intersectionOperator;
        protected IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> _unionOperator;
        protected IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> _differenceOperator;
        protected IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> _symmetricExceptionOperator;

        public FactorySetOperationExecutersContainer(
            TOperationResultFactory factory,
            IFactoryUnaryOperator<CTOperand, TOperationResultFactory, BTOperand> complementionOperator,
            IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> intersectionOperator,
            IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> unionOperator,
            IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> differenceOperator,
            IFactoryBinaryOperator<CTOperand, BTOperand, TOperationResultFactory, BTOperand> symmetricExceptionOperator,
            IInstantBinaryOperator<CTOperand, BTOperand, bool> inclusionComparer,
            IInstantBinaryOperator<CTOperand, BTOperand, bool> equalityComparer,
            IInstantBinaryOperator<CTOperand, BTOperand, bool> inclusionOrEquationComparer)
            : base(inclusionComparer,
                   equalityComparer,
                   inclusionOrEquationComparer)
        {
            _componentFactory = factory;

            _complementionOperator = complementionOperator;
            _intersectionOperator = intersectionOperator;
            _unionOperator = unionOperator;
            _differenceOperator = differenceOperator;
            _symmetricExceptionOperator = symmetricExceptionOperator;
        }

        public override BTOperand Complement(CTOperand first)
        {
            return _complementionOperator.Accept(first, _componentFactory);
        }

        public override BTOperand Intersect(CTOperand first, BTOperand second)
        {
            return _intersectionOperator.Accept(first, second, _componentFactory);
        }

        public override BTOperand Union(CTOperand first, BTOperand second)
        {
            return _unionOperator.Accept(first, second, _componentFactory);
        }

        public override BTOperand Except(CTOperand first, BTOperand second)
        {
            return _differenceOperator.Accept(first, second, _componentFactory);
        }

        public override BTOperand SymmetricExcept(CTOperand first, BTOperand second)
        {
            return _symmetricExceptionOperator.Accept(first, second, _componentFactory);
        }
    }

    public interface IFactoryAttributeComponentOperationExecutersContainer<TData, CTOperand>
        : ISetOperationExecutersContainer<AttributeComponent<TData>, CTOperand>
        where CTOperand : AttributeComponent<TData>
    {
        public AttributeComponent<TProducedData> Produce<TProducedData>(AttributeComponentFactoryArgs factoryArgs);
    }

    public abstract class FactoryAttributeComponentOperationExecutersContainer<TData, CTOperand1>
        : FactorySetOperationExecutersContainer<AttributeComponent<TData>, CTOperand1, AttributeComponentFactory, AttributeComponentFactoryArgs>,
          IFactoryAttributeComponentOperationExecutersContainer<TData, CTOperand1>
        where CTOperand1 : AttributeComponent<TData>
    {
        public FactoryAttributeComponentOperationExecutersContainer(
            AttributeComponentFactory componentFactory,
            IFactoryUnaryAttributeComponentAcceptor<TData, CTOperand1, AttributeComponent<TData>> complementionOperator,
            IFactoryBinaryOperator<CTOperand1, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>> intersectionOperator,
            IFactoryBinaryOperator<CTOperand1, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>> unionOperator,
            IFactoryBinaryOperator<CTOperand1, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>> differenceOperator,
            IFactoryBinaryOperator<CTOperand1, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>> symmetricExceptionOperator,
            IInstantBinaryOperator<CTOperand1, AttributeComponent<TData>, bool> inclusionComparer,
            IInstantBinaryOperator<CTOperand1, AttributeComponent<TData>, bool> equalityComparer,
            IInstantBinaryOperator<CTOperand1, AttributeComponent<TData>, bool> inclusionOrEquationComparer)
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

        public AttributeComponent<TProducedData> Produce<TProducedData>(AttributeComponentFactoryArgs factoryArgs)
        {
            return _componentFactory.CreateNonFictional<TProducedData>(factoryArgs);
        }
    }
}
