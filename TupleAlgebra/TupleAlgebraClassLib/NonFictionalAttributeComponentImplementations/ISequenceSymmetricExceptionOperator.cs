using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    public interface IAttributeComponentSymmetricExceptionOperator<
        TData,
        TOperand1,
        TOperand2,
        TFactory,
        TFactoryArgs>
        : IFactoryBinaryAttributeComponentAcceptor<
            TData,
            TOperand1,
            TOperand2,
            TFactory,
            TFactoryArgs,
            AttributeComponent<TData>>,
          ISequenceOperator<
            TData,
            TOperand1,
            TOperand2>
        where TOperand1 : NonFictionalAttributeComponent<TData>
        where TOperand2 : IAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TOperand1, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    {
        AttributeComponent<TData>
             IFactoryBinaryOperator<
                TOperand1,
                TOperand2,
                TFactory,
                AttributeComponent<TData>>
            .Accept(
                TOperand1 first,
                TOperand2 second,
                TFactory factory)
        {
            IEnumerable<TData> resultElements = SymmetricExcept();

            return factory.CreateNonFictional(first, resultElements);

            IEnumerable<TData> SymmetricExcept()
            {
                IEnumerable<TData> streaming;
                HashSet<TData> buffered, occurences = null;

                GetStreamingAndBuffered(
                    first,
                    second,
                    out streaming,
                    out buffered,
                    out _);
                occurences = new HashSet<TData>(buffered.Count);

                foreach (TData data in streaming)
                {
                    if (occurences.Contains(data)) continue;

                    if (buffered.Count == 0)
                    {
                        yield return data;
                    }
                    else if (buffered.Contains(data))
                    {
                        buffered.Remove(data);
                        occurences.Add(data);
                    }
                }

                foreach (TData data in buffered)
                    yield return data;

                yield break;
            }
        }
    }
}
