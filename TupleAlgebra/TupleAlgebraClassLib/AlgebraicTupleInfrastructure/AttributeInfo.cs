using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AlgebraicTupleInfrastructure
{
    /// <summary>
    /// Информация об атрибуте в составе схемы кортежа.
    /// </summary>
    public struct AttributeInfo
    {
        #region Instance fields

        /// <summary>
        /// Домен атрибута.
        /// </summary>
        private object _domain;

        #endregion

        #region Instance properties

        /// <summary>
        /// Флаг подключения атрибута к схеме.
        /// </summary>
        public readonly bool IsPlugged;

        /// <summary>
        /// Настройщик.
        /// </summary>
        public object SetupWizard { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="isPlugged"></param>
        /// <param name="domain"></param>
        private AttributeInfo(bool isPlugged, object domain = null)
        {
            IsPlugged = isPlugged;
            _domain = domain;
            SetupWizard = null;
        }

        #endregion

        public static AttributeInfo Construct<TAttribute>(bool isPlugged, AttributeDomain<TAttribute> domain = null)
        {
            return new AttributeInfo(isPlugged, domain);
        }

        public AttributeDomain<TAttribute> GetDomain<TAttribute>()
        {
            return _domain as AttributeDomain<TAttribute>;
        }

        public dynamic GetDomain()
        {
            return _domain;
        }

        public dynamic GetSetupWizard()
        {
            return SetupWizard;
        }

        public void SetSetupWizard<TData>(
            IAlgebraicTupleAttributeSetupWizard<TData> setupWizard)
        {
            SetupWizard = setupWizard;

            return;
        }

        public AttributeInfo PlugIn()
        {
            return new AttributeInfo(true, _domain);
        }

        public AttributeInfo UnPlug()
        {
            return new AttributeInfo(false, _domain);
        }

        public static bool operator ==(AttributeInfo first, AttributeInfo second)
        {
            return first.IsPlugged == second.IsPlugged;
        }

        public static bool operator !=(AttributeInfo first, AttributeInfo second)
        {
            return !(first == second);
        }
    }
}
