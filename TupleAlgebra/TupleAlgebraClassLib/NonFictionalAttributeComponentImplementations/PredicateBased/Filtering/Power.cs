using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    public class FilteringAttributeComponentPower
        : NonFictionalAttributeComponentPower
    {
        #region Static properties

        public static FilteringAttributeComponentPower Instance { get; private set; }

        #endregion

        #region Constructors

        static FilteringAttributeComponentPower()
        {
            Instance = new FilteringAttributeComponentPower();

            return;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Предполагается усложнение.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="ac"></param>
        /// <returns></returns>
        private AttributeComponentContentType GetProbableRange<TData>(
            AttributeComponent<TData> ac)
        {
            return AttributeComponentContentType.NonFictional;
        }

        protected override int CompareToZero<TData>(AttributeComponent<TData> ac)
        {
            return GetProbableRange(ac).CompareToZero();
        }

        public override int CompareToNonFictional<TData>(
            AttributeComponentPower second,
            AttributeComponent<TData> ac1,
            AttributeComponent<TData> ac2)
        {
            /*
             * Если вызывается этот метод, то мощность заведомо равняется
             * мощности некоторой нефиктивной компоненты, не пустой и 
             * не полной. Определить более точно, какая мощность больше,
             * можно лишь с затратой времени и ресурсов.
             */
            return 0;
        }

        public override bool EqualsContinuum<TData>(AttributeComponent<TData> ac)
        {
            return GetProbableRange(ac).CompareToContinuum() == 0;
        }

        #endregion
    }
}
