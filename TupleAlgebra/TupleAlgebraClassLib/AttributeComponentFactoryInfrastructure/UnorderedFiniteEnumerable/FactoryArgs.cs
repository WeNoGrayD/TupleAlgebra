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
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable
{
    public record UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
        : NonFictionalAttributeComponentFactoryArgs<TData>,
          INonFictionalAttributeComponentFactoryArgs<UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
    {
        public HashSet<TData> Values { get; private set; }

        public UnorderedFiniteEnumerableAttributeComponentFactoryArgs(
            HashSet<TData> values = null,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            Values = values;

            return;
        }

        public UnorderedFiniteEnumerableAttributeComponentFactoryArgs(
            IEnumerable<TData> values = null,
            bool isQuery = false,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(isQuery, queryProvider, queryExpression)
        {
            Values = values.ToHashSet();

            return;
        }

        protected override AttributeComponentPower CreatePower()
        {
            return FiniteEnumerableAttributeComponentPower.Instance;
        }
    }
}
