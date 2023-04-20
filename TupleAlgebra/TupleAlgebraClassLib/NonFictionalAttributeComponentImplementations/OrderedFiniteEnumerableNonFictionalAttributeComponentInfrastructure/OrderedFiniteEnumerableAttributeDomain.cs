using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public class OrderedFiniteEnumerableAttributeDomain<TAttributeComponent, TData>
        : AttributeDomain<TData>
        where TAttributeComponent : OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, new()
    {
        public OrderedFiniteEnumerableAttributeDomain(IEnumerable<TData> universum)
            : base(BuildUniversum(universum))
        {
            return;
        }

        protected static OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> BuildUniversum(
            IEnumerable<TData> universum)
        {
            TAttributeComponent universumComponent = new TAttributeComponent();
            universumComponent.InitValues(universum);

            return universumComponent;
        }

        /*
        public override IReproducingQueryable<TReproducedData> Reproduce<TReproducedData>(
            IEnumerable<TReproducedData> reproducedData)
        {
            return new OrderedFiniteEnumerableAttributeDomain<TReproducedData>(reproducedData);
        }
        */
    }

    public class OrderedFiniteEnumerableAttributeDomain<TData> 
        : OrderedFiniteEnumerableAttributeDomain<OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, TData>
    {
        public OrderedFiniteEnumerableAttributeDomain(IEnumerable<TData> universum)
            : base(universum)
        {
            return;
        }

        /*
        public OrderedFiniteEnumerableAttributeDomain(IEnumerable<TData> universum)
            : base(BuildUniversum(universum, out _setDomainCallback))
        {
            _setDomainCallback(this);
            _setDomainCallback = null;
        }
        */

        /*
        protected OrderedFiniteEnumerableAttributeDomain(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> universum)
            : base(universum)
        {
            _setDomainCallback(this);
            _setDomainCallback = null;
        }
        */

        /*
        protected static OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> BuildUniversum(
            IEnumerable<TData> universum, out Action<AttributeDomain<TData>> setDomainCallback)
        {
            return new OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>(
                universum, 
                out setDomainCallback);
        }
        */
    }
}
