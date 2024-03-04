using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    public interface ISequenceComparer<TData, TOperand1, TOperand2>
        : ISequenceOperator<TData, TOperand1, TOperand2>
        where TOperand1 : IEnumerable<TData>
        where TOperand2 : IEnumerable<TData>
    {
        sealed bool SequenceEquals(
            TOperand1 first,
            TOperand2 second)
        {
            bool equals;

            SequenceIncludeOrEquals(
                first,
                second,
                out equals,
                out _);

            return equals;
        }

        sealed bool SequenceInclude(
            TOperand1 first,
            TOperand2 second)
        {
            bool include;

            SequenceIncludeOrEquals(
                first,
                second,
                out _,
                out include);

            return include;
        }

        sealed bool SequenceIncludeOrEquals(
            TOperand1 first,
            TOperand2 second)
        {
            bool include, equals;

            SequenceIncludeOrEquals(
                first,
                second,
                out equals,
                out include);

            return include | equals;
        }

        private void SequenceIncludeOrEquals(
            TOperand1 greater,
            TOperand2 lower,
            out bool include,
            out bool equals)
        {
            IEnumerable<TData> streaming;
            HashSet<TData> buffered, occurences = null;
            bool wereSwitched;

            GetStreamingAndBuffered(
                greater,
                lower,
                out streaming,
                out buffered,
                out wereSwitched);
            occurences = new HashSet<TData>(buffered.Count);

            equals = true;
            bool bufferedIsSubsetOfStreaming = false,
                 streamingIsSubsetOfBuffered = false;

            foreach (TData data in streaming)
            {
                if (buffered.Count == 0)
                {
                    break;
                }

                if (buffered.Contains(data))
                {
                    buffered.Remove(data);
                    occurences.Add(data);
                }
                else if (!occurences.Contains(data))
                    equals = false;
            }

            if (equals && buffered.Count > 0)
            {
                equals = false;
                streamingIsSubsetOfBuffered = true;
            }

            include = wereSwitched & streamingIsSubsetOfBuffered |
                      !wereSwitched & bufferedIsSubsetOfStreaming;

            return;
        }
    }
}
