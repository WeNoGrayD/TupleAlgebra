using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure
{
    public class OrderedFiniteEnumerableAttributeDomain<TValue> : AttributeDomain<TValue>
    {
        public OrderedFiniteEnumerableAttributeDomain(IEnumerable<TValue> universum)
            : base(BuildUniversum(universum, out _setDomainCallback))
        {
            _setDomainCallback(this);
            _setDomainCallback = null;
        }

        protected OrderedFiniteEnumerableAttributeDomain(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue> universum)
            : base(universum)
        {
            _setDomainCallback(this);
            _setDomainCallback = null;
        }

        protected static OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue> BuildUniversum(
            IEnumerable<TValue> universum, out Action<AttributeDomain<TValue>> setDomainCallback)
        {
            return new OrderedFiniteEnumerableNonFictionalAttributeComponent<TValue>(
                universum, 
                out setDomainCallback);
        }
    }
}
