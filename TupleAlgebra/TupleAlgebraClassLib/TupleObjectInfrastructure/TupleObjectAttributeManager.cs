using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public class TupleObjectAttributeManager<TAttribute>
        : ITupleObjectAttributeManager
    {
        private ITupleObjectAttributeSetupWizard<TAttribute> _setupWizard;

        private Lazy<IAttributeComponent> _lastSetComponent = null;

        private IAttributeComponent _defaultComponent = null;

        public ITupleObjectSchemaProvider Schema { get => _setupWizard.Schema; }

        private AttributeName _attributeName;

        protected virtual ITupleObjectAttributeInfo<TAttribute> AttributeInfo
        {
            get => (Schema[_attributeName] as ITupleObjectAttributeInfo<TAttribute>)!;
            set => Schema[_attributeName] = value;
        }

        public TupleObjectAttributeManager(
            ITupleObjectAttributeSetupWizard<TAttribute> setupWizard)
        {
            _setupWizard = setupWizard;
            _attributeName = setupWizard.AttributeName;

            return;
        }

        public bool IsEmpty(ISingleTupleObject tuple)
        {
            return tuple[_attributeName].IsEmpty;
        }

        public bool IsFull(ISingleTupleObject tuple)
        {
            return tuple[_attributeName].IsFull;
        }

        public AttributeComponent<TAttribute> CreateVariable(string name)
        {
            IAttributeComponentFactory<TAttribute> factory =
                AttributeInfo.ComponentFactory;

            IAttributeComponentFactoryArgs factoryArgs =
                new VariableAttributeComponentFactoryArgs<TAttribute>(name);

            return factoryArgs.ProvideTo(factory);
        }

        IVariableAttributeComponent ITupleObjectAttributeManager
            .CreateVariable(string name)
        {
            return CreateVariable(name) as IVariableAttributeComponent;
        }

        public ITupleObjectAttributeManager SetComponent(
            ISingleTupleObject tuple,
            IAttributeComponent ac)
        {
            tuple[_attributeName] = ac;
            if (ac is IAttributeComponentWithVariables acWithVariables)
            {
                if (tuple.VariableContainer is null)
                    tuple.VariableContainer = new VariableContainer();
                tuple.VariableContainer.AddVariableRange(acWithVariables);
            }
            if (ac is IVariableAttributeComponent variable)
            {
                if (tuple.VariableContainer is null)
                    tuple.VariableContainer = new VariableContainer();
                tuple.VariableContainer.AddVariable(variable);
            }

            return this;
        }

        public ITupleObjectAttributeManager SetComponentWithComplementionAccumulation(
            ISingleTupleObject tuple,
            IAttributeComponent component)
        {
            if (_lastSetComponent is null)
            {
                _lastSetComponent = new Lazy<IAttributeComponent>(
                    component.ComplementThe);

                return SetComponent(tuple, component);
            }

            return SetComponent(tuple, _lastSetComponent.Value);
        }

        public IAttributeComponent GetComponent(Expression factoryArgs)
        {
            IAttributeComponent ac = factoryArgs switch
            {
                Expression<Func<TAttribute, bool>> predicate => GetFiltering(predicate),
                MethodCallExpression call =>
                    Expression.Lambda<Func<IAttributeComponent>>(call).Compile()(),
                _ => default
            };

            return ac;

            IAttributeComponent GetFiltering(
                Expression<Func<TAttribute, bool>> predicate)
            {
                IAttributeComponentFactory<TAttribute> factory =
                    AttributeInfo.ComponentFactory;
                if (factory is not INonFictionalAttributeComponentFactory2<
                    TAttribute, FilteringAttributeComponentFactoryArgs<TAttribute>>)
                {
                    AttributeDomain<TAttribute> domain =
                        AttributeInfo.ComponentFactory.Domain;
                    factory = new FilteringAttributeComponentFactory<TAttribute>(domain);
                }

                IAttributeComponentFactoryArgs factoryArgs =
                    new FilteringAttributeComponentFactoryArgs<TAttribute>(predicate);

                return factoryArgs.ProvideTo(factory);
            }
        }

        public ITupleObjectAttributeManager SetComponent(
            ISingleTupleObject tuple,
            IAttributeComponentFactoryArgs factoryArgs)
        {
            IAttributeComponentFactory<TAttribute> factory =
                AttributeInfo.ComponentFactory;
            IAttributeComponent ac = factoryArgs.ProvideTo(factory);

            return SetComponent(tuple, ac);
        }

        public ITupleObjectAttributeManager SetComponentWithComplementionAccumulation(
            ISingleTupleObject tuple,
            IAttributeComponentFactoryArgs factoryArgs)
        {
            IAttributeComponentFactory<TAttribute> factory =
                AttributeInfo.ComponentFactory;
            IAttributeComponent ac = factoryArgs.ProvideTo(factory);

            return SetComponentWithComplementionAccumulation(tuple, ac);
        }

        public ITupleObjectAttributeManager SetComponent(
            IQueriedSingleTupleObject tuple,
            Expression factoryArgs)
        {
            tuple[_attributeName] = factoryArgs;

            return this;
        }

        public ITupleObjectAttributeManager SetDefaultFictionalAttributeComponent(
            ISingleTupleObject tuple)
        {
            _defaultComponent = _defaultComponent ??
                tuple.GetDefaultFictionalAttributeComponent(
                    AttributeInfo.ComponentFactory);

            return SetComponent(tuple, _defaultComponent);
        }

        public ITupleObjectAttributeManager SetDefaultFictionalAttributeComponent(
            IQueriedSingleTupleObject tuple)
        {
            return SetComponent(
                tuple,
                tuple.GetDefaultFictionalAttributeComponent(
                    AttributeInfo.ComponentFactory));
        }

        public ITupleObjectAttributeManager
                SetComponentToProjectionOfOntoMember<TEntity>(
                    ISingleTupleObject tuple,
                    TEntity entity,
                    bool withTrailingComplement = false)
        {
            ITupleObjectAttributeInfo<TEntity, TAttribute> attrInfo =
                (Schema[_attributeName] as ITupleObjectAttributeInfo<TEntity, TAttribute>)!;
            if (attrInfo.ComponentFactory is not
                IEnumerableNonFictionalAttributeComponentFactory<TAttribute> componentFactory)
            {
                throw new Exception($"Был создан запрос на создание объекта алгебры кортежей из экземпляра типа {nameof(TEntity)}. Фабрика компонент атрибутов для атрибута {_attributeName} не реализует интерфейс ITupleObjectAttributeInfo<{nameof(TEntity)}, {nameof(TAttribute)}>, что требуется в процессе программы.");
            }

            return withTrailingComplement ?
                SetComponentWithComplementionAccumulation(tuple, LookLastSetComponent()) :
                SetComponent(tuple, CreateNonFictional());

            IAttributeComponent CreateNonFictional()
            {
                return componentFactory
                    .CreateNonFictional([attrInfo.AttributeGetter(entity)]);
            }

            IAttributeComponent LookLastSetComponent()
            {
                return _lastSetComponent is null ?
                    CreateNonFictional() :
                    null;
            }
        }

        public ITupleObjectAttributeManager
                SetComponentToProjectionOfOntoMember<TEntity>(
                    ISingleTupleObject tuple,
                    IEnumerable<TEntity> entitySet)
        {
            ITupleObjectAttributeInfo<TEntity, TAttribute> attrInfo =
                (Schema[_attributeName] as ITupleObjectAttributeInfo<TEntity, TAttribute>)!;
            AttributeComponent<TAttribute> component = null;
            if (attrInfo.ComponentFactory is not
                IEnumerableNonFictionalAttributeComponentFactory<TAttribute> componentFactory)
            {
                throw new Exception($"Был создан запрос на создание объекта алгебры кортежей из перечисления экземпляров типа {nameof(TEntity)}. Фабрика компонент атрибутов для атрибута {_attributeName} не реализует интерфейс ITupleObjectAttributeInfo<{nameof(TEntity)}, {nameof(TAttribute)}>, что требуется в процессе программы.");
            }
            else
            {
                component = componentFactory
                    .CreateNonFictional(
                        entitySet.Select(entity => attrInfo.AttributeGetter(entity)));
            }

            return SetComponent(tuple, component);
        }
    }

    /*
    public class TupleObjectNavigationalAttributeManager<TKey, TAttribute>
        : ITupleObjectAttributeManager
    {
        private ITupleObjectAttributeSetupWizard<KeyValuePair<TKey, TAttribute>>
            _setupWizard;

        private Lazy<IAttributeComponent> _lastSetComponent = null;

        private IAttributeComponent _defaultComponent = null;

        public ITupleObjectSchemaProvider Schema { get => _setupWizard.Schema; }

        private AttributeName _attributeName;

        protected virtual ITupleObjectAttributeInfo<TAttribute> AttributeInfo
        {
            get => (Schema[_attributeName] as ITupleObjectAttributeInfo<TAttribute>)!;
            set => Schema[_attributeName] = value;
        }

        public TupleObjectAttributeManager(
            ITupleObjectAttributeSetupWizard<TAttribute> setupWizard)
        {
            _setupWizard = setupWizard;
            _attributeName = setupWizard.AttributeName;

            return;
        }

        public bool IsEmpty(ISingleTupleObject tuple)
        {
            return tuple[_attributeName].IsEmpty;
        }

        public bool IsFull(ISingleTupleObject tuple)
        {
            return tuple[_attributeName].IsFull;
        }

        public AttributeComponent<TAttribute> CreateVariable(string name)
        {
            IAttributeComponentFactory<TAttribute> factory =
                AttributeInfo.ComponentFactory;

            IAttributeComponentFactoryArgs factoryArgs =
                new VariableAttributeComponentFactoryArgs<TAttribute>(name);

            return factoryArgs.ProvideTo(factory);
        }

        IVariableAttributeComponent ITupleObjectAttributeManager
            .CreateVariable(string name)
        {
            return CreateVariable(name) as IVariableAttributeComponent;
        }

        public ITupleObjectAttributeManager SetComponent(
            ISingleTupleObject tuple,
            IAttributeComponent ac)
        {
            tuple[_attributeName] = ac;
            if (ac is IAttributeComponentWithVariables acWithVariables)
            {
                if (tuple.VariableContainer is null)
                    tuple.VariableContainer = new VariableContainer();
                tuple.VariableContainer.AddVariableRange(acWithVariables);
            }
            if (ac is IVariableAttributeComponent variable)
            {
                if (tuple.VariableContainer is null)
                    tuple.VariableContainer = new VariableContainer();
                tuple.VariableContainer.AddVariable(variable);
            }

            return this;
        }

        public ITupleObjectAttributeManager SetComponentWithComplementionAccumulation(
            ISingleTupleObject tuple,
            IAttributeComponent component)
        {
            if (_lastSetComponent is null)
            {
                _lastSetComponent = new Lazy<IAttributeComponent>(
                    component.ComplementThe);

                return SetComponent(tuple, component);
            }

            return SetComponent(tuple, _lastSetComponent.Value);
        }

        public IAttributeComponent GetComponent(Expression factoryArgs)
        {
            IAttributeComponent ac = factoryArgs switch
            {
                Expression<Func<TAttribute, bool>> predicate => GetFiltering(predicate),
                MethodCallExpression call =>
                    Expression.Lambda<Func<IAttributeComponent>>(call).Compile()(),
                _ => default
            };

            return ac;

            IAttributeComponent GetFiltering(
                Expression<Func<TAttribute, bool>> predicate)
            {
                IAttributeComponentFactory<TAttribute> factory =
                    AttributeInfo.ComponentFactory;
                if (factory is not INonFictionalAttributeComponentFactory2<
                    TAttribute, FilteringAttributeComponentFactoryArgs<TAttribute>>)
                {
                    AttributeDomain<TAttribute> domain =
                        AttributeInfo.ComponentFactory.Domain;
                    factory = new FilteringAttributeComponentFactory<TAttribute>(domain);
                }

                IAttributeComponentFactoryArgs factoryArgs =
                    new FilteringAttributeComponentFactoryArgs<TAttribute>(predicate);

                return factoryArgs.ProvideTo(factory);
            }
        }

        public ITupleObjectAttributeManager SetComponent(
            ISingleTupleObject tuple,
            IAttributeComponentFactoryArgs factoryArgs)
        {
            IAttributeComponentFactory<TAttribute> factory =
                AttributeInfo.ComponentFactory;
            IAttributeComponent ac = factoryArgs.ProvideTo(factory);

            return SetComponent(tuple, ac);
        }

        public ITupleObjectAttributeManager SetComponentWithComplementionAccumulation(
            ISingleTupleObject tuple,
            IAttributeComponentFactoryArgs factoryArgs)
        {
            IAttributeComponentFactory<TAttribute> factory =
                AttributeInfo.ComponentFactory;
            IAttributeComponent ac = factoryArgs.ProvideTo(factory);

            return SetComponentWithComplementionAccumulation(tuple, ac);
        }

        public ITupleObjectAttributeManager SetComponent(
            IQueriedSingleTupleObject tuple,
            Expression factoryArgs)
        {
            tuple[_attributeName] = factoryArgs;

            return this;
        }

        public ITupleObjectAttributeManager SetDefaultFictionalAttributeComponent(
            ISingleTupleObject tuple)
        {
            _defaultComponent = _defaultComponent ??
                tuple.GetDefaultFictionalAttributeComponent(
                    AttributeInfo.ComponentFactory);

            return SetComponent(tuple, _defaultComponent);
        }

        public ITupleObjectAttributeManager SetDefaultFictionalAttributeComponent(
            IQueriedSingleTupleObject tuple)
        {
            return SetComponent(
                tuple,
                tuple.GetDefaultFictionalAttributeComponent(
                    AttributeInfo.ComponentFactory));
        }

        public ITupleObjectAttributeManager
                SetComponentToProjectionOfOntoMember<TEntity>(
                    ISingleTupleObject tuple,
                    TEntity entity,
                    bool withTrailingComplement = false)
        {
            ITupleObjectAttributeInfo<TEntity, TAttribute> attrInfo =
                (Schema[_attributeName] as ITupleObjectAttributeInfo<TEntity, TAttribute>)!;
            if (attrInfo.ComponentFactory is not
                IEnumerableNonFictionalAttributeComponentFactory<TAttribute> componentFactory)
            {
                throw new Exception($"Был создан запрос на создание объекта алгебры кортежей из экземпляра типа {nameof(TEntity)}. Фабрика компонент атрибутов для атрибута {_attributeName} не реализует интерфейс ITupleObjectAttributeInfo<{nameof(TEntity)}, {nameof(TAttribute)}>, что требуется в процессе программы.");
            }

            return withTrailingComplement ?
                SetComponentWithComplementionAccumulation(tuple, LookLastSetComponent()) :
                SetComponent(tuple, CreateNonFictional());

            IAttributeComponent CreateNonFictional()
            {
                return componentFactory
                    .CreateNonFictional([attrInfo.AttributeGetter(entity)]);
            }

            IAttributeComponent LookLastSetComponent()
            {
                return _lastSetComponent is null ?
                    CreateNonFictional() :
                    null;
            }
        }

        public ITupleObjectAttributeManager
                SetComponentToProjectionOfOntoMember<TEntity>(
                    ISingleTupleObject tuple,
                    IEnumerable<TEntity> entitySet)
        {
            ITupleObjectAttributeInfo<TEntity, TAttribute> attrInfo =
                (Schema[_attributeName] as ITupleObjectAttributeInfo<TEntity, TAttribute>)!;
            AttributeComponent<TAttribute> component = null;
            if (attrInfo.ComponentFactory is not
                IEnumerableNonFictionalAttributeComponentFactory<TAttribute> componentFactory)
            {
                throw new Exception($"Был создан запрос на создание объекта алгебры кортежей из перечисления экземпляров типа {nameof(TEntity)}. Фабрика компонент атрибутов для атрибута {_attributeName} не реализует интерфейс ITupleObjectAttributeInfo<{nameof(TEntity)}, {nameof(TAttribute)}>, что требуется в процессе программы.");
            }
            else
            {
                component = componentFactory
                    .CreateNonFictional(
                        entitySet.Select(entity => attrInfo.AttributeGetter(entity)));
            }

            return SetComponent(tuple, component);
        }
    }
    */
}
