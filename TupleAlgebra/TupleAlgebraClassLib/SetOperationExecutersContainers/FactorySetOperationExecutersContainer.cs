using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public abstract class FactorySetOperationExecutersContainer<TValue> : SetOperationExecutersContainer<TValue>
    {
        protected AttributeComponentFactory<TValue> _componentFactory;

        protected FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _intersectionOperator;
        protected FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _unionOperator;
        protected FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _differenceOperator;
        protected FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> _symmetricExceptionOperator;

        public FactorySetOperationExecutersContainer(
            AttributeComponentFactory<TValue> componentFactory,
            FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> intersectionOperator,
            FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> unionOperator,
            FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> differenceOperator,
            FactoryBinaryAttributeComponentAcceptor<TValue, AttributeComponent<TValue>> symmetricExceptionOperator,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> inclusionComparer,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> equalityComparer,
            InstantBinaryAttributeComponentAcceptor<TValue, bool> inclusionOrEquationComparer)
            : base(inclusionComparer,
                   equalityComparer,
                   inclusionOrEquationComparer)
        {
            _componentFactory = componentFactory;

            _intersectionOperator = intersectionOperator;
            _unionOperator = unionOperator;
            _differenceOperator = differenceOperator;
            _symmetricExceptionOperator = symmetricExceptionOperator;
        }

        public AttributeComponent<TQueryResult> ProduceNonFictionalAttributeComponent<TQueryResult>(
            AttributeComponentFactoryArgs<TQueryResult> factoryArgs)
        {
            return AttributeComponentGeneralFactory.CreateNonFictional(_componentFactory.GetType(), factoryArgs);
        }

        public override AttributeComponent<TValue> Intersect(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _intersectionOperator.Accept(first, second, _componentFactory);
        }

        public override AttributeComponent<TValue> Union(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _unionOperator.Accept(first, second, _componentFactory);
        }

        public override AttributeComponent<TValue> Except(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _differenceOperator.Accept(first, second, _componentFactory);
        }

        public override AttributeComponent<TValue> SymmetricExcept(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return _symmetricExceptionOperator.Accept(first, second, _componentFactory);
        }
    }
}
