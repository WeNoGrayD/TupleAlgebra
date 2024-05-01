using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    public interface IFilteringAttributeComponentBinaryOperator<TData>
        : IFactoryBinaryAttributeComponentAcceptor<
            TData,
            FilteringAttributeComponentFactoryArgs<TData>,
            FilteringAttributeComponent<TData>,
            FilteringAttributeComponent<TData>,
            IFilteringAttributeComponentFactory<TData>,
            FilteringAttributeComponentFactoryArgs<TData>,
            IAttributeComponent<TData>>
    {
        delegate Expression<Func<TData, bool>> BuildPredicateHandler(
            Expression<Func<TData, bool>> first,
            Expression<Func<TData, bool>> second);

        delegate AttributeComponentContentType GetProbableRangeHandler(
            AttributeComponentContentType first,
            AttributeComponentContentType second);

        public IAttributeComponent<TData> AcceptStrategy(
            FilteringAttributeComponent<TData> first,
            FilteringAttributeComponent<TData> second,
            IFilteringAttributeComponentFactory<TData> factory,
            BuildPredicateHandler buildPredicateHandler,
            GetProbableRangeHandler getProbableRangeHandler)
        {
            Expression<Func<TData, bool>> predicateExpr =
                buildPredicateHandler(
                    first.PredicateExpression,
                    second.PredicateExpression);
            AttributeComponentContentType contentType =
                getProbableRangeHandler(
                    first.Power.ContentType,
                    second.Power.ContentType);

            return factory.CreateNonFictional(
                new FilteringAttributeComponentFactoryArgs<TData>(
                    predicateExpr));
        }
    }
}
