using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    public abstract class AttributeDomain<TValue> : IEnumerable, IEnumerable<TValue>
        where TValue: IComparable<TValue>
    {
        protected NonFictionalAttributeComponent<TValue> _universum;

        public AttributeDomain(
            NonFictionalAttributeComponent<TValue> universum)
        {
            _universum = universum;
        }

        //public abstract bool EqualsTo(NonFictionalAttributeComponent<TValue> component);

        protected abstract AttributeComponent<TValue> ComplementOf(AttributeComponent<TValue> component);

        public abstract IEnumerator<TValue> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Оператор сравнения на равенство двух мощностей.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool operator ==(AttributeDomain<TValue> domain, NonFictionalAttributeComponent<TValue> component)
        {
            return domain._universum == component;
            //return domain.EqualsTo(component);
        }

        /// <summary>
        /// Оператор сравнения на неравенство двух мощностей.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static bool operator !=(AttributeDomain<TValue> domain, NonFictionalAttributeComponent<TValue> component)
        {
            return !(domain == component);
            //return !domain.EqualsTo(component);
        }
    }

    public class FiniteEnumerableAttributeDomain<TValue> : AttributeDomain<TValue>
        where TValue : IComparable<TValue>
    {
        public FiniteEnumerableAttributeDomain(IEnumerable<TValue> universum)
            : base(new FiniteEnumerableNonFictionalAttributeComponent<TValue>(null, universum))
        {
            //Universum = universum;
        }

        protected override AttributeComponent<TValue> ComplementOf(AttributeComponent<TValue> component)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<TValue> GetEnumerator()
        {
            return _universum.GetEnumerator();
        }

        /*
        public override bool EqualsTo(NonFictionalAttributeComponent<TValue> component)
        {
            return this.Power == component.Power;
        }

        public class FiniteEnumerableAttributeDomainPower : AttributeComponentPower
        {
            internal override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

            private IEnumerable<TValue> _universum;

            protected int Value { get => _universum.Count(); }

            public FiniteEnumerableAttributeDomainPower(IEnumerable<TValue> universum)
            {
                _universum = universum;
            }

            public override int CompareTo(AttributeComponentPower componentPower)
            {
                return this.Value.CompareTo((componentPower as NonFictionalAttributeComponent<TValue>.NonFictionalAttributeComponentPower).Value);
            }
        }
        */
    }
}
