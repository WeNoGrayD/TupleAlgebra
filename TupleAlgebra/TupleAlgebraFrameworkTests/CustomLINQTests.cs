using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TupleAlgebraFrameworkTests.DataModels;
using System.Linq.Expressions;
using TupleAlgebraClassLib.LINQ2TAFramework;
using System.Reflection;
using LINQProvider;

namespace TupleAlgebraFrameworkTests
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

        public static LambdaExpression? CreateExpression(QueryKind queryKind, MockQueryable source) =>
            queryKind switch
            {
                QueryKind.Aggregate => null,
                QueryKind.All => null,
                QueryKind.Any => null,
                QueryKind.Count => null,
                QueryKind.First => null,
                QueryKind.Where => (Expression<Predicate<int>>)((int data) => data > 5),
                QueryKind.Skip => null,
                QueryKind.Single => null,
                QueryKind.Take => null,
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

        public static IQueryable<int> Where(this MockQueryable source, Expression<Predicate<int>> predicate = null)
        {
            MethodInfo queryMethodInfo = (MethodBase.GetCurrentMethod() as MethodInfo);

            LambdaExpression predicateExpr = MockQueryable.CreateExpression(QueryKind.Where, source);

            return source.Provider.CreateQuery<int>(BuildCoveredExpression(source, queryMethodInfo, predicateExpr));
        }

        private static Expression BuildCoveredExpression(
            MockQueryable source,
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
        public void CreateQuery()
        {
            MockQueryable source = new MockQueryable();
            MockQueryable query = source.Where().AsMockQueryable();

            List<int> whereGreaterThan5Predefined = MockQueryable.DataSource.Where(data => data > 5).ToList(),
                      whereGreaterThan5 = new List<int>();

            foreach (int data in query)
                whereGreaterThan5.Add(data);

            Assert.IsTrue(Enumerable.SequenceEqual(whereGreaterThan5, whereGreaterThan5Predefined));
        }
    }
}
