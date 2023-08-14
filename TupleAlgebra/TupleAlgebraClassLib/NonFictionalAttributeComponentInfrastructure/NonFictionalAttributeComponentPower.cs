using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    /// <summary>
    /// Мощность нефиктивной компоненты.
    /// </summary>
    public abstract class NonFictionalAttributeComponentPower<TData> : AttributeComponentPower
    {
        #region Constants

        /// <summary>
        /// Константное значение типа наполнения компоненты - нефиктивный.
        /// </summary>
        public override AttributeComponentContentType ContentType
        { get => AttributeComponentContentType.NonFictional; }

        protected abstract NonFictionalAttributeComponent<TData> Component { get; }

        #endregion

        #region Instance methods

        public abstract void InitAttributeComponent(NonFictionalAttributeComponent<TData> component);

        public override bool EqualsContinuum()
        {
            return Component.Domain == Component;
        }

        protected abstract int CompareToSame(dynamic second);

        public override sealed int CompareTo(AttributeComponentPower second)
        {
            int comparisonResult = base.CompareTo(second);
            if (comparisonResult == 0)
                comparisonResult = CompareToSame(second);

            return comparisonResult;
        }

        #endregion
    }
}
