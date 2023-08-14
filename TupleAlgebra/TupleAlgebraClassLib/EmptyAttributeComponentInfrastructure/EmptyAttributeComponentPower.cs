using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.EmptyAttributeComponentInfrastructure
{
    public sealed class EmptyAttributeComponentPower : AttributeComponentPower
    {
        #region Static fields

        private static Lazy<EmptyAttributeComponentPower> _instance;

        #endregion

        #region Instance fields

        public override AttributeComponentContentType ContentType
        { get => AttributeComponentContentType.Empty; }

        #endregion

        #region Static properties

        public static EmptyAttributeComponentPower Instance { get => _instance.Value; }

        #endregion

        #region Constructors

        static EmptyAttributeComponentPower()
        {
            _instance = new Lazy<EmptyAttributeComponentPower>(() => new EmptyAttributeComponentPower());

            return;
        }

        private EmptyAttributeComponentPower() { }

        #endregion

        #region IAttributeComponentPower implementation

        public override bool EqualsZero() => true;

        public override bool EqualsContinuum() => false;

        #endregion
    }
}
