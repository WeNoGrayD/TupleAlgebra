using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable
{
    /*
    /// <summary>
    /// Провайдер запросов к упорядоченной конечной перечислимой компонентой аттрибута.
    /// </summary>
    public class OrderedFiniteEnumerableAttributeComponentQueryPipelineExecutor
        : QueryPipelineExecutor
    {
        
        /// <summary>
        /// Построение фабричных аргументов для создания IQueryable-компоненты.
        /// </summary>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <param name="component"></param>
        /// <param name="queryResultDomain"></param>
        /// <returns></returns>
        protected override AttributeComponentFactoryArgs<TQueryResult> ConstructFactoryArgs<TQueryResult>(
            AttributeComponent<TQueryResult> component,
            IEnumerable<TQueryResult> values,
            AttributeComponentQueryProvider queryProvider,
            Expression queryExpression)
        {
            return new OrderedFiniteEnumerableAttributeComponentFactoryArgs<TQueryResult>(
                component.Domain,
                values,
                queryProvider,
                queryExpression);
        }
    }
    */
}
