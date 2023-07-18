using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TupleAlgebraTests.DataModels;
using System.Linq.Expressions;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Diagnostics;
using LINQProvider;
using LINQProvider.QueryPipelineInfrastructure;

[assembly: InternalsVisibleTo("LINQProvider")]

namespace TupleAlgebraTests
{
    internal enum QueryKind
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
        Where,
        ToArray,
        ToDictionary,
        ToList
    }

    internal enum QueryContent
    {
        All,
        KeyIsNumberSquare,
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
        Number10,
        Number1000
    }

    internal class ForumUsersMockQueryable : GenericMockQueryable<ForumUser>
    {
        private static IEnumerable<ForumUser> _dataSource = ForumDatabase.Domain;

        public static IEnumerable<ForumUser> DataSource => _dataSource;

        public ForumUsersMockQueryable()
        {
            Expression = Expression.Constant(_dataSource.AsQueryable());
        }

        public ForumUsersMockQueryable(Expression queryExpression, IQueryProvider provider)
            : base(queryExpression, provider)
        { }
    }

    internal class MockQueryable : GenericMockQueryable<int>
    {
        private static IEnumerable<int> _dataSource = Enumerable.Range(1, 1000000);

        public static IEnumerable<int> DataSource => _dataSource;

        public IEnumerable<int> InstanceDataSource => _dataSource;

        public MockQueryable()
        {
            Expression = Expression.Constant(_dataSource.AsQueryable());
        }

        public MockQueryable(Expression queryExpression, IQueryProvider provider) 
            : base(queryExpression, provider)
        { }
    }

    internal class MultidimensionalMockQueryable : GenericMockQueryable<MockQueryable>
    {
        private static IEnumerable<MockQueryable> _dataSource = new List<MockQueryable>()
        {
            new MockQueryable(),
            new MockQueryable(),
            new MockQueryable()
        };

        public static IEnumerable<MockQueryable> DataSource => _dataSource;

        public MultidimensionalMockQueryable()
        {
            Expression = Expression.Constant(_dataSource.AsQueryable());
        }

        public MultidimensionalMockQueryable(Expression queryExpression, IQueryProvider provider)
            : base(queryExpression, provider)
        { }
    }

    internal class GenericMockQueryable<T> : IQueryable<T>
    {
        public Type ElementType => typeof(T);

        protected IQueryProvider _provider;

        public IQueryProvider Provider { get => _provider; }

        public Expression Expression { get; protected set; }

        public GenericMockQueryable()
        {
            Expression = Expression.Constant(null);
            _provider = new MockQueryProvider<T>();
        }

        public GenericMockQueryable(Expression queryExpression, IQueryProvider provider)
        {
            Expression = queryExpression;
            _provider = provider;
        }

        public IEnumerator<T> GetEnumerator() => Provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class MockQueryProvider<T> : QueryProvider
    {
        static MockQueryProvider()
        {
            //_queryContext = new MockQueryContext();
        }

        protected override IQueryable<TQueryResult> CreateQueryImpl<TQueryResult>(
            Expression queryExpression,
            IQueryable dataSource)
        {
            return new GenericMockQueryable<TQueryResult>(queryExpression, this);
        }

        protected override QueryPipelineScheduler CreateQueryPipelineExecutor(
            QueryContext queryContext,
            IEnumerable<MethodCallExpression> methodCallChain,
            IEnumerable dataSource)
            => new MockQueryPipelineScheduler(queryContext, methodCallChain, dataSource);
    }

    internal class MockQueryContext : QueryContext
    {
    }

    internal class MockQueryPipelineScheduler : QueryPipelineScheduler
    {
        public MockQueryPipelineScheduler(
            QueryContext queryContext,
            IEnumerable<MethodCallExpression> methodCallChain,
            IEnumerable dataSource) 
            : base(queryContext, methodCallChain, dataSource) { }
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

            return SelectMany(source, (dynamic)innerEnumerableSelectorExpr, (dynamic) resultSelectorExpr);
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

        public static TData[] ToArray<TData>(this GenericMockQueryable<TData> source)
        {
            return ToArray(source);
        }

        private static TData[] ToArray<TData>(
            this IQueryable<TData> source)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<TData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) })) as TData[];
        }

        public static Dictionary<TKey, TValue> ToDictionary<TData, TKey, TValue>(
            this GenericMockQueryable<TData> source,
            QueryContent keySelectorContent,
            QueryContent valueSelectorContent)
        {
            LambdaExpression keySelectorExpr =
                MockQueryableHelper.CreateExpression(QueryKind.ToDictionary, keySelectorContent)
                as LambdaExpression,
                             valueSelectorExpr =
                MockQueryableHelper.CreateExpression(QueryKind.Select, valueSelectorContent)
                as LambdaExpression;

            return ToDictionary(
                source, 
                keySelectorExpr as Expression<Func<TData, TKey>>, 
                valueSelectorExpr as Expression<Func<TData, TValue>>);
        }

        private static Dictionary<TKey, TValue> ToDictionary<TData, TKey, TValue>(
            this IQueryable<TData> source,
            Expression<Func<TData, TKey>> keySelectorExpr = null,
            Expression<Func<TData, TValue>> valueSelectorExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<KeyValuePair<TKey, TValue>>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) },
                    keySelectorExpr,
                    valueSelectorExpr)) as Dictionary<TKey, TValue>;
        }

        public static List<TData> ToList<TData>(this GenericMockQueryable<TData> source)
        {
            return ToList(source);
        }

        private static List<TData> ToList<TData>(
            this IQueryable<TData> source)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<TData>(
                BuildCoveredExpression(
                    source,
                    queryMethodInfo,
                    new Type[] { typeof(TData) })) as List<TData>;
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
                //QueryKind.Aggregate => null,
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
                (QueryKind.Take, QueryContent.Number10) => queryParameter,
                (QueryKind.Take, QueryContent.Number1000) => queryParameter,
                (QueryKind.Take, _) => throw new ArgumentException(),
                (QueryKind.TakeWhile, QueryContent.GreaterThan0) => queryBody,
                (QueryKind.TakeWhile, QueryContent.LesserThan5) => queryBody,
                (QueryKind.TakeWhile, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.TakeWhile, QueryContent.LesserThan10) => queryBody,
                (QueryKind.TakeWhile, _) => throw new ArgumentException(),
                (QueryKind.Where, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.Where, QueryContent.LesserThan10) => queryBody,
                (QueryKind.Where, _) => throw new ArgumentException(),
                (QueryKind.ToArray, QueryContent.All) => queryBody,
                (QueryKind.ToArray, _) => throw new ArgumentException(),
                (QueryKind.ToDictionary, QueryContent.KeyIsNumberSquare) => queryBody,
                (QueryKind.ToDictionary, _) => throw new ArgumentException(),
                (QueryKind.ToList, QueryContent.All) => queryBody,
                (QueryKind.ToList, _) => throw new ArgumentException(),
                //QueryKind.Skip => null,
                //QueryKind.Single => null,
                //QueryKind.Take => null,
                _ => throw new ArgumentException()
            };
        }
    }

    [TestClass]
    public class CustomLINQTests
    {
        [TestMethod]
        public void AggregateQuery()
        {
            MockQueryable source = new MockQueryable();
            int query = source.Aggregate(0, (acc, data) => acc + data);

            int sum = MockQueryable.DataSource.Aggregate(0, (acc, data) => acc + data);

            Assert.IsTrue(sum == query);
        }

        [TestMethod]
        public void AllQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.All(QueryContent.GreaterThan0);

            bool allGreaterThan0Predefined = MockQueryable.DataSource.All(data => data > 0),
                 allGreaterThan0 = query;

            Assert.IsTrue(allGreaterThan0Predefined == allGreaterThan0);
        }

        [TestMethod]
        public void AnyQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.Any(QueryContent.GreaterThan5);

            bool anyGreaterThan5Predefined = MockQueryable.DataSource.Any(data => data > 5),
                 anyGreaterThan5 = query;

            Assert.IsTrue(anyGreaterThan5Predefined == anyGreaterThan5);
        }

        [TestMethod]
        public void ContainsQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.Contains(QueryContent.Number10);

            bool contains10Predefined = MockQueryable.DataSource.Contains(10),
                 contains10 = query;

            Assert.IsTrue(contains10Predefined == contains10);
        }

        [TestMethod]
        public void CountQuery()
        {
            MockQueryable source = new MockQueryable();
            int query = source.Count();

            int countPredefined = MockQueryable.DataSource.Count(),
                count = query;

            Assert.IsTrue(countPredefined == count);
        }

        [TestMethod]
        public void CountByFilterQuery()
        {
            MockQueryable source = new MockQueryable();
            int query = source.Count(QueryContent.GreaterThan5);
        
            int countOfGreaterThan5Predefined = MockQueryable.DataSource.Count(data => data > 5),
                countOfGreaterThan5 = query;

            Assert.IsTrue(countOfGreaterThan5 == countOfGreaterThan5Predefined);
        }

        [TestMethod]
        public void FirstQuery()
        {
            MockQueryable source = new MockQueryable();
            int query = source.First(QueryContent.GreaterThan7);

           int firstGreaterThan7Predefined = MockQueryable.DataSource.First(data => data > 7),
               firstGreaterThan7 = query;

            Assert.IsTrue(firstGreaterThan7Predefined == firstGreaterThan7);
        }

        [TestMethod]
        public void FirstWhereQuery()
        {
            MultidimensionalMockQueryable source = new MultidimensionalMockQueryable();
            GenericMockQueryable<int> query = source.First().Where(x => x < 5).AsMockQueryable();

            IEnumerable<int> firstGreaterThan7Predefined = MultidimensionalMockQueryable.DataSource.First().InstanceDataSource.Where(x => x < 5),
                             firstGreaterThan7 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(firstGreaterThan7Predefined, firstGreaterThan7));
        }

        [TestMethod]
        public void GroupJoinQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            var query = (from fuVisited in source
                         join fuVisitor in source
                         on true equals true
                         into visitors
                         select new { Visited = fuVisited.Nickname, Visitors = visitors })
                        .AsMockQueryable();

            var groupJoinedForumUsersPredefined = from fuVisited in ForumUsersMockQueryable.DataSource
                                             join fuVisitor in ForumUsersMockQueryable.DataSource
                                             on true equals true
                                             into visitors
                                             select new { Visited = fuVisited.Nickname, Visitors = visitors };
            var groupJoinedForumUsers = query;

            foreach ((var groupJoinedForumUserPredefinedData, var groupJoinedForumUserData) 
                     in groupJoinedForumUsersPredefined.Zip(groupJoinedForumUsers))
            {
                Assert.IsTrue(groupJoinedForumUserPredefinedData.Visited == groupJoinedForumUserData.Visited);
                Assert.IsTrue(Enumerable.SequenceEqual(groupJoinedForumUserPredefinedData.Visitors, groupJoinedForumUserData.Visitors));
            }
        }

        [TestMethod]
        public void JoinQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            var query = (from fuVisited in source
                                   join fuVisitor in source
                                   on true equals true
                                   select new { Visited = fuVisited.Nickname, Visitor = fuVisitor.Nickname })
                                   .AsMockQueryable();

            var joinedForumUsersPredefined = from fuVisited in ForumUsersMockQueryable.DataSource
                                              join fuVisitor in ForumUsersMockQueryable.DataSource
                                              on true equals true
                                              select new { Visited = fuVisited.Nickname, Visitor = fuVisitor.Nickname };
            var joinedForumUsers = query;

            Assert.IsTrue(Enumerable.SequenceEqual(joinedForumUsersPredefined, joinedForumUsers));
        }

        [TestMethod]
        public void JoinWhereQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            var query = (from visitingInfo in (
                                       from fuVisited in source
                                       join fuVisitor in source
                                       on true equals true
                                       select new { 
                                           Visited = fuVisited.Nickname, 
                                           Visitor = fuVisitor })
                                   where visitingInfo.Visitor.LikeCount > 99
                                   select visitingInfo).AsMockQueryable();

            var joinedForumUsersWhereLikesCountGreaterThan99Predefined = from visitingInfo in (
                                                from fuVisited in ForumUsersMockQueryable.DataSource
                                                join fuVisitor in ForumUsersMockQueryable.DataSource
                                                on true equals true
                                                select new { Visited = fuVisited.Nickname, Visitor = fuVisitor })
                                             where visitingInfo.Visitor.LikeCount > 99
                                             select visitingInfo;
            var joinedForumUsersWhereLikesCountGreaterThan99 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(joinedForumUsersWhereLikesCountGreaterThan99, joinedForumUsersWhereLikesCountGreaterThan99));
        }

        [TestMethod]
        public void JoinAnyQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            bool fromJoinedForumUsersAnyHas100Likes = (
                                       from fuVisited in source
                                       join fuVisitor in source
                                       on true equals true
                                       select new
                                       {
                                           Visited = fuVisited.Nickname,
                                           Visitor = fuVisitor
                                       }
                                    ).AsMockQueryable()
                                    .Any(visitingInfo => visitingInfo.Visitor.LikeCount > 99);

            bool fromJoinedForumUsersAnyHas100LikesPredefined = (
                                                from fuVisited in ForumUsersMockQueryable.DataSource
                                                join fuVisitor in ForumUsersMockQueryable.DataSource
                                                on true equals true
                                                select new { Visited = fuVisited.Nickname, Visitor = fuVisitor }
                                             )
                                             .Any(visitingInfo => visitingInfo.Visitor.LikeCount > 99);

            Assert.IsTrue(fromJoinedForumUsersAnyHas100LikesPredefined == fromJoinedForumUsersAnyHas100Likes);
        }


        [TestMethod]
        public void TakeQuery()
        {
            GenericMockQueryable<int> source = new MockQueryable();
            GenericMockQueryable<int> query = source.Take(QueryContent.Number10).AsMockQueryable();

            IEnumerable<int> taken10Predefined = MockQueryable.DataSource.Take(10),
                             taken10 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(taken10Predefined, taken10));
        }


        [TestMethod]
        public void TakeWhileJoinQuery()
        {
            GenericMockQueryable<int> source = new MockQueryable().TakeWhile(QueryContent.LesserThan10).AsMockQueryable();
            GenericMockQueryable<int> query = source
                .TakeWhile<int>(QueryContent.LesserThan10).AsMockQueryable()
                .Join<int, int, int>(
                    source.TakeWhile<int>(QueryContent.LesserThan10), 
                    QueryContent.IsEven, 
                    QueryContent.Mod3Is0, 
                    QueryContent.Mod6).AsMockQueryable();

            IEnumerable<int> takenWhileLesserThan10JoinIsOddOnMod3Is0ToMod6Predefined = MockQueryable.DataSource
                .TakeWhile(data => data < 10)
                .Join(
                    MockQueryable.DataSource.TakeWhile(data => data < 10), 
                    outerData => (outerData & 1) == 0, 
                    innerData => innerData % 3 == 0, 
                    (outerData, innerData) => outerData == innerData ? outerData : 0),
                             takenWhileLesserThan10JoinIsOddOnMod3Is0ToMod6 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(takenWhileLesserThan10JoinIsOddOnMod3Is0ToMod6Predefined, takenWhileLesserThan10JoinIsOddOnMod3Is0ToMod6));
        }

        [TestMethod]
        public void SelectQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Select<int, bool>(QueryContent.IsOdd).AsMockQueryable();

            IEnumerable<bool> selectIsOddPredefined =
                MockQueryable.DataSource.Select(data => (data & 1) == 1),
                      selectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, selectIsOdd));
        }

        [TestMethod]
        public void SelectQuery2()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<float> query = (from data in source select 1f / data).AsMockQueryable();

            IEnumerable<float> selectIsOddPredefined =
                MockQueryable.DataSource.Select(data => 1f / data),
                      selectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, selectIsOdd));
        }

        [TestMethod]
        public void SelectSkipQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Select<int, bool>(QueryContent.IsOdd).AsMockQueryable()
                .Skip(QueryContent.Number10).AsMockQueryable()
                .Skip(QueryContent.Number1000).AsMockQueryable();

            IEnumerable<bool> selectIsOddAndSkip1000Predefined =
                MockQueryable.DataSource.Select(data => (data & 1) == 1).Skip(10).Skip(1000),
                      selectIsOddAndSkip1000 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddAndSkip1000Predefined, selectIsOddAndSkip1000));
        }

        [TestMethod]
        public void SelectTakeQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query = source
                .Select<int, bool>(QueryContent.IsOdd).AsMockQueryable()
                .Take(QueryContent.Number1000).AsMockQueryable();

            IEnumerable<bool> selectIsOddAndTake1000Predefined =
                MockQueryable.DataSource.Select(data => (data & 1) == 1).Take(1000),
                              selectIsOddAndTake1000 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddAndTake1000Predefined, selectIsOddAndTake1000));
        }

        [TestMethod]
        public void SelectManyQuery()
        {
            GenericMockQueryable<ForumUser> source = new ForumUsersMockQueryable();
            var selectMany = (from fu1 in source
                              from fu2 in source
                              select new { Visited = fu1, Visitor = fu2 }).AsMockQueryable();

            var selectManyPredefined = (from fu1 in ForumUsersMockQueryable.DataSource
                                        from fu2 in ForumUsersMockQueryable.DataSource
                                        select new { Visited = fu1, Visitor = fu2 });

            Assert.IsTrue(Enumerable.SequenceEqual(selectManyPredefined, selectMany));
        }


        [TestMethod]
        public void SkipQuery()
        {
            GenericMockQueryable<int> source = new MockQueryable();
            GenericMockQueryable<int> query = source.Skip(QueryContent.Number10).AsMockQueryable();

            IEnumerable<int> skipped10Predefined = MockQueryable.DataSource.Skip(10),
                             skipped10 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(skipped10Predefined, skipped10));
        }

        [TestMethod]
        public void SkipWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .SkipWhile(QueryContent.LesserThan5).AsMockQueryable();

            IEnumerable<int> skippedWhileLesserThan5Predefined =
                MockQueryable.DataSource.SkipWhile(data => data < 5),
                             skippedWhileLesserThan5 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(skippedWhileLesserThan5Predefined, skippedWhileLesserThan5));
        }

        [TestMethod]
        public void SkipWhileQuery2()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .SkipWhile(QueryContent.GreaterThan5).AsMockQueryable();

            IEnumerable<int> skippedWhileGreaterThan5Predefined =
                MockQueryable.DataSource.SkipWhile(data => data > 5),
                             skippedWhileGreaterThan5 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(skippedWhileGreaterThan5Predefined, skippedWhileGreaterThan5));
        }

        [TestMethod]
        public void TakeWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .TakeWhile(QueryContent.LesserThan5).AsMockQueryable();

            IEnumerable<int> takenWhileLesserThan5Predefined =
                MockQueryable.DataSource.TakeWhile(data => data < 5),
                             takenWhileLesserThan5 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(takenWhileLesserThan5Predefined, takenWhileLesserThan5));
        }

        [TestMethod]
        public void WhereQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source.Where(QueryContent.GreaterThan5).AsMockQueryable();

            IEnumerable<int> whereGreaterThan5Predefined = MockQueryable.DataSource.Where(data => data > 5),
                             whereGreaterThan5 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(whereGreaterThan5, whereGreaterThan5Predefined));
        }

        [TestMethod]
        public void WhereSelectQuery()
        {
            MockQueryable source = new MockQueryable();
            IQueryable<bool> query = source
                .Where(QueryContent.LesserThan10).AsMockQueryable()
                .Select<int, bool>(QueryContent.IsOdd);

            IEnumerable<bool> whereLesserThan10SelectIsOddPredefined =
                MockQueryable.DataSource.Where(data => data < 10).Select(data => (data & 1) == 1),
                              whereLesserThan10SelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(whereLesserThan10SelectIsOddPredefined, whereLesserThan10SelectIsOdd));
        }

        [TestMethod]
        public void WhereSelectQuery2()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<bool> query =
                (from data in source
                 where data < 10
                 let data2 = data ^ 3
                 select data % 1 == 1 || data2 % 3 == 0).AsMockQueryable();

            IEnumerable<bool> whereLesserThan10SelectIsOddPredefined =
                (from data in MockQueryable.DataSource
                 where data < 10
                 let data2 = data ^ 3
                 select data % 1 == 1 || data2 % 3 == 0),
                              whereLesserThan10SelectIsOdd = query;

            Assert.IsTrue(Enumerable.SequenceEqual(whereLesserThan10SelectIsOddPredefined, whereLesserThan10SelectIsOdd));
        }

        /*
        [TestMethod]
        public void WhereSelectQuery3()
        {
            List<bool> whereLesserThan10SelectIsOddPredefined =
                (from data in MockQueryable.DataSource
                 where data < 10
                 let data2 = data ^ 3
                 select data % 1 == 1 || data2 % 3 == 0).ToList(),
                      whereLesserThan10SelectIsOdd;
        }
        */

        [TestMethod]
        public void WhereWhereQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .Where(QueryContent.GreaterThan5).AsMockQueryable()
                .Where(QueryContent.LesserThan10).AsMockQueryable();

            IEnumerable<int> whereGreaterThan5AndLesserThan10Predefined = 
                MockQueryable.DataSource.Where(data1 => data1 > 5).Where(data2 => data2 < 10),
                             whereGreaterThan5AndLesserThan10 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(whereGreaterThan5AndLesserThan10Predefined, whereGreaterThan5AndLesserThan10));
        }

        /*
        [TestMethod]
        public void WhereWhereQuery2()
        {
            List<int> whereGreaterThan5AndLesserThan10Predefined =
                MockQueryable.DataSource.Where(data => data > 5).Where(data => data < 10).ToList();
        }
        */

        [TestMethod]
        public void WhereTakeWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .Where(QueryContent.GreaterThan5).AsMockQueryable()
                .TakeWhile(QueryContent.LesserThan10).AsMockQueryable();

            IEnumerable<int> whereGreaterThan5AndTakenWhileLesserThan10Predefined =
                MockQueryable.DataSource.Where(data => data > 5).Where(data => data < 10),
                             whereGreaterThan5AndTakenWhileLesserThan10 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(
                whereGreaterThan5AndTakenWhileLesserThan10Predefined, whereGreaterThan5AndTakenWhileLesserThan10));
        }

        [TestMethod]
        public void TakeWhileTakeWhileTakeWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .TakeWhile(QueryContent.LesserThan10).AsMockQueryable()
                .TakeWhile(QueryContent.GreaterThan0).AsMockQueryable()
                .TakeWhile(QueryContent.LesserThan5).AsMockQueryable();

            IEnumerable<int> takenWhileLesserThan10AndGreaterThan0AndLesserThan5Predefined =
                MockQueryable.DataSource.TakeWhile(data => data < 5).Where(data => data < 10),
                             takenWhileLesserThan10AndGreaterThan0AndLesserThan5 = query;

            Assert.IsTrue(Enumerable.SequenceEqual(
                takenWhileLesserThan10AndGreaterThan0AndLesserThan5, takenWhileLesserThan10AndGreaterThan0AndLesserThan5));
        }

        [TestMethod]
        public void WhereAllQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.Where(QueryContent.GreaterThan5).AsMockQueryable()
                               .All(QueryContent.GreaterThan0);

            bool whereGreaterThan5AllGreaterThan0Predefined = 
                MockQueryable.DataSource.Where(data => data > 5).All(data => data > 0),
                 whereGreaterThan5AllGreaterThan0 = query;

            Assert.IsTrue(whereGreaterThan5AllGreaterThan0Predefined == whereGreaterThan5AllGreaterThan0);
        }

        [TestMethod]
        public void WhereAnyQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.Where(QueryContent.GreaterThan5).AsMockQueryable()
                               .Any(QueryContent.LesserThan10);

            bool whereGreaterThan5AllLesserThan10Predefined = 
                MockQueryable.DataSource.Where(data => data > 5).Any(data => data < 10),
                 whereGreaterThan5AllLesserThan10 = query;

            Assert.IsTrue(whereGreaterThan5AllLesserThan10Predefined == whereGreaterThan5AllLesserThan10);
        }

        [TestMethod]
        public void ToArrayQuery()
        {
            MockQueryable source = new MockQueryable();
            int[] query = source.ToArray();

            IEnumerable<int> arrayedPredefined = MockQueryable.DataSource.ToArray(),
                             arrayed = query;

            Assert.IsTrue(Enumerable.SequenceEqual(arrayedPredefined, arrayed));
        }

        [TestMethod]
        public void ToDictionaryQuery()
        {
            MockQueryable source = new MockQueryable();
            Dictionary<int, int> query = 
                source.ToDictionary<int, int, int>(QueryContent.KeyIsNumberSquare, QueryContent.All);

            Dictionary<int, int> dictionariedPredefined = 
                MockQueryable.DataSource.ToDictionary((x) => x * x),
                                 dictionaried = query;

            Assert.IsTrue(dictionariedPredefined.Keys.Count == dictionaried.Keys.Count);

            foreach (var key in dictionariedPredefined.Keys)
            {
                Assert.IsTrue(dictionaried.ContainsKey(key));
                Assert.AreEqual(dictionariedPredefined[key], dictionaried[key]);
            }
        }

        [TestMethod]
        public void ToListQuery()
        {
            MockQueryable source = new MockQueryable();
            List<int> query = source.ToList();

            IEnumerable<int> listedPredefined = MockQueryable.DataSource.ToList(),
                             listed = query;

            Assert.IsTrue(Enumerable.SequenceEqual(listedPredefined, listed));
        }

        [TestMethod]
        public void ZZZTest()
        {
            object o;
            int i = 123;
            int? i2;
            string s = "123";
            DateTime dt = new DateTime(2001, 2, 3);
            DateTime? dt2;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            i2 = Ret<int, int?>(i);
            sw.Stop();
            (long ms, long ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            i2 = Ret<int, int?>(i);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);

            sw.Restart();
            o = Ret<int, object>(i);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            o = Ret<int, object>(i);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            i = Ret<object, int>(o);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            i = Ret<object, int>(o);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            o = Ret<string, object>(s);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            o = Ret<string, object>(s);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            s = Ret<object, string>(o);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            s = Ret<object, string>(o);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            dt2 = Ret<DateTime, DateTime?>(dt);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            dt2 = Ret<DateTime, DateTime?>(dt);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            o = Ret<DateTime, object>(dt);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            o = Ret<DateTime, object>(dt);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            dt = Ret<object, DateTime>(o);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            dt = Ret<object, DateTime>(o);
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);

            ;

            T2 Ret<T1, T2>(T1 instance) => (dynamic)instance;

            void Test<T1, T2>(T1 obj1)
            {
                T2 obj2;

                sw.Restart();
                obj2 = Ret<T1, T2>(obj1);
                sw.Stop();
                (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
                sw.Restart();
                obj2 = Ret<T1, T2>(obj1);
                sw.Stop();
                (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
                sw.Restart();
                obj1 = Ret<T2, T1>(obj2);
                sw.Stop();
                (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
                sw.Restart();
                obj1 = Ret<T2, T1>(obj2);
                sw.Stop();
                (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
                //System.ComponentModel.TypeConverter tc2 = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T2));
                //tc2.ConvertFrom(obj1);
                //obj2 = Converter<T1, T2>. ChangeType(obj1, typeof(T1));
            }
        }
    }
}
