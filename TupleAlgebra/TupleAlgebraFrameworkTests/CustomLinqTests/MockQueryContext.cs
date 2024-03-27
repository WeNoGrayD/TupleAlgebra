using LINQProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TupleAlgebraFrameworkTests.CustomLinqTests
{
    using static QueryContextHelper;

    internal class MockQueryContext : LinqQueryContext
    {
        #region Constructors

        static MockQueryContext()
        {
            IDictionary<string, IList<MethodInfo>> queryMethodPatterns =
                GetQueryMethodPatterns<MockQueryContext>(_queryMethodPattern);
            Helper.RegisterType<MockQueryContext>(
                queryMethodGroups: queryMethodPatterns);

            return;
        }

        #endregion

        #region Instance methods

        #region Query methods

        protected SingleQueryExecutor<TData, IEnumerable<TQueryResultData>> BuildBufSelectQuery<TData, TQueryResultData>(
            Func<TData, TQueryResultData> transform)
        {
            return new BufSelectQueryExecutor<TData, TQueryResultData>(transform);
        }

        #endregion

        #endregion
    }
}
