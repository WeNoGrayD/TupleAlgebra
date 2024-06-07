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
    public interface ITupleObjectAttributeManager
    {
        public bool IsEmpty(ISingleTupleObject tuple);

        public bool IsFull(ISingleTupleObject tuple);

        public IAttributeComponent GetComponent(
            System.Linq.Expressions.Expression factoryArgs);

        public ITupleObjectAttributeManager SetComponent(
            ISingleTupleObject tuple,
            IAttributeComponent ac);

        public ITupleObjectAttributeManager SetComponent(
            ISingleTupleObject tuple,
            IAttributeComponentFactoryArgs factoryArgs);

        public ITupleObjectAttributeManager SetComponent(
            IQueriedSingleTupleObject tuple,
            System.Linq.Expressions.Expression factoryArgs);

        public ITupleObjectAttributeManager SetComponentWithComplementionAccumulation(
            ISingleTupleObject tuple,
            IAttributeComponent component);

        public ITupleObjectAttributeManager SetComponentWithComplementionAccumulation(
            ISingleTupleObject tuple,
            IAttributeComponentFactoryArgs factoryArgs);

        public ITupleObjectAttributeManager SetDefaultFictionalAttributeComponent(
            ISingleTupleObject tuple);

        public ITupleObjectAttributeManager SetDefaultFictionalAttributeComponent(
            IQueriedSingleTupleObject tuple);

        public ITupleObjectAttributeManager
                SetComponentToProjectionOfOntoMember<TEntity>(
                    ISingleTupleObject tuple,
                    TEntity entity,
                    bool withTrailingComplement = false);

        public ITupleObjectAttributeManager
                SetComponentToProjectionOfOntoMember<TEntity>(
                    ISingleTupleObject tuple,
                    IEnumerable<TEntity> entitySet);
    }

    public interface ITupleObjectAttributeSetupWizard
    {
        public ITupleObjectSchemaProvider Schema { get; }

        public AttributeName AttributeName { get; }

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
        
        public new ITupleObjectAttributeSetupWizard<TAttribute> Attach();

        public new ITupleObjectAttributeSetupWizard<TAttribute> Detach();

        ITupleObjectAttributeSetupWizard ITupleObjectAttributeSetupWizard.Attach()
            => Attach();

        ITupleObjectAttributeSetupWizard ITupleObjectAttributeSetupWizard.Detach()
            => Detach();
    }
}
