using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    public sealed class BooleanAttributeDomain : FiniteEnumerableAttributeDomain<bool>
    {
        public BooleanAttributeDomain()
            : base(new bool[2] { true, false })
        { }
    }

    public sealed class Integer32FiniteEnumerableAttributeDomain : FiniteEnumerableAttributeDomain<int>
    {
        public Integer32FiniteEnumerableAttributeDomain(IEnumerable<int> universum)
            : base(universum)
        { }

        public Integer32FiniteEnumerableAttributeDomain(params (int, int)[] ranges)
            : base(new bool[2] { true, false })
        { }

        private IEnumerable<int> Build((int, int)[] ranges)
        {
            Comparison<(int Start, int End)> rangeComparisonByRangeStart = 
                (range1, range2) => range1.Start.CompareTo(range2.Start),
                                             rangeComparisonByRangeEnd =
                (range1, range2) => range1.End.CompareTo(range2.End);

            List<(int, int)> rangesBefore = new List<(int, int)>(ranges),
                             rangesBeforeSortedByRangeStart = rangesBefore.Select(r => r) as List<(int, int)>,
                             rangesBeforeSortedByRangeEnd = rangesBefore.Select(r => r) as List<(int, int)>;
            
            .Sort(rangeComparisonByRangeStart);
        }
    }

    public sealed class Integer64FiniteEnumerableAttributeDomain : FiniteEnumerableAttributeDomain<long>
    {
        public Integer64FiniteEnumerableAttributeDomain(IEnumerable<long> universum)
            : base(universum)
        { }

        public Integer64FiniteEnumerableAttributeDomain()
            : base(new bool[2] { true, false })
        { }
    }
}
