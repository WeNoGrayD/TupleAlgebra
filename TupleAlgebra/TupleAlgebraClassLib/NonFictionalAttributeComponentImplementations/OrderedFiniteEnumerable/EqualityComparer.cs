using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public abstract class EqualityComparer<TData, TAttributeComponent>
        : NonFictionalAttributeComponentEqualityComparer<TData, TAttributeComponent>,
          IOrderedFiniteEnumerableAttributeComponentBooleanOperator<TData, TAttributeComponent, AttributeComponent<TData>>,
          IOrderedFiniteEnumerableAttributeComponentBooleanOperator<TData, TAttributeComponent>,
          IFiniteEnumerableAttributeComponentEqualityComparer<TData>,
          ICountableAttributeComponentEqualityComparer<TData>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IOrderedFiniteEnumerableAttributeComponent<TData>
    {
        public bool Accept(
            TAttributeComponent first,
            IOrderedFiniteEnumerableAttributeComponent<TData> second)
        {
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
