using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UniversalClassLib.ExpressionVisitors
{
    public class LambdaExpressionParameterAligner
        : ExpressionVisitor
    {
        private LambdaExpression[] _attributeGetters;

        private ParameterExpression _entityParameterExpr;

        public ParameterExpression EntityParameterExpr { get => _entityParameterExpr; }

        public LambdaExpressionParameterAligner(
            params LambdaExpression[] attributeGetters)
        {
            AttributeGetterExpressionInspector inspector = new();
            foreach (LambdaExpression attrGetter in attributeGetters)
            {
                inspector.Inspect(attrGetter);
                inspector.Reset();
            }

            _attributeGetters = attributeGetters;
            
            return;
        }

        public MemberExpression[] Align()
        {
            _entityParameterExpr = null;

            if (_attributeGetters.Length == 0)
            {
                return [];
            }

            _entityParameterExpr = _attributeGetters[0].Parameters[0];

            return AlignImpl().ToArray();

            IEnumerable<MemberExpression> AlignImpl()
            {
                yield return (_attributeGetters[0].Body as MemberExpression)!;

                for (int i = 1; i < _attributeGetters.Length; i++)
                {
                    yield return ((Visit(_attributeGetters[i]) as LambdaExpression)!
                        .Body as MemberExpression)!;
                }

                yield break;
            }
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _entityParameterExpr;
        }
    }

    public static class LambdaExpressionHelper
    {
        public static Expression<Func<TEntity, KeyValuePair<TKey, TValue>>>
            ProduceKeyValuePairGetter<TEntity, TKey, TValue>(
            params LambdaExpression[] attributeGetters)
        {
            LambdaExpressionParameterAligner aligner = new(attributeGetters);
            MemberExpression[] attributeMembers = aligner.Align();
            ParameterExpression entityParameter = aligner.EntityParameterExpr;
            Expression ctorExpr = Expression.New(
                typeof(KeyValuePair<TKey, TValue>)
                .GetConstructor([typeof(TKey), typeof(TValue)]),
                attributeMembers);
            var resExpr = Expression.Lambda<Func<TEntity, KeyValuePair<TKey, TValue>>>(
                    ctorExpr, entityParameter);

            return resExpr;
        }

        public static Expression<Func<TEntity, KeyValuePair<TKey, TValue>>>
            ProduceKeyValuePairGetterWithAutoFill<TEntity, TKey, TValue>(
            Func<TValue, TKey> principalKeySelector,
            Func<TKey, TValue> navigationByKey,
            params LambdaExpression[] attributeGetters)
        {
            LambdaExpressionParameterAligner aligner = new(attributeGetters);
            MemberExpression[] attributeMembers = aligner.Align();
            ParameterExpression entityParameter = aligner.EntityParameterExpr;
            Expression ctorExpr = Expression.New(
                typeof(KeyValuePair<TKey, TValue>)
                .GetConstructor([typeof(TKey), typeof(TValue)]),
                attributeMembers);
            var resExpr = Expression.Lambda<Func<TEntity, KeyValuePair<TKey, TValue>>>(
                    ctorExpr, entityParameter);

            return resExpr;
        }
    }
}