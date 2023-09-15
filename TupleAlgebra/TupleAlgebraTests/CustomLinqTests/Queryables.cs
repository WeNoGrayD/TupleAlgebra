using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraTests.DataModels;

namespace TupleAlgebraTests.CustomLinqTests
{
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
}
