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

namespace TupleAlgebraTests
{
    internal enum QueryKind
    {
        Aggregate,
        All,
        Any,
        Count,
        First,
        Where,
        Skip,
        Single,
        Take,
        TakeWhile
    }

    internal enum QueryContent
    {
        LesserThan5,
        GreaterThan5,
        GreaterThan7,
        GreaterThan0,
        LesserThan10
    }

    internal class MockQueryable : IQueryable<int>
    {
        private static IEnumerable<int> _dataSource = Enumerable.Range(1, 10);

        public static IEnumerable<int> DataSource => _dataSource;

        public Type ElementType => typeof(int);

        private static IQueryProvider _provider = new MockQueryProvider();

        public IQueryProvider Provider => _provider;

        public Expression Expression { get; private set; }

        public MockQueryable()
        {
            Expression = Expression.Constant(_dataSource);
        }

        public MockQueryable(Expression queryExpression)
        {
            Expression = queryExpression;
        }

        public static LambdaExpression? CreateExpression(QueryKind queryKind, QueryContent queryContent) =>
            (queryKind, queryContent) switch
            {
                //QueryKind.Aggregate => null,
                (QueryKind.All, QueryContent.GreaterThan0) => (Expression<Predicate<int>>)((int data) => data > 0),
                (QueryKind.Any, QueryContent.GreaterThan5) => (Expression<Predicate<int>>)((int data) => data > 5),
                (QueryKind.Any, QueryContent.LesserThan10) => (Expression<Predicate<int>>)((int data) => data < 10),
                //QueryKind.Count => null,
                //QueryKind.First => null,
                (QueryKind.Where, QueryContent.GreaterThan5) => (Expression<Predicate<int>>)((int data) => data > 5),
                (QueryKind.Where, QueryContent.LesserThan10) => (Expression<Predicate<int>>)((int data) => data < 10),
                (QueryKind.TakeWhile, QueryContent.LesserThan5) => (Expression<Predicate<int>>)((int data) => data < 5),
                (QueryKind.TakeWhile, QueryContent.GreaterThan5) => (Expression<Predicate<int>>)((int data) => data > 5),
                (QueryKind.TakeWhile, QueryContent.LesserThan10) => (Expression<Predicate<int>>)((int data) => data < 10),
                //QueryKind.Skip => null,
                //QueryKind.Single => null,
                //QueryKind.Take => null,
                _ => throw new ArgumentException()
            };

        public IEnumerator<int> GetEnumerator() => 
            (Provider.Execute<IEnumerable<int>>(Expression) as IEnumerable<int>).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    internal class MockQueryProvider : QueryProvider
    {
        private static QueryContext _queryContext;

        protected override QueryContext QueryContext => _queryContext;

        static MockQueryProvider()
        {
            _queryContext = new MockQueryContext();
        }

        public override IQueryable<TQueryResult> CreateQuery<TQueryResult>(Expression queryExpression)
            => (IQueryable<TQueryResult>)CreateQueryImpl(queryExpression);

        private IEnumerable<int> CreateQueryImpl(Expression queryExpression)
        {
            IEnumerable<int> queryable = new DataSourceExtractor<IEnumerable<int>>().Extract(queryExpression);

            /*
               Проверка выражения запроса на избыточность (а также корректность). 
               Если оно избыточно, то экземпляр MethodCallInspector вернёт
               неизбыточное выражение, равное константному выражению источника данных для запроса.
             */
            if (queryExpression != new QueryInspector().Visit(queryExpression))
                return queryable;

            return new MockQueryable(queryExpression);
        }

        protected override QueryPipelineExecutor2 CreateQueryPipelineExecutor(object dataSource)
            => new MockQueryPipelineExecutor(dataSource);
    }

    internal class MockQueryContext : QueryContext
    {
        protected override SingleQueryExecutor<TData, IEnumerable<TData>> BuildWhereQuery<TData>(MethodCallExpression whereExpr)
        {
            return base.BuildWhereQuery<TData>(whereExpr);
        }
    }

    internal class MockQueryPipelineExecutor : QueryPipelineExecutor2
    {
        public MockQueryPipelineExecutor(object dataSource) : base(dataSource) { }
    }

