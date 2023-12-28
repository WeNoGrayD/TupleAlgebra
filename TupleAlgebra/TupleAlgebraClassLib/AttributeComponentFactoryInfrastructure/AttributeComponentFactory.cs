using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure;
using TupleAlgebraClassLib.FullAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public class AttributeComponentFactory
    {
        #region Instance fields

        private MethodInfo _createSpecificNonFictionalMIPattern;

        #endregion

        #region Constructors

        public AttributeComponentFactory()
        {
            _createSpecificNonFictionalMIPattern = this.GetType()
                .GetMethod(nameof(CreateSpecificNonFictional), BindingFlags.NonPublic | BindingFlags.Instance);

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
        public virtual EmptyAttributeComponent<TData> CreateEmpty<TData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            factoryArgs.Power = EmptyAttributeComponentPower.Instance;
            EmptyAttributeComponent<TData> empty = new EmptyAttributeComponent<TData>(
                (factoryArgs.Power as EmptyAttributeComponentPower)!,
                factoryArgs.QueryProvider, 
                factoryArgs.QueryExpression);
            if (factoryArgs.DomainGetter is not null)
                empty.GetDomain = (factoryArgs.DomainGetter as Func<AttributeDomain<TData>>)!;

            return empty;
        }

        public virtual EmptyAttributeComponent<TData> CreateEmpty<TData>(
            Func<AttributeDomain<TData>> domainGetter)
        {
            AttributeComponentFactoryArgs factoryArgs = new EmptyAttributeComponentFactoryArgs();
            factoryArgs.SetDomainGetter(domainGetter);

            return CreateEmpty<TData>(factoryArgs);
        }

        /// <summary>
        /// Создание нефиктивной компоненты атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public AttributeComponent<TData> CreateNonFictional<TData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            /*
             * Прежде всего создаётся нефиктивная компонента с предоставленными фабричными аргументами.
             */
            AttributeComponent<TData> ac = CreateSpecificNonFictional();
            ac.GetDomain = (factoryArgs.DomainGetter as Func<AttributeDomain<TData>>)!;

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
                if (factoryArgs.Power.EqualsZero())
                    ac = CreateEmpty<TData>(factoryArgs);
                else if (factoryArgs.Power.EqualsContinuum())
                    ac = CreateFull<TData>(factoryArgs);
            }

            return ac;

            /*
             * Построение и вызов обобщённого метода создания нефиктивной компоненты атрибута
             * конкретного типа.
             */
            AttributeComponent<TData> CreateSpecificNonFictional()
            {
                return _createSpecificNonFictionalMIPattern
                    .MakeGenericMethod(factoryArgs.GetType(), typeof(TData))
                    .Invoke(this, new object[] { factoryArgs })
                    as AttributeComponent<TData>;
            }
        }

        /// <summary>
        /// Создание нефиктивной компоненты атрибута конкретного типа.
        /// </summary>
        /// <typeparam name="TFactoryArgs"></typeparam>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        protected AttributeComponent<TData> CreateSpecificNonFictional<TFactoryArgs, TData>(
            TFactoryArgs factoryArgs)
            where TFactoryArgs : AttributeComponentFactoryArgs
        {
            return (this as INonFictionalAttributeComponentFactory<TFactoryArgs>)
                .CreateSpecificNonFictional<TData>(factoryArgs);
        }

        /// <summary>
        /// Создание полной фиктивной компоненты атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="factoryArgs"></param>
        /// <returns></returns>
        public virtual FullAttributeComponent<TData> CreateFull<TData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            factoryArgs.Power = new FullAttributeComponentPower();
            FullAttributeComponent<TData> full = 
                new FullAttributeComponent<TData>(
                    (factoryArgs.Power as FullAttributeComponentPower)!, 
                    factoryArgs.QueryProvider, 
                    factoryArgs.QueryExpression);
            if (factoryArgs.DomainGetter is not null)
                full.GetDomain = (factoryArgs.DomainGetter as Func<AttributeDomain<TData>>)!;

            return full;
        }

        public virtual FullAttributeComponent<TData> CreateFull<TData>(
            Func<AttributeDomain<TData>> domainGetter)
        {
            AttributeComponentFactoryArgs factoryArgs = new FullAttributeComponentFactoryArgs();
            factoryArgs.SetDomainGetter(domainGetter);

            return CreateFull<TData>(factoryArgs);
        }

        #endregion
    }
}
