using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    public class FilteringAttributeComponentPower<TData>
        : NonFictionalAttributeComponentPower<TData>
    {
        #region Instance fields

        private AttributeComponentContentType _probableRange;

        #endregion

        #region Instance properties

        public override AttributeComponentContentType ContentType
        { get => _probableRange; }

        #endregion

        #region Constructors

        public FilteringAttributeComponentPower(
            AttributeComponentContentType probableRange)
        {
            _probableRange = probableRange;

            return;
        }

        #endregion

        #region Instance methods

        protected override int CompareToZero()
        {
            return _probableRange.CompareToZero();
        }

        protected override int CompareToNonFictional(
            AttributeComponentPower second)
        {
            /*
             * Если вызывается этот метод, то мощность заведомо равняется
             * мощности некоторой нефиктивной компоненты, не пустой и 
             * не полной. Определить более точно, какая мощность больше,
             * можно лишь с затратой времени и ресурсов.
             */
            return 0;
        }

        public override bool EqualsContinuum()
        {
            return _probableRange.CompareToContinuum() == 0;
        }

        #endregion
    }
}
