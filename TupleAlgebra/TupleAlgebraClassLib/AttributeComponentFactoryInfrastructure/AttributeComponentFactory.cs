﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure;
using TupleAlgebraClassLib.FullAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public static class AttributeComponentFactoryHelper
    {
        #region Delegates

        public delegate AttributeComponent<TData>
            CreateSpecificNonFictionalComponentHandler<TData, CTFactoryArgs>(
                CTFactoryArgs factoryArgs);

        #endregion

        public static AttributeComponent<TData> CreateNonFictional<
            TData, CTFactoryArgs>(
            this IAttributeComponentFactory<TData> factory,
            CTFactoryArgs factoryArgs,
            CreateSpecificNonFictionalComponentHandler<TData, CTFactoryArgs> factoryFunc)
            where CTFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
        {
            /*
             * Прежде всего создаётся нефиктивная компонента с предоставленными фабричными аргументами.
             */
            AttributeComponent<TData> ac = factoryFunc(factoryArgs);

            /*
             * Если созданная нефиктивная компонента не является продуктом запроса, то
             * появляется возможность проверить компоненту на пустоту и полноту.
             */
            if (!factoryArgs.IsQuery)
            /*
             * Предполагается, что проверка компоненты на пустотность
             * является более простой, чем проверка на полноту.
             */
            {
                if (factoryArgs.Power.EqualsZero(ac))
                    ac = factory.CreateEmpty(factoryArgs);
                else if (factoryArgs.Power.EqualsContinuum(ac))
                    ac = factory.CreateFull(factoryArgs);
            }

            return ac;
        }
    }

    public class AttributeComponentFactory<TData>
        : IAttributeComponentFactory<TData>
    {
        #region Instance fields

        private MethodInfo _createSpecificNonFictionalMIPattern;

        #endregion

        #region Instance properties

        public AttributeDomain<TData> Domain { get; protected set; }

        #endregion

        #region Constructors

        protected AttributeComponentFactory()
        {
            _createSpecificNonFictionalMIPattern = this.GetType()
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(m => m.Name == nameof(CreateSpecificNonFictional) &&
                            m.IsGenericMethod);

            return;
        }

        public AttributeComponentFactory(AttributeDomain<TData> domain)
            : this()
        {
            Domain = domain;

            return;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Создание пустой фиктивной компоненты атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public virtual EmptyAttributeComponent<TData> CreateEmpty(
            AttributeComponentFactoryArgs factoryArgs)
        {
            factoryArgs.Power = EmptyAttributeComponentPower.Instance;
            EmptyAttributeComponent<TData> empty =
                new EmptyAttributeComponent<TData>(
                    (factoryArgs.Power as EmptyAttributeComponentPower)!,
                    factoryArgs.QueryProvider,
                    factoryArgs.QueryExpression);
            empty.Domain = Domain;

            return empty;
        }

        public virtual EmptyAttributeComponent<TData> CreateEmpty()
        {
            AttributeComponentFactoryArgs factoryArgs =
                new EmptyAttributeComponentFactoryArgs();

            return CreateEmpty(factoryArgs);
        }

        /// <summary>
        /// Создание нефиктивной компоненты атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public AttributeComponent<TData> CreateNonFictional(
            NonFictionalAttributeComponentFactoryArgs<TData> factoryArgs)
        {
            return this.CreateNonFictional(
                factoryArgs,
                CreateSpecificNonFictional);

            /*
             * Построение и вызов обобщённого метода создания нефиктивной компоненты атрибута
             * конкретного типа.
             */
            AttributeComponent<TData> CreateSpecificNonFictional(
                AttributeComponentFactoryArgs factoryArgs)
            {
                return _createSpecificNonFictionalMIPattern
                    .MakeGenericMethod(factoryArgs.GetType())
                    .Invoke(this, new object[] { factoryArgs })
                    as AttributeComponent<TData>;
            }
        }

        /// <summary>
        /// Создание нефиктивной компоненты атрибута конкретного типа.
        /// </summary>
        /// <typeparam name="TFactoryArgs"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public AttributeComponent<TData> CreateNonFictional<TFactoryArgs>(
            TFactoryArgs factoryArgs)
            where TFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
        {
            return this.CreateNonFictional(
                factoryArgs,
                CreateSpecificNonFictional<TFactoryArgs>);
        }

        protected NonFictionalAttributeComponent<TData> 
            CreateSpecificNonFictional<TFactoryArgs>(
            TFactoryArgs factoryArgs)
            where TFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
        {
            var ac = (this as INonFictionalAttributeComponentFactory2<TData, TFactoryArgs>)
                .CreateSpecificNonFictional(factoryArgs);
            ac.Domain = Domain;

            return ac;
        }

        /// <summary>
        /// Создание константной нефиктивной компоненты конкретного типа, 
        /// которая заведомо не является пустой или полной.
        /// </summary>
        /// <typeparam name="TAttributeComponent"></typeparam>
        /// <typeparam name="TFactoryArgs"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public TAttributeComponent
            CreateConstantNonFictional<TAttributeComponent, TFactoryArgs>(
            TFactoryArgs factoryArgs)
            where TAttributeComponent : NonFictionalAttributeComponent<TData>
            where TFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
        {
            return CreateSpecificNonFictional(factoryArgs)
                as TAttributeComponent;
        }

        /// <summary>
        /// Создание полной фиктивной компоненты атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public virtual FullAttributeComponent<TData> CreateFull(
            AttributeComponentFactoryArgs factoryArgs)
        {
            factoryArgs.Power = new FullAttributeComponentPower();
            FullAttributeComponent<TData> full =
                new FullAttributeComponent<TData>(
                    (factoryArgs.Power as FullAttributeComponentPower)!,
                    factoryArgs.QueryProvider,
                    factoryArgs.QueryExpression);
            full.Domain = Domain;

            return full;
        }

        public virtual FullAttributeComponent<TData> CreateFull()
        {
            AttributeComponentFactoryArgs factoryArgs =
                new FullAttributeComponentFactoryArgs();

            return CreateFull(factoryArgs);
        }

        #endregion
    }

    public abstract class AttributeComponentFactory<TData, CTFactoryArgs>
        : AttributeComponentFactory<TData>
        where CTFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
    {
        public AttributeComponentFactory(AttributeDomain<TData> domain)
            : base(domain)
        { }

        public AttributeComponentFactory(CTFactoryArgs factoryArgs)
            : base()
        {
            InitDomainFrom(factoryArgs);

            return;
        }

        protected void InitDomainFrom(CTFactoryArgs factoryArgs)
        {
            NonFictionalAttributeComponent<TData> domainUniverse =
                CreateSpecificNonFictional(factoryArgs);
            Domain = new AttributeDomain<TData>(domainUniverse);

            return;
        }
    }
}
