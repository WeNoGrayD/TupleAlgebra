using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TupleAlgebraTests.DataModels;
using System.Linq.Expressions;
using TupleAlgebraClassLib.LINQ2TAFramework;
using TupleAlgebraClassLib;
using System.Collections;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

[assembly: InternalsVisibleTo("TupleAlgebraClassLib")]

namespace TupleAlgebraTests
{
    internal enum QueryKind
    {
        Aggregate,
        All,
        Any,
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
    }

    internal enum QueryContent
    {
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
        OneToManyForumUsers
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

        public MockQueryable()
        {
            Expression = Expression.Constant(_dataSource.AsQueryable());
        }

        public MockQueryable(Expression queryExpression, IQueryProvider provider) 
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
        private QueryContext _queryContext = new MockQueryContext();

        protected override QueryContext QueryContext => _queryContext;

        static MockQueryProvider()
        {
            //_queryContext = new MockQueryContext();
        }

        public override IQueryable<TQueryResult> CreateQuery<TQueryResult>(Expression queryExpression)
        {
            IEnumerable<T> queryable = new DataSourceExtractor<IEnumerable<T>>().Extract(queryExpression);

            /*
               Проверка выражения запроса на избыточность (а также корректность). 
               Если оно избыточно, то экземпляр MethodCallInspector вернёт
               неизбыточное выражение, равное константному выражению источника данных для запроса.
             */
            if (queryExpression != new QueryInspector().Visit(queryExpression))
                return queryable as IQueryable<TQueryResult>;

            return new GenericMockQueryable<TQueryResult>(queryExpression, this);
        }

        protected override QueryPipelineExecutor CreateQueryPipelineExecutor(
            object dataSource,
            IQueryPipelineMiddleware firstQueryExecutor)
            => new MockQueryPipelineExecutor(dataSource, firstQueryExecutor);
    }

    internal class MockQueryContext : QueryContext
    {
    }

    internal class MockQueryPipelineExecutor : QueryPipelineExecutor
    {
        public MockQueryPipelineExecutor(
            object dataSource,
            IQueryPipelineMiddleware firstQueryExecutor) 
            : base(dataSource, firstQueryExecutor) { }
    }

    internal static class MockQueryableHelper
    {
        public static GenericMockQueryable<T> AsMockQueryable<T>(this IQueryable<T> queryable) => 
            (GenericMockQueryable<T>)queryable;
        
