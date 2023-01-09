using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    public class SetOperationExecutersContainer
    {
        public readonly AttributeComponentAcceptor<AttributeComponent> IntersectionOperator;
        public readonly AttributeComponentAcceptor<AttributeComponent> UnionOperator;
        public readonly AttributeComponentAcceptor<bool> InclusionComparer;
        public readonly AttributeComponentAcceptor<bool> EqualityComparer;
        public readonly AttributeComponentAcceptor<bool> InclusionOrEqualityComparer;

        public SetOperationExecutersContainer(
            AttributeComponentAcceptor<AttributeComponent> intersectionOperator,
            AttributeComponentAcceptor<AttributeComponent> unionOperator,
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
        
        public AttributeComponent<TValue> Intersect<TValue>(
            AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        where TValue : IComparable<TValue>
        {
            return IntersectionOperator.Accept<TValue>(first, second) as AttributeComponent<TValue>;
        }

        public AttributeComponent<TValue> Union<TValue>(
            AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        where TValue : IComparable<TValue>
        {
            return UnionOperator.Accept<TValue>(first, second) as AttributeComponent<TValue>;
        }

        public bool Include<TValue>(
            AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        where TValue : IComparable<TValue>
        {
            return InclusionComparer.Accept<TValue>(first, second);
        }

        public bool Equal<TValue>(
            AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        where TValue : IComparable<TValue>
        {
            return EqualityComparer.Accept<TValue>(first, second);
        }

        public bool IncludeOrEqual<TValue>(
            AttributeComponent<TValue> first, AttributeComponent<TValue> second)
        where TValue : IComparable<TValue>
        {
            return InclusionOrEqualityComparer.Accept<TValue>(first, second);
        }
    }
}
