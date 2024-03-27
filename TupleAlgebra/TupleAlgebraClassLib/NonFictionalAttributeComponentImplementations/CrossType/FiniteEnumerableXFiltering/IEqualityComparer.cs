using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering
{
    public interface IFiniteEnumerableXFilteringEqualityComparer<
        TData>
        : IFiniteEnumerableXFilteringAttributeComponentBooleanOperator<
            TData,
            IFiniteEnumerableAttributeComponent<TData>>
    {
        bool IInstantBinaryOperator<
            IFiniteEnumerableAttributeComponent<TData>,
            FilteringAttributeComponent<TData>,
            bool>
            .Accept(
                IFiniteEnumerableAttributeComponent<TData> first,
                FilteringAttributeComponent<TData> second)
        {
            return first.EqualsTo(
                second.ToIterableAttributeComponent());
        }
    }
}
