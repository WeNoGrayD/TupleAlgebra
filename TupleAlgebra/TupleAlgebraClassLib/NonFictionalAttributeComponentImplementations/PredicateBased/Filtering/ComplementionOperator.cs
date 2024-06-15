using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using System.Linq.Expressions;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    public class ComplementionOperator<TData>
        : NonFictionalAttributeComponentComplementationOperator<
            TData,
            FilteringAttributeComponentFactoryArgs<TData>,
            FilteringAttributeComponent<TData>,
            IFilteringAttributeComponentFactory<TData>,
            FilteringAttributeComponentFactoryArgs<TData>>,
          IFactoryUnaryAttributeComponentVisitor<
            TData,
            FilteringAttributeComponentFactoryArgs<TData>,
            FilteringAttributeComponent<TData>,
            IFilteringAttributeComponentFactory<TData>,
            FilteringAttributeComponentFactoryArgs<TData>,
            IAttributeComponent<TData>>
    {
        IAttributeComponent<TData> IFactoryUnaryOperator<
            FilteringAttributeComponent<TData>,
            IFilteringAttributeComponentFactory<TData>,
            IAttributeComponent<TData>>
            .Visit(
                FilteringAttributeComponent<TData> first,
                IFilteringAttributeComponentFactory<TData> factory)
        {
            Expression<Func<TData, bool>> predicateExpr =
                PredicateBuilder<TData>.Complement(
                    first.PredicateExpression);
            AttributeComponentContentType contentType =
                first.Power.ContentType.ComplementThe();

            return factory.CreateNonFictional(
                new FilteringAttributeComponentFactoryArgs<TData>(
                    predicateExpr));
        }
    }
}
