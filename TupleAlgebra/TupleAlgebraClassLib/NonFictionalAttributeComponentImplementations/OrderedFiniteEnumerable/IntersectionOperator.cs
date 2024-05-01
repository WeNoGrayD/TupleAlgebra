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
    public abstract class IntersectionOperator<TData, CTOperand1, TFactory, TFactoryArgs>
        : NonFictionalAttributeComponentIntersectionOperator<TData, IEnumerable<TData>, CTOperand1, TFactory, TFactoryArgs>,
          IOrderedFiniteEnumerableAttributeComponentBinaryOperator<TData, CTOperand1, TFactory, TFactoryArgs>,
          IFiniteEnumerableAttributeComponentIntersectionOperator<TData, CTOperand1, TFactory, TFactoryArgs>
        where CTOperand1 : NonFictionalAttributeComponent<TData>, IOrderedFiniteEnumerableAttributeComponent<TData>
        where TFactoryArgs : OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
        where TFactory : IOrderedFiniteEnumerableAttributeComponentFactory<TData, CTOperand1, TFactoryArgs>
    {
        public IAttributeComponent<TData> Accept(
            CTOperand1 first,
            IOrderedFiniteEnumerableAttributeComponent<TData> second,
            TFactory factory)
        {
            OperationResultEnumerableResultProvider<TData> intersectedElements = 
                new OperationResultEnumerableResultProvider<TData>(
                    IntersectComponentsElements(), true);

            return factory.CreateNonFictional(first, intersectedElements);

            IEnumerable<TData> IntersectComponentsElements()
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

                return ReadComponentsUntilAtLeastOneIsOver();

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
                                    yield return firstElement;
                                    WithGreaterBoundEnumeratorMoveNextAndReadCurrent();
                                    
                                    break;
                                }
                            case 1:
                                {
                                    SwapEnumeratorsAndCurrentElements();
                                    
                                    break;
                                }
                        }
                        WithLowerBoundEnumeratorMoveNextAndReadCurrent();
                    }

                    DisposeEnumerators();

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
