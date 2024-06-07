using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace LINQProvider
{
    using static QueryContextHelper;

    /// <summary>
    /// Контекст запросов к IQueryable-объекту.
    /// </summary>
    public class QueryContext
    {
        #region Instance methods

        protected static IDictionary<string, IList<MethodInfo>> GetQueryMethodPatterns<TQueryContext>(
            Regex queryBuildingMethodNamePattern)
            where TQueryContext : QueryContext
        {
            IDictionary<string, IList<MethodInfo>> queryMethodPatterns =
                new Dictionary<string, IList<MethodInfo>>();

            MethodInfo[] queryContextMethods = typeof(TQueryContext)
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo queryMethod;
            Match queryMethodMatch;
            string queryName;

            for (int i = 0; i < queryContextMethods.Length; i++)
            {
                queryMethod = queryContextMethods[i];
                queryMethodMatch = queryBuildingMethodNamePattern.Match(queryMethod.Name);
                if (!queryMethodMatch.Success) continue;

                queryName = queryMethodMatch.Groups["Query"].Captures[0].Value;
                if (!queryMethodPatterns.ContainsKey(queryName))
                    queryMethodPatterns[queryName] = new List<MethodInfo>(1);
                queryMethodPatterns[queryName].Add(queryMethod);
            }

            return queryMethodPatterns;
        }

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

            return RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
                methodExpr, arguments);
        }

        protected object RecognizeUnderlyingExpressionAndBuildSingleQueryExecutor(
            MethodCallExpression methodExpr,
            params object[] methodParams)
        {
            MethodInfo queryBuildingMethod = FindQueryBuildingMethod(methodExpr, methodParams);

            return BuildSingleQueryExecutorImpl(queryBuildingMethod, methodParams);
        }

        protected MethodInfo FindQueryBuildingMethod(
            MethodCallExpression methodExpr,
            object[] methodParams)
        {
            MethodInfo queryMethod = methodExpr.Method;
            IList<MethodInfo> matchedMethods = Helper.GetQueryMethodOverloads(this, queryMethod.Name);
            MethodInfo targetMethod = null!;

            for (int i = 0; i < matchedMethods.Count; i++)
            {
                targetMethod = matchedMethods[i].MakeGenericMethod(methodExpr.Method.GetGenericArguments());

                if (CheckCompatibilityWithTarget(targetMethod, methodParams)) break;
            }

            return targetMethod!;
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

        #endregion
    }
}
