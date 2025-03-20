using LINQProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default
{
    using static QueryContextHelper;

    public class DefaultAttributeComponentQueryContext : LinqQueryContext
    {
        #region Constructors

        static DefaultAttributeComponentQueryContext()
        {
            IDictionary<string, IList<MethodInfo>> queryMethodPatterns =
                GetQueryMethodPatterns<DefaultAttributeComponentQueryContext>(_queryMethodPattern);
            Helper.RegisterType<DefaultAttributeComponentQueryContext>(
                queryMethodGroups: queryMethodPatterns);

            return;
        }

        #endregion
    }
}
