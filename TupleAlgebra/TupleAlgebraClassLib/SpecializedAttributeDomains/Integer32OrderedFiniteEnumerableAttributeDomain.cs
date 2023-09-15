using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraClassLib.SpecializedAttributeDomains
{
    public sealed class Integer32OrderedFiniteEnumerableAttributeDomain 
        : NumericOrderedFiniteEnumerableAttributeDomain<int>
    {
        public Integer32OrderedFiniteEnumerableAttributeDomain(IEnumerable<int> universum)
            : base(universum)
        { }

        public Integer32OrderedFiniteEnumerableAttributeDomain(
            (int Start, int End, int Stride) range)
            : base(range)
        { }

        public Integer32OrderedFiniteEnumerableAttributeDomain(
            (int Start, int End) range)
            : base(range)
        { }

        public Integer32OrderedFiniteEnumerableAttributeDomain(
            params (int Start, int End, int Stride)[] ranges)
            : base(ranges)
        { }
    }
}
