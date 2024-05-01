using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public interface ITupleObjectAttributeSetupWizard
    {
        //ITupleObjectAttributeSetupWizard Set(ITupleObjectAttributeInfo attrInfo);

        public ITupleObjectAttributeSetupWizard SetComponent(
            ISingleTupleObject tuple,
            IAttributeComponent ac);

        public ITupleObjectAttributeSetupWizard SetComponent(
            ISingleTupleObject tuple,
            IAttributeComponentFactoryArgs factoryArgs);

        public ITupleObjectAttributeSetupWizard SetDefaultFictionalAttributeComponent(
            ISingleTupleObject tuple);
    }

    /// <summary>
    /// Интерфейс мастера настройки атрибута в схеме кортежа.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public interface ITupleObjectAttributeSetupWizard<TAttribute>
        : ITupleObjectAttributeSetupWizard
    {
        /// <summary>
        /// Установка домена атрибута.
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
        public ITupleObjectAttributeSetupWizard<TAttribute> Ignore();
        
        public ITupleObjectAttributeSetupWizard<TAttribute> Attach();

        public ITupleObjectAttributeSetupWizard<TAttribute> Detach();
    }
}
