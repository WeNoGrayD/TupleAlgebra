using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework;
using System.Reflection;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure
{
    /// <summary>
    /// Провайдер запросов к упорядоченной конечной перечислимой компонентой аттрибута.
    /// </summary>
    public class OrderedFiniteEnumerableNonFictionalAttributeComponentQueryProvider
        : NonFictionalAttributeComponentQueryProvider
    {
        /// <summary>
        /// Построение фабричных аргументов для создания IQueryable-компоненты.
        /// </summary>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <param name="component"></param>
        /// <param name="queryResultDomain"></param>
        /// <returns></returns>
        protected override AttributeComponentFactoryArgs<TQueryResult> ConstructFactoryArgs<TQueryResult>(
            NonFictionalAttributeComponent<TQueryResult> component,
            IEnumerable<TQueryResult> values,
            NonFictionalAttributeComponentQueryProvider queryProvider,
            Expression queryExpression = null)
        {
            return new OrderedFiniteEnumerableNonFictionalAttributeComponentFactoryArgs<TQueryResult>(
                component.Domain,
                values,
                queryProvider,
                queryExpression);
        }
    }
}
