using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable
{
    public interface ICountableAttributeComponentInclusionOrEqualityComparer<TData>
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
            .Visit(
                ICountableAttributeComponent<TData> greater,
                ICountableAttributeComponent<TData> lower)
        {
            return SequenceIncludeOrEquals(greater, lower);
        }
    }
}
