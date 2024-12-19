using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Complex;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex
{
    public interface IComplexAttributeComponentFactoryBinarySetOperator<
        TData>
        : IFactoryBinaryAttributeComponentVisitor<
            TData,
            TupleObject<TData>,
            ComplexAttributeComponent<TData>,
            ComplexAttributeComponent<TData>,
            IComplexAttributeComponentFactory<TData>,
            ComplexAttributeComponentFactoryArgs<TData>,
            IAttributeComponent<TData>>
        where TData : new()
    {
        IAttributeComponent<TData>
             IFactoryBinaryOperator<
                ComplexAttributeComponent<TData>,
                ComplexAttributeComponent<TData>,
                IComplexAttributeComponentFactory<TData>,
                IAttributeComponent<TData>>
            .Visit(
                ComplexAttributeComponent<TData> first,
                ComplexAttributeComponent<TData> second,
                IComplexAttributeComponentFactory<TData> factory)
        {
            return factory.CreateNonFictional(Operator(first.Tuple, second.Tuple));
        }

        protected abstract TupleObject<TData> Operator(
            TupleObject<TData> first,
            TupleObject<TData> second);
    }
}
