using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    public class IntersectionOperator<TData>
        : NonFictionalAttributeComponentIntersectionOperator<
            TData,
            FilteringAttributeComponentFactoryArgs<TData>,
            FilteringAttributeComponent<TData>,
            IFilteringAttributeComponentFactory<TData>,
            FilteringAttributeComponentFactoryArgs<TData>>,
          IFilteringAttributeComponentBinaryOperator<TData>
    {
        public IAttributeComponent<TData> Accept(
            FilteringAttributeComponent<TData> first,
            FilteringAttributeComponent<TData> second,
            IFilteringAttributeComponentFactory<TData> factory)
        {
            IFilteringAttributeComponentBinaryOperator<TData>
                binOp = this;

            return binOp.AcceptStrategy(
                first,
                second,
                factory,
                PredicateBuilder<TData>.Intersect,
                AttributeComponentContentTypeHelper.IntersectWith);
        }
    }
}
