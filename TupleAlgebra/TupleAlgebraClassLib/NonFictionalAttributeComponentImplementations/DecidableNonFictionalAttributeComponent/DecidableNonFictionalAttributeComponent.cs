using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public abstract class DecidableNonFictionalAttributeComponent<TData> : NonFictionalAttributeComponent<TData>
    {
        public DecidableNonFictionalAttributeComponent(
            AttributeDomain<TData> domain, NonFictionalAttributeComponentPower power)
            : base(domain, power)
        { }

        public abstract bool Decide(TData value);

        public override bool IsEmpty()
        {
            return (Domain & this) is EmptyAttributeComponent<TData>;
        }

        public override bool IsFull()
        {
            return this.Domain == this;
        }

        /*
        public override IEnumerator<TData> GetEnumerator()
        {
            yield break;
        }
        */
    }

    public abstract class GeneratingDecidableNonFictionalAttributeComponent<TData>
        : DecidableNonFictionalAttributeComponent<TData>
    {
        private DecidableNonFictionalAttributeComponent<TData> _slave;

        public GeneratingDecidableNonFictionalAttributeComponent(
            DecidableNonFictionalAttributeComponent<TData> slave)
            : base(slave.Domain, slave.Power as NonFictionalAttributeComponentPower)
        { }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            yield break;
        }
    }

    /*
    internal class PredicateBasedRuleSet<TData> : 
    {
        List<IEnumerable<Predicate<TData>>> _rule
    }
    */
    public class PredicateBasedDecidableNonFictionalAttributeComponent<TData>
        : DecidableNonFictionalAttributeComponent<TData>
    {
        private const string NATURE_TYPE = "PredicateBasedDecidable";

        protected override string NatureType { get => NATURE_TYPE; }

        public IEnumerable<Predicate<TData>> Rules { get; private set; }

        static PredicateBasedDecidableNonFictionalAttributeComponent()
        {
            NonFictionalAttributeComponent<TData>.InitSetOperations(
                NATURE_TYPE,
                new PredicateBasedDecidableNonFictionalAttributeComponentOperationExecutersContainer());
        }

        public PredicateBasedDecidableNonFictionalAttributeComponent(
            AttributeDomain<TData> domain,
            IEnumerable<Predicate<TData>> rules)
            : base(domain, new PredicateBasedDecidableNonFictionalAttributeComponentPower(rules))
        {
            Rules = rules;
        }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            yield break;
        }

        public override bool Decide(TData value)
        {
            return Rules.All(rule => rule(value));
        }

        private class PredicateBasedDecidableNonFictionalAttributeComponentOperationExecutersContainer : FactorySetOperationExecutersContainer<TData>
        {
            public PredicateBasedDecidableNonFictionalAttributeComponentOperationExecutersContainer() : base(
                new PredicateBasedDecidableNonFictionalAttributeComponentFactory<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentIntersectionOperator<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentUnionOperator<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentExceptionOperator<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentSymmetricExceptionOperator<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentInclusionComparer<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentEqualityComparer<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentInclusionOrEqualityComparer<TData>())
            { }
        }

        public class PredicateBasedDecidableNonFictionalAttributeComponentPower : NonFictionalAttributeComponentPower
        {
            private IEnumerable<Predicate<TData>> _componentRules;

            public int Value { get => _componentRules.Count(); }

            public PredicateBasedDecidableNonFictionalAttributeComponentPower(
                IEnumerable<Predicate<TData>> componentRules)
            {
                _componentRules = componentRules;
            }

            public override void InitAttributeComponent(AttributeComponent<TData> component)
            {
                return;
            }

            public override bool IsZero()
            {
                return false;
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
