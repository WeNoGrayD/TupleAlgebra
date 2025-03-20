using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Complex;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex
{
    public interface IComplexAttributeComponentInstantBinaryBooleanOperator<TData>
        : IAttributeComponentBooleanOperator<
            TData,
            ComplexAttributeComponent<TData>,
            ComplexAttributeComponent<TData>>
        where TData : new()
    {
        bool IInstantBinaryOperator<
            ComplexAttributeComponent<TData>,
            ComplexAttributeComponent<TData>,
            bool>
            .Visit(
                ComplexAttributeComponent<TData> first,
                ComplexAttributeComponent<TData> second)
        {
            return Operator(first.Tuple, second.Tuple);
        }

        protected abstract bool Operator(
                TupleObject<TData> first,
                TupleObject<TData> second);
    }
}