        public static bool All<TData>(
            this GenericMockQueryable<TData> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr = MockQueryableHelper.CreateExpression(QueryKind.All, queryContent);

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
            LambdaExpression predicateExpr = MockQueryableHelper.CreateExpression(QueryKind.Any, queryContent);

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

        public static int Count<TData>(this GenericMockQueryable<TData> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr = MockQueryableHelper.CreateExpression(QueryKind.First, queryContent);

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
            LambdaExpression predicateExpr = MockQueryableHelper.CreateExpression(QueryKind.First, queryContent);

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
            LambdaExpression outerKeySelectorExpr = MockQueryableHelper.CreateExpression(QueryKind.Select, outerKeySelectorContent),
                             innerKeySelectorExpr = MockQueryableHelper.CreateExpression(QueryKind.Select, innerKeySelectorContent),
                             resultSelectorExpr = MockQueryableHelper.CreateExpression(QueryKind.Select, resultSelectorContent);

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
            LambdaExpression funcExpr = MockQueryableHelper.CreateExpression(QueryKind.Select, queryContent);

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
                MockQueryableHelper.CreateExpression(QueryKind.SelectMany, innerEnumerableSelectorContent),
                             resultSelectorExpr = 
                MockQueryableHelper.CreateExpression(QueryKind.Select, resultSelectorContent);

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
            LambdaExpression predicateExpr = MockQueryableHelper.CreateExpression(QueryKind.Where, queryContent);

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

        public static IQueryable<TData> TakeWhile<TData>(
            this GenericMockQueryable<TData> source, 
            QueryContent queryContent)
        {
            LambdaExpression predicateExpr = MockQueryableHelper.CreateExpression(QueryKind.TakeWhile, queryContent);

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

        public static IQueryable<TData> SkipWhile<TData>(this GenericMockQueryable<TData> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr = MockQueryableHelper.CreateExpression(QueryKind.SkipWhile, queryContent);

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
                    Expression => param
                })
                .ToArray().CopyTo(queryMethodParamsBuf, 1);

            return Expression.Call(
                null,
                queryMethodInfo.MakeGenericMethod(genericArguments),
                queryMethodParamsBuf);
        }

        public static LambdaExpression? CreateExpression(QueryKind queryKind, QueryContent queryContent)
        {
            LambdaExpression queryBody = queryContent switch
            {
                QueryContent.GreaterThan0 => (Expression<Func<int, bool>>)((int data) => data > 0),
                QueryContent.GreaterThan5 => (Expression<Func<int, bool>>)((int data) => data > 5),
                QueryContent.GreaterThan7 => (Expression<Func<int, bool>>)((int data) => data > 7),
                QueryContent.GreaterThanX => (Expression<Func<int, IEnumerable<int>>>)
                    ((int data) => MockQueryable.DataSource.SkipWhile(i => i <= data)),
                QueryContent.LesserThan5 => (Expression<Func<int, bool>>)((int data) => data < 5),
                QueryContent.LesserThan10 => (Expression<Func<int, bool>>)((int data) => data < 10),
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
                _ => throw new ArgumentException()
            };;

            return (queryKind, queryContent) switch
            {
                //QueryKind.Aggregate => null,
                (QueryKind.All, QueryContent.GreaterThan0) => queryBody,
                (QueryKind.All, _) => throw new ArgumentException(),
                (QueryKind.Any, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.Any, QueryContent.LesserThan10) => queryBody,
                (QueryKind.Any, _) => throw new ArgumentException(),
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
                (QueryKind.SkipWhile, QueryContent.LesserThan5) => queryBody,
                (QueryKind.SkipWhile, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.SkipWhile, QueryContent.LesserThan10) => queryBody,
                (QueryKind.SkipWhile, _) => throw new ArgumentException(),
                (QueryKind.TakeWhile, QueryContent.GreaterThan0) => queryBody,
                (QueryKind.TakeWhile, QueryContent.LesserThan5) => queryBody,
                (QueryKind.TakeWhile, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.TakeWhile, QueryContent.LesserThan10) => queryBody,
                (QueryKind.TakeWhile, _) => throw new ArgumentException(),
                (QueryKind.Where, QueryContent.GreaterThan5) => queryBody,
                (QueryKind.Where, QueryContent.LesserThan10) => queryBody,
                (QueryKind.Where, _) => throw new ArgumentException(),
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
        public void GroupJoinQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            var joinedForumUsers = from fuVisited in source
                                   join fuVisitor in source
                                   on true equals true
                                   into visitors
                                   select new { Visited = fuVisited.Nickname, Visitors = visitors };

            var joinedForumUsersPredefined = from fuVisited in ForumUsersMockQueryable.DataSource
                                             join fuVisitor in ForumUsersMockQueryable.DataSource
                                             on true equals true
                                             into visitors
                                             select new { Visited = fuVisited.Nickname, Visitors = visitors };
            foreach ((var joinedForumUserPredefinedData, var joinedForumUserData) 
                     in joinedForumUsersPredefined.Zip(joinedForumUsers))
            {
                Assert.IsTrue(joinedForumUserPredefinedData.Visited == joinedForumUserData.Visited);
                Assert.IsTrue(Enumerable.SequenceEqual(joinedForumUserPredefinedData.Visitors, joinedForumUserData.Visitors));
            }
        }

        [TestMethod]
        public void JoinQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            var joinedForumUsers = from fuVisited in source
                                   join fuVisitor in source
                                   on true equals true
                                   select new { Visited = fuVisited.Nickname, Visitor = fuVisitor.Nickname };
            var joinedForumUsers2 = joinedForumUsers.ToList();

            var joinedForumUsersPredefined = from fuVisited in ForumUsersMockQueryable.DataSource
                                              join fuVisitor in ForumUsersMockQueryable.DataSource
                                              on true equals true
                                              select new { Visited = fuVisited.Nickname, Visitor = fuVisitor.Nickname };

            Assert.IsTrue(Enumerable.SequenceEqual(joinedForumUsersPredefined, joinedForumUsers));
        }

        [TestMethod]
        public void JoinWhereQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            var query = from visitingInfo in (
                                       from fuVisited in source
                                       join fuVisitor in source
                                       on true equals true
                                       select new { 
                                           Visited = fuVisited.Nickname, 
                                           Visitor = fuVisitor })
                                   where visitingInfo.Visitor.LikeCount > 99
                                   select visitingInfo;

            var joinedForumUsersPredefined = from visitingInfo in (
                                                from fuVisited in ForumUsersMockQueryable.DataSource
                                                join fuVisitor in ForumUsersMockQueryable.DataSource
                                                on true equals true
                                                select new { Visited = fuVisited.Nickname, Visitor = fuVisitor })
                                             where visitingInfo.Visitor.LikeCount > 99
                                             select visitingInfo;
            var joinedForumUsers = query.ToList();

            Assert.IsTrue(Enumerable.SequenceEqual(joinedForumUsersPredefined, joinedForumUsers));
        }

