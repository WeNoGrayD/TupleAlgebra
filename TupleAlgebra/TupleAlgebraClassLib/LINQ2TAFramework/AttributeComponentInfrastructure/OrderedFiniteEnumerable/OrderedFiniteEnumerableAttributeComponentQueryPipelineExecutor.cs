using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable
{
    /// <summary>
    /// Провайдер запросов к упорядоченной конечной перечислимой компонентой аттрибута.
    /// </summary>
    public class OrderedFiniteEnumerableAttributeComponentQueryPipelineExecutor
        : QueryPipelineExecutor
    {
        #region Contstructors

        public OrderedFiniteEnumerableAttributeComponentQueryPipelineExecutor(
            IEnumerable dataSource,
            IQueryPipelineMiddleware firstQueryExecutor)
            : base(dataSource, firstQueryExecutor) 
        {
            return;
        }

        #endregion
    }
}
