using LINQProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using System.Text.RegularExpressions;
using TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure.QueryExecutors;

namespace TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure
{
    using static QueryContextHelper;

    public class TupleObjectQueryContext
        : QueryContext
    {
        #region Constants

        protected static Regex _queryMethodPattern { get; private set; }
            = new Regex($@"^Build(?<Query>\w+)Query$");

        #endregion

        #region Constructors

        static TupleObjectQueryContext()
        {
            IDictionary<string, IList<MethodInfo>> queryMethodPatterns =
                GetQueryMethodPatterns<TupleObjectQueryContext>(_queryMethodPattern);
            Helper.RegisterType<TupleObjectQueryContext>(
                queryMethodGroups: queryMethodPatterns);

            return;
        }

        #endregion

        #region Instance methods

        #region Query methods

        private SingleQueryExecutor<TEntity, bool>
            BuildAllQuery<TEntity>(
            QueriedTupleObject<TEntity> filterTuple)
            where TEntity : new()
        {
            return new TupleObjectAllQueryExecutor<TEntity>(
                filterTuple.ExecuteQuery());
        }

        private SingleQueryExecutor<TEntity, bool>
            BuildAnyQuery<TEntity>(
            QueriedTupleObject<TEntity> filterTuple)
            where TEntity : new()
        {
            return new TupleObjectAnyQueryExecutor<TEntity>(
                filterTuple.ExecuteQuery());
        }

        private SingleQueryExecutor<TEntity, TupleObject<TEntity>> 
            BuildWhereQuery<TEntity>(
            QueriedTupleObject<TEntity> filterTuple)
            where TEntity : new()
        {
            return new TupleObjectWhereQueryExecutor<TEntity>(
                filterTuple.ExecuteQuery());
        }

        #endregion

        #endregion

        /*
        public TupleObject<TEntity> TranslateToTupleObject<TEntity>(
            Expression queryExpression,
            ITupleObject queryableTO)
            where TEntity : new()
        {
            queryExpression = ((queryExpression as MethodCallExpression)
                .Arguments[1] as UnaryExpression).Operand;

            return Where();

            TupleObject<TEntity> TranslatePredicate(
                Expression<Func<TEntity, bool>> predicateExpr)
            {
                PredicateQueryTranslator<TEntity> queryTranslator =
                    new PredicateQueryTranslator<TEntity>();
                return queryTranslator.Translate(
                    predicateExpr,
                    queryableTO.Factory,
                    TupleObjectBuilder<TEntity>.StaticBuilder);
            }

            TupleObject<TEntity> TranslateQueryExpression()
            {
                return queryExpression switch
                {
                    Expression<Func<TEntity, bool>> predicateExpr =>
                        TranslatePredicate(predicateExpr),
                    _ => null
                };
            }

            TupleObject<TEntity> Where()
            {
                Type toType = typeof(TupleObject<TEntity>);
                Expression andExpr = Expression.MakeBinary(
                    ExpressionType.And,
                    Expression.TypeAs(
                        Expression.Constant(queryableTO),
                        toType),
                    Expression.TypeAs(
                        Expression.Constant(TranslateQueryExpression()),
                        toType));

                return queryableTO.Factory.CreateQueried<TEntity>(
                    andExpr,
                    TupleObjectBuilder<TEntity>.StaticBuilder);
            }
        }
        */
    }
}
