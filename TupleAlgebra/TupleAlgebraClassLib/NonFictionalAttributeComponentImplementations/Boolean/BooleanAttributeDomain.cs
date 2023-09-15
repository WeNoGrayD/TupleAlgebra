using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    public sealed class BooleanAttributeDomain : OrderedFiniteEnumerableAttributeDomain<bool>
    {
        #region Static properties

        public static BooleanAttributeDomain Instance { get; private set; }

        #endregion

        #region Constructors

        static BooleanAttributeDomain()
        {
            Instance = new BooleanAttributeDomain();

            return;
        }

        private BooleanAttributeDomain()
            : base(new bool[2] { false, true })
        {
            return;
        }

        #endregion
    }
}
