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
        #region Instance fields

        private AttributeComponentPower _universumPower;

        #endregion

        #region Instance properties

        public override AttributeComponentContentType ContentType
        { get => AttributeComponentContentType.Full; }

        public AttributeComponentPower UniversumPower { get; set; }

        #endregion

        #region IAttributeComponentPower implementation

        public override int CompareTo(AttributeComponentPower second)
        {
            /*
             * Если вторая компонента тоже полная, то сравнивается мощность представляемая 
             * ей универсума с универсумом, который представляется этой компонентой.
             * Если вторая компонента пустая, то универсум этой компоненты сравнивается с ней.
             * Если вторая компонента нефиктивная, то она сравнивается с универсумом этой компоненты.
             */
            return base.CompareTo(second) >= 0 ? 
                -second.CompareTo(_universumPower) : 
                _universumPower.CompareTo(second);
        }

        public override bool EqualsZero() => false;

        public override bool EqualsContinuum() => true;

        #endregion
    }
}
