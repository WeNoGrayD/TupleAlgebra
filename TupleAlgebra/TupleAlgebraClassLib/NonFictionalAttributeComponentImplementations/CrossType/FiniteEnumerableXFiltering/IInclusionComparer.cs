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
    public interface IFiniteEnumerableXFilteringInclusionComparer<TData>
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
            return first.Includes(second.ToIterableAttributeComponent());
        }
    }

    public interface IFilteringXFiniteEnumerableInclusionComparer<
        TData,
        CTOperand1,
        CTFactory,
        CTFactoryArgs>
        : IFilteringXFiniteEnumerableAttributeComponentBooleanOperator<
            TData>
        where CTOperand1 : NonFictionalAttributeComponent<TData>, IFiniteEnumerableAttributeComponent<TData>
    {
        bool IInstantBinaryOperator<
            FilteringAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>,
            bool>
            .Accept(
                FilteringAttributeComponent<TData> first,
                IFiniteEnumerableAttributeComponent<TData> second)
        {
            return first.ToIterableAttributeComponent().Includes(second);
        }
    }
}
