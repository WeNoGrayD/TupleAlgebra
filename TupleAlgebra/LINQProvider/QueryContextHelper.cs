using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniversalClassLib;

namespace LINQProvider
{
    public class QueryContextHelper : ITypeHierarchyRegistrar<QueryContextHelper>
    {
        #region Static fields

        private ITypeHierarchyRegistrar<QueryContextHelper> _registrar { get => this; }

        private static IDictionary<
            Type,
            IDictionary<string, IList<MethodInfo>>> _queryMethodGroups;

        #endregion

        #region Static properties

        public static QueryContextHelper Helper { get; }

        #endregion

        #region Constructors

        static QueryContextHelper()
        {
            Helper = new QueryContextHelper();
            _queryMethodGroups = new Dictionary<Type, IDictionary<string, IList<MethodInfo>>>();

            return;
        }

        private QueryContextHelper()
        {
            _registrar.Init();

            return;
        }

        #endregion

        #region Instance methods

        #region Type registration methods

        /// <summary>
        /// Регистрация типа.
        /// </summary>
        /// <typeparam name="TQueryContext"></typeparam>
        /// <param name="queryMethodOverloads"></param>
        public void RegisterType<TQueryContext>(
            IDictionary<string, IList<MethodInfo>> queryMethodGroups = null!)
            where TQueryContext : QueryContext
        {
            Type qcType = typeof(TQueryContext);

            // Если тип зарегистрирован, то дальнейшая регистрация не производится.
            if (_registrar.IsTypeRegistered(qcType)) return;
            // Обязательно выполняется принудительная регистрация базового типа.
            _registrar.RegisterTypeWithForce(qcType.BaseType!);

            RegisterQueryMethodOverloads(qcType, queryMethodGroups);

            return;
        }

        #endregion

        #region Type static variables registration methods

        private void RegisterQueryMethodOverloads(
            Type qcType,
            IDictionary<string, IList<MethodInfo>> queryMethodGroups)
        {
            if (_queryMethodGroups.ContainsKey(qcType.BaseType!))
            {
                IDictionary<string, IList<MethodInfo>> baseTypeQueryMethodGroups =
                    _queryMethodGroups[qcType.BaseType!];

                if (queryMethodGroups is null)
                    queryMethodGroups = baseTypeQueryMethodGroups;
                else 
                    AddBaseTypeMethodOverloads(baseTypeQueryMethodGroups);
            }

            if (queryMethodGroups is null)
            {
                throw new ArgumentNullException("queryMethodOverloads",
                    $"Не предоставлен список перегрузок методов для контекста запросов типа {qcType.Name}.");
            }

            _queryMethodGroups.Add(qcType, queryMethodGroups);

            return;

            void AddBaseTypeMethodOverloads(
                IDictionary<string, IList<MethodInfo>> baseTypeQueryMethodGroups)
            {
                IList<MethodInfo> queryMethodOverloads,
                                  baseTypeQueryMethodOverloads;

                foreach (string queryName in baseTypeQueryMethodGroups.Keys)
                {
                    baseTypeQueryMethodOverloads = baseTypeQueryMethodGroups[queryName];

                    /*
                     * Если группы методов с таким названием не существует в предоставленном
                     * словаре групп методов, то она добавляется в него целиком.
                     */
                    if (!queryMethodGroups.ContainsKey(queryName))
                    {
                        queryMethodGroups.Add(queryName, baseTypeQueryMethodOverloads);

                        continue;
                    }

                    /*
                     * В противном случае ищутся такие перегрузки метода в базовом классе,
                     * которые отсутствуют в текущей группе методов из предоставленного словаря.
                     */
                    queryMethodOverloads = queryMethodGroups[queryName];
                    foreach (MethodInfo baseTypeQueryMethodOverload in baseTypeQueryMethodOverloads)
                    {
                        if (queryMethodOverloads.FirstOrDefault(overload =>
                                overload.GetBaseDefinition() == baseTypeQueryMethodOverload)
                            is null)
                        {
                            queryMethodOverloads.Add(baseTypeQueryMethodOverload);
                        }
                    }
                }

                return;
            }
        }

        #endregion

        #region Type static variables getters

        /// <summary>
        /// Получени списка перегрузок метода с заданным именем.
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        public IList<MethodInfo> GetQueryMethodOverloads(QueryContext context, string queryName)
        {
            Type qcType = context.GetType();

            if (!_registrar.IsTypeRegistered(qcType))
            {
                _registrar.RegisterTypeWithForce(qcType);
                if (!_queryMethodGroups.ContainsKey(qcType))
                {
                    throw new InvalidOperationException($"Тип {qcType} не выполнил свою регистрацию в классе-помощнике.");
                }
            }

            return _queryMethodGroups[qcType][queryName];
        }

        #endregion

        #endregion
    }
}
