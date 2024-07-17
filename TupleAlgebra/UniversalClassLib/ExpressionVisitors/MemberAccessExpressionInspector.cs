using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UniversalClassLib.ExpressionVisitors
{
    public abstract class AttributeGetterExpressionVisitor : ExpressionVisitor
    {
        protected override Expression VisitLambda<T>(Expression<T> expr)
        {
            return base.Visit(expr.Body);
        }
    }

    public class AttributeGetterExpressionInspector<TEntity, TMember>
        : AttributeGetterExpressionVisitor
    {
        private INodeTreeInspector _inspector;

        private ParameterExpression _instanceParameter;

        public void Inspect(
            Expression<Func<TEntity, TMember>> attributeGetterExpr)
        {
            _inspector = new NodeTreeInspector<Expression>(
                "Инспектор выражения (x) => x.y",
                InspectAttributeGetterExpr().ToArray());
            Visit(attributeGetterExpr);
            _inspector.InspectionEndedOk();

            return;
        }

        public override Expression Visit(Expression expr)
        {
            VisitImpl(expr);

            return base.Visit(expr);
        }

        protected override Expression VisitLambda<T>(Expression<T> expr)
        {
            VisitLambdaImpl(expr);

            return base.Visit(expr.Body);
        }

        protected override Expression VisitParameter(ParameterExpression expr)
        {
            VisitParameterImpl(expr);

            return expr;
        }

        protected override Expression VisitMember(MemberExpression expr)
        {
            VisitMemberImpl(expr);

            return expr;
        }

        protected void VisitImpl(Expression expr)
        {
            _inspector.InspectNode(expr);

            return;
        }

        protected void VisitLambdaImpl(LambdaExpression expr)
        {
            _inspector.InspectNode(expr);
            _instanceParameter = expr.Parameters[0];

            return;
        }

        protected void VisitParameterImpl(ParameterExpression expr)
        {
            _inspector.InspectNode(
                NodeWithMemory<Expression>.Create(expr, _instanceParameter));

            return;
        }

        protected void VisitMemberImpl(MemberExpression expr)
        {
            _inspector.InspectNode(
                NodeWithMemory<Expression>.Create(expr, _instanceParameter));

            return;
        }

        private static IEnumerable<IInspectionRule<Expression>>
            InspectAttributeGetterExpr()
        {
            IInspectionRule<Expression> nop = new SimpleNodeInspectionRule<Expression>(
                (e) => true,
                null);

            yield return nop;

            Exception onFailure =
                new Exception("Выражение получения атрибута должно являться лямбда-выражением и соответствовать форме\ngetter = (d) => d.Property");

            yield return new SimpleNodeInspectionRule<Expression>(
                (e) =>
                {
                    return e is LambdaExpression le &&
                        le.Parameters.Count == 1;
                },
                onFailure);

            yield return nop;

            yield return new NodeWithMemoryInspectionRule<Expression>(
                (e) =>
                {
                    return (e.Node is MemberExpression me &&
                        me.Expression == e.Memory &&
                        me.Member switch
                        {
                            FieldInfo fi => fi.FieldType == typeof(TMember),
                            PropertyInfo pi => pi.PropertyType == typeof(TMember),
                            _ => false
                        }) ||
                        (e.Node is ParameterExpression pe &&
                         pe == e.Memory);
                },
                onFailure);

            yield break;
        }
    }

    public class AttributeGetterExpressionInspector 
        : AttributeGetterExpressionVisitor
    {
        private INodeTreeInspector _inspector;

        private ParameterExpression _instanceParameter;

        public AttributeGetterExpressionInspector()
        {
            _inspector = new NodeTreeInspector<Expression>(
                "Инспектор выражения (x) => x.y",
                InspectAttributeGetterExpr().ToArray());

            return;
        }

        public void Reset()
        {
            _inspector.Reset();

            return;
        }

        public void Inspect(LambdaExpression attributeGetterExpr)
        {
            Visit(attributeGetterExpr);
            _inspector.InspectionEndedOk();

            return;
        }

        /*
        public override Expression Visit(Expression expr)
        {
            VisitImpl(expr);

            return base.Visit(expr);
        }
        */

        protected override Expression VisitLambda<T>(Expression<T> expr)
        {
            VisitLambdaImpl(expr);

            return Visit(expr.Body);
        }

        protected override Expression VisitParameter(ParameterExpression expr)
        {
            VisitParameterImpl(expr);

            return expr;
        }

        protected override Expression VisitMember(MemberExpression expr)
        {
            VisitMemberImpl(expr);

            return expr;
        }

        protected void VisitImpl(Expression expr)
        {
            //_inspector.InspectNode(expr);

            return;
        }

        protected void VisitLambdaImpl(LambdaExpression expr)
        {
            _inspector.InspectNode(expr);
            _instanceParameter = expr.Parameters[0];

            return;
        }

        protected void VisitParameterImpl(ParameterExpression expr)
        {
            _inspector.InspectNode(
                NodeWithMemory<Expression>.Create(expr, _instanceParameter));

            return;
        }

        protected void VisitMemberImpl(MemberExpression expr)
        {
            _inspector.InspectNode(
                NodeWithMemory<Expression>.Create(expr, _instanceParameter));

            return;
        }

        private static IEnumerable<IInspectionRule<Expression>>
            InspectAttributeGetterExpr()
        {
            IInspectionRule<Expression> nop = new SimpleNodeInspectionRule<Expression>(
                (e) => true,
                null);

            //yield return nop;

            Exception onFailure =
                new Exception("Выражение получения атрибута должно являться лямбда-выражением и соответствовать форме\ngetter = (d) => d.Property");

            yield return new SimpleNodeInspectionRule<Expression>(
                (e) =>
                {
                    return e is LambdaExpression le &&
                        le.Parameters.Count == 1;
                },
                onFailure);

            //yield return nop;

            yield return new NodeWithMemoryInspectionRule<Expression>(
                (e) =>
                {
                    return (e.Node is MemberExpression me &&
                        me.Expression == e.Memory) ||
                        (e.Node is ParameterExpression pe &&
                         pe == e.Memory);
                },
                onFailure);

            yield break;
        }
    }
}
