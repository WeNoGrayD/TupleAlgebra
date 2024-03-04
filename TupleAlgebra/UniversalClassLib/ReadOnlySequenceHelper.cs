using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace UniversalClassLib;

public static class ReadOnlySequenceHelper
{
    public static IEnumerable<TData> ToEnumerable<TData>(
        this ReadOnlySequence<TData> sequence)
    {
        if (sequence.IsEmpty)
            return Enumerable.Empty<TData>();

        return ToNonEmptyEnumerable(sequence);
    }

    public static int GetLength<TData>(
        this ReadOnlySequence<TData> sequence)
    {
        if (sequence.IsEmpty)
            return 0;

        int len = 0;

        foreach (var mem in sequence)
        {
            len += mem.Length;
        }

        return len;
    }

    private static IEnumerable<TData> ToNonEmptyEnumerable<TData>(
        this ReadOnlySequence<TData> sequence)
    {
        ReadOnlySpan<TData> span;
        int i;

        foreach (var mem in sequence)
        {
            for (i = 0; i < mem.Length; i++)
                yield return GetData(mem, i);
        }

        yield break;
    }

    private static TData GetData<TData>(
        ReadOnlyMemory<TData> memory,
        int dataIndex)
    {
        return memory.Span[dataIndex];
    }

    private static TData GetData<TData>(
        ReadOnlySequence<TData> source,
        SequencePosition dataPos)
    {
        ReadOnlyMemory<TData> mem;
        source.TryGet(ref dataPos, out mem, false);

        return mem.Span[0];
    }

    public static ReadOnlySequence<TData>
        ToSubSequence<TData>(
            this IEnumerable<TData> values,
            ReadOnlySequence<TData> superSequence,
            IComparer<TData> orderingComparer)
    {
        if (superSequence.IsSingleSegment)
        {
            return ToSubSequenceFromSingleSegmentSuperSequence(
                values, superSequence, orderingComparer);
        }
        return ToSubSequenceFromSegmentedSuperSequence(
            values, superSequence, orderingComparer);
    }

    public static ReadOnlySequence<TData>
        ToSubSequenceFromSingleSegmentSuperSequence<TData>(
            this IEnumerable<TData> values,
            ReadOnlySequence<TData> sequence,
            IComparer<TData> orderingComparer)
    {
        if (sequence.IsEmpty) return sequence;

        ReadOnlyMemory<TData> currentROM;
        ReadOnlyChunk<TData> currentChunk = new ReadOnlyChunk<TData>();
        ReadOnlySequenceSegment<TData> startChunk = currentChunk;
        int startPos = 0, endPos = sequence.First.Length;
        bool isContiguous = false;
        SequencePosition? startPosition = null, endPosition = null;
        TData foundData;

        foreach (TData data in values)
        {
            if (isContiguous)
            {
                foundData = GetData(
                    sequence,
                    endPosition.Value);

                if (orderingComparer.Compare(data, foundData) == 0)
                {
                    endPosition = sequence
                        .GetPosition(1, endPosition.Value);

                    continue;
                }
                else
                {
                    UpdateChunk();
                }
            }

            startPosition = BinarySearchWithoutRecursion(
                data,
                sequence,
                orderingComparer,
                ref startPos,
                endPos);
            endPosition = sequence
                .GetPosition(1, startPosition.Value);
            isContiguous = true;
        }
        /*
         * Обновляем последний чанк, если перечисление values
         * не было пустым.
         */
        if (isContiguous)
        {
            UpdateChunk();

            startChunk = startChunk.Next;

            return new ReadOnlySequence<TData>(
                startChunk, 0, currentChunk, currentChunk.Memory.Length);
        }

        return new ReadOnlySequence<TData>();

        void UpdateChunk()
        {
            currentROM = sequence
                .Slice(startPosition.Value, endPosition.Value)
                .First;
            currentChunk = currentChunk.Append(currentROM);

            return;
        }
    }

