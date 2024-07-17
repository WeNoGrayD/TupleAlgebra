using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using System.Xml.Linq;

namespace TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure
{
    public static class LINQ2TupleAlgebra
    {
        private static Expression BuildCoveredExpression(
            IQueryable source,
            MethodInfo queryMethodInfo,
            Type[] genericArguments,
            params Expression[] queryMethodParams)
        {
            Expression[] queryMethodParamsBuf = 
                new Expression[queryMethodParams.Length + 1];
            queryMethodParamsBuf[0] = source.Expression;
            int i = 1;
            foreach (Expression paramExpr in queryMethodParams.Select(
                param => param switch
                {
                    LambdaExpression lambda => Expression.Quote(lambda),
                    Expression constant => constant
                })
                )
            {
                queryMethodParamsBuf[i] = paramExpr;
            }

            return Expression.Call(
                null,
                queryMethodInfo.MakeGenericMethod(genericArguments),
                queryMethodParamsBuf);
        }

        public static TupleObject<TQueryEntity> Select<TEntity, TQueryEntity>(
            this TupleObject<TEntity> tuple,
            Expression<Func<TEntity, TQueryEntity>> selectorExpr)
            where TEntity : new()
            where TQueryEntity : new()
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return tuple.Provider.CreateQuery<TQueryEntity>(
                BuildCoveredExpression(
                    tuple,
                    queryMethodInfo,
                    [typeof(TEntity), typeof(TQueryEntity)],
                    selectorExpr)) as TupleObject<TQueryEntity>;
        }

        public static TupleObject<TEntity> Where<TEntity>(
            this TupleObject<TEntity> tuple,
            Expression<Func<TEntity, bool>> filterExpr)
            where TEntity : new()
        {
            return Where(tuple, TranslatePredicate(tuple, filterExpr));
        }

