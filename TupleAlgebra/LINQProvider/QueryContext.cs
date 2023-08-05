using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace LINQProvider
{
    public class QueryContext
    {
        public object BuildSingleQueryExecutor(MethodCallExpression methodExpr)
        {
            int argc = methodExpr.Arguments.Count - 1;
            object[] arguments = new object[argc];
            if (argc > 0)
            {
                Expression argExpr;
                for (int i = 0; i < argc; i++)
                {
                    argExpr = methodExpr.Arguments[i + 1];
                    arguments[i] = argExpr switch
                    {
                        ConstantExpression cExpr => cExpr.Expand(),
                        UnaryExpression unaryExpr => unaryExpr.ExpandAsDelegate(),
                        _ => throw new NotImplementedException()
                    };
                }
            }

            return RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(methodExpr, arguments);
        }

        protected object RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
            MethodCallExpression methodExpr,
            params object[] methodParams)
        {
            MethodInfo queryBuildingMethod = FindQueryBuildingMethod(methodExpr, methodParams);

            return BuildSingleQueryExecutorImpl(queryBuildingMethod, methodParams);
        }

        protected virtual MethodInfo FindQueryBuildingMethod(
            MethodCallExpression methodExpr,
            object[] methodParams)
        {
            MethodInfo queryMethod = methodExpr.Method;
            string queryBuildingMethodName = "Build" + queryMethod.Name + "Query";

            return this.GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(method => method.Name == queryBuildingMethodName &&
                       method.GetParameters().Length == methodParams.Length)
                .Single()
                .MakeGenericMethod(methodExpr.Method.GetGenericArguments());
        }

        private object BuildSingleQueryExecutorImpl(
            MethodInfo queryBuildingMethod,
            params object[] queryBuildingMethodParams)
        {
            return queryBuildingMethod.Invoke(this, queryBuildingMethodParams)!;
        }

        protected bool CheckCompatibilityWithTarget(
            MethodInfo targetMethod, 
            object[] providedParams)
        {
            ParameterInfo[] declaredParams = targetMethod.GetParameters();

            if (declaredParams.Length != providedParams.Length) return false;

            Type providedParamType, declaredParamType;
            bool parametersMatched = true;
            
            for (int j = 0; j < declaredParams.Length; j++)
            {
                providedParamType = providedParams[j].GetType();
                declaredParamType = declaredParams[j].ParameterType;

                parametersMatched = providedParamType.IsAssignableTo(declaredParamType);

                if (!parametersMatched) break;
            }

            return parametersMatched;
        }
    }
}
