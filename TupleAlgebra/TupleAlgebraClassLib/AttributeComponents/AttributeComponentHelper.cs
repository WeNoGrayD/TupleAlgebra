using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Reflection;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using UniversalClassLib;
using static TupleAlgebraClassLib.AttributeComponents.AttributeComponentHelper;
using System.Reflection.Metadata;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Navigational;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace TupleAlgebraClassLib.AttributeComponents
{
    /// <summary>
    /// Класс для хранения статических переменных типов компонент атрибутов.
    /// </summary>
    public class AttributeComponentHelper 
        : ITypeHierarchyRegistrar<AttributeComponentHelper>
    {
        #region Static fields

        private ITypeHierarchyRegistrar<AttributeComponentHelper> _registrar 
        { get => this; }

        /// <summary>
        /// Фабрики специфических компонент атрибутов.
        /// </summary>
        private static IDictionary<Type, IAttributeDomainManifold> 
            _nodes;

        #endregion

        #region Static properties

        public static AttributeComponentHelper Helper { get; set; }

        #endregion

        #region Delegates

        public delegate IAttributeComponentFactory<TData>
            AttributeComponentFactoryCreationHandler<TData>(
                AttributeDomain<TData> domain);

        public delegate ISetOperationExecutorsContainer<IAttributeComponent<TData>>
            AttributeComponentSetOperationsCreationHandler<TData>(
                IAttributeComponentFactory<TData> factory);

        internal delegate AttributeComponent<TResult> NavigationHandler<
            TBy, 
            TResult>(
            AttributeComponent<TBy> key);

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static AttributeComponentHelper()
        {
            Helper = new AttributeComponentHelper();
            _nodes = new Dictionary<Type, IAttributeDomainManifold>();

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
        private void RegisterTypeImpl<TData, TAttributeComponent>(
            bool considerAsNonGeneric = false,
            AttributeComponentFactoryCreationHandler<TData>
                acFactory = null,
            AttributeComponentSetOperationsCreationHandler<TData>
                setOperations = null,
            params (object Data, Action<object> Registration)[] additionalParamsToRegister)
            where TAttributeComponent : IAttributeComponent<TData>
        {
            Type acType = typeof(TAttributeComponent);
            // Если тип зарегистрирован, то дальнейшая регистрация не производится.
            if (_registrar.IsTypeRegistered(acType)) return;

            AttributeDomainManifold<TData> manifold =
                new AttributeDomainManifold<TData>();
            _nodes.Add(acType, manifold);

            if (acType.IsGenericType && !considerAsNonGeneric)
            {
                RegisterGenericType(
                    acType,
                    manifold,
                    acFactory,
                    setOperations);
            }
            else
            {
                RegisterNonGenericType(
                    acType,
                    manifold,
                    acFactory,
                    setOperations);
            }

            if (additionalParamsToRegister.Length > 0)
            {
                foreach (var p in additionalParamsToRegister)
                    p.Registration(p.Data);
            }

            return;
        }

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
            AttributeComponentFactoryCreationHandler<TData> 
                acFactory = null,
            AttributeComponentSetOperationsCreationHandler<TData>
                setOperations = null)
            where TAttributeComponent : IAttributeComponent<TData>
        {
            RegisterTypeImpl<TData, TAttributeComponent>(
                considerAsNonGeneric, 
                acFactory, 
                setOperations);

            return;
        }

        internal void RegisterNavigational<
            TKey,
            TData,
            TAttributeComponent>(
            IAttributeComponentFactory<KeyValuePair<TKey, TData>> factory,
            NavigationHandler<TKey, TData> navigationByKey,
            Func<TData, TKey> principalKeySelector,
            NavigationHandler<TData, TKey> selectKey)
            where TKey : new()
            where TData : new()
            where TAttributeComponent : NavigationalAttributeComponent<TKey, TData>
        {
            Type registeredType = typeof(TAttributeComponent);
            _registrar.RegisterTypeWithForce(registeredType);
            _nodes[registeredType].RegisterNodeForNavigational(
                factory,
                navigationByKey,
                principalKeySelector,
                selectKey);

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
            AttributeDomainManifold<TData> manifold,
            AttributeComponentFactoryCreationHandler<TData>
                acFactory,
            AttributeComponentSetOperationsCreationHandler<TData>
                setOperations)
        {
            // Если тип зарегистрирован, то дальнейшая регистрация не производится.
            if (_registrar.IsTypeRegistered(acType)) return;
            //Type acGenericTypeDefinition = acType.GetGenericTypeDefinition();
            // Обязательно выполняется принудительная регистрация базового типа.
            _registrar.RegisterTypeWithForce(acType.BaseType);
            RegisterFactoryForType(acType, manifold, acFactory);
            RegisterSetOperationsForType(acType, manifold, setOperations);

            //_registrar.RegisterType(acGenericTypeDefinition);
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
            AttributeDomainManifold<TData> manifold,
            AttributeComponentFactoryCreationHandler<TData> 
                acFactory,
            AttributeComponentSetOperationsCreationHandler<TData>
                setOperations)
        {
            // Обязательно выполняется принудительная регистрация базового типа.
            _registrar.RegisterTypeWithForce(acType.BaseType);
            RegisterFactoryForType(acType, manifold, acFactory);
            RegisterSetOperationsForType(acType, manifold, setOperations);

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
        private void RegisterFactoryForType<TData>(
            Type acType,
            AttributeDomainManifold<TData> manifold,
            AttributeComponentFactoryCreationHandler<TData>
                acFactory)
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
                    (t) => t,
                    (t) => _nodes.ContainsKey(t),
                    (registeredType) => manifold.NodePattern
                        .ProvideFactorySetterSource(_nodes[registeredType].GetNodePattern<TData>()),
                    new ArgumentNullException(
                        nameof(acFactory), 
                        "Фабрика компоненты должна быть предоставлена для типа " +
                        $"{acType.Name} или быть определена ранее для одного из его базовых типов."));
            }
            else manifold.NodePattern.ProvideFactorySetter(acFactory);

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
            AttributeDomainManifold<TData> manifold,
            AttributeComponentSetOperationsCreationHandler<TData>
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
                    (t) => _nodes.ContainsKey(t),
                    (registeredType) => manifold.NodePattern
                        .ProvideSetOperationsSetterSource(
                            _nodes[registeredType].GetNodePattern<TData>()),
                    null);// new ArgumentNullException(
                        //"setOperations",
                        //"Контейнер операций над компонентой должен быть предоставлен для типа " +
                        //$"{acType.Name} или быть определен ранее для одного из его базовых типов."));
            }
            else manifold.NodePattern
                    .ProvideSetOperationsSetter(setOperations);

            return;
        }

        #endregion

        #region Type static variables getters

        /// <summary>
        /// Получени фабрики компоненты атрибута.
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        public IAttributeComponentFactory<TData> GetFactory<TData>(
            AttributeComponent<TData> ac)
        {
            return GetFactory<TData>(ac.GetType(), ac.Domain);
        }

        /// <summary>
        /// Получени фабрики компоненты атрибута.
        /// </summary>
        /// <param name="acType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IAttributeComponentFactory<TData> GetFactory<TData>(
            Type acType,
            AttributeDomain<TData> domain)
        {
            IAttributeComponentFactory<TData> acFactory;

            if (!_registrar.IsTypeRegistered(acType))
            {
                _registrar.RegisterTypeWithForce(acType);
                /*
                if (!_factories.TryGetValue(acTypeDefinition, out acFactory))
                {
                    throw new InvalidOperationException($"Тип {acType} не выполнил свою регистрацию в классе-помощнике.");
                }
                */
            }
            acFactory = _nodes[acType]
                .GetNode<TData>(domain).Factory.Value;

            return acFactory;
        }

        /// <summary>
        /// Полученик контейнера операций над компонентой атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="ac"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public ISetOperationExecutorsContainer<IAttributeComponent<TData>>
            GetSetOperations<TData>(
                AttributeComponent<TData> ac)
        {
            ISetOperationExecutorsContainer<IAttributeComponent<TData>> 
                setOpsContainer;

            Type acType = ac.GetType();
            if (!_registrar.IsTypeRegistered(acType))
            {
                _registrar.RegisterTypeWithForce(acType);
                /*
                if (!_setOperationContainers.TryGetValue(acType, out setOpsContainer))
                {
                    throw new InvalidOperationException($"Тип {acType} не выполнил свою регистрацию в классе-помощнике.");
                }
                */
            }
            setOpsContainer = _nodes[acType]
                .GetNode<TData>(ac.Domain).SetOperations.Value;

            return setOpsContainer;
        }

        internal NavigationHandler<TKey, TData>
            GetNavigationByKeyHandler<TKey, TData>(
            NavigationalAttributeComponent<TKey, TData> ac)
            where TKey : new()
            where TData : new()
        {
            NavigationHandler<TKey, TData> navigator;

            Type acType = ac.GetType();
            if (!_registrar.IsTypeRegistered(acType))
            {
                _registrar.RegisterTypeWithForce(acType);
            }
            navigator = (_nodes[acType].GetNode<KeyValuePair<TKey, TData>>(ac.Domain)
                as NavigationalAttributeComponentNode<TKey, TData>)
                .NavigateByKey.Value;

            return navigator;
        }

        internal Func<TData, TKey>
            GetPrincipleKeySelector<TKey, TData>(
            NavigationalAttributeComponent<TKey, TData> ac)
            where TKey : new()
            where TData : new()
        {
            Func<TData, TKey> principleKeySelector;

            Type acType = ac.GetType();
            if (!_registrar.IsTypeRegistered(acType))
            {
                _registrar.RegisterTypeWithForce(acType);
            }
            principleKeySelector = (_nodes[acType].GetNode<KeyValuePair<TKey, TData>>(ac.Domain)
                as NavigationalAttributeComponentNode<TKey, TData>)
                .PrincipalKeySelector.Value;

            return principleKeySelector;
        }

        internal NavigationHandler<TData, TKey>
            GetKeySelectionHandler<TKey, TData>(
            NavigationalAttributeComponent<TKey, TData> ac)
            where TKey : new()
            where TData : new()
        {
            NavigationHandler<TData, TKey> selector;

            Type acType = ac.GetType();
            if (!_registrar.IsTypeRegistered(acType))
            {
                _registrar.RegisterTypeWithForce(acType);
            }
            selector = (_nodes[acType].GetNode<KeyValuePair<TKey, TData>>(ac.Domain)
                as NavigationalAttributeComponentNode<TKey, TData>)
                .SelectKey.Value;

            return selector;
        }

        #endregion

        #endregion

        #region Nested types

        public interface IAttributeDomainManifold
        {
            AttributeComponentNodePattern<TData> GetNodePattern<TData>()
            {
                return (this as IAttributeDomainManifold<TData>)!
                    .NodePattern;
            }

            AttributeComponentNode<TData> GetNode<TData>(
                AttributeDomain<TData> domain)
            {
                return (this as IAttributeDomainManifold<TData>)![domain];
            }

            internal void RegisterNodeForNavigational<TKey, TValue>(
                IAttributeComponentFactory<KeyValuePair<TKey, TValue>> factory,
                NavigationHandler<TKey, TValue> navigateByKey,
                Func<TValue, TKey> principalKeySelector,
                NavigationHandler<TValue, TKey> selectKey)
                where TKey : new()
            {
                (this as IAttributeDomainManifold<KeyValuePair<TKey, TValue>>)!.
                    RegisterNodeForNavigational(
                    factory, 
                    navigateByKey,
                    principalKeySelector,
                    selectKey);

                return;
            }
        };

        public interface IAttributeDomainManifold<TData>
            : IAttributeDomainManifold
        {
            public AttributeComponentNodePattern<TData> NodePattern
            { get; }

            public AttributeComponentNode<TData>
                this[AttributeDomain<TData> domain] { get; }

            public void AddNode(
                AttributeDomain<TData> domain,
                AttributeComponentNode<TData> node);

            internal void RegisterNodeForNavigational<TKey, TValue>(
                IAttributeComponentFactory<KeyValuePair<TKey, TValue>> factory,
                NavigationHandler<TKey, TValue> navigateByKey,
                Func<TValue, TKey> principalKeySelector,
                NavigationHandler<TValue, TKey> selectKey)
                where TKey : new()
            {
                var navAttrDomain = factory.Domain;
                var node = new NavigationalAttributeComponentNode<TKey, TValue>(
                    factory);
                IAttributeDomainManifold<KeyValuePair<TKey, TValue>> kvpDomainManifold =
                    (this as IAttributeDomainManifold<KeyValuePair<TKey, TValue>>);
                kvpDomainManifold.AddNode(navAttrDomain, node);
                node.ProvideFactorySetter(
                    kvpDomainManifold.NodePattern.FactorySetter.Value);
                node.ProvideSetOperationsSetter(
                    kvpDomainManifold.NodePattern.SetOperationsSetter.Value);
                node.ProvideNavigationByKeyHandler(navigateByKey);
                node.ProvidePrincipalKeySelector(principalKeySelector);
                node.ProvideKeySelectionHandler(selectKey);

                return;
            }
        };

        public class AttributeDomainManifold<TData>
            : IAttributeDomainManifold<TData>
        {
            private AttributeComponentNodePattern<TData> _nodePattern;

            private IDictionary<
                AttributeDomain<TData>,
                AttributeComponentNode<TData>> _nodes;

            public AttributeComponentNodePattern<TData> NodePattern
            { get => _nodePattern; }

            public AttributeComponentNode<TData> 
                this[AttributeDomain<TData> domain]
            {
                get
                {
                    AttributeComponentNode<TData> node;
                    if (!_nodes.TryGetValue(domain, out node))
                        node = RegisterDomain(domain);

                    return node;
                }
            }

            public AttributeDomainManifold()
            {
                _nodePattern = new AttributeComponentNodePattern<TData>();
                _nodes = new Dictionary<
                    AttributeDomain<TData>, 
                    AttributeComponentNode<TData>>();

                return;
            }

            public AttributeComponentNode<TData> 
                RegisterDomain(AttributeDomain<TData> domain)
            {
                AttributeComponentNode<TData> node =
                    new AttributeComponentNode<TData>(domain);
                AddNode(domain, node);
                node.ProvideFactorySetter(
                    NodePattern.FactorySetter.Value);
                node.ProvideSetOperationsSetter(
                    NodePattern.SetOperationsSetter.Value);

                return node;
            }

            public void AddNode(
                AttributeDomain<TData> domain,
                AttributeComponentNode<TData> node)
            {
                _nodes.Add(domain, node);
                return;
            }
        }

        /*
        internal class NavigationalAttributeDomainManifold<TKey, TData>
            : AttributeDomainManifold<KeyValuePair<TKey, TData>>
            where TKey : new()
        {
            public NavigationalAttributeDomainManifold() : base()
            {
                return;
            }

            internal void RegisterNodeForNavigational(
                IAttributeComponentFactory<KeyValuePair<TKey, TData>> factory,
                NavigationHandler<TKey, TData> navigateByKey,
                Func<TData, TKey> principalKeySelector,
                NavigationHandler<TData, TKey> selectKey)
            {
                var navAttrDomain = factory.Domain;
                var node = new NavigationalAttributeComponentNode<TKey, TData>(
                    factory);
                AddNode(navAttrDomain, node);
                node.ProvideFactorySetter(
                    NodePattern.FactorySetter.Value);
                node.ProvideSetOperationsSetter(
                    NodePattern.SetOperationsSetter.Value);
                node.ProvideNavigationByKeyHandler(navigateByKey);
                node.ProvidePrincipalKeySelector(principalKeySelector);
                node.ProvideKeySelectionHandler(selectKey);

                return;
            }
        }
        */

        public class AttributeComponentNodePattern<TData>
            : TypeHierarchyNode<AttributeComponentNodePattern<TData>>
        {
            public PropertyNode<AttributeComponentFactoryCreationHandler<TData>>
                    FactorySetter
            { get; private set; }

            public PropertyNode<AttributeComponentSetOperationsCreationHandler<TData>>
                    SetOperationsSetter
            { get; private set; }

            public void ProvideFactorySetterSource(
                AttributeComponentNodePattern<TData> ancestorNode)
            {
                this.FactorySetter = InitProperty(
                    ancestorNode,
                    (node) => node.FactorySetter);

                return;
            }

            public void ProvideFactorySetter(
                AttributeComponentFactoryCreationHandler<TData>
                    factorySetter)
            {
                this.FactorySetter = InitProperty(factorySetter);

                return;
            }

            public void ProvideSetOperationsSetterSource(
                AttributeComponentNodePattern<TData> ancestorNode)
            {
                this.SetOperationsSetter = InitProperty(
                    ancestorNode,
                    (node) => node.SetOperationsSetter);

                return;
            }

            public void ProvideSetOperationsSetter(
                AttributeComponentSetOperationsCreationHandler<TData>
                    setOperationsSetter)
            {
                this.SetOperationsSetter = 
                    InitProperty(setOperationsSetter);

                return;
            }
        }

        public class AttributeComponentNode<TData>
            : TypeHierarchyNode<AttributeComponentNode<TData>>
        {
            private AttributeDomain<TData> _domain;

            public PropertyNode<IAttributeComponentFactory<TData>>
                Factory
            { get; protected set; }

            public PropertyNode<ISetOperationExecutorsContainer<IAttributeComponent<TData>>>
                SetOperations
            { get; protected set; }

            public AttributeComponentNode(AttributeDomain<TData> domain)
            {
                _domain = domain;

                return;
            }

            public void ProvideFactorySetter(
                AttributeComponentFactoryCreationHandler<TData>
                    factorySetter)
            {
                this.Factory = InitProperty(() => factorySetter(_domain));

                return;
            }

            public void ProvideSetOperationsSetter(
                AttributeComponentSetOperationsCreationHandler<TData>
                    setOperationsSetter)
            {
                this.SetOperations = InitProperty(
                    () => setOperationsSetter(Factory.Value));

                return;
            }
        }

        internal class NavigationalAttributeComponentNode<TKey, TData>
            : AttributeComponentNode<KeyValuePair<TKey, TData>>
            where TKey : new()
        {
            public PropertyNode<NavigationHandler<TKey, TData>>
                NavigateByKey
            { get; protected set; }

            public PropertyNode<Func<TData, TKey>> PrincipalKeySelector
            { get; protected set; }

            public PropertyNode<NavigationHandler<TData, TKey>>
                SelectKey
            { get; protected set; }

            public NavigationalAttributeComponentNode(
                IAttributeComponentFactory<KeyValuePair<TKey, TData>> factory)
                : base(factory.Domain)
            {
                ProvideFactory(factory);

                return;
            }

            public void ProvideFactory(
                IAttributeComponentFactory<KeyValuePair<TKey, TData>> factory)
            {
                this.Factory = InitProperty(factory);

                return;
            }

            public void ProvideNavigationByKeyHandler(
                NavigationHandler<TKey, TData> navigateByKey)
            {
                this.NavigateByKey = InitProperty(navigateByKey);

                return;
            }

            public void ProvidePrincipalKeySelector(
                Func<TData, TKey> principalKeySelector)
            {
                this.PrincipalKeySelector = InitProperty(principalKeySelector);

                return;
            }

            public void ProvideKeySelectionHandler(
                NavigationHandler<TData, TKey> selectKey)
            {
                this.SelectKey = InitProperty(selectKey);

                return;
            }
        }

        #endregion
    }
}
