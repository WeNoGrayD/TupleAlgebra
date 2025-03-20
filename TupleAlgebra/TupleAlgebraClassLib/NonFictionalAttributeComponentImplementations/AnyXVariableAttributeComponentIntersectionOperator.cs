using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using System.ComponentModel;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    public interface IAnyXVariableAttributeComponentIntersectionOperator<TData>
        : IFactoryBinaryOperator<
            NonFictionalAttributeComponent<TData>,
            VariableAttributeComponent<TData>,
            IAttributeComponentFactory<TData>,
            IAttributeComponent<TData>>
    {
        IAttributeComponent<TData> IFactoryBinaryOperator<
            NonFictionalAttributeComponent<TData>,
            VariableAttributeComponent<TData>,
            IAttributeComponentFactory<TData>,
            IAttributeComponent<TData>>
            .Visit(
            NonFictionalAttributeComponent<TData> first,
            VariableAttributeComponent<TData> second,
            IAttributeComponentFactory<TData> factory)
        {
            return first & second.CurrentValue;
        }
    }
}
