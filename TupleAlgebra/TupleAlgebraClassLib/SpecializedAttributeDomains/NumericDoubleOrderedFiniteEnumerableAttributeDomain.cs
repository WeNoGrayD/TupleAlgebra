using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public sealed class NumericDoubleOrderedFiniteEnumerableAttributeDomain 
        : NumericOrderedFiniteEnumerableAttributeDomain<double>
    {
        public NumericDoubleOrderedFiniteEnumerableAttributeDomain(IEnumerable<double> universum)
            : base(universum)
        { }

        public NumericDoubleOrderedFiniteEnumerableAttributeDomain(
            (double Start, double End, double Stride) range)
            : base(range)
        { }

        public NumericDoubleOrderedFiniteEnumerableAttributeDomain(
            (double Start, double End) range)
            : base(range)
        { }

        public NumericDoubleOrderedFiniteEnumerableAttributeDomain(
            params (double Start, double End, double Stride)[] ranges)
            : base(ranges)
        { }
    }
}
