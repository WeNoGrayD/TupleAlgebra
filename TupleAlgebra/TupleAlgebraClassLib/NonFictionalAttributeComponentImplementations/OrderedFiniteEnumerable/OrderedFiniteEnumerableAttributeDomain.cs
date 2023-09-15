using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public class OrderedFiniteEnumerableAttributeDomain<TAttributeComponent, TData>
        : AttributeDomain<TData>
        where TAttributeComponent : OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>
    {
        public OrderedFiniteEnumerableAttributeDomain(
            IEnumerable<TData> universum)
            : base()
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
    }
}
