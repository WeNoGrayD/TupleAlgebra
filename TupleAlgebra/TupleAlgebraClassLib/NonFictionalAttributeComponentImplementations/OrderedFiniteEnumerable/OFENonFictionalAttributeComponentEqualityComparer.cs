using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public sealed class OrderedFiniteEnumerableNonFictionalAttributeComponentEqualityComparer<TData>
        : NonFictionalAttributeComponentEqualityComparer<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>>,
          IInstantBinaryAttributeComponentAcceptor<TData, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>, bool>
    {
        public bool Accept(
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> first, 
            OrderedFiniteEnumerableNonFictionalAttributeComponent<TData> second)
        {
            //return Enumerable.SequenceEqual(first, second);
            IEnumerator<TData> firstEnumerator = first.GetEnumerator(),
                               secondEnumerator = second.GetEnumerator();
            TData firstElement = default(TData), secondElement = default(TData);
            bool isFirstEnumeratorContinue = true,
                 isSecondEnumeratorContinue = true,
                 areEqual = true;

            ReadComponentsUntilAtLeastOneIsOver();
            DisposeEnumerators();
            if (areEqual) areEqual = !isFirstEnumeratorContinue && !isSecondEnumeratorContinue;

            return areEqual;

            void ReadComponentsUntilAtLeastOneIsOver()
            {
                IComparer<TData> orderingComparer = first.OrderingComparer;
                int elementsComparisonResult;

                FirstEnumeratorMoveNextAndReadCurrent();
                SecondEnumeratorMoveNextAndReadCurrent();

                while (isFirstEnumeratorContinue && isSecondEnumeratorContinue)
                {
                    elementsComparisonResult = orderingComparer.Compare(firstElement, secondElement);
                    areEqual = (elementsComparisonResult == 0);
                    if (areEqual)
                    {
                        FirstEnumeratorMoveNextAndReadCurrent();
                        SecondEnumeratorMoveNextAndReadCurrent();
                    }
                    else
                        break;
                }

                return;
            }

            void FirstEnumeratorMoveNextAndReadCurrent()
            {
                if (isFirstEnumeratorContinue = firstEnumerator.MoveNext())
                    firstElement = firstEnumerator.Current;

                return;
            }

            void SecondEnumeratorMoveNextAndReadCurrent()
            {
                if (isSecondEnumeratorContinue = secondEnumerator.MoveNext())
                    secondElement = secondEnumerator.Current;

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