        private static TupleObject<TEntity> Where<TEntity>(
            TupleObject<TEntity> tuple,
            TupleObject<TEntity> filterTuple)
            where TEntity : new()
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            return tuple.Provider.CreateQuery<TEntity>(
                BuildCoveredExpression(
                    tuple,
                    queryMethodInfo,
                    [typeof(TEntity)],
                    filterTuple.Expression)) as TupleObject<TEntity>;
        }

        public static bool All<TEntity>(
            this TupleObject<TEntity> tuple,
            Expression<Func<TEntity, bool>> filterExpr)
            where TEntity : new()
        {
            return All(tuple, TranslatePredicate(tuple, filterExpr));
        }

        private static bool All<TEntity>(
            TupleObject<TEntity> tuple,
            TupleObject<TEntity> filterTuple)
            where TEntity : new()
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<TEntity> res = tuple.Provider.CreateQuery<TEntity>(
                BuildCoveredExpression(
                    tuple,
                    queryMethodInfo,
                    [typeof(TEntity)],
                    filterTuple.Expression));
            return res.Provider.Execute<bool>(res.Expression);
        }

        public static bool Any<TEntity>(
            this TupleObject<TEntity> tuple,
            Expression<Func<TEntity, bool>> filterExpr)
            where TEntity : new()
        {
            return Any(tuple, TranslatePredicate(tuple, filterExpr));
        }

        private static bool Any<TEntity>(
            TupleObject<TEntity> tuple,
            TupleObject<TEntity> filterTuple)
            where TEntity : new()
        {
            MethodInfo queryMethodInfo = MethodBase.GetCurrentMethod() as MethodInfo;

            IQueryable<TEntity> res = tuple.Provider.CreateQuery<TEntity>(
                BuildCoveredExpression(
                    tuple,
                    queryMethodInfo,
                    [typeof(TEntity)],
                    filterTuple.Expression));
            return res.Provider.Execute<bool>(res.Expression);
        }

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

        private static TupleObject<TEntity> TranslatePredicate<TEntity>(
            ITupleObject queryableTO,
            Expression<Func<TEntity, bool>> predicateExpr)
            where TEntity : new()
        {
            PredicateQueryTranslator<TEntity> queryTranslator =
                new PredicateQueryTranslator<TEntity>();
            return queryTranslator.Translate(
                predicateExpr,
                queryableTO.Factory);
        }

        #region Nested types

        private class PredicateQueryTranslator<TEntity>
            : ExpressionVisitor
            where TEntity : new()
        {
            private ParameterExpression _entityParameterExpr;

            private MemberExpression _memberExpr;

            private bool _searchForMember;

            private MemberAccessExpressionReplacer _memberAccessReplacer;

            private QueryLayer _currentQueryLayer;

            public TupleObject<TEntity> Translate(
                Expression<Func<TEntity, bool>> predicateExpr,
                TupleObjectFactory factory,
                TupleObjectBuilder<TEntity> builder = null)
            {
                _entityParameterExpr = predicateExpr.Parameters[0];
                _memberAccessReplacer =
                new MemberAccessExpressionReplacer(
                    _entityParameterExpr);

                _searchForMember = true;
                VisitLambda(predicateExpr);
                if (!_searchForMember)
                {
                    CreateQueryLayer(ExpressionType.AndAlso);
                    AddQueryToLayer(ExpressionType.AndAlso, _memberExpr, predicateExpr.Body);
                }

                return _currentQueryLayer.AsTupleObject(factory, builder);
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

            private void CreateQueryLayer(ExpressionType logicalForm)
            {
                _currentQueryLayer = new QueryRow(logicalForm);
                /*
                if (_currentQueryLayer is null)
                {
                    _currentQueryLayer = new QueryRow(logicalForm);
                }*/
                /*
                else if (logicalForm != _currentQueryLayer.LogicalForm)
                {
                    QueryColumn newQueryLayer = new QueryColumn(logicalForm);
                    newQueryLayer.AddChild(_currentQueryLayer);
                    _currentQueryLayer = newQueryLayer;
                }
                */

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
                QueryLayer currentLayer = _currentQueryLayer,
                           leftLayer, rightLayer;

                _searchForMember = true;
                _currentQueryLayer = null;
                left = Visit(node.Left);
                leftLayer = _currentQueryLayer;
                if (!_searchForMember)
                {
                    leftMember = _memberExpr;
                }

                _searchForMember = true;
                _currentQueryLayer = null;
                right = Visit(node.Right);
                rightLayer = _currentQueryLayer;
                if (!_searchForMember)
                {
                    rightMember = _memberExpr;
                }

                Expression result =
                    Expression.MakeBinary(node.NodeType, left, right);
                _searchForMember = true;

                switch ((leftMember, rightMember))
                {
                    case (null, null):
                        {
                            _currentQueryLayer =
                                leftLayer.Visit(rightLayer, node.NodeType);

                            break;
                        }
                    case (_, null):
                        {
                            CreateQueryLayer(node.NodeType);
                            AddQueryToLayer(node.NodeType, leftMember, result);
                            _currentQueryLayer =
                                _currentQueryLayer.Visit(rightLayer, node.NodeType);

                            break;
                        }
                    case (null, _):
                        {
                            _currentQueryLayer = leftLayer;
                            AddQueryToLayer(node.NodeType, rightMember, right);
                            /*
                            if (leftLayer.HasSameLogicalForm(node.NodeType))
                            {
                                _currentQueryLayer = leftLayer;
                                AddQueryToLayer(rightMember, right);
                            }
                            else
                            {
                                CreateQueryLayer(node.NodeType);
                                AddQueryToLayer(rightMember, right);
                                _currentQueryLayer =
                                    leftLayer.Visit(_currentQueryLayer, node.NodeType);
                            }
                            */

                            break;
                        }
                    case (_, _):
                        {
                            CreateQueryLayer(node.NodeType);
                            if (leftMember.Member.Equals(rightMember.Member))
                            {
                                /*
                                 * TODO:
                                 * Нужно научиться соединять запроса вида:
                                 * (f1(e.A) && f2(e.A)) && f3(e.A)
                                 */
                                AddQueryToLayer(node.NodeType, leftMember, result);
                            }
                            else
                            {
                                AddQueryToLayer(node.NodeType, leftMember, left);
                                AddQueryToLayer(node.NodeType, rightMember, right);
                            }

                            break;
                        }
                }

                return result;
            }

            private void AddQueryToLayer(
                ExpressionType nodeType,
                MemberExpression memberExpr,
                Expression queryBody)
            {
                LambdaExpression attributeGettetExpr =
                    DefineMemberAccess(memberExpr);
                ParameterExpression memberParameterExpr =
                    GetMemberAccessAsParameter(memberExpr);
                queryBody = _memberAccessReplacer.Replace(
                    memberExpr,
                    memberParameterExpr,
                    queryBody);
                LambdaExpression queryBodyLambda =
                    Expression.Lambda(queryBody, memberParameterExpr);
                _currentQueryLayer = _currentQueryLayer.Visit(
                    new QueryTreeNode(attributeGettetExpr, queryBodyLambda),
                    nodeType);

                return;
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

            private LambdaExpression//Expression<Func<TEntity, TAttribute>>
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
                MemberExpression memberExpr)
            {
                ParameterExpression attrParamExpr =
                    Expression.Parameter(
                        memberExpr.Member.MemberType switch
                        {
                            MemberTypes.Field =>
                                (memberExpr.Member as FieldInfo).FieldType,
                            MemberTypes.Property =>
                                (memberExpr.Member as PropertyInfo).PropertyType
                        },
                        memberExpr.Member.Name);

                return attrParamExpr;
            }

            private abstract class QueryLayer
            {
                public ExpressionType LogicalForm { get; private set; }

                public abstract IReadOnlySet<AttributeName> PluggedAttributes { get; }

                public QueryLayer(ExpressionType logicalForm)
                {
                    LogicalForm = logicalForm;

                    return;
                }

                protected static ExpressionType Not(ExpressionType operatorType)
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

                public abstract bool HasSameLogicalForm(ExpressionType logicalForm);

                public static QueryLayer CreateQueryRow(
                    QueryTreeNode node,
                    ExpressionType logicalForm)
                {
                    QueryRow row = new QueryRow(logicalForm);
                    row.AddAttribute(node);

                    return row;
                }

                public static QueryLayer CreateQueryTable(
                    QueryRow leftLayer,
                    QueryRow rightLayer,
                    ExpressionType logicalForm)
                {
                    QueryTable table = new QueryTable(logicalForm);
                    table.AddChild(leftLayer);
                    table.AddChild(rightLayer);

                    return table;
                }

                public static QueryLayer CreateQueryExpression(
                    QueryLayer leftLayer,
                    QueryLayer rightLayer,
                    ExpressionType logicalForm)
                {
                    QueryColumnExpression col = new QueryColumnExpression(logicalForm);
                    col.AddChild(leftLayer);
                    col.AddChild(rightLayer);

                    return col;
                }

                public abstract QueryLayer Visit(
                    QueryTreeNode node,
                    ExpressionType operatorType);

                public abstract QueryLayer Visit(
                    QueryLayer queryLayer,
                    ExpressionType logicalForm);

                public abstract TupleObject<TEntity> AsTupleObject(
                    TupleObjectFactory factory,
                    TupleObjectBuilder<TEntity> builder);

                protected void PassSchema(
                    TupleObjectBuilder<TEntity> builder)
                {
                    builder.Schema = TupleObjectBuilder<TEntity>
                        .StaticBuilder.Schema.Clone();
                    foreach (AttributeName attrName in builder.Schema.Attributes.Keys)
                    {
                        if (PluggedAttributes.Contains(attrName))
                        {
                            builder.Attribute(attrName).Attach();
                        }
                        else
                        {
                            builder.Attribute(attrName).Detach();
                        }
                    }

                    builder.EndSchemaInitialization();

                    return;
                }
            }

            private class QueryRow : QueryLayer
            {
                private IDictionary<AttributeName, QueryTreeNode> _attributes;

                public override IReadOnlySet<AttributeName> PluggedAttributes
                { get => _attributes.Keys.ToHashSet(); }

                public QueryRow(ExpressionType logicalForm)
                    : base(logicalForm)
                {
                    _attributes = new Dictionary<AttributeName, QueryTreeNode>();

                    return;
                }

                public void AddAttribute(QueryTreeNode node)
                {
                    AttributeName attrName = node.AttributeGetterExpression;
                    if (_attributes.ContainsKey(attrName))
                    {
                        /*
                         * f1(e.A) и f2(e.A) => f3 = f1(e.A) x f2(e.A)
                         */
                        QueryTreeNode left = _attributes[attrName];
                        BinaryExpression queryBody = Expression.MakeBinary(
                            LogicalForm,
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

                public override bool HasSameLogicalForm(ExpressionType logicalForm)
                {
                    return LogicalForm == logicalForm;
                }

                public override QueryLayer Visit(
                    QueryTreeNode node,
                    ExpressionType operatorType)
                {
                    AddAttribute(node);

                    return this;
                }

                public override QueryLayer Visit(
                    QueryLayer queryLayer,
                    ExpressionType logicalForm)
                {
                    switch (queryLayer)
                    {
                        case QueryRow row:
                            {
                                if (LogicalForm == queryLayer.LogicalForm)
                                {
                                    return CreateQueryTable(this, row, logicalForm);
                                }

                                return CreateQueryExpression(
                                    this,
                                    queryLayer,
                                    logicalForm);
                            }
                        default:
                            {
                                return queryLayer.Visit(this, logicalForm);
                            }
                    }
                }

                public IEnumerable<NamedComponentFactoryArgs<Expression>>
                    GetAttributeExpressions(TupleObjectBuilder<TEntity> builder)
                {
                    return _attributes.Select(
                        (qtn) => new NamedComponentFactoryArgs<Expression>(
                            qtn.Key,
                            builder,
                            qtn.Value.QueryDescription));
                }

                public override TupleObject<TEntity> AsTupleObject(
                    TupleObjectFactory factory,
                    TupleObjectBuilder<TEntity> builder)
                {
                    builder ??= factory.GetBuilder<TEntity>(PassSchema);

                    return factory.CreateQueriedSingleTupleObject(
                        ProjectOntoQueriedTupleType(LogicalForm),
                        GetAttributeExpressions(builder),
                        builder);
                }
            }

            private abstract class QueryColumn<TChild>
                : QueryLayer
                where TChild : QueryLayer
            {
                private IList<TChild> _children;

                protected IReadOnlyList<TChild> Children
                { get => _children.AsReadOnly(); }

                public override IReadOnlySet<AttributeName> PluggedAttributes
                { get => _children.SelectMany(c => c.PluggedAttributes).ToHashSet(); }

                public QueryColumn(ExpressionType logicalForm)
                    : base(logicalForm)
                {
                    _children = new List<TChild>();

                    return;
                }

                public void AddChild(TChild child)
                {
                    _children.Add(child);

                    return;
                }

                public override bool HasSameLogicalForm(ExpressionType logicalForm)
                {
                    return LogicalForm == Not(logicalForm);
                }
            }

            private class QueryTable
                : QueryColumn<QueryRow>
            {
                public QueryTable(ExpressionType logicalForm)
                    : base(logicalForm)
                {
                    return;
                }

                public override QueryLayer Visit(
                    QueryTreeNode node,
                    ExpressionType operatorType)
                {
                    return Visit(
                        CreateQueryRow(node, Not(LogicalForm)), operatorType);
                }

                public override QueryLayer Visit(
                    QueryLayer queryLayer,
                    ExpressionType logicalForm)
                {
                    switch (queryLayer)
                    {
                        case QueryRow row:
                            {
                                if (LogicalForm == logicalForm &&
                                    LogicalForm == Not(row.LogicalForm))
                                {
                                    this.AddChild(row);

                                    return this;
                                }

                                goto default;
                            }
                        case QueryTable table:
                            {
                                if (LogicalForm == logicalForm &&
                                    LogicalForm == table.LogicalForm)
                                {
                                    foreach (var row in table.Children)
                                        this.AddChild(row);

                                    return this;
                                }

                                goto default;
                            }
                        case QueryColumnExpression expr:
                            {
                                /*
                                if (logicalForm == expr.LogicalForm)
                                {
                                    expr.AddChild(this);

                                    return expr;
                                }
                                */

                                return expr.Visit(this, logicalForm);
                            }
                        default:
                            {
                                return CreateQueryExpression(
                                    this,
                                    queryLayer,
                                    logicalForm);
                            }
                    }
                }

                public override TupleObject<TEntity> AsTupleObject(
                    TupleObjectFactory factory,
                    TupleObjectBuilder<TEntity> builder = null)
                {
                    builder ??= factory.GetBuilder<TEntity>(PassSchema);

                    return factory.CreateQueriedTupleObjectSystem(
                        ProjectOntoQueriedTupleType(Not(LogicalForm)), // not - потому что c-системы - это объединение кортежей, d-системы - пересечение
                        new SquareEnumerable<NamedComponentFactoryArgs<Expression>>(
                            Children.Select(
                            (child) => child.GetAttributeExpressions(builder))),
                        builder);
                }
            }

            private class QueryColumnExpression
                : QueryColumn<QueryLayer>
            {
                public QueryColumnExpression(ExpressionType logicalForm)
                    : base(logicalForm)
                {
                    return;
                }

                public override QueryLayer Visit(
                    QueryTreeNode node,
                    ExpressionType operatorType)
                {
                    return Visit(CreateQueryRow(node, operatorType), operatorType);
                }

                public override QueryLayer Visit(
                    QueryLayer queryLayer,
                    ExpressionType logicalForm)
                {
                    if (LogicalForm == logicalForm)
                    {
                        AddChild(queryLayer);

                        return this;
                    }

                    return CreateQueryExpression(this, queryLayer, logicalForm);
                }

                private Expression MakeExpression(
                    TupleObjectFactory factory,
                    TupleObjectBuilder<TEntity> builder)
                {
                    IReadOnlyList<QueryLayer> children = Children;
                    Type toType = typeof(TupleObject<TEntity>);
                    Expression res = Expression.TypeAs(
                        Expression.Constant(
                            children[0].AsTupleObject(factory, builder)),
                        toType);
                    ExpressionType logicalForm = LogicalForm switch
                    {
                        ExpressionType.AndAlso => ExpressionType.And,
                        ExpressionType.OrElse => ExpressionType.Or
                    };

                    for (int i = 1; i < children.Count; i++)
                    {
                        res = Expression.MakeBinary(
                            logicalForm,
                            res,
                            Expression.TypeAs(
                                Expression.Constant(
                                    children[i].AsTupleObject(factory, builder)),
                            toType));
                    }

                    return res;
                }

                public override TupleObject<TEntity> AsTupleObject(
                    TupleObjectFactory factory,
                    TupleObjectBuilder<TEntity> builder = null)
                {
                    builder ??= factory.GetBuilder<TEntity>(PassSchema);

                    return factory.CreateQueriedComplexTupleObject(
                        MakeExpression(factory, builder),
                        builder);
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
                     * у параметра сущности, которая проверяется предикатом.
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

        #endregion
    }
}
