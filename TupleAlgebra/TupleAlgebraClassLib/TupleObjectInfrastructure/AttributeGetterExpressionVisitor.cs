using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using UniversalClassLib;
using System.Collections;
using UniversalClassLib.ExpressionVisitors;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public class AttributeMemberExtractor : AttributeGetterExpressionVisitor
    {
        private List<MemberInfo> _members;

        public MemberInfo ExtractFrom<TEntity, TData>(
            Expression<Func<TEntity, TData>> attributeGetterExpr)
        {
            if (_members is null)
                _members = new();
            else
                _members.Clear();

            Visit(attributeGetterExpr);

            return _members.Single();
            /*
            Expression attributeMemberValueExpr =
                Visit(attributeGetterExpr);

            return attributeMemberValueExpr switch
            {
                ConstantExpression ce => (ce.Value as MemberInfo)!,
                ParameterExpression pe => (pe.Type).GetTypeInfo()
            };
            */
        }

        public MemberInfo ExtractFrom(
            LambdaExpression attributeGetterExpr)
        {
            if (_members is null)
                _members = new();
            else
                _members.Clear();

            Visit(attributeGetterExpr);

            return _members.Single();
            /*
            Expression attributeMemberValueExpr =
                Visit(attributeGetterExpr);

            return attributeMemberValueExpr switch
            {
                ConstantExpression ce => (ce.Value as MemberInfo)!,
                ParameterExpression pe => (pe.Type).GetTypeInfo()
            };
            */
        }

        public IReadOnlyList<MemberInfo> ExtractManyFrom<TEntity, TData>(
            Expression<Func<TEntity, TData>> attributeGetterExpr)
        {
            if (_members is null)
                _members = new();
            else
                _members.Clear();

            Visit(attributeGetterExpr);

            return _members;
        }

        /*
        protected override Expression VisitNew(NewExpression expr)
        {
            return expr;
        }

        protected override Expression VisitParameter(ParameterExpression expr)
        {
            return expr;
        }
        */

        protected override Expression VisitMember(MemberExpression expr)
        {
            //return Expression.Constant(expr.Member);
            _members.Add(expr.Member);

            return expr;
        }
    }
}
