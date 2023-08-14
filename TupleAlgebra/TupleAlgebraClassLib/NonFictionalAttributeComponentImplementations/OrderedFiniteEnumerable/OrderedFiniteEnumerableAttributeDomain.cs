using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;

/* WARNING!!! ИСПРАВИТЬ new CONSTRAINT! */

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{

    public class OrderedFiniteEnumerableAttributeDomain<TAttributeComponent, TData>
        : AttributeDomain<TData>
        where TAttributeComponent : OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>//, new()
    {
        public OrderedFiniteEnumerableAttributeDomain(
            IEnumerable<TData> universum,
            System.Linq.Expressions.Expression queryExpression = null)
            : base(queryExpression)
        {
            Universum = BuildUniversum(universum);

            return;
        }

        protected OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> BuildUniversum(
            IEnumerable<TData> universum)
        {
            AttributeComponentFactoryArgs factoryArgs = 
                OrderedFiniteEnumerableAttributeComponentFactoryArgs.Construct(
                    values: universum,
                    domainGetter: UniversumDomainGetter);
            factoryArgs.Power = new AttributeUniversumPower(
                (factoryArgs.Power as NonFictionalAttributeComponentInfrastructure.NonFictionalAttributeComponentPower<TData>)!);

            return BuildUniversum<TAttributeComponent>(factoryArgs);
        }
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
