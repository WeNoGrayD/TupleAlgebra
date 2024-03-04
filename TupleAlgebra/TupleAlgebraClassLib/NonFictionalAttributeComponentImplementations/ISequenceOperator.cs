using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    public interface ISequenceOperator<TData, TOperand1, TOperand2>
        where TOperand1 : IEnumerable<TData>
        where TOperand2 : IEnumerable<TData>
    {
        protected virtual void GetStreamingAndBuffered(
            TOperand1 first,
            TOperand2 second,
            out IEnumerable<TData> streaming,
            out HashSet<TData> buffered,
            out bool wereSwitched)
        {
            int bufferedLen;

            if (first.TryGetNonEnumeratedCount(out bufferedLen))
            {
                streaming = second;
                buffered = first.ToHashSet();
                wereSwitched = true;
            }
            else
            {
                streaming = first;
                buffered = second.ToHashSet();
                wereSwitched = false;
            }

            return;
        }
    }
}
