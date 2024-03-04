using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    public interface IFiniteEnumerableAttributeComponentPower
        : IComparable<IFiniteEnumerableAttributeComponentPower>
    {
        int NumericalRepresentation { get; }

        int IComparable<IFiniteEnumerableAttributeComponentPower>.CompareTo(
            IFiniteEnumerableAttributeComponentPower other)
        {
            return this.NumericalRepresentation.CompareTo(other.NumericalRepresentation);
        }

        int CompareToZero()
        {
            return this.NumericalRepresentation.CompareTo(0);
        }
    }
}
