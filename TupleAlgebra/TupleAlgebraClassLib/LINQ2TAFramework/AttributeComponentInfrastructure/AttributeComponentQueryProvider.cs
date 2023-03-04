using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Diagnostics;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Reflection;

namespace TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure
{
    public abstract class AttributeComponentQueryProvider : QueryProvider
    {
        public override IQueryable<TData> CreateQuery<TData>(Expression queryExpression)
        {
            AttributeComponent<TData> queryableAC = 
                new DataSourceExtractor<AttributeComponent<TData>>().Extract(queryExpression);

            return CreateQuery(queryExpression, queryableAC);
        }

        public IQueryable<TData> CreateQuery<TData>(
            Expression queryExpression,
            AttributeComponent<TData> queryableAC)
        {
            /*
               Проверка выражения запроса на избыточность (а также корректность). 
               Если оно избыточно, то экземпляр MethodCallInspector вернёт
               неизбыточное выражение, равное константному выражению источника данных для запроса.
             */
            if (queryExpression != new QueryInspector().Visit(queryExpression))
                return queryableAC;

            AttributeComponentFactoryArgs<TData> factoryArgs = queryableAC.ZipInfo(null);
            factoryArgs.QueryExpression = queryExpression;

            return queryableAC.Reproduce(factoryArgs);
        }

        protected class AttributeComponentQueryInspector : QueryInspector
        {
            protected override void CheckSelectQueryOnAcceptability(MethodCallExpression selectExpression)
            {
                if (selectExpression.Arguments[1].NodeType != ExpressionType.Quote)
                    throw new InvalidOperationException(
                        $"Выражение {selectExpression.ToString()} недопустимо:\n" +
                        "выражение вида AttributeComponent.Select может содержать только" +
                        "проекцию вида { (el) => el }.");
            }
        }
    }
}
