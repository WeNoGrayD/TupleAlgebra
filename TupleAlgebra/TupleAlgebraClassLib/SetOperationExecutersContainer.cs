using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    public class SetOperationExecutersContainer<TValue>
        where TValue : IComparable<TValue>
    {
        private AttributeComponentFactory<TValue> _componentFactory;

        public readonly FactoryAttributeComponentAcceptor<AttributeComponent<TValue>> IntersectionOperator;
        public readonly FactoryAttributeComponentAcceptor<AttributeComponent<TValue>> UnionOperator;
        public readonly AttributeComponentAcceptor<bool> InclusionComparer;
        public readonly AttributeComponentAcceptor<bool> EqualityComparer;
        public readonly AttributeComponentAcceptor<bool> InclusionOrEqualityComparer;

        public SetOperationExecutersContainer(
            FactoryAttributeComponentAcceptor<AttributeComponent<TValue>> intersectionOperator,
            FactoryAttributeComponentAcceptor<AttributeComponent<TValue>> unionOperator,
            AttributeComponentAcceptor<bool> inclusionComparer,
            AttributeComponentAcceptor<bool> equationComparer,
            AttributeComponentAcceptor<bool> inlusionOrEquationComparer)
        {
            IntersectionOperator = intersectionOperator;
            UnionOperator = unionOperator;
            InclusionComparer = inclusionComparer;
            EqualityComparer = equationComparer;
            InclusionOrEqualityComparer = inlusionOrEquationComparer;
        }
        
        public AttributeComponent<TValue> Intersect(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return IntersectionOperator.Accept(first, second, _componentFactory) as AttributeComponent<TValue>;
        }

        public AttributeComponent<TValue> Union(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return UnionOperator.Accept(first, second, _componentFactory) as AttributeComponent<TValue>;
        }

        public bool Include(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return InclusionComparer.Accept(first, second);
        }

        public bool Equal(AttributeComponent<TValue> first, AttributeComponent<TValue> second)

        {
            return EqualityComparer.Accept(first, second);
        }

        public bool IncludeOrEqual(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        {
            return InclusionOrEqualityComparer.Accept(first, second);
        }
    }
}
