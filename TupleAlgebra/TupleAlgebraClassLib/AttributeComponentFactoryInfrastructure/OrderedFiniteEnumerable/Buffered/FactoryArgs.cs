using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered
{
    public record BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
        : OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    {
        public BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs(
            IEnumerable<TData> values,
            IComparer<TData> orderingComparer = null,
            bool valuesAreOrdered = false,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  values,
                  orderingComparer,
                  valuesAreOrdered,
                  isQuery,
                  queryProvider,
                  queryExpression)
        {
            return;
        }
    }
}
