using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering
{
    public record FilteringAttributeComponentFactoryArgs<TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<FilteringAttributeComponentFactoryArgs<TData>>
    {
        public Expression<Func<TData, bool>> PredicateExpression 
        { get; private set; }

        public AttributeComponentContentType ProbableRange 
        { get; private set; }

        public FilteringAttributeComponentFactoryArgs(
            Expression<Func<TData, bool>> predicateExpr,
            AttributeComponentContentType probableRange = 
                AttributeComponentContentType.NonFictional,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            PredicateExpression = predicateExpr;
            ProbableRange = probableRange;
            Power = CreatePower();

            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return new FilteringAttributeComponentPower<TData>(
                ProbableRange);
        }
    }
}
