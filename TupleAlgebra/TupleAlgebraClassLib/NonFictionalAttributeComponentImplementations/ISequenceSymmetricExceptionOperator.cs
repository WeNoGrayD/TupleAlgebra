using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

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
            IEnumerable<TData>,
            TOperand1,
            TOperand2,
            TFactory,
            TFactoryArgs,
            IAttributeComponent<TData>>,
          ISequenceOperator<
            TData,
            TOperand1,
            TOperand2>
        where TOperand1 : NonFictionalAttributeComponent<TData>
        where TOperand2 : IAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, IEnumerable<TData>, TOperand1, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    {
        IAttributeComponent<TData>
             IFactoryBinaryOperator<
                TOperand1,
                TOperand2,
                TFactory,
                IAttributeComponent<TData>>
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
                    else 
                    {
                        yield return data;
                    }
                }

                foreach (TData data in buffered)
                    yield return data;

                yield break;
            }
        }
    }
}
