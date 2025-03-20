using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQProvider;
using System.Reflection;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.UnorderedFiniteEnumerable
{
    using static QueryContextHelper;

    public class UnorderedFiniteEnumerableAttributeComponentQueryContext : LinqQueryContext
    {
        #region Constructors

        static UnorderedFiniteEnumerableAttributeComponentQueryContext()
        {
            IDictionary<string, IList<MethodInfo>> queryMethodPatterns =
                GetQueryMethodPatterns<UnorderedFiniteEnumerableAttributeComponentQueryContext>(_queryMethodPattern);
            Helper.RegisterType<UnorderedFiniteEnumerableAttributeComponentQueryContext>(
                queryMethodGroups: queryMethodPatterns);

            return;
        }

        #endregion
    }
}
