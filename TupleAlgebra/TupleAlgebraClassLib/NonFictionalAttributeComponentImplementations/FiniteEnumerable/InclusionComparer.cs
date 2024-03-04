using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable
{
    public interface IFiniteEnumerableAttributeComponentInclusionComparer<TData>
        : IAttributeComponentBooleanOperator<
            TData,
            IFiniteEnumerableAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>>,
          IFiniteEnumerableSequenceComparer<TData>
    {
        bool IInstantBinaryOperator<
            IFiniteEnumerableAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>,
            bool>
            .Accept(
                IFiniteEnumerableAttributeComponent<TData> greater,
                IFiniteEnumerableAttributeComponent<TData> lower)
        {
            return SequenceInclude(greater, lower);
        }
    }
}
