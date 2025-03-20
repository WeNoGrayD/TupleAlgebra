using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider;
using System.Reflection;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable
{
    using static QueryContextHelper;

    public class OrderedFiniteEnumerableAttributeComponentQueryContext : LinqQueryContext
    {
        #region Constructors

        static OrderedFiniteEnumerableAttributeComponentQueryContext()
        {
            IDictionary<string, IList<MethodInfo>> queryMethodPatterns =
                GetQueryMethodPatterns<OrderedFiniteEnumerableAttributeComponentQueryContext>(_queryMethodPattern);
            Helper.RegisterType<OrderedFiniteEnumerableAttributeComponentQueryContext>(
                queryMethodGroups: queryMethodPatterns);

            return;
        }

        #endregion
    }
}
