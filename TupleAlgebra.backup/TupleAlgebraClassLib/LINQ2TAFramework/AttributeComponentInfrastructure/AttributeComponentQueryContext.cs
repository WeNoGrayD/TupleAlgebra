using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure
{
    public class AttributeComponentQueryContext
    {
        public IQueryPipelineExecutorAcceptor BuildSingleQueryExecutor2(
            MethodCallExpression node)
        {
            Type queryContextType = typeof(AttributeComponentQueryContext),
                 queryResultType = node.Type;
            MethodInfo queryMethod;
            string queryMethodName = node.Method.Name;

            switch (queryMethodName)
            {
                case nameof(Queryable.Any):
                    { break; }
                case nameof(Queryable.All):
                    { break; }
                case nameof(Queryable.Select):
                    { break; }
                case nameof(Queryable.SelectMany):
                    { break; }
                case nameof(Queryable.Where):
                    {
                        queryMethod = queryContextType
                            .GetMethod(queryMethodName + "Query")
                            .MakeGenericMethod(queryResultType.GenericTypeArguments[0]);
                        return queryMethod.Invoke(this, new object[] { node }) as IQueryPipelineExecutorAcceptor;
                    }
                case nameof(Queryable.First):
                    { break; }
                case nameof(Queryable.FirstOrDefault):
                    { break; }
                case nameof(Queryable.Single):
                    { break; }
                case nameof(Queryable.SingleOrDefault):
                    { break; }
            }

            return null;
        }

        public IQueryPipelineExecutorAcceptor<TData> BuildSingleQueryExecutor<TData>(
            MethodCallExpression node)
        {
            Type queryContextType = typeof(AttributeComponentQueryContext),
                 queryResultType = node.Type;
            MethodInfo queryMethod;
            string queryMethodName = node.Method.Name;

            switch (queryMethodName)
            {
                case nameof(Queryable.Any):
                    { break; }
                case nameof(Queryable.All):
                    { break; }
                case nameof(Queryable.Select):
                    { break; }
                case nameof(Queryable.SelectMany):
                    { break; }
                case nameof(Queryable.Where):
                    {
                        queryMethod = queryContextType
                            .GetMethod(queryMethodName + "Query")
                            .MakeGenericMethod(queryResultType.GenericTypeArguments[0]);
                        return queryMethod.Invoke(this, new object[] { node }) as IQueryPipelineExecutorAcceptor<TData>;
                    }
                case nameof(Queryable.First):
                    { break; }
                case nameof(Queryable.FirstOrDefault):
                    { break; }
                case nameof(Queryable.Single):
                    { break; }
                case nameof(Queryable.SingleOrDefault):
                    { break; }
            }

            return null;
        }

        public virtual ISingleQueryExecutor<TData, IEnumerable<TData>> WhereQuery<TData>(
            MethodCallExpression whereExpr)
        {
            var a = (whereExpr.Arguments[1] as UnaryExpression).Operand;

            Type t1 = typeof(TData),
                 t2 = a.GetType();

            Expression<Func<TData, bool>> whereQueryBody =
                ((whereExpr.Arguments[1] as UnaryExpression).Operand as LambdaExpression)
                as Expression<Func<TData, bool>>;

            return new WhereQueryExecutor<TData>(whereQueryBody.Compile());
        }
    }
}
