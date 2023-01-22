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

        public abstract IEnumerator<TValue> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static AttributeComponent<TValue> operator &(AttributeDomain<TValue> domain, NonFictionalAttributeComponent<TValue> component)
        {
            return domain._universum & component;
        }

        public static AttributeComponent<TValue> operator |(AttributeDomain<TValue> domain, NonFictionalAttributeComponent<TValue> component)
        {
            return domain._universum | component;
        }

        public static AttributeComponent<TValue> operator /(AttributeDomain<TValue> domain, NonFictionalAttributeComponent<TValue> component)
        {
            return domain._universum / component;
        }

        public static AttributeComponent<TValue> operator ^(AttributeDomain<TValue> domain, NonFictionalAttributeComponent<TValue> component)
        {
            return domain._universum ^ component;
        }

        public static bool operator ==(AttributeDomain<TValue> domain, NonFictionalAttributeComponent<TValue> component)
        {
            return domain._universum == component;
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
        }
    }

    public class FiniteEnumerableAttributeDomain<TValue> : AttributeDomain<TValue>
        where TValue : IComparable<TValue>
    {
        public FiniteEnumerableAttributeDomain(IEnumerable<TValue> universum)
            : base(new FiniteEnumerableNonFictionalAttributeComponent<TValue>(null, universum))
        { }

        public override IEnumerator<TValue> GetEnumerator()
        {
            return _universum.GetEnumerator();
        }
    }
}
