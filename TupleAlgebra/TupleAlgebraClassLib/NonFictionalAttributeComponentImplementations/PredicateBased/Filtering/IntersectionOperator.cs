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
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    public class IntersectionOperator<TData>
        : NonFictionalAttributeComponentIntersectionOperator<
            TData,
            FilteringAttributeComponentFactoryArgs<TData>,
            FilteringAttributeComponent<TData>,
            IFilteringAttributeComponentFactory<TData>,
            FilteringAttributeComponentFactoryArgs<TData>>,
          IFilteringAttributeComponentBinaryOperator<TData>,
          IFactoryBinaryOperator<
              FilteringAttributeComponent<TData>,
              NonFictionalAttributeComponent<TData>,
              IAttributeComponentFactory<TData>,
              IAttributeComponent<TData>>
    {
        public IAttributeComponent<TData> Visit(
            FilteringAttributeComponent<TData> first,
            FilteringAttributeComponent<TData> second,
            IFilteringAttributeComponentFactory<TData> factory)
        {
            IFilteringAttributeComponentBinaryOperator<TData>
                binOp = this;

            return binOp.VisitStrategy(
                first,
                second,
                factory,
                PredicateBuilder<TData>.Intersect,
                AttributeComponentContentTypeHelper.IntersectWith);
        }

        IAttributeComponent<TData> IFactoryBinaryOperator<
              FilteringAttributeComponent<TData>,
              NonFictionalAttributeComponent<TData>,
              IAttributeComponentFactory<TData>,
              IAttributeComponent<TData>>
            .Visit(
            FilteringAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second,
            IAttributeComponentFactory<TData> factory)
        {
            return second.IntersectWith(first);
        }
    }
}