        [TestMethod]
        public void JoinAnyQuery()
        {
            ForumUsersMockQueryable source = new ForumUsersMockQueryable();
            var fromJoinedForumUsersAnyHas100Likes = (
                                       from fuVisited in source
                                       join fuVisitor in source
                                       on true equals true
                                       select new
                                       {
                                           Visited = fuVisited.Nickname,
                                           Visitor = fuVisitor
                                       }
                                    )
                                    .Any(visitingInfo => visitingInfo.Visitor.LikeCount > 99);

            var fromJoinedForumUsersAnyHas100LikesPredefined = (
                                                from fuVisited in ForumUsersMockQueryable.DataSource
                                                join fuVisitor in ForumUsersMockQueryable.DataSource
                                                on true equals true
                                                select new { Visited = fuVisited.Nickname, Visitor = fuVisitor }
                                             )
                                             .Any(visitingInfo => visitingInfo.Visitor.LikeCount > 99);

            Assert.IsTrue(fromJoinedForumUsersAnyHas100LikesPredefined == fromJoinedForumUsersAnyHas100Likes);
        }


        [TestMethod]
        public void TakeWhileJoinQuery()
        {
            GenericMockQueryable<int> source = new MockQueryable().TakeWhile(QueryContent.LesserThan10).AsMockQueryable();
            IQueryable<int> query = source
                .Join<int, int, int>(
                    source, 
                    QueryContent.IsEven, 
                    QueryContent.Mod3Is0, 
                    QueryContent.Mod6);

            List<int> joinIsOddOnMod3Is0ToMod6Predefined = MockQueryable.DataSource
                .TakeWhile(data => data < 10)
                .Join(
                    MockQueryable.DataSource.TakeWhile(data => data < 10), 
                    outerData => (outerData & 1) == 0, 
                    innerData => innerData % 3 == 0, 
                    (outerData, innerData) => outerData == innerData ? outerData : 0).ToList(),
                     joinIsOddOnMod3Is0ToMod6 = new List<int>();

            foreach (int data in query)
                joinIsOddOnMod3Is0ToMod6.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(joinIsOddOnMod3Is0ToMod6Predefined, joinIsOddOnMod3Is0ToMod6));
        }

