using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using LINQProvider;
using System.Reflection;
using TupleAlgebraFrameworkTests.DataModels;

namespace TupleAlgebraFrameworkTests.CustomLinqTests
{
    internal enum QueryKind : byte
    {
        Aggregate,
        All,
        Any,
        Contains,
        Count,
        First,
        Select,
        SelectMany,
        Single,
        Skip,
        SkipWhile,
        Take,
        TakeWhile,
        Where
    }

    public enum QueryContent : byte
    {
        All,
        GreaterThan0,
        GreaterThan5,
        GreaterThan7,
        GreaterThanX,
        LesserThan5,
        LesserThan10,
        LesserThanX,
        Pow2,
        Pow3,
        PowN2,
        ToString,
        IsOdd,
        IsEven,
        Mod3Is0,
        Mod6,
        OneToManyForumUsers,
        Number3,
        Number10,
        Number1000
    }

    internal static class MockQueryableHelper
    {
        public static GenericMockQueryable<T> AsMockQueryable<T>(this IQueryable<T> queryable) =>
            (GenericMockQueryable<T>)queryable;

        public static bool All<TData>(
            this GenericMockQueryable<TData> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr =
                MockQueryableHelper.CreateExpression(QueryKind.All, queryContent)
                as LambdaExpression;

            return All(source, predicateExpr as Expression<Func<TData, bool>>);
        }

        private static bool All<TData>(
            this IQueryable<TData> source,
            Expression<Func<TData, bool>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<int> queryied = source.Provider.CreateQuery<int>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    predicateExpr));

