using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable
{
    public interface IFiniteEnumerableSequenceOperator<
        TData,
        TOperand1>
        : ISequenceOperator<
            TData, 
            TOperand1,
            IFiniteEnumerableAttributeComponent<TData>>
        where TOperand1 : IFiniteEnumerableAttributeComponent<TData>
    {
        void ISequenceOperator<
            TData,
            TOperand1,
            IFiniteEnumerableAttributeComponent<TData>>
            .GetStreamingAndBuffered(
                TOperand1 first,
                IFiniteEnumerableAttributeComponent<TData> second,
                out IEnumerable<TData> streaming,
                out HashSet<TData> buffered,
                out bool wereSwitched)
        {
            streaming = first;
            buffered = second.Values.ToHashSet();
            wereSwitched = false;

            return;
        }
    }

    public interface IFiniteEnumerableSequenceComparer<TData>
        : ISequenceComparer<
            TData,
            IFiniteEnumerableAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>>,
          IFiniteEnumerableSequenceOperator<
            TData,
            IFiniteEnumerableAttributeComponent<TData>>
    { }
}
