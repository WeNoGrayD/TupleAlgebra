using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    /// <summary>
    /// Мощность полной фиктивной компоненты атрибута.
    /// </summary>
    public class FullAttributeComponentPower : AttributeComponentPower
    {
        #region Static fields

        private static Lazy<FullAttributeComponentPower> _instance;

        #endregion

        #region Instance fields

        public override AttributeComponentContentType ContentType
        { get => AttributeComponentContentType.Full; }

        #endregion

        #region Static properties

        public static FullAttributeComponentPower Instance { get => _instance.Value; }

        #endregion

        #region Constructors

        static FullAttributeComponentPower()
        {
            _instance = new Lazy<FullAttributeComponentPower>(() => new FullAttributeComponentPower());

            return;
        }

        private FullAttributeComponentPower() { }

        #endregion

        #region IAttributeComponentPower implementation

        public override bool EqualsZero() => false;

        public override bool EqualsContinuum() => true;

        #endregion
    }
}
