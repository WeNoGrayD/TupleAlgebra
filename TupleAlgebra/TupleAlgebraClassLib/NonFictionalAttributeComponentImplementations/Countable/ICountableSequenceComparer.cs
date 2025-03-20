using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable
{
    public interface ICountableSequenceOperator<TData, TOperand1>
        : ISequenceOperator<
            TData,
            TOperand1,
            ICountableAttributeComponent<TData>>
        where TOperand1 : ICountableAttributeComponent<TData>
    {
        void ISequenceOperator<
            TData,
            TOperand1,
            ICountableAttributeComponent<TData>>
            .GetStreamingAndBuffered(
                TOperand1 first,
                ICountableAttributeComponent<TData> second,
                out IEnumerable<TData> streaming,
                out HashSet<TData> buffered,
                out bool wereSwitched)
        {
            if (first.Count < second.Count)
            {
                streaming = second;
                buffered = first.Values.ToHashSet();
                wereSwitched = true;
            }
            else
            {
                streaming = first;
                buffered = second.Values.ToHashSet();
                wereSwitched = false;
            }

            return;
        }
    }

    public interface ICountableSequenceComparer<TData>
        : ISequenceComparer<
            TData,
            ICountableAttributeComponent<TData>,
            ICountableAttributeComponent<TData>>,
          ICountableSequenceOperator<TData, ICountableAttributeComponent<TData>>
    { }
}
