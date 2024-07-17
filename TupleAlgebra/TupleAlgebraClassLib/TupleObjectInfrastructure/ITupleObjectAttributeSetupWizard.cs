using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public interface ITupleObjectAttributeSetupWizard
    {
        public ITupleObjectSchemaProvider Schema { get; }

        public AttributeName AttributeName { get; }

        public IAttributeComponentFactory<TAttribute> GetFactory<TAttribute>();

        public ITupleObjectAttributeSetupWizard Ignore();

        public ITupleObjectAttributeSetupWizard Attach();

        public ITupleObjectAttributeSetupWizard Detach();

        public ITupleObjectAttributeManager CreateManager();

        //ITupleObjectAttributeSetupWizard Set(ITupleObjectAttributeInfo attrInfo);
    }

    /// <summary>
    /// Интерфейс мастера настройки атрибута в схеме кортежа.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public interface ITupleObjectAttributeSetupWizard<TAttribute>
        : ITupleObjectAttributeSetupWizard
    {
        IAttributeComponentFactory<TA>
            ITupleObjectAttributeSetupWizard.GetFactory<TA>()
        {
            return GetFactory() as IAttributeComponentFactory<TA>;
        }

        /// <summary>
        /// Получение фабрики компонент атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public IAttributeComponentFactory<TAttribute> GetFactory();

        /// <summary>
        /// Установка фабрики компонент атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> SetFactory(
            IAttributeComponentFactory<TAttribute> factory);

        /// <summary>
        /// Установка отношения эквивалентности по атрибуту.
        /// Это означает, что при загрузке данных в кортеж будет производиться их слияние
        /// по значениям этого атрибута.
        /// </summary>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> SetEquivalenceRelation();

        /// <summary>
        /// Деинсталляция отношения эквивалентности по атрибуту.
        /// Это означает, что при загрузке данных в кортеж их слияние по значениям этого атрибута
        /// производиться НЕ будет.
        /// </summary>
        /// <returns></returns>
        public ITupleObjectAttributeSetupWizard<TAttribute> UnsetEquivalenceRelation();

        /// <summary>
        /// Игнорирование атрибута.
        /// Это означает удаление атрибута из схемы кортежа.
        /// </summary>
        /// <returns></returns>
        public new ITupleObjectAttributeSetupWizard<TAttribute> Ignore();
        
        public new ITupleObjectAttributeSetupWizard<TAttribute> Attach();

        public new ITupleObjectAttributeSetupWizard<TAttribute> Detach();

        ITupleObjectAttributeSetupWizard ITupleObjectAttributeSetupWizard.Ignore() 
            => Ignore();

        ITupleObjectAttributeSetupWizard ITupleObjectAttributeSetupWizard.Attach()
            => Attach();

        ITupleObjectAttributeSetupWizard ITupleObjectAttributeSetupWizard.Detach()
            => Detach();
    }
}