    internal static class MockQueryableHelper
    {
        public static MockQueryable AsMockQueryable(this IQueryable<int> queryable) => (MockQueryable)queryable;

        public static bool All(this IQueryable<int> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr = MockQueryable.CreateExpression(QueryKind.All, queryContent);

            return All(source, predicateExpr as Expression<Predicate<int>>);
        }

        private static bool All(this IQueryable<int> source, Expression<Predicate<int>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<int> queryied = source.Provider.CreateQuery<int>(
                BuildCoveredExpression(source, queryMethodInfo, predicateExpr));

            return queryied.Provider.Execute<bool>(queryied.Expression);
        }

        public static bool Any(this IQueryable<int> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr = MockQueryable.CreateExpression(QueryKind.Any, queryContent);

            return Any(source, predicateExpr as Expression<Predicate<int>>);
        }

        private static bool Any(this IQueryable<int> source, Expression<Predicate<int>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<int> queryied = source.Provider.CreateQuery<int>(BuildCoveredExpression(
                source, queryMethodInfo, predicateExpr));

            return queryied.Provider.Execute<bool>(queryied.Expression);
        }

        public static IQueryable<int> Where(this IQueryable<int> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr = MockQueryable.CreateExpression(QueryKind.Where, queryContent);

            return Where(source, predicateExpr as Expression<Predicate<int>>);
        }

        private static IQueryable<int> Where(this IQueryable<int> source, Expression<Predicate<int>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<int>(BuildCoveredExpression(source, queryMethodInfo, predicateExpr));
        }

        public static IQueryable<int> TakeWhile(this IQueryable<int> source, QueryContent queryContent)
        {
            LambdaExpression predicateExpr = MockQueryable.CreateExpression(QueryKind.TakeWhile, queryContent);

            return TakeWhile(source, predicateExpr as Expression<Predicate<int>>);
        }

        private static IQueryable<int> TakeWhile(this IQueryable<int> source, Expression<Predicate<int>> predicateExpr = null)
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return source.Provider.CreateQuery<int>(BuildCoveredExpression(source, queryMethodInfo, predicateExpr));
        }

        private static Expression BuildCoveredExpression(
            IQueryable<int> source,
            MethodInfo queryMethodInfo,
            params Expression[] queryMethodParams)
        {
            Expression[] queryMethodParamsBuf = new Expression[queryMethodParams.Length + 1];
            queryMethodParamsBuf[0] = Expression.Constant(source);
            queryMethodParams.Select(expr => Expression.Quote(expr)).ToArray().CopyTo(queryMethodParamsBuf, 1);

            return Expression.Call(
                null,
                queryMethodInfo,
                queryMethodParamsBuf);
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
        public void WhereQuery()
        {
            MockQueryable source = new MockQueryable();
            MockQueryable query = source.Where(QueryContent.GreaterThan5).AsMockQueryable();

            List<int> whereGreaterThan5Predefined = MockQueryable.DataSource.Where(data => data > 5).ToList(),
                      whereGreaterThan5 = new List<int>();

            foreach (int data in query)
                whereGreaterThan5.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(whereGreaterThan5, whereGreaterThan5Predefined));
        }

        [TestMethod]
        public void TakeWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            MockQueryable query = source
                .TakeWhile(QueryContent.LesserThan5).AsMockQueryable();

            List<int> takenWhileLesserThan5Predefined =
                MockQueryable.DataSource.TakeWhile(data => data < 5).ToList(),
                      takenWhileLesserThan5 = new List<int>();

            foreach (int data in query)
                takenWhileLesserThan5.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(takenWhileLesserThan5Predefined, takenWhileLesserThan5));
        }

        [TestMethod]
        public void WhereWhereQuery()
        {
            MockQueryable source = new MockQueryable();
            MockQueryable query = source
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
        public void WhereTakeWhileQuery()
        {
            MockQueryable source = new MockQueryable();
            MockQueryable query = source
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
        public void TakeWhileWhereQuery()
        {
            MockQueryable source = new MockQueryable();
            MockQueryable query = source
                .TakeWhile(QueryContent.LesserThan5).AsMockQueryable()
                .Where(QueryContent.LesserThan10).AsMockQueryable();

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
    }
}
