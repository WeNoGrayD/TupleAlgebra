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
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable
{
    public abstract record OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>
    {
        public IEnumerable<TData> Values { get; private set; }

        public IComparer<TData> OrderingComparer { get; private set; }

        public bool ValuesAreOrdered { get; set; }

        public OrderedFiniteEnumerableAttributeComponentFactoryArgs(
            IEnumerable<TData> values,
            IComparer<TData> orderingComparer = null,
            bool valuesAreOrdered = false,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            Values = values;
            OrderingComparer = orderingComparer;
            ValuesAreOrdered = valuesAreOrdered;

            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return FiniteEnumerableAttributeComponentPower.Instance;
        }
    }
}