    public static ReadOnlySequence<TData>
        ToSubSequenceFromSegmentedSuperSequence<TData>(
            this IEnumerable<TData> values,
            ReadOnlySequence<TData> sequence,
            IComparer<TData> orderingComparer)
    {
        ReadOnlyMemory<TData> currentROM = null;
        ReadOnlyChunk<TData> currentChunk = new ReadOnlyChunk<TData>();
        ReadOnlySequenceSegment<TData> startChunk = currentChunk;
        int startPos = 0, endPos = (int)sequence.GetOffset(sequence.End);
        bool isContiguous = false;
        SequencePosition? startPosition = null, endPosition = null;
        SequencePosition startPosRef;
        TData foundData;
        int continuityLen = 0;

        foreach (TData data in values)
        {
            if (isContiguous)
            {
                if (continuityLen == currentROM.Length)
                {
                    UpdateChunk();
                }
                else
                {
                    continuityLen++;

                    foundData = GetData(
                        sequence,
                        endPosition.Value);

                    if (orderingComparer.Compare(data, foundData) == 0)
                    {
                        endPosition = sequence
                            .GetPosition(1, endPosition.Value);

                        continue;
                    }
                    else
                    {
                        UpdateChunk();
                    }
                }
            }

            startPosition = BinarySearchWithoutRecursion(
                data,
                sequence,
                orderingComparer,
                ref startPos,
                endPos);
            endPosition = sequence
                .GetPosition(1, startPosition.Value);
            isContiguous = true;
            startPosRef = startPosition.Value;
            sequence.TryGet(
                ref startPosRef, out currentROM, false);
            continuityLen = 1;
        }
        /*
         * Обновляем последний чанк, если перечисление values
         * не было пустым.
         */
        if (continuityLen > 0)
            UpdateChunk();

        startChunk = startChunk.Next;

        return new ReadOnlySequence<TData>(
            startChunk, 0, currentChunk, currentChunk.Memory.Length);

        void UpdateChunk()
        {
            currentROM = currentROM
                .Slice(0, continuityLen);
            currentChunk = currentChunk.Append(currentROM);
            continuityLen = 0;

            return;
        }
    }

    private static SequencePosition? BinarySearch<TData>(
        TData data,
        ReadOnlySequence<TData> source,
        IComparer<TData> orderingComparer,
        ref int startOffset,
        int endOffset)
    {
        if (startOffset >= endOffset) return null;

        int midOffset = startOffset + ((endOffset - startOffset) >> 1);
        SequencePosition midPosition = source.GetPosition(midOffset);
        TData midData = GetData(source, midPosition);

        switch (orderingComparer.Compare(data, midData))
        {
            case -1:
                {
                    return BinarySearch(
                        data,
                        source,
                        orderingComparer,
                        ref startOffset,
                        midOffset);
                }
            case 0:
                {
                    startOffset = midOffset + 1;

                    return midPosition;
                }
            case 1:
                {
                    SequencePosition? pos = BinarySearch(
                        data,
                        source,
                        orderingComparer,
                        ref midOffset,
                        endOffset);

                    startOffset = midOffset;

                    return pos;
                }
        }

        return null;
    }

    private static SequencePosition? BinarySearchWithoutRecursion<TData>(
        TData data,
        ReadOnlySequence<TData> source,
        IComparer<TData> orderingComparer,
        ref int startOffset0,
        int endOffset)
    {
        int startOffset = startOffset0;
        int midOffset;
        ReadOnlyMemory<TData> mem;
        SequencePosition midPosition = default;
        TData midData;
        bool resultIsGained = false;

        while (!resultIsGained)
        {
            if (startOffset >= endOffset) return null;

            midOffset = startOffset + ((endOffset - startOffset) >> 1);
            midPosition = source.GetPosition(midOffset);
            midData = GetData(source, midPosition);

            switch (orderingComparer.Compare(data, midData))
            {
                case -1:
                    {
                        endOffset = midOffset;

                        break;
                    }
                case 0:
                    {
                        resultIsGained = true;
                        startOffset = midOffset + 1;

                        break;
                    }
                case 1:
                    {
                        startOffset = midOffset;

                        break;
                    }
            }
        }

        startOffset0 = startOffset;

        return midPosition;
    }

    private sealed class ReadOnlyChunk<T> : ReadOnlySequenceSegment<T>
    {
        public ReadOnlyChunk() { }

        public ReadOnlyChunk(ReadOnlyMemory<T> memory)
        {
            Memory = memory;
        }

        public ReadOnlyChunk<T> Append(ReadOnlyMemory<T> memory)
        {
            ReadOnlyChunk<T> nextChunk = new ReadOnlyChunk<T>(memory)
            {
                RunningIndex = RunningIndex + Memory.Length
            };

            Next = nextChunk;

            return nextChunk;
        }
    }
}
