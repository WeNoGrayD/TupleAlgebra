using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public abstract class DecidableNonFictionalAttributeComponent<TValue> : NonFictionalAttributeComponent<TValue>
    {
        public DecidableNonFictionalAttributeComponent(
            AttributeDomain<TValue> domain, NonFictionalAttributeComponentPower power)
            : base(domain, power)
        { }

        public abstract bool Decide(TValue value);

        public override bool IsEmpty()
        {
            return (Domain & this) is EmptyAttributeComponent<TValue>;
        }

        public override bool IsFull()
        {
            return this.Domain == this;
        }

        /*
        public override IEnumerator<TValue> GetEnumerator()
        {
            yield break;
        }
        */
    }

    public abstract class GeneratingDecidableNonFictionalAttributeComponent<TValue>
        : DecidableNonFictionalAttributeComponent<TValue>
    {
        private DecidableNonFictionalAttributeComponent<TValue> _slave;

        public GeneratingDecidableNonFictionalAttributeComponent(
            DecidableNonFictionalAttributeComponent<TValue> slave)
            : base(slave.Domain, slave.Power as NonFictionalAttributeComponentPower)
        { }

        public override IEnumerator<TValue> GetEnumeratorImpl()
        {
            yield break;
        }
    }

    /*
    internal class PredicateBasedRuleSet<TValue> : 
    {
        List<IEnumerable<Predicate<TValue>>> _rule
    }
    */
    public class PredicateBasedDecidableNonFictionalAttributeComponent<TValue>
        : DecidableNonFictionalAttributeComponent<TValue>
    {
        private const string NATURE_TYPE = "PredicateBasedDecidable";

        protected override string NatureType { get => NATURE_TYPE; }

        public IEnumerable<Predicate<TValue>> Rules { get; private set; }

        static PredicateBasedDecidableNonFictionalAttributeComponent()
        {
            NonFictionalAttributeComponent<TValue>.InitSetOperations(
                NATURE_TYPE,
                new PredicateBasedDecidableNonFictionalAttributeComponentOperationExecutersContainer());
        }

        public PredicateBasedDecidableNonFictionalAttributeComponent(
            AttributeDomain<TValue> domain,
            IEnumerable<Predicate<TValue>> rules)
            : base(domain, new PredicateBasedDecidableNonFictionalAttributeComponentPower(rules))
        {
            Rules = rules;
        }

        public override IEnumerator<TValue> GetEnumeratorImpl()
        {
            yield break;
        }

        public override bool Decide(TValue value)
        {
            return Rules.All(rule => rule(value));
        }

        private class PredicateBasedDecidableNonFictionalAttributeComponentOperationExecutersContainer : FactorySetOperationExecutersContainer<TValue>
        {
            public PredicateBasedDecidableNonFictionalAttributeComponentOperationExecutersContainer() : base(
                new PredicateBasedDecidableNonFictionalAttributeComponentFactory<TValue>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentIntersectionOperator<TValue>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentUnionOperator<TValue>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentExceptionOperator<TValue>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentSymmetricExceptionOperator<TValue>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentInclusionComparer<TValue>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentEqualityComparer<TValue>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentInclusionOrEqualityComparer<TValue>())
            { }
        }

        public class PredicateBasedDecidableNonFictionalAttributeComponentPower : NonFictionalAttributeComponentPower
        {
            private IEnumerable<Predicate<TValue>> _componentRules;

            public int Value { get => _componentRules.Count(); }

            public PredicateBasedDecidableNonFictionalAttributeComponentPower(
                IEnumerable<Predicate<TValue>> componentRules)
            {
                _componentRules = componentRules;
            }

            protected override int CompareToSame(dynamic second)
            {
                if (second is PredicateBasedDecidableNonFictionalAttributeComponentPower second2)
                    return this.CompareToSame(second);
                else
                    throw new InvalidCastException("Непустая компонента с разрешимым содержимым на основе предиката сравнивается с непустой компонентой другого вида, в данный момент эта операция не поддерживается.");
            }

            protected int CompareToSame(PredicateBasedDecidableNonFictionalAttributeComponentPower second)
            {
                return this.Value.CompareTo(second.Value);
            }
        }
    }
}
