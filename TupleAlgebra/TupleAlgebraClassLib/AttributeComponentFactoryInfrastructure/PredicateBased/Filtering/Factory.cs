using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering
{
    public interface IFilteringAttributeComponentFactory<TData>
        : INonFictionalAttributeComponentFactory<
            TData,
            FilteringAttributeComponentFactoryArgs<TData>,
            FilteringAttributeComponent<TData>,
            FilteringAttributeComponentFactoryArgs<TData>>
    {
        static PredicateBuilder<TData> _predicateBuilder = 
            new PredicateBuilder<TData>();

        FilteringAttributeComponentFactoryArgs<TData>
            ISetOperationResultFactory<
                FilteringAttributeComponent<TData>,
                FilteringAttributeComponentFactoryArgs<TData>,
                FilteringAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
                FilteringAttributeComponentFactoryArgs<TData>
                opResultFactoryArgs)
        {
            Expression<Func<TData, bool>> predicateExpr =
                _predicateBuilder.Prepare(
                    opResultFactoryArgs.PredicateExpression);

            return new FilteringAttributeComponentFactoryArgs<TData>(
                predicateExpr,
                opResultFactoryArgs.ProbableRange);
        }

        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                FilteringAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictional(
                FilteringAttributeComponentFactoryArgs<TData> args)
        {
            return new FilteringAttributeComponent<TData>(
                args.Power,
                args.PredicateExpression,
                args.QueryProvider,
                args.QueryExpression);
        }

        FilteringAttributeComponentFactoryArgs<TData>
            ISetOperationResultFactory<
                FilteringAttributeComponent<TData>,
                FilteringAttributeComponentFactoryArgs<TData>,
                FilteringAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
            FilteringAttributeComponent<TData> first,
            FilteringAttributeComponentFactoryArgs<TData> resultElements)
        {
            return new FilteringAttributeComponentFactoryArgs<TData>(
                resultElements.PredicateExpression,
                resultElements.ProbableRange,
                isQuery: false,
                queryProvider: first.Provider);
        }
    }

    public class FilteringAttributeComponentFactory<TData>
        : AttributeComponentFactory<TData>,
          IFilteringAttributeComponentFactory<TData>
    {
        public FilteringAttributeComponentFactory(
            AttributeDomain<TData> domain)
            : base(domain)
        { return; }
    }
}
