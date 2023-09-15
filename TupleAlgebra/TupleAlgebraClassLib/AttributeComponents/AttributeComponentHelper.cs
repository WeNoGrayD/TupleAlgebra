using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Reflection;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using UniversalClassLib;

namespace TupleAlgebraClassLib.AttributeComponents
{
    /// <summary>
    /// Класс для хранения статических переменных типов компонент атрибутов.
    /// </summary>
    public class AttributeComponentHelper : ITypeHierarchyRegistrar<AttributeComponentHelper>
    {
        #region Static fields

        private ITypeHierarchyRegistrar<AttributeComponentHelper> _registrar { get => this; }

        /// <summary>
        /// Фабрики специфических компонент атрибутов.
        /// </summary>
        private static IDictionary<Type, AttributeComponentFactory> _factories;

        /// <summary>
        /// Контейнеры операций над компонентами атрибутов.
        /// </summary>
        private static IDictionary<Type, object> _setOperationContainers;

        #endregion

        #region Static properties

        public static AttributeComponentHelper Helper { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static AttributeComponentHelper()
        {
            Helper = new AttributeComponentHelper();
            _factories = new Dictionary<Type, AttributeComponentFactory>();
            _setOperationContainers = new Dictionary<Type, object>();

            return;
        }

        private AttributeComponentHelper()
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
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TAttributeComponent"></typeparam>
        /// <param name="considerAsNonGeneric"></param>
        /// <param name="acFactory"></param>
        /// <param name="setOperations"></param>
        public void RegisterType<TData, TAttributeComponent>(
            bool considerAsNonGeneric = false,
            AttributeComponentFactory acFactory = null,
            ISetOperationExecutorsContainer<AttributeComponent<TData>>
                setOperations = null)
            where TAttributeComponent : AttributeComponent<TData>
        {
            Type acType = typeof(TAttributeComponent);

            if (acType.IsGenericType && !considerAsNonGeneric)
            {
                RegisterGenericType(acType, acFactory, setOperations);
            }
            else
            {
                RegisterNonGenericType(acType, acFactory, setOperations);
            }

            return;
        }

        /// <summary>
        /// Регистрация обобщённого типа.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="acType"></param>
        /// <param name="acFactory"></param>
        /// <param name="setOperations"></param>
        private void RegisterGenericType<TData>(
            Type acType,
            AttributeComponentFactory acFactory,
            ISetOperationExecutorsContainer<AttributeComponent<TData>>
                setOperations)
        {
            // Если тип зарегистрирован, то дальнейшая регистрация не производится.
            if (_registrar.IsTypeRegistered(acType)) return;
            Type acGenericTypeDefinition = acType.GetGenericTypeDefinition();
            // Обязательно выполняется принудительная регистрация базового типа.
            _registrar.RegisterTypeWithForce(acType.BaseType);
            RegisterFactoryForType(acGenericTypeDefinition, acFactory);
            RegisterSetOperationsForType(acType, setOperations);

            _registrar.RegisterType(acGenericTypeDefinition);
            _registrar.RegisterType(acType);

            return;
        }

        /// <summary>
        /// Регистрация необобщённого типа.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="acType"></param>
        /// <param name="acFactory"></param>
        /// <param name="setOperations"></param>
        private void RegisterNonGenericType<TData>(
            Type acType,
            AttributeComponentFactory acFactory,
            ISetOperationExecutorsContainer<AttributeComponent<TData>>
                setOperations)
        {
            // Если тип зарегистрирован, то дальнейшая регистрация не производится.
            if (_registrar.IsTypeRegistered(acType)) return;
            // Обязательно выполняется принудительная регистрация базового типа.
            _registrar.RegisterTypeWithForce(acType.BaseType);
            RegisterFactoryForType(acType, acFactory);
            RegisterSetOperationsForType(acType, setOperations);

            _registrar.RegisterType(acType);

            return;
        }

        #endregion

        #region Type static variables registration methods

        /// <summary>
        /// Регистрация фабрики для типа.
        /// </summary>
        /// <param name="acType"></param>
        /// <param name="acFactory"></param>
        private void RegisterFactoryForType(
            Type acType,
            AttributeComponentFactory acFactory)
        {
            // Если зарегистрировано определение типа, то дальнейшая регистрация не производится.
            if (_registrar.IsTypeRegistered(acType)) return;
            /*
             * Если объект фабрики не предоставлен, то подходящий объект ищется
             * в иерархии классов нужного типа.
             */
            if (acFactory is null)
            {
                _registrar.FindRelevantTypeAncestorAndExecute(
                    acType.BaseType,
                    _registrar.GetDefinition,
                    (t) => _factories.ContainsKey(t),
                    (registeredType) => _factories.Add(acType, _factories[registeredType]),
                    new ArgumentNullException(
                        "acFactory", 
                        "Фабрика компоненты должна быть предоставлена для типа " +
                        $"{acType.Name} или быть определена ранее для одного из его базовых типов."));
            }
            else _factories.Add(acType, acFactory);

            return;
        }

        /// <summary>
        /// Регистрация контейнера операций над типом компоненты атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="acType"></param>
        /// <param name="setOperations"></param>
        private void RegisterSetOperationsForType<TData>(
            Type acType,
            ISetOperationExecutorsContainer<AttributeComponent<TData>>
                setOperations)
        {
            /*
            if (setOperations is not null)
            {
                _setOperationContainers.Add(acType, setOperations);
            }
            */
            /*
             * Если объект контейнера операций не предоставлен, то подходящий объект ищется
             * в иерархии классов нужного типа.
             */
            if (setOperations is null)
            {
                _registrar.FindRelevantTypeAncestorAndExecute(
                    acType.BaseType,
                    (t) => t,
                    (t) => _setOperationContainers.ContainsKey(t),
                    (registeredType) => _setOperationContainers
                        .Add(acType, _setOperationContainers[registeredType]),
                    null);// new ArgumentNullException(
                        //"setOperations",
                        //"Контейнер операций над компонентой должен быть предоставлен для типа " +
                        //$"{acType.Name} или быть определен ранее для одного из его базовых типов."));
            }
            else _setOperationContainers.Add(acType, setOperations);

            return;
        }

        #endregion

        #region Type static variables getters

        /// <summary>
        /// Получени фабрики компоненты атрибута.
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        public AttributeComponentFactory GetFactory(object ac)
        {
            return GetFactory(ac.GetType());
        }

        /// <summary>
        /// Получени фабрики компоненты атрибута.
        /// </summary>
        /// <param name="acType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public AttributeComponentFactory GetFactory(Type acType)
        {
            Type acTypeDefinition = _registrar.GetDefinition(acType);
            if (!_registrar.IsTypeRegistered(acTypeDefinition))
            {
                _registrar.RegisterTypeWithForce(acType);
                if (!_factories.ContainsKey(acTypeDefinition))
                {
                    throw new InvalidOperationException($"Тип {acType} не выполнил свою регистрацию в классе-помощнике.");
                }
            }

            return _factories[acTypeDefinition];
        }

        /// <summary>
        /// Полученик контейнера операций над компонентой атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="ac"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public ISetOperationExecutorsContainer<AttributeComponent<TData>>
            GetSetOperations<TData>(object ac)
        {
            Type acType = ac.GetType();

            if (!_registrar.IsTypeRegistered(acType))
            {
                _registrar.RegisterTypeWithForce(acType);
                if (!_setOperationContainers.ContainsKey(acType))
                {
                    throw new InvalidOperationException($"Тип {acType} не выполнил свою регистрацию в классе-помощнике.");
                }
            }

            return (_setOperationContainers[acType] as ISetOperationExecutorsContainer<AttributeComponent<TData>>)!;
        }

        #endregion

        #endregion
    }
}
