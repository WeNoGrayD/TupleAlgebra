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
        #region Instance fields

        public override AttributeComponentContentType ContentType
        { get => AttributeComponentContentType.Empty; }

        #endregion

        #region Static properties

        public static EmptyAttributeComponentPower Instance 
        { get; private set; }

        #endregion

        #region Constructors

        static EmptyAttributeComponentPower()
        {
            Instance = new EmptyAttributeComponentPower();

            return;
        }

        private EmptyAttributeComponentPower() { }

        #endregion

        #region IAttributeComponentPower implementation

        public override bool EqualsZero<TData>(AttributeComponent<TData> ac) 
            => true;

        public override bool EqualsContinuum<TData>(AttributeComponent<TData> ac)
            => false;

        #endregion
    }
}
