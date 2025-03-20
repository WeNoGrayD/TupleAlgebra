using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure;
using TupleAlgebraClassLib.FullAttributeComponentInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering
{
    public record FilteringAttributeComponentFactoryArgs<TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<FilteringAttributeComponentFactoryArgs<TData>>,
          INonFictionalAttributeComponentFactoryArgs<TData, FilteringAttributeComponentFactoryArgs<TData>>
    {
        public Expression<Func<TData, bool>> PredicateExpression 
        { get; private set; }

        public AttributeComponentContentType ContentType { get; }

        public FilteringAttributeComponentFactoryArgs(
            Expression<Func<TData, bool>> predicateExpr,
            AttributeComponentContentType contentType = AttributeComponentContentType.NonFictional,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            PredicateExpression = predicateExpr;
            ContentType = contentType;
            Power = CreatePower();

            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return ContentType switch
            {
                AttributeComponentContentType.Empty => EmptyAttributeComponentPower.Instance,
                AttributeComponentContentType.Full => FullAttributeComponentPower.Instance,
                _ => FilteringAttributeComponentPower.Instance
            };
        }
    }
}