        [TestMethod]
        public void SelectQuery()
        {
            MockQueryable source = new MockQueryable();
            IQueryable query = source
                .Select<int, bool>(QueryContent.IsOdd);

            List<bool> selectIsOddPredefined =
                MockQueryable.DataSource.Select(data => (data & 1) == 1).ToList(),
                      selectIsOdd = new List<bool>();

            foreach (bool data in query)
                selectIsOdd.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(selectIsOddPredefined, selectIsOdd));
        }

        [TestMethod]
        public void SelectManyQuery()
        {
            GenericMockQueryable<ForumUser> source = new ForumUsersMockQueryable();
            var selectMany = (from fu1 in source
                              from fu2 in source
                              select new { Visited = fu1, Visitor = fu2 });

            var selectManyPredefined = (from fu1 in source
                                        from fu2 in source
                                        select new { Visited = fu1, Visitor = fu2 });

            Assert.IsTrue(Enumerable.SequenceEqual(selectManyPredefined, selectMany));
        }

        [TestMethod]
        public void SkipWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .SkipWhile(QueryContent.LesserThan5).AsMockQueryable();

            List<int> skippedWhileLesserThan5Predefined =
                MockQueryable.DataSource.SkipWhile(data => data < 5).ToList(),
                      skippedWhileLesserThan5 = new List<int>();

            foreach (int data in query)
                skippedWhileLesserThan5.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(skippedWhileLesserThan5Predefined, skippedWhileLesserThan5));
        }

        [TestMethod]
        public void SkipWhileQuery2()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .SkipWhile(QueryContent.GreaterThan5).AsMockQueryable();

            List<int> skippedWhileGreaterThan5Predefined =
                MockQueryable.DataSource.SkipWhile(data => data > 5).ToList(),
                      skippedWhileGreaterThan5 = new List<int>();

            foreach (int data in query)
                skippedWhileGreaterThan5.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(skippedWhileGreaterThan5Predefined, skippedWhileGreaterThan5));
        }

        [TestMethod]
        public void TakeWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .TakeWhile(QueryContent.LesserThan5).AsMockQueryable();

            List<int> takenWhileLesserThan5Predefined =
                MockQueryable.DataSource.TakeWhile(data => data < 5).ToList(),
                      takenWhileLesserThan5 = new List<int>();

            foreach (int data in query)
                takenWhileLesserThan5.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(takenWhileLesserThan5Predefined, takenWhileLesserThan5));
        }

        [TestMethod]
        public void WhereQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source.Where(QueryContent.GreaterThan5).AsMockQueryable();

            List<int> whereGreaterThan5Predefined = MockQueryable.DataSource.Where(data => data > 5).ToList(),
                      whereGreaterThan5 = new List<int>();

            foreach (int data in query)
                whereGreaterThan5.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(whereGreaterThan5, whereGreaterThan5Predefined));
        }

        [TestMethod]
        public void WhereSelectQuery()
        {
            MockQueryable source = new MockQueryable();
            IQueryable query = source
                .Where(QueryContent.LesserThan10).AsMockQueryable()
                .Select<int, bool>(QueryContent.IsOdd);

            List<bool> whereLesserThan10SelectIsOddPredefined =
                MockQueryable.DataSource.Where(data => data < 10).Select(data => (data & 1) == 1).ToList(),
                      whereLesserThan10SelectIsOdd = new List<bool>();

            foreach (bool data in query)
                whereLesserThan10SelectIsOdd.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(whereLesserThan10SelectIsOddPredefined, whereLesserThan10SelectIsOdd));
        }

        [TestMethod]
        public void WhereSelectQuery2()
        {
            MockQueryable source = new MockQueryable();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<bool> whereLesserThan10SelectIsOddPredefined =
                (from data in MockQueryable.DataSource
                 where data < 10
                 let data2 = data ^ 3
                 select data % 1 == 1 || data2 % 3 == 0).ToList(),
                      whereLesserThan10SelectIsOdd;

            sw.Stop();
            (long ms, long ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);

            whereLesserThan10SelectIsOdd =
                (from data in source
                 where data < 10
                 let data2 = data ^ 3
                 select data % 1 == 1 || data2 % 3 == 0).ToList();

            Assert.IsTrue(Enumerable.SequenceEqual(whereLesserThan10SelectIsOddPredefined, whereLesserThan10SelectIsOdd));
        }

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

        [TestMethod]
        public void WhereWhereQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .Where(QueryContent.GreaterThan5).AsMockQueryable()
                .Where(QueryContent.LesserThan10).AsMockQueryable();

            List<int> whereGreaterThan5AndLesserThan10Predefined = 
                MockQueryable.DataSource.Where(data => data > 5).Where(data => data < 10).ToList(),
                      whereGreaterThan5AndLesserThan10 = new List<int>();

            foreach (int data in query)
                whereGreaterThan5AndLesserThan10.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(whereGreaterThan5AndLesserThan10Predefined, whereGreaterThan5AndLesserThan10));
        }

        [TestMethod]
        public void WhereWhereQuery2()
        {
            List<int> whereGreaterThan5AndLesserThan10Predefined =
                MockQueryable.DataSource.Where(data => data > 5).Where(data => data < 10).ToList();
        }

        [TestMethod]
        public void WhereTakeWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            GenericMockQueryable<int> query = source
                .Where(QueryContent.GreaterThan5).AsMockQueryable()
                .TakeWhile(QueryContent.LesserThan10).AsMockQueryable();

            List<int> whereGreaterThan5AndTakenWhileLesserThan10Predefined =
                MockQueryable.DataSource.Where(data => data > 5).Where(data => data < 10).ToList(),
                      whereGreaterThan5AndTakenWhileLesserThan10 = new List<int>();

            foreach (int data in query)
                whereGreaterThan5AndTakenWhileLesserThan10.Add(data);

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

            List<int> takenWhileLesserThan5AndWhereLesserThan10Predefined =
                MockQueryable.DataSource.TakeWhile(data => data < 5).Where(data => data < 10).ToList(),
                      takenWhileLesserThan5AndWhereLesserThan10 = new List<int>();

            foreach (int data in query)
                takenWhileLesserThan5AndWhereLesserThan10.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(
                takenWhileLesserThan5AndWhereLesserThan10Predefined, takenWhileLesserThan5AndWhereLesserThan10));
        }

        [TestMethod]
        public void WhereAllQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.Where(QueryContent.GreaterThan5).AsMockQueryable().All(QueryContent.GreaterThan0);

            bool allGreaterThan5AlsoGreaterThan0Predefined = MockQueryable.DataSource.Where(data => data > 5).ToList().All(data => data > 0),
                 allGreaterThan5AlsoGreaterThan0 = query;

            Assert.IsTrue(allGreaterThan5AlsoGreaterThan0Predefined == allGreaterThan5AlsoGreaterThan0);
        }

        [TestMethod]
        public void WhereAnyQuery()
        {
            MockQueryable source = new MockQueryable();
            bool query = source.Where(QueryContent.GreaterThan5).AsMockQueryable().Any(QueryContent.LesserThan10);

            bool anyGreaterThan5AlsoLesserThan10Predefined = MockQueryable.DataSource.Where(data => data > 5).ToList().Any(data => data < 10),
                 anyGreaterThan5AlsoLesserThan10 = query;

            Assert.IsTrue(anyGreaterThan5AlsoLesserThan10Predefined == anyGreaterThan5AlsoLesserThan10);
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
