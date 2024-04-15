using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public abstract class AttributeGetterExpressionVisitor : ExpressionVisitor
    {
        protected abstract void VisitImpl(Expression expr);

        public override Expression Visit(Expression expr)
        {
            VisitImpl(expr);

            return base.Visit(expr);
        }

        protected abstract void VisitLambdaImpl(Expression expr);

        protected override Expression VisitLambda(LambdaExpression expr)
        {
            VisitLambdaImpl(expr);

            return base.Visit(expr.Body);
        }

        protected abstract void VisitMemberImpl(Expression expr);

        protected override Expression VisitMember(MemberExpression expr)
        {
            VisitMemberImpl(expr);

            return expr;
        }
    }

    public class AttributeGetterExpressionInspector : AttributeGetterExpressionVisitor
    {
        private INodeTreeInspector<Expression> _inspector;

        private ParameterExpression _instanceParameter;

        public PropertyInfo AttributeProperty { get; private set; }

        public void Inspect<TEntity, TData>(
            Expression<AttributeGetterHandler<TEntity, TData>> attributeGetterExpr)
        {
            _inspector = new NodeTreeInspector<Expression>(InspectAttributeGetterExpr());
            Visit(attributeGetterExpr);
            _inspector.InspectionEndedOk();

            return;
        }

        protected override void VisitImpl(Expression expr)
        {
            _inspector.Inspect(expr);

            return;
        }

        protected override void VisitLambdaImpl(LambdaExpression expr)
        {
            _inspector.Inspect(expr);
            _instanceParameter = expr.Parameters[0];

            return;
        }

        protected override void VisitMemberImpl(MemberExpression expr)
        {
            _inspector.Inspect(new NodeWithMemory<Expression>(expr, _instanceParameter));
            AttributeProperty = (expr.Member as PropertyInfo)!;

            return;
        }

        private static IEnumerable<IInspectionRule<Expression>> InspectAttributeGetterExpr<TMember>()
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
                    return e is MemberExpression me &&
                        me.Expression == e.Memory &&
                        me.Member is PropertyInfo pi &&
                        pi.PropertyType == TMember;
                },
                onFailure);

            yield break;
        }
    }

    public class AttributePropertyExtractor : AttributeGetterExpressionVisitor
    {
        public PropertyInfo _attributeProperty;

        public PropertyInfo Extract<TEntity, TData>(
            Expression<AttributeGetterHandler<TEntity, TData>> attributeGetterExpr)
        {
            Visit(attributeGetterExpr);

            return _attributeProperty;
        }

        protected override void VisitImpl(Expression expr)
        { }

        protected override void VisitLambdaImpl(LambdaExpression expr)
        { }

        protected override void VisitMemberImpl(MemberExpression expr)
        {
            _attributeProperty = (expr.Member as PropertyInfo)!;

            return;
        }
    }
}
