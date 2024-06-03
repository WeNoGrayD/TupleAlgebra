using LINQProvider;
using LINQProvider.QueryPipelineInfrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure
{
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;
    using static TupleObjectStaticDataStorage;

    public class TupleObjectQueryProvider
        : QueryProvider
    {
        #region Constructors

        public TupleObjectQueryProvider()
            : base()
        {
            return;
        }

        #endregion

        #region Istance methods

        /*
        protected virtual QueryContext CreateQueryContext()
        {
            return new QueryContext();
        }
        */

        protected override IDataSourceExtractor<IQueryable> 
            CreateDataSourceExtractor<TQueryEntity>()
        {
            return new DataSourceExtractor<IFactoryProvider<TupleObjectFactory>>()
                as IDataSourceExtractor<IQueryable>;
        }

        protected virtual QueryInspector CreateQueryInspector(IQueryable dataSource)
        {
            return new QueryInspector(dataSource);
        }

        /// <summary>
        /// Оборачивание перечислимого нефиктивного результата запроса в компоненту атрибута.
        /// </summary>
        /// <typeparam name="TQueryResultData"></typeparam>
        /// <param name="queryableAC"></param>
        /// <param name="queryResult"></param>
        /// <returns></returns>
        /*
        protected object WrapEnumerableResultWithTupleObject<TQueryEntity>(
            TupleObject<TQueryEntity> queryableTO,
            IEnumerable<TQueryEntity> queryResult)
            where TQueryEntity : new()
        {
            return queryableTO.Factory.CreateQueried<TQueryEntity>(factoryArgs);
        }
        */

        #endregion

        #region IQueryProvider implementation

        /// <summary>
        /// Имплементированное обобщённое создание запроса.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="queryExpression"></param>
        /// <param name="queryableAC"></param>
        /// <returns></returns>
        protected override IQueryable<TQueryEntity> CreateQueryImpl<TQueryEntity>(
            Expression queryExpression,
            IQueryable dataSource)
        {
            IQueryable<TQueryEntity> res;

            CreateQueryImplRestricted(
                queryExpression,
                (dynamic)dataSource,
                out res);

            return res;
        }

        private void CreateQueryImplRestricted<
            TSourceEntity, TQueryEntity>(
            Expression queryExpression,
            TupleObject<TSourceEntity> queryableTO,
            out IQueryable<TQueryEntity> res)
            where TSourceEntity : new()
            where TQueryEntity : new()
        {
            res = queryableTO.Factory.CreateQueried<TQueryEntity>(
                queryExpression,
                TupleObjectBuilder<TQueryEntity>.StaticBuilder.Schema.PassToBuilder);

            return;
        }

        /// <summary>
        /// Обобщённое выполнение запроса.
        /// </summary>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <param name="queryExpression"></param>
        /// <returns></returns>
        public override TQueryResult Execute<TQueryResult>(Expression queryExpression)
        {
            TQueryResult queryResult = base.Execute<TQueryResult>(queryExpression);
            /*
            if (_isResultEnumerable && !_queryIsFiction)
                return (TQueryResult)WrapEnumerableResultWithTupleObject(
                    (dynamic)_queryDataSource, 
                    queryResult);
            */
            return queryResult;
        }

        #endregion

        private static QueriedTupleType ProjectOntoQueriedTupleType(
            ExpressionType operatorType)
        {
            return operatorType switch
            {
                ExpressionType.AndAlso => QueriedTupleType.C,
                ExpressionType.OrElse => QueriedTupleType.D,
                _ => QueriedTupleType.U
            };
        }

        private class PredicateReplacer<TEntity> 
            : ExpressionVisitor
            where TEntity : new()
        {
            private ParameterExpression _entityParameterExpr;

            private MemberExpression _memberExpr;

            private bool _searchForMember;

            private MemberAccessExpressionReplacer _memberAccessReplacer;

            private QueryLayer _currentQueryLayer;

            public Expression Replace(
                Expression<Func<TEntity, bool>> predicateExpr)
            {
                _entityParameterExpr = predicateExpr.Parameters[0];
                _memberAccessReplacer =
                new MemberAccessExpressionReplacer(
                    _entityParameterExpr);

                return VisitLambda(predicateExpr);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                /*
                 * Определение того, что этот доступ к атрибуту запрашивается
                 * у параметра сущности, которая проверяется предикатом.
                 */
                if (object.ReferenceEquals(
                    node.Expression, _entityParameterExpr))
                {
                    MemberTypes memberType;
                    switch (memberType = node.Member.MemberType)
                    {
                        case MemberTypes.Field:
                        case MemberTypes.Property:
                            {
                                _memberExpr = node;
                                _searchForMember = false;

                                break;
                            }
                        default: break;
                    }
                }

                return base.VisitMember(node);
            }

            private void CreateQueryLayerIfNeeded(ExpressionType logicalForm)
            {
                if (_currentQueryLayer is null)
                {
                    _currentQueryLayer = new QueryRow(logicalForm);
                }
                else if (logicalForm != _currentQueryLayer.LogicalForm)
                {
                    QueryColumn newQueryLayer = new QueryColumn(logicalForm);
                    newQueryLayer.AddChild(_currentQueryLayer);
                    _currentQueryLayer = newQueryLayer;
                }

                return;
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {

                return node.NodeType switch
                {
                    ExpressionType.AndAlso => VisitBinaryImpl(node),
                    ExpressionType.OrElse => VisitBinaryImpl(node),
                    _ => base.VisitBinary(node)
                };
            }

            private Expression VisitBinaryImpl(BinaryExpression node)
            {
                Expression left, right;
                MemberExpression leftMember = null, rightMember = null;

                _searchForMember = true;
                left = Visit(node.Left);
                if (!_searchForMember)
                {
                    leftMember = _memberExpr;
                }

                _searchForMember = true;
                right = Visit(node.Right);
                if (!_searchForMember)
                {
                    rightMember = _memberExpr;
                }

                Expression result =
                    Expression.MakeBinary(node.NodeType, left, right);

                _searchForMember = true;

                switch ((leftMember, rightMember))
                {
                    case (null, null): break;
                    case (_, null):
                        {
                            CreateQueryLayerIfNeeded(node.NodeType);
                            AddQueryToLayer(leftMember, result);

                            break;
                        }
                    case (null, _):
                        {
                            CreateQueryLayerIfNeeded(node.NodeType);
                            AddQueryToLayer(rightMember, result);

                            break;
                        }
                    case (_, _):
                        {
                            CreateQueryLayerIfNeeded(node.NodeType);
                            if (leftMember.Member.Equals(rightMember.Member))
                            {
                                AddQueryToLayer(leftMember, result);
                            }
                            else
                            {
                                AddQueryToLayer(leftMember, left);
                                AddQueryToLayer(rightMember, right);
                            }

                            break;
                        }
                }

                return result;

                void AddQueryToLayer(MemberExpression memberExpr, Expression queryBody)
                {
                    LambdaExpression attributeGettetExpr =
                        DefineMemberAccess(memberExpr);
                    ParameterExpression memberParameterExpr =
                        GetMemberAccessAsParameter(memberExpr.Member);
                    queryBody = _memberAccessReplacer.Replace(
                        memberExpr,
                        memberParameterExpr,
                        queryBody);
                    LambdaExpression queryBodyLambda =
                        Expression.Lambda(queryBody, memberParameterExpr);

                    return;
                }
            }

            protected override Expression VisitUnary(UnaryExpression node)
            {
                return node.NodeType switch
                {
                    ExpressionType.Not => VisitNot(node),
                    _ => base.VisitUnary(node)
                };
            }

            private Expression VisitAndAlso(
                BinaryExpression node)
            {
                return null;
            }

            private Expression VisitOrElse(
                BinaryExpression node)
            {
                return null;
            }

            private Expression VisitNot(UnaryExpression node)
            {
                return null;
            }

            private LambdaExpression//Expression<AttributeGetterHandler<TEntity, TAttribute>>
                DefineMemberAccess(
                    MemberExpression memberExpr)
            {
                LambdaExpression
                    attributeGetterExpr = Expression
                    .Lambda(
                        memberExpr,
                        _entityParameterExpr);

                return attributeGetterExpr;
            }

            private ParameterExpression GetMemberAccessAsParameter(
                MemberInfo memberInfo)
            {
                ParameterExpression attrParamExpr =
                    Expression.Parameter(
                        _memberExpr.Member.MemberType switch
                        {
                            MemberTypes.Field =>
                                (_memberExpr.Member as FieldInfo).FieldType,
                            MemberTypes.Property =>
                                (_memberExpr.Member as PropertyInfo).PropertyType
                        },
                        memberInfo.Name);

                return attrParamExpr;
            }

            private abstract class QueryLayer
            {
                public ExpressionType LogicalForm { get; private set; }

                public QueryLayer(ExpressionType logicalForm)
                {
                    LogicalForm = logicalForm;

                    return;
                }

                protected ExpressionType Not(ExpressionType operatorType)
                {
                    return operatorType switch
                    {
                        ExpressionType.AndAlso => ExpressionType.OrElse,
                        ExpressionType.OrElse => ExpressionType.AndAlso,
                        _ => throw new ArgumentException(
                            "В создании выражений не поддерживаются операторы, кроме && и ||.", 
                            nameof(operatorType))
                    };
                }

                public abstract void Accept(
                    QueryTreeNode node,
                    ExpressionType operatorType);

                public abstract QueryLayer Accept(QueryLayer queryLayer);

                public abstract TupleObject<TEntity> AsTupleObject(
                    TupleObjectFactory factory,
                    TupleObjectBuilder<TEntity> builder);
            }

            private class QueryRow : QueryLayer
            {
                private IDictionary<AttributeName, QueryTreeNode> _attributes;

                public QueryRow(ExpressionType logicalForm)
                    : base(logicalForm)
                {
                    _attributes = new Dictionary<AttributeName, QueryTreeNode>();

                    return;
                }

                public void AddAttribute(
                    QueryTreeNode node,
                    ExpressionType operatorType)
                {
                    AttributeName attrName = node.AttributeGetterExpression;
                    if (_attributes.ContainsKey(attrName))
                    {
                        /*
                         * f1(e.A) и f2(e.A) => f3 = f1(e.A) x f2(e.A)
                         */
                        QueryTreeNode left = _attributes[attrName];
                        BinaryExpression queryBody = Expression.MakeBinary(
                            operatorType,
                            left.QueryDescription.Body,
                            node.QueryDescription.Body);
                        ParameterExpression memberParamExpr =
                            node.QueryDescription.Parameters[0];
                        LambdaExpression newQueryDescription =
                            Expression.Lambda(
                                queryBody,
                                memberParamExpr);
                        node = new(
                            node.AttributeGetterExpression,
                            newQueryDescription);
                    }
                    _attributes.Add(attrName, node);

                    return;
                }

                public override void Accept(
                    QueryTreeNode node,
                    ExpressionType operatorType)
                {
                    AddAttribute(node, operatorType);

                    return;
                }

                public override QueryLayer Accept(QueryLayer queryLayer)
                {
                    switch (queryLayer)
                    {
                        case QueryRow row:
                            {
                                QueryColumn col = new QueryColumn(
                                    Not(LogicalForm));
                                col.AddChild(this);
                                col.AddChild(queryLayer);

                                return col; 
                            }
                        case QueryColumn col:
                            {
                                col.AddChild(this);

                                return col;
                            }
                    }

                    return queryLayer;
                }

                public override TupleObject<TEntity> AsTupleObject(
                    TupleObjectFactory factory,
                    TupleObjectBuilder<TEntity> builder)
                {
                    builder ??=
                        factory.GetDefaultBuilder<TEntity>();

                    return factory.CreateQueriedSingleTupleObject(
                        ProjectOntoQueriedTupleType(LogicalForm),
                        _attributes.Select(
                            (qtn) => new NamedComponentFactoryArgs<Expression>(
                                qtn.Key,
                                builder,
                                qtn.Value.QueryDescription)),
                        builder);
                }
            }

            private class QueryColumn : QueryLayer
            {
                private IList<QueryLayer> _children;

                public QueryColumn(ExpressionType logicalForm)
                    : base(logicalForm)
                {
                    _children = new List<QueryLayer>();

                    return;
                }

                public void AddChild(QueryLayer child)
                {
                    _children.Add(child);

                    return;
                }

                public override void Accept(
                    QueryTreeNode node,
                    ExpressionType operatorType)
                {
                    _children.Last().Accept(node, operatorType);

                    return;
                }

                public override QueryLayer Accept(QueryLayer queryLayer)
                {
                    ExpressionType complementedOperatorType
                        = Not(this.LogicalForm);
                    if (queryLayer.LogicalForm == complementedOperatorType)
                    {
                        this.AddChild(queryLayer);

                        return this;
                    }

                    QueryColumn nextLayer = new QueryColumn(
                        complementedOperatorType);
                    nextLayer.AddChild(this);
                    nextLayer.AddChild(queryLayer);

                    return nextLayer;
                }

                public override TupleObject<TEntity> AsTupleObject(
                    TupleObjectFactory factory,
                    TupleObjectBuilder<TEntity> builder = null)
                {
                    builder ??= factory.GetDefaultBuilder<TEntity>();

                    /*
                    return factory.CreateQueriedSingleTupleObject(
                        ProjectOntoQueriedTupleType(LogicalForm),
                        _attributes.Select(
                            (qtn) => new NamedComponentFactoryArgs<Expression>(
                                qtn.Key,
                                builder,
                                qtn.Value.QueryDescription)),
                        builder);
                    */
                    return null;
                }
            }

            private record struct QueryTreeNode(
                LambdaExpression AttributeGetterExpression, // выражение доступа к атрибуту сущности
                LambdaExpression QueryDescription) // описание запроса к атрибуту, единственный параметр определяет атрибут сущности
            { }

            private class MemberAccessExpressionReplacer
                : ExpressionVisitor
            {
                ParameterExpression _entityParameterExpr;
                ParameterExpression _memberParameterExpr;
                MemberInfo _memberInfo;

                public MemberAccessExpressionReplacer(
                    ParameterExpression entityParameterExpr)
                {
                    _entityParameterExpr = entityParameterExpr;

                    return;
                }

                public Expression Replace(
                    MemberExpression memberExpr,
                    ParameterExpression memberParameterExpr,
                    Expression body)
                {
                    _memberInfo = memberExpr.Member;
                    _memberParameterExpr = memberParameterExpr;

                    return Visit(body);
                }

                protected override Expression VisitMember(MemberExpression node)
                {
                    /*
                     * Определение того, что этот доступ к атрибуту запрашивается
                     * у параметра сущнсоти, которая проверяется предикатом.
                     */
                    if (object.ReferenceEquals(
                            node.Expression, _entityParameterExpr) &&
                        object.ReferenceEquals(
                            node.Member, _memberInfo))
                    {
                        return VisitParameter(_memberParameterExpr);
                    }

                    return base.VisitMember(node);
                }
            }
        }
    }

    public interface IFactoryProvider<TFactory>
        : IEnumerable
    {
        public TFactory Factory { get; }
    }
}
