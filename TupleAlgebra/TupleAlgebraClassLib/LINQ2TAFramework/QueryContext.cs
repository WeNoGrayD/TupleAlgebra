using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.DefaultQueryExecutors;

namespace TupleAlgebraClassLib.LINQ2TAFramework
{
    public abstract class QueryContext
    {
        IQueryPipelineExecutorAcceptor LastQueryExecutor;

        IQueryPipelineMiddleware FirstQueryExecutor;

        IQueryPipelineMiddleware _lastQueryExecutor;

        IQueryPipelineMiddleware LastQueryExecutor2
        {
            get => _lastQueryExecutor;
            set
            {
                if (_lastQueryExecutor is null)
                    FirstQueryExecutor = value;
                _lastQueryExecutor = value;
            }
        }

        public IQueryPipelineMiddleware BuildSingleQueryExecutor(
            MethodCallExpression node)
        {
            Type queryContextType = this.GetType(),
                 queryResultType = node.Arguments[0].Type.GetInterfaces()
                 .Where(type => type.Name == "IQueryable`1").Single();
            MethodInfo queryMethod;
            string queryMethodName = node.Method.Name;

            queryMethod = queryContextType
                .GetMethod("Build" + queryMethodName + "Query", BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(queryResultType.GenericTypeArguments[0]);

            IQueryPipelineAcceptor iqpa = queryMethod.Invoke(this, new object[] { node }) as IQueryPipelineAcceptor;
            IQueryPipelineMiddleware iqpm = SingleQueryExecutorWithAccumulationFactory.Create((dynamic)iqpa);
            FirstQueryExecutor = FirstQueryExecutor is null ? iqpm : FirstQueryExecutor.ContinueWith(iqpm);

            return iqpm;

            /*
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

                        IQueryPipelineAcceptor iqpa = queryMethod.Invoke(this, new object[] { node }) as IQueryPipelineAcceptor;
                        IQueryPipelineMiddleware iqpm = SingleQueryExecutorWithAccumulationFactory.Create((dynamic)iqpa);
                        FirstQueryExecutor = FirstQueryExecutor is null ? iqpm

                            : FirstQueryExecutor.ContinueWith(iqpm);

                        return iqpm;
                    }
                case nameof(Queryable.First):
                    { break; }
                case nameof(Queryable.FirstOrDefault):
                    { break; }
                case nameof(Queryable.Single): { break; }
                case nameof(Queryable.SingleOrDefault):
                    { break; }
            }

            return null;
            */
        }

        protected virtual SingleQueryExecutor<TData, bool> BuildAllQuery<TData>(
            MethodCallExpression allExpr)
        {
            Expression predicateExpr = GetPredicateExpression<TData>(allExpr);
            Predicate<TData> predicate = GetPredicate<TData>(predicateExpr);

            return new AllQueryExecutor<TData>(predicate);
        }

        protected virtual SingleQueryExecutor<TData, bool> BuildAnyQuery<TData>(
            MethodCallExpression anyExpr)
        {
            Expression predicateExpr = GetPredicateExpression<TData>(anyExpr);
            Predicate<TData> predicate = GetPredicate<TData>(predicateExpr);

            return new AnyQueryExecutor<TData>(predicate);
        }

        protected virtual SingleQueryExecutor<TData, IEnumerable<TQueryResultData>> BuildSelectQuery<TData, TQueryResultData>(
            MethodCallExpression whereExpr)
        {
            Expression predicateExpr = GetFuncExpression<TData, TQueryResultData>(whereExpr);
            Func<TData, TQueryResultData> predicate = GetFunc<TData, TQueryResultData>(predicateExpr);

            return new SelectQueryExecutor<TData, TQueryResultData>(predicate);
        }

        protected virtual SingleQueryExecutor<TData, IEnumerable<TData>> BuildWhereQuery<TData>(
            MethodCallExpression whereExpr)
        {
            Expression predicateExpr = GetPredicateExpression<TData>(whereExpr);
            Predicate<TData> predicate = GetPredicate<TData>(predicateExpr);

            return new WhereQueryExecutor<TData>(predicate);
        }

        protected virtual SingleQueryExecutor<TData, IEnumerable<TData>> BuildTakeWhileQuery<TData>(
            MethodCallExpression takeWhileExpr)
        {
            Expression predicateExpr = GetPredicateExpression<TData>(takeWhileExpr);
            Predicate<TData> predicate = GetPredicate<TData>(predicateExpr);

            return new TakeWhileQueryExecutor<TData>(predicate);
        }

        protected Expression GetPredicateExpression<TData>(MethodCallExpression methodExpr) => 
            (methodExpr.Arguments[1] as UnaryExpression).Operand switch
            {
                Expression<Predicate<TData>> lambdaPredicate => lambdaPredicate,
                MethodCallExpression methodPredicate when methodPredicate.Method.ReturnType == typeof(bool) => methodPredicate,
                var unknown => 
                    throw new ArgumentException($"Для запроса предоставлено неверное выражение предиката: {unknown.GetType()}.")
            };

        protected Expression GetPredicateExpression<TData>(MethodCallExpression methodExpr) =>
            (methodExpr.Arguments[1] as UnaryExpression).Operand switch
            {
                Expression<Predicate<TData>> lambdaPredicate => lambdaPredicate,
                MethodCallExpression methodPredicate when methodPredicate.Method.ReturnType == typeof(bool) => methodPredicate,
                var unknown =>
                    throw new ArgumentException($"Для запроса предоставлено неверное выражение предиката: {unknown.GetType()}.")
            };

        protected virtual Predicate<TData> GetPredicate<TData>(Expression predicateExpr) =>
            predicateExpr switch
            {
                Expression<Predicate<TData>> lambdaPredicate => lambdaPredicate.Compile(),
                MethodCallExpression methodPredicate when methodPredicate.Method.ReturnType == typeof(bool) => 
                    (TData data) => (bool)methodPredicate.Method.Invoke(methodPredicate.Object, new object[] { data }),
                var unknown =>
                    throw new ArgumentException($"Для запроса предоставлено неверное выражение предиката: {unknown.GetType()}.")
            };

        protected Expression GetFuncExpression<TData, TQueryResultData>(MethodCallExpression methodExpr) =>
            (methodExpr.Arguments[1] as UnaryExpression).Operand switch
            {
                Expression<Func<TData, TQueryResultData>> lambdaPredicate => lambdaPredicate,
                MethodCallExpression methodPredicate when methodPredicate.Method.ReturnType == typeof(TQueryResultData) => methodPredicate,
                var unknown =>
                    throw new ArgumentException($"Для запроса предоставлено неверное выражение предиката: {unknown.GetType()}.")
            };

        protected virtual Func<TData, TQueryResultData> GetFunc<TData, TQueryResultData>(Expression predicateExpr) =>
            predicateExpr switch
            {
                Expression<Func<TData, TQueryResultData>> lambdaPredicate => lambdaPredicate.Compile(),
                MethodCallExpression methodPredicate when methodPredicate.Method.ReturnType == typeof(TQueryResultData) =>
                    (TData data) => (TQueryResultData)methodPredicate.Method.Invoke(methodPredicate.Object, new object[] { data }),
                var unknown =>
                    throw new ArgumentException($"Для запроса предоставлено неверное выражение предиката: {unknown.GetType()}.")
            };

        /*
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

                        IQueryPipelineExecutorAcceptor currentQueryExecutor =
                            queryMethod.Invoke(this, new object[] { node }) as IQueryPipelineExecutorAcceptor;
                        //LastQueryExecutor2 = LastQueryExecutor2 is null ?
                        //    currentQueryExecutor : ContinueLastQueryExecutor((dynamic)LastQueryExecutor2, currentQueryExecutor);

                        //FirstQueryExecutor = FirstQueryExecutor is null ?
                        //    SingleQueryExecutorWithAccumulationFactory.Create((dynamic)currentQueryExecutor)
                        //    : FirstQueryExecutor.ContinueWith(currentQueryExecutor);

                        return queryMethod.Invoke(this, new object[] { node }) as IQueryPipelineExecutorAcceptor;
                    }
                case nameof(Queryable.First):
                    { break; }
                case nameof(Queryable.FirstOrDefault):
                    { break; }
                case nameof(Queryable.Single): { break; }
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
                (whereExpr.Arguments[1] as UnaryExpression).Operand as LambdaExpression
                as Expression<Func<TData, bool>>;

            return null;//new WhereQueryExecutor<TData>(whereQueryBody.Compile());
        }
        */


        /*
        public SingleQueryExecutor<TData, IEnumerable<TQueryResultData>> 
            ContinueLastQueryExecutor<TData, TQueryResultData, TNextQueryResult>(
            SingleQueryExecutor<TData, IEnumerable<TQueryResultData>> currentExecutor,
            SingleQueryExecutor<TQueryResultData, TNextQueryResult> nextExecutor,
            bool updateFirstQueryExecutor = true)
        {
            SingleQueryExecutor<TData, IEnumerable<TQueryResultData>> continuedExecutor = 
                currentExecutor.ContinueWith(nextExecutor);
            /*
            if (LastQueryExecutor2.GetType().GetGenericTypeDefinition() == typeof(SingleQueryExecutorWithContinuation<,,,>))
            {
                UpdateLastQueryExecutorWithContinuation((dynamic)LastQueryExecutor2, currentExecutor);
            }
            */
        /* 
         * Если метод вызывается с двумя обязательными параметрами, то это означает, что первый исполнитель запроса
         * ещё не был ни разу продолжен. Если цепочка запросов состоит более чем из одного запроса, то первый исполнитель
         * должен указывать на обёртку пёрвого исполнителя с продолжением, т.е. на экземпляр класса 
         * SingleQueryExecutorWithContinuation.
         */
        /*
        if (updateFirstQueryExecutor) FirstQueryExecutor = continuedExecutor;

        return continuedExecutor;
    }
    */

        /*
        public SingleQueryExecutor<TQueryResultData, TNextQueryResult>
            ContinueLastQueryExecutor<TInnerQueryExecutor, TData, TQueryResultData, TNextQueryResult>(
            SingleQueryExecutorWithContinuation<TInnerQueryExecutor, TData, TQueryResultData, TNextQueryResult> lastQueryExecutor,
            ISingleQueryExecutorVisitor2 nextExecutor)
            where TInnerQueryExecutor : SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
        {
            SingleQueryExecutor<TQueryResultData, TNextQueryResult> continuedExecutor =
                ContinueLastQueryExecutor(lastQueryExecutor.NextExecutor, (dynamic)nextExecutor, updateFirstQueryExecutor: false);
            lastQueryExecutor.UpdateNextQueryExecutor(continuedExecutor);

            return continuedExecutor;
        }
        */

        /// <summary>
        /// Обновление у последнего исполнителя запроса ссылки на следующий исполнитель запроса. 
        /// </summary>
        /// <typeparam name="TInnerQueryExecutor"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TQueryResultData"></typeparam>
        /// <typeparam name="TNextQueryResult"></typeparam>
        /// <param name="lastQueryExecutor"></param>
        /// <param name="nextExecutor"></param>
        /*
        public void UpdateLastQueryExecutorWithContinuation<TInnerQueryExecutor, TData, TQueryResultData, TNextQueryResult>(
            SingleQueryExecutorWithContinuation<TInnerQueryExecutor, TData, TQueryResultData, TNextQueryResult> lastQueryExecutor,
            SingleQueryExecutor<TQueryResultData, TNextQueryResult> nextExecutor)
            where TInnerQueryExecutor : SingleQueryExecutor<TData, IEnumerable<TQueryResultData>>
            => lastQueryExecutor.UpdateNextQueryExecutor(nextExecutor);*/
    }
}
