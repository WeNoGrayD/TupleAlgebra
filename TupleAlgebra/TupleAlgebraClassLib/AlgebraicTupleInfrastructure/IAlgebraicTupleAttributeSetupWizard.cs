using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AlgebraicTupleInfrastructure
{
    /// <summary>
    /// Интерфейс мастера настройки атрибута в схеме кортежа.
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    public interface IAlgebraicTupleAttributeSetupWizard<TAttribute>
    {
        /// <summary>
        /// Установка домена атрибута.
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        IAlgebraicTupleAttributeSetupWizard<TAttribute> SetDomain(AttributeDomain<TAttribute> domain);

        /// <summary>
        /// Установка отношения эквивалентности по атрибуту.
        /// Это означает, что при загрузке данных в кортеж будет производиться их слияние
        /// по значениям этого атрибута.
        /// </summary>
        /// <returns></returns>
        IAlgebraicTupleAttributeSetupWizard<TAttribute> SetEquivalenceRelation();

        /// <summary>
        /// Деинсталляция отношения эквивалентности по атрибуту.
        /// Это означает, что при загрузке данных в кортеж их слияние по значениям этого атрибута
        /// производиться НЕ будет.
        /// </summary>
        /// <returns></returns>
        IAlgebraicTupleAttributeSetupWizard<TAttribute> UnsetEquivalenceRelation();

        /// <summary>
        /// Игнорирование атрибута.
        /// Это означает удаление атрибута из схемы кортежа.
        /// </summary>
        /// <returns></returns>
        IAlgebraicTupleAttributeSetupWizard<TAttribute> Ignore();
    }
}
