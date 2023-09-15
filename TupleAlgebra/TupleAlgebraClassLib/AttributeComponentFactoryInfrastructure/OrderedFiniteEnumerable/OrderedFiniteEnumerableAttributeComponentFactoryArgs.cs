using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Linq.Expressions;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable
{
    public class OrderedFiniteEnumerableAttributeComponentFactoryArgs
        : NonFictionalAttributeComponentFactoryArgs
    {
        public IEnumerable Values { get; private set; }

        public object OrderingComparer { get; private set; }

        public bool ValuesAreOrdered { get; set; }

        private OrderedFiniteEnumerableAttributeComponentFactoryArgs(
            object orderingComparer = null,
            IEnumerable values = null,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            Values = values;
            OrderingComparer = orderingComparer;
            ValuesAreOrdered = false;

            return;
        }

        public static OrderedFiniteEnumerableAttributeComponentFactoryArgs Construct<TData>(
            IComparer<TData> orderingComparer = null,
            IEnumerable<TData> values = null,
            bool isQuery = false,
            Func<AttributeDomain<TData>> domainGetter = null,
            OrderedFiniteEnumerableAttributeComponentQueryProvider queryProvider = null,
            Expression queryExpression = null)
        {
            var factoryArgs = new OrderedFiniteEnumerableAttributeComponentFactoryArgs(
                orderingComparer,
                values, 
                isQuery,
                queryProvider,
                queryExpression);
            factoryArgs.SetDomainGetter(domainGetter);

            return factoryArgs;
        }

        protected override AttributeComponentPower CreatePower<TData>()
        {
            return new OrderedFiniteEnumerableNonFictionalAttributeComponentPower<TData>();
        }
    }
}
