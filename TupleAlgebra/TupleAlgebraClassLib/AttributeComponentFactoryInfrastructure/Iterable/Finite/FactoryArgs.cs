using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Iterable.Finite;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite
{
    public record FiniteIterableAttributeComponentFactoryArgs<TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<FiniteIterableAttributeComponentFactoryArgs<TData>>,
          INonFictionalAttributeComponentFactoryArgs<TData, FiniteIterableAttributeComponentFactoryArgs<TData>>
    {
        public IEnumerable<TData> Values { get; private set; }

        public FiniteIterableAttributeComponentFactoryArgs(
            IEnumerable<TData> values = null,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            Values = values;

            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return FiniteEnumerableAttributeComponentPower.Instance;
        }
    }
}
