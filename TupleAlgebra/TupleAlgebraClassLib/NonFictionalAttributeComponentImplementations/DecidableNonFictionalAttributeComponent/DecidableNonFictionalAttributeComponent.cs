using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.DecidableNonFictionalAttributeComponent
{
    public abstract class DecidableNonFictionalAttributeComponent<TData> : NonFictionalAttributeComponent<TData>
    {
        public DecidableNonFictionalAttributeComponent(NonFictionalAttributeComponentPower<TData> power)
            : base(power, null)
        { }

        public abstract bool Decide(TData value);

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
            : base(slave.Power as NonFictionalAttributeComponentPower<TData>)
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
    public abstract class PredicateBasedDecidableNonFictionalAttributeComponent<TData>
        : DecidableNonFictionalAttributeComponent<TData>
    {
        public IEnumerable<Predicate<TData>> Rules { get; private set; }

        static PredicateBasedDecidableNonFictionalAttributeComponent()
        {
            //NonFictionalAttributeComponent<TData>.InitSetOperations(
            //    NATURE_TYPE,
            //    new PredicateBasedDecidableNonFictionalAttributeComponentOperationExecutorsContainer());
        }

        public PredicateBasedDecidableNonFictionalAttributeComponent(
            IEnumerable<Predicate<TData>> rules)
            : base(new PredicateBasedDecidableNonFictionalAttributeComponentPower(rules))
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

        /*
        private class PredicateBasedDecidableNonFictionalAttributeComponentOperationExecutorsContainer : FactorySetOperationExecutorsContainer<TData>
        {
            public PredicateBasedDecidableNonFictionalAttributeComponentOperationExecutorsContainer() : base(
                new PredicateBasedDecidableNonFictionalAttributeComponentFactory(),
                new PredicateBasedDecidableNonFictionalAttributeComponentIntersectionOperator<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentUnionOperator<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentExceptionOperator<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentSymmetricExceptionOperator<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentInclusionComparer<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentEqualityComparer<TData>(),
                new PredicateBasedDecidableNonFictionalAttributeComponentInclusionOrEqualityComparer<TData>())
            { }
        }
        */

        public class PredicateBasedDecidableNonFictionalAttributeComponentPower : NonFictionalAttributeComponentPower<TData>
        {
            private PredicateBasedDecidableNonFictionalAttributeComponent<TData> _component;

            private IEnumerable<Predicate<TData>> _componentRules;

            protected override NonFictionalAttributeComponent<TData> Component { get => _component; }

            public int Value { get => _componentRules.Count(); }

            public PredicateBasedDecidableNonFictionalAttributeComponentPower(
                IEnumerable<Predicate<TData>> componentRules)
            {
                _componentRules = componentRules;
            }

            public override void InitAttributeComponent(NonFictionalAttributeComponent<TData> component)
            {
                _component = (component as PredicateBasedDecidableNonFictionalAttributeComponent<TData>)!;

                return;
            }

            public override bool EqualsZero()
            {
                return (_component.Domain & _component).Power.EqualsZero();
            }

            protected override int CompareToZero()
            {
                return 1;
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
