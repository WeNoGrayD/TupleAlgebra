using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable
{
    public interface ICountableAttributeComponentEqualityComparer<TData>
        : IAttributeComponentBooleanOperator<
            TData,
            ICountableAttributeComponent<TData>,
            ICountableAttributeComponent<TData>>,
          ICountableSequenceComparer<TData>
    {
        bool IInstantBinaryOperator<
            ICountableAttributeComponent<TData>,
            ICountableAttributeComponent<TData>,
            bool>
            .Accept(
                ICountableAttributeComponent<TData> greater,
                ICountableAttributeComponent<TData> lower)
        {
            return SequenceEquals(greater, lower);
        }
    }
}
