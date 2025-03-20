using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public class OrderedFiniteEnumerableAttributeDomain<TData> : AttributeDomain<TData>
    {
        public OrderedFiniteEnumerableAttributeDomain(IEnumerable<TData> universum)
            : base(BuildUniversum(universum, out _setDomainCallback))
        {
            _setDomainCallback(this);
            _setDomainCallback = null;
        }

        protected OrderedFiniteEnumerableAttributeDomain(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> universum)
            : base(universum)
        {
            _setDomainCallback(this);
            _setDomainCallback = null;
        }

        protected static OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> BuildUniversum(
            IEnumerable<TData> universum, out Action<AttributeDomain<TData>> setDomainCallback)
        {
            return new OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>(
                universum, 
                out setDomainCallback);
        }
    }
}