            return queryied.Provider.Execute<bool>(queryied.Expression);
        }

        public static bool Any<TData>(this GenericMockQueryable<TData> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr =
                MockQueryableHelper.CreateExpression(QueryKind.Any, queryContent)
                as LambdaExpression;

            return Any(source, predicateExpr as Expression<Func<TData, bool>>);
        }

        private static bool Any<TData>(
            this IQueryable<TData> source,
            Expression<Func<TData, bool>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<int> queryied = source.Provider.CreateQuery<int>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    predicateExpr));

            return queryied.Provider.Execute<bool>(queryied.Expression);
        }

        public static IQueryable<TQueryResultData> BufSelect<TData, TQueryResultData>(
            this GenericMockQueryable<TData> source,
            QueryContent queryContent)
        {
            LambdaExpression funcExpr =
                MockQueryableHelper.CreateExpression(QueryKind.Select, queryContent)
                as LambdaExpression
                ??
                CreateGenericExpression<TQueryResultData>(QueryKind.Select, queryContent)
                as LambdaExpression;

            return BufSelect(source, (dynamic)funcExpr);
        }

        private static IQueryable<TQueryResultData> BufSelect<TData, TQueryResultData>(
            this IQueryable<TData> source, Expression<Func<TData, TQueryResultData>> selectorExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<TQueryResultData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData), typeof(TQueryResultData) },
                    selectorExpr)); ;
        }

        public static bool Contains<TData>(
            this GenericMockQueryable<TData> source,
            QueryContent queryContent)
        {
            TData item = (TData)
                MockQueryableHelper.CreateExpression(QueryKind.Contains, queryContent);

            return Contains(source, item);
        }

        private static bool Contains<TData>(
            this IQueryable<TData> source,
            TData item = default(TData))
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<TData> queryied = source.Provider.CreateQuery<TData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    Expression.Constant(item)));

            return queryied.Provider.Execute<bool>(queryied.Expression);
        }

        public static int Count<TData>(this GenericMockQueryable<TData> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr =
                MockQueryableHelper.CreateExpression(QueryKind.First, queryContent)
                as LambdaExpression;

            return Count(source, predicateExpr as Expression<Func<TData, bool>>);
        }

        private static int Count<TData>(
            this IQueryable<TData> source,
            Expression<Func<TData, bool>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<int> queryied = source.Provider.CreateQuery<int>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    predicateExpr));

            return queryied.Provider.Execute<int>(queryied.Expression);
        }

        public static TData First<TData>(this GenericMockQueryable<TData> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr =
                MockQueryableHelper.CreateExpression(QueryKind.First, queryContent)
                as LambdaExpression;

            return First(source, predicateExpr as Expression<Func<TData, bool>>);
        }

        private static TData First<TData>(
            this IQueryable<TData> source,
            Expression<Func<TData, bool>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<int> queryied = source.Provider.CreateQuery<int>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    predicateExpr));

            return queryied.Provider.Execute<TData>(queryied.Expression);
        }

        public static IQueryable<TQueryResultData> Join<TOuterData, TInnerData, TQueryResultData>(
            this GenericMockQueryable<TOuterData> outerSource,
            IEnumerable<TInnerData> innerSource,
            QueryContent outerKeySelectorContent,
            QueryContent innerKeySelectorContent,
            QueryContent resultSelectorContent)
        {
            LambdaExpression outerKeySelectorExpr =
                MockQueryableHelper.CreateExpression(QueryKind.Select, outerKeySelectorContent)
                as LambdaExpression,
                             innerKeySelectorExpr =
                MockQueryableHelper.CreateExpression(QueryKind.Select, innerKeySelectorContent)
                as LambdaExpression,
                             resultSelectorExpr =
                MockQueryableHelper.CreateExpression(QueryKind.Select, resultSelectorContent)
                as LambdaExpression;

            return Join(
                outerSource,
                innerSource,
                (dynamic)outerKeySelectorExpr,
                (dynamic)innerKeySelectorExpr,
                (dynamic)resultSelectorExpr);
        }

        private static IQueryable<TQueryResultData> Join<TOuterData, TInnerData, TKey, TQueryResultData>(
            this IQueryable<TOuterData> outerSource,
            IEnumerable<TInnerData> innerSource,
            Expression<Func<TOuterData, TKey>> outerKeySelector = null,
            Expression<Func<TInnerData, TKey>> innerKeySelector = null,
            Expression<Func<TOuterData, TInnerData, TQueryResultData>> resultSelector = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return outerSource.Provider.CreateQuery<TQueryResultData>(
                BuildCoveredExpression(
                    outerSource,
                    queryMethodInfo,
                    new Type[] { typeof(TOuterData), typeof(TInnerData), typeof(TKey), typeof(TQueryResultData) },
                    Expression.Constant(innerSource),
                    outerKeySelector,
                    innerKeySelector,
                    resultSelector));
        }

        public static IQueryable<TQueryResultData> Select<TData, TQueryResultData>(
            this GenericMockQueryable<TData> source,
            QueryContent queryContent)
        {
            LambdaExpression funcExpr =
                MockQueryableHelper.CreateExpression(QueryKind.Select, queryContent)
                as LambdaExpression;

            return Select(source, (dynamic)funcExpr);
        }

        private static IQueryable<TQueryResultData> Select<TData, TQueryResultData>(
            this IQueryable<TData> source, Expression<Func<TData, TQueryResultData>> selectorExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<TQueryResultData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData), typeof(TQueryResultData) },
                    selectorExpr)); ;
        }

        public static IQueryable<TQueryResultData> SelectMany<TData, TQueryResultData>(
            this GenericMockQueryable<TData> source,
            QueryContent innerEnumerableSelectorContent,
            QueryContent resultSelectorContent)
        {
            LambdaExpression innerEnumerableSelectorExpr =
                MockQueryableHelper.CreateExpression(QueryKind.SelectMany, innerEnumerableSelectorContent)
                as LambdaExpression,
                             resultSelectorExpr =
                MockQueryableHelper.CreateExpression(QueryKind.Select, resultSelectorContent)
                as LambdaExpression;

            return SelectMany(source, (dynamic)innerEnumerableSelectorExpr, (dynamic)resultSelectorExpr);
        }

        private static IQueryable<TQueryResultData> SelectMany<TOuterData, TInnerData, TQueryResultData>(
            this IQueryable<TOuterData> source,
            Expression<Func<TOuterData, IEnumerable<TInnerData>>> innerEnumerableSelectorExpr = null,
            Expression<Func<TOuterData, TInnerData, TQueryResultData>> resultSelectorExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<TQueryResultData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TOuterData), typeof(TInnerData), typeof(TQueryResultData) },
                    innerEnumerableSelectorExpr,
                    resultSelectorExpr));
        }

        public static IQueryable<TData> Where<TData>(this GenericMockQueryable<TData> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr =
                MockQueryableHelper.CreateExpression(QueryKind.Where, queryContent)
                as LambdaExpression;

            return Where(source, predicateExpr as Expression<Func<TData, bool>>);
        }

        private static IQueryable<TData> Where<TData>(
            this IQueryable<TData> source,
            Expression<Func<TData, bool>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<TData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    predicateExpr));
        }

        public static IQueryable<TData> Take<TData>(
            this GenericMockQueryable<TData> source,
            QueryContent queryContent)
        {

            int takingCount = (int)
                MockQueryableHelper.CreateExpression(QueryKind.Take, queryContent);

            return Take(source, takingCount);
        }

        private static IQueryable<TData> Take<TData>(
            this IQueryable<TData> source,
            int takingCount = 0)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<TData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    Expression.Constant(takingCount)));
        }

        public static IQueryable<TData> TakeWhile<TData>(
            this GenericMockQueryable<TData> source,
            QueryContent queryContent)
        {
            LambdaExpression predicateExpr =
                MockQueryableHelper.CreateExpression(QueryKind.TakeWhile, queryContent)
                as LambdaExpression;

            return TakeWhile(source, predicateExpr as Expression<Func<TData, bool>>);
        }

        private static IQueryable<TData> TakeWhile<TData>(
            this IQueryable<TData> source,
            Expression<Func<TData, bool>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<TData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    predicateExpr));
        }

        public static IQueryable<TData> Skip<TData>(
            this GenericMockQueryable<TData> source,
            QueryContent queryContent)
        {
            int skippingCount = (int)
                MockQueryableHelper.CreateExpression(QueryKind.Skip, queryContent);

            return Skip(source, skippingCount);
        }

        private static IQueryable<TData> Skip<TData>(
            this IQueryable<TData> source,
            int skippingCount = 0)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<TData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    Expression.Constant(skippingCount)));
        }

        public static IQueryable<TData> SkipWhile<TData>(
            this GenericMockQueryable<TData> source,
            QueryContent queryContent)
        {
            LambdaExpression predicateExpr =
                MockQueryableHelper.CreateExpression(QueryKind.SkipWhile, queryContent)
                as LambdaExpression;

            return SkipWhile(source, predicateExpr as Expression<Func<TData, bool>>);
        }

        private static IQueryable<TData> SkipWhile<TData>(
            this IQueryable<TData> source,
            Expression<Func<TData, bool>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<TData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    predicateExpr));
        }

        public static TData[] ToArray<TData>(this IQueryable<TData> source)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<TData> queryied = source.Provider.CreateQuery<TData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) }));

            return queryied.Provider.Execute<TData[]>(queryied.Expression);
        }

        public static Dictionary<TKey, TValue> ToDictionary<TData, TKey, TValue>(
            this GenericMockQueryable<TData> source,
            QueryContent keySelectorContent,
            QueryContent valueSelectorContent)
            where TKey : notnull
        {
            Expression<Func<TData, TKey>> keySelectorExpr =
                (MockQueryableHelper.CreateGenericExpression<TData>(QueryKind.Select, keySelectorContent)
                 as Expression<Func<TData, TKey>>)!;
            Expression<Func<TData, TValue>> valueSelectorExpr =
                (MockQueryableHelper.CreateExpression(QueryKind.Select, valueSelectorContent)
                 as Expression<Func<TData, TValue>>)!;

            return ToDictionary(
                source,
                keySelectorExpr,
                valueSelectorExpr);
        }

        private static Dictionary<TKey, TValue> ToDictionary<TData, TKey, TValue>(
            this IQueryable<TData> source,
            Expression<Func<TData, TKey>> keySelectorExpr = null,
            Expression<Func<TData, TValue>> valueSelectorExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<KeyValuePair<TKey, TValue>> queryied =
                source.Provider.CreateQuery<KeyValuePair<TKey, TValue>>(
                    BuildCoveredExpression(
                        source,
                        queryMethodInfo,
                        new Type[] { typeof(TData), typeof(TKey), typeof(TValue) },
                        keySelectorExpr,
                        valueSelectorExpr));

            return queryied.Provider.Execute<Dictionary<TKey, TValue>>(queryied.Expression);
        }

        public static List<TData> ToList<TData>(this IQueryable<TData> source)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<TData> queryied = source.Provider.CreateQuery<TData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) }));

            return queryied.Provider.Execute<List<TData>>(queryied.Expression);
        }

        private static Expression BuildCoveredExpression<TData>(
            IQueryable<TData> source,
            MethodInfo queryMethodInfo,
            Type[] genericArguments,
            params Expression[] queryMethodParams)
        {
            Expression[] queryMethodParamsBuf = new Expression[queryMethodParams.Length + 1];
            queryMethodParamsBuf[0] = source.Expression;
            queryMethodParams.Select(param => param switch
            {
                LambdaExpression lambda => Expression.Quote(lambda),
                Expression constant => constant//Expression.Constant(constant) as Expression
            })
                .ToArray().CopyTo(queryMethodParamsBuf, 1);

            return Expression.Call(
                null,
                queryMethodInfo.MakeGenericMethod(genericArguments),
                queryMethodParamsBuf);
        }

        public static object CreateExpression(QueryKind queryKind, QueryContent queryContent)
        {
            object queryParameter = queryContent switch
            {
                QueryContent.Number3 => 3,
                QueryContent.Number10 => 10,
                QueryContent.Number1000 => 1000,
                _ => null
            };
            LambdaExpression queryBody = queryContent switch
            {
                QueryContent.GreaterThan0 => (Expression<Func<int, bool>>)((int data) => data > 0),
                QueryContent.GreaterThan5 => (Expression<Func<int, bool>>)((int data1) => data1 > 5),
                QueryContent.GreaterThan7 => (Expression<Func<int, bool>>)((int data) => data > 7),
                QueryContent.GreaterThanX => (Expression<Func<int, IEnumerable<int>>>)
                    ((int data) => MockQueryable.DataSource.SkipWhile(i => i <= data)),
                QueryContent.LesserThan5 => (Expression<Func<int, bool>>)((int data) => data < 5),
                QueryContent.LesserThan10 => (Expression<Func<int, bool>>)((int data2) => data2 < 10),
                QueryContent.LesserThanX => (Expression<Func<int, IEnumerable<int>>>)
                    ((int data) => MockQueryable.DataSource.TakeWhile(i => i < data)),
                QueryContent.IsOdd => (Expression<Func<int, bool>>)((int data) => (data & 1) == 1),
                QueryContent.IsEven => (Expression<Func<int, bool>>)((int data) => (data & 1) == 0),
                QueryContent.ToString => (Expression<Func<int, string>>)((int data) => $"The number is {data}."),
                QueryContent.Pow2 => (Expression<Func<int, int>>)((int data) => data ^ 2),
                QueryContent.Pow3 => (Expression<Func<int, int>>)((int data) => data ^ 3),
                QueryContent.PowN2 => (Expression<Func<int, int>>)((int data) => data ^ -2),
                QueryContent.Mod3Is0 => (Expression<Func<int, bool>>)((int data) => data % 3 == 0),
                QueryContent.Mod6 => (Expression<Func<int, int, int>>)
                    ((int outerData, int innerData) => outerData == innerData ? outerData : 0),
                QueryContent.OneToManyForumUsers => (Expression<Func<ForumUser, IEnumerable<ForumUser>>>)
                    ((ForumUser fu) => ForumUsersMockQueryable.DataSource),
                _ => null
            };

            return (queryKind, queryContent) switch
            {
                (QueryKind.All, QueryContent.GreaterThan0) => queryBody,
                (QueryKind.All, _) => throw new ArgumentException(),
                (QueryKind.Any, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.Any, QueryContent.LesserThan10) => queryBody,
                (QueryKind.Any, _) => throw new ArgumentException(),
                (QueryKind.Contains, QueryContent.Number10) => queryParameter,
                (QueryKind.Contains, QueryContent.Number1000) => queryParameter,
                (QueryKind.Contains, _) => throw new ArgumentException(),
                (QueryKind.Count, QueryContent.LesserThan5) => queryBody,
                (QueryKind.Count, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.Count, QueryContent.GreaterThan7) => queryBody,
                (QueryKind.Count, QueryContent.LesserThan10) => queryBody,
                (QueryKind.Count, _) => throw new ArgumentException(),
                (QueryKind.First, QueryContent.LesserThan5) => queryBody,
                (QueryKind.First, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.First, QueryContent.GreaterThan7) => queryBody,
                (QueryKind.First, QueryContent.LesserThan10) => queryBody,
                (QueryKind.First, _) => throw new ArgumentException(),
                (QueryKind.Select, QueryContent.All) => queryBody,
                (QueryKind.Select, QueryContent.IsOdd) => queryBody,
                (QueryKind.Select, QueryContent.IsEven) => queryBody,
                (QueryKind.Select, QueryContent.ToString) => queryBody,
                (QueryKind.Select, QueryContent.Pow2) => queryBody,
                (QueryKind.Select, QueryContent.Pow3) => queryBody,
                (QueryKind.Select, QueryContent.PowN2) => queryBody,
                (QueryKind.Select, QueryContent.Mod3Is0) => queryBody,
                (QueryKind.Select, QueryContent.Mod6) => queryBody,
                (QueryKind.Select, _) => throw new ArgumentException(),
                (QueryKind.SelectMany, QueryContent.GreaterThanX) => queryBody,
                (QueryKind.SelectMany, QueryContent.LesserThanX) => queryBody,
                (QueryKind.SelectMany, QueryContent.OneToManyForumUsers) => queryBody,
                (QueryKind.SelectMany, _) => throw new ArgumentException(),
                (QueryKind.Skip, QueryContent.Number10) => queryParameter,
                (QueryKind.Skip, QueryContent.Number1000) => queryParameter,
                (QueryKind.Skip, _) => throw new ArgumentException(),
                (QueryKind.SkipWhile, QueryContent.LesserThan5) => queryBody,
                (QueryKind.SkipWhile, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.SkipWhile, QueryContent.LesserThan10) => queryBody,
                (QueryKind.SkipWhile, _) => throw new ArgumentException(),
                (QueryKind.Take, QueryContent.Number3) => queryParameter,
                (QueryKind.Take, QueryContent.Number10) => queryParameter,
                (QueryKind.Take, QueryContent.Number1000) => queryParameter,
                (QueryKind.Take, _) => throw new ArgumentException(),
                (QueryKind.TakeWhile, QueryContent.GreaterThan0) => queryBody,
                (QueryKind.TakeWhile, QueryContent.LesserThan5) => queryBody,
                (QueryKind.TakeWhile, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.TakeWhile, QueryContent.LesserThan10) => queryBody,
                (QueryKind.TakeWhile, QueryContent.IsOdd) => queryBody,
                (QueryKind.TakeWhile, QueryContent.IsEven) => queryBody,
                (QueryKind.TakeWhile, _) => throw new ArgumentException(),
                (QueryKind.Where, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.Where, QueryContent.LesserThan10) => queryBody,
                (QueryKind.Where, _) => throw new ArgumentException(),
                _ => throw new ArgumentException()
            };
        }

        public static object CreateGenericExpression<TData>(QueryKind queryKind, QueryContent queryContent)
        {
            LambdaExpression queryBody = queryContent switch
            {
                QueryContent.All => (Expression<Func<TData, TData>>)((d) => d),
                _ => null
            };

            return (queryKind, queryContent) switch
            {
                (QueryKind.Select, QueryContent.All) => queryBody,
                _ => throw new ArgumentException()
            };
        }
    }
}
