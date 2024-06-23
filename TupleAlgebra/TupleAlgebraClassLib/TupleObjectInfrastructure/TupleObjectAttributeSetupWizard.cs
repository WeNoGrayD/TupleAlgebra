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
                component = componentFactory
                    .CreateNonFictional(
                        entitySet.Select(entity => attrInfo.AttributeGetter(entity)));

            return SetComponent(tuple, component);
        }
    }

    public abstract class TupleObjectAttributeSetupWizard<TAttribute> :
        ITupleObjectAttributeSetupWizard<TAttribute>
    {
        public ITupleObjectSchemaProvider Schema { get; protected set; }

        public AttributeName AttributeName { get => _attributeName; }

        protected AttributeName _attributeName;

        protected virtual ITupleObjectAttributeInfo<TAttribute> AttributeInfo
        {
            get => (Schema[_attributeName] as ITupleObjectAttributeInfo<TAttribute>)!;
            set => Schema[_attributeName] = value;
        }

        protected TupleObjectAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            LambdaExpression memberAccess)
        {
            Schema = schema;
            _attributeName = MemberExtractor.ExtractFrom(memberAccess).Name;

            return;
        }

        protected TupleObjectAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            string attributeName)
        {
            Schema = schema;
            _attributeName = attributeName;

            return;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> SetFactory(
            IAttributeComponentFactory<TAttribute> factory)
        {
            Schema[_attributeName] = AttributeInfo.SetFactory(factory);

            return this;
        }

        /*
        /// <summary>
        /// Установка домена атрибута с проекцией.
        /// Побочным эффектом реализации метода является включение атрибута в схему.  
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> SetDomain<TDomain>(
            AttributeDomain<TDomain> domain,
            Func<TDomain, TAttribute> selector)
        {
            AttributeDomain<TAttribute> domain2 = domain.Select(selector);
            Schema[_attributeName] = AttributeInfo.Construct(true, domain2);
            return this;
        }

        /// <summary>
        /// Установка домена атрибута со множественной проекцией.
        /// Побочным эффектом реализации метода является включение атрибута в схему.  
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> SetDomain<TDomain>(
            AttributeDomain<TDomain> domain,
            Func<TDomain, IEnumerable<TAttribute>> selector)
        {
            AttributeDomain<TAttribute> domain2 = domain.Select(selector);
            Schema[_attributeName] = AttributeInfo.Construct(true, domain2);
            return this;
        }
        */

        /// <summary>
        /// Установка отношения эквивалентности по атрибуту. 
		/// (сжатие объектов по этому атрибуту).
		/// (можно не сжимать, тогда кортеж будет раздутый).
        /// </summary>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> SetEquivalenceRelation()
        {
            /*
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                isPlugged: attribute.IsPlugged,
                hasEquivalenceRelation: true);

            return this;
            */

            return this;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> SetEquivalenceRelation(
            IEqualityComparer<TAttribute> equivalenceRelationComparer)
        {
            /*
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                isPlugged: attribute.IsPlugged,
                equivalenceRelationComparer: equivalenceRelationComparer);

            return this;
            */

            return this;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> SetEquivalenceRelation(
            Func<TAttribute, TAttribute, bool> equivalenceRelation)
        {
            /*
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                isPlugged: attribute.IsPlugged,
                equivalenceRelation: equivalenceRelation);

            return this;
            */

            return this;
        }

        /// <summary>
        /// Деинсталляция отношения эквивалентности по атрибуту.
        /// </summary>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> UnsetEquivalenceRelation()
        {
            /*
            AttributeInfo attribute = Schema[_attributeName].Value;
            Schema[_attributeName] = attribute.CloneWith(
                isPlugged: attribute.IsPlugged,
                hasEquivalenceRelation: false);

            return this;
            */

            return this;
        }

        /// <summary>
        /// Игнорирование атрибута.
        /// </summary>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> Ignore()
        {
            Schema.RemoveAttribute(_attributeName);

            return this;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> Attach()
        {
            Schema.AttachAttribute(_attributeName);

            return this;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> Detach()
        {
            Schema.DetachAttribute(_attributeName);

            return this;
        }

        public ITupleObjectAttributeManager CreateManager()
        {
            return new TupleObjectAttributeManager<TAttribute>(this);
        }
    }
}
