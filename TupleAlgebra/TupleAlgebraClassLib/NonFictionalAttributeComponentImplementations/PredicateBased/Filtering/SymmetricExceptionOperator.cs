using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    public class SymmetricExceptionOperator<TData>
        : NonFictionalAttributeComponentSymmetricExceptionOperator<
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
                PredicateBuilder<TData>.SymmetricExcept,
                AttributeComponentContentTypeHelper.SymmetricExceptOf);
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
            return second.SymmetricExceptWith(first);
        }
    }
}
