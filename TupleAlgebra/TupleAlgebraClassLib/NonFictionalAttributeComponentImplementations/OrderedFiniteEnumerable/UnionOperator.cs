using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public abstract class UnionOperator<TData, CTOperand1, TFactory, TFactoryArgs>
        : NonFictionalAttributeComponentUnionOperator<TData, CTOperand1, TFactory, TFactoryArgs>,
          IOrderedFiniteEnumerableAttributeComponentBinaryOperator<TData, CTOperand1, AttributeComponent<TData>, TFactory, TFactoryArgs>,
          IOrderedFiniteEnumerableAttributeComponentBinaryOperator<TData, CTOperand1, TFactory, TFactoryArgs>,
          IFiniteEnumerableAttributeComponentUnionOperator<TData, CTOperand1, TFactory, TFactoryArgs>
        where CTOperand1 : NonFictionalAttributeComponent<TData>, IOrderedFiniteEnumerableAttributeComponent<TData>
        where TFactoryArgs : OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
        where TFactory : IOrderedFiniteEnumerableAttributeComponentFactory<TData, CTOperand1, TFactoryArgs>
    {
        public AttributeComponent<TData> Accept(
            CTOperand1 first,
            IOrderedFiniteEnumerableAttributeComponent<TData> second,
            TFactory factory)
        {
            IEnumerable<TData> unitedElements = UnionComponentsElements();

            return factory.CreateNonFictional(first, unitedElements);

            IEnumerable<TData> UnionComponentsElements()
            {
                IEnumerator<TData> firstEnumerator = first.GetEnumerator(),
                                   secondEnumerator = second.GetEnumerator(),
                                   withLowerBoundEnumerator = firstEnumerator,
                                   withGreaterBoundEnumerator = secondEnumerator;
                bool isContinuesWithLowerBoundEnumerator = true,
                     isContinuesWithGreaterBoundEnumerator = true;
                TData firstElement = default(TData), secondElement = default(TData);
                IComparer<TData> orderingComparer = first.OrderingComparer;
                int elementsComparisonResult;

                foreach (TData resultData in ReadComponentsUntilAtLeastOneIsOver())
                    yield return resultData;

                foreach (TData resultData in FinishReadingIfAnyComponentRemains())
                    yield return resultData;

                DisposeEnumerators();

                yield break;

                IEnumerable<TData> ReadComponentsUntilAtLeastOneIsOver()
                {
                    WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();

                    while (isContinuesWithLowerBoundEnumerator && isContinuesWithGreaterBoundEnumerator)
                    {
                        elementsComparisonResult = orderingComparer.Compare(firstElement, secondElement);
                        switch (elementsComparisonResult)
                        {
                            case -1: break;
                            case 0:
                                {
                                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();
                                    break;
                                }
                            case 1:
                                {
                                    SwapEnumeratorsAndCurrentElements();
                                    break;
                                }
                        }
                        yield return firstElement;
                        WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    }

                    yield break;
                }

                IEnumerable<TData> FinishReadingIfAnyComponentRemains()
                {
                    if (isContinuesWithLowerBoundEnumerator || isContinuesWithGreaterBoundEnumerator)
                    {
                        if (isContinuesWithGreaterBoundEnumerator)
                            SwapEnumeratorsAndCurrentElements();
                        return FinishReadingRemainingComponent();
                    }

                    return Enumerable.Empty<TData>();
                }

                IEnumerable<TData> FinishReadingRemainingComponent()
                {
                    do
                    {
                        yield return firstElement;
                        WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    }
                    while (isContinuesWithLowerBoundEnumerator);

                    yield break;
                }

                void SwapEnumeratorsAndCurrentElements()
                {
                    (withLowerBoundEnumerator, withGreaterBoundEnumerator) =
                        (withGreaterBoundEnumerator, withLowerBoundEnumerator);
                    (firstElement, secondElement) = (secondElement, firstElement);

                    return;
                }

                void WithLowerBoundEnumeratorMoveNextAndReadCurrent()
                {
                    if (isContinuesWithLowerBoundEnumerator = withLowerBoundEnumerator.MoveNext())
                        firstElement = withLowerBoundEnumerator.Current;

                    return;
                }

                void WithGreaterBoundEnumeratorMoveNextAndReadCurrent()
                {
                    if (isContinuesWithGreaterBoundEnumerator = withGreaterBoundEnumerator.MoveNext())
                        secondElement = withGreaterBoundEnumerator.Current;

                    return;
                }

                void DisposeEnumerators()
                {
                    firstEnumerator.Dispose();
                    secondEnumerator.Dispose();

                    return;
                }
            }
        }
    }
}
