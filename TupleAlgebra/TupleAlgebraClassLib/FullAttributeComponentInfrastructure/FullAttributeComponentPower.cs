using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean;

namespace TupleAlgebraClassLib.FullAttributeComponentInfrastructure
{
    /// <summary>
    /// Мощность полной фиктивной компоненты атрибута.
    /// </summary>
    public class FullAttributeComponentPower : AttributeComponentPower
    {
        #region Static properties

        public static FullAttributeComponentPower Instance { get; private set; }

        #endregion

        #region Instance properties

        public override AttributeComponentContentType ContentType
        { get => AttributeComponentContentType.Full; }

        //public AttributeComponentPower UniversePower { get; set; }

        #endregion

        #region Constructors

        static FullAttributeComponentPower()
        {
            Instance = new FullAttributeComponentPower();

            return;
        }

        #endregion

        #region IAttributeComponentPower implementation

        /*
        public override int CompareTo(AttributeComponentPower second)
        {
            return _universePower.CompareTo(second);
        }
        */

        public override bool EqualsZero<TData>(AttributeComponent<TData> ac)
            => false;

        public override bool EqualsContinuum<TData>(AttributeComponent<TData> ac)
            => true;

        #endregion
    }
}
