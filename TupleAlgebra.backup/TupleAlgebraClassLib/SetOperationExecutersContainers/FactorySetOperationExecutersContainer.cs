using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public abstract class FactorySetOperationExecutersContainer<TData> : SetOperationExecutersContainer<TData>
    {
        protected AttributeComponentFactory<TData> _componentFactory;

        protected FactoryBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> _intersectionOperator;
        protected FactoryBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> _unionOperator;
        protected FactoryBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> _differenceOperator;
        protected FactoryBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> _symmetricExceptionOperator;

        public FactorySetOperationExecutersContainer(
            AttributeComponentFactory<TData> componentFactory,
            FactoryBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> intersectionOperator,
            FactoryBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> unionOperator,
            FactoryBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> differenceOperator,
            FactoryBinaryAttributeComponentAcceptor<TData, AttributeComponent<TData>> symmetricExceptionOperator,
            InstantBinaryAttributeComponentAcceptor<TData, bool> inclusionComparer,
            InstantBinaryAttributeComponentAcceptor<TData, bool> equalityComparer,
            InstantBinaryAttributeComponentAcceptor<TData, bool> inclusionOrEquationComparer)
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

        public override AttributeComponent<TData> Intersect(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _intersectionOperator.Accept(first, second, _componentFactory);
        }

        public override AttributeComponent<TData> Union(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _unionOperator.Accept(first, second, _componentFactory);
        }

        public override AttributeComponent<TData> Except(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _differenceOperator.Accept(first, second, _componentFactory);
        }

        public override AttributeComponent<TData> SymmetricExcept(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return _symmetricExceptionOperator.Accept(first, second, _componentFactory);
        }
    }
}
