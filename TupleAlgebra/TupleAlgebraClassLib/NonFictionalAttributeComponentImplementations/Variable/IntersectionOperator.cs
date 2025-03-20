using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable
{
    public class IntersectionOperator<TData>
        : NonFictionalAttributeComponentIntersectionOperator<
            TData,
            VariableAttributeComponentFactoryArgs<TData>,
            VariableAttributeComponent<TData>,
            IVariableAttributeComponentFactory<TData>,
            VariableAttributeComponentFactoryArgs<TData>>,
          IFactoryBinaryOperator<
              VariableAttributeComponent<TData>,
              NonFictionalAttributeComponent<TData>,
              IAttributeComponentFactory<TData>,
              IAttributeComponent<TData>>
    {
        /*
        public IAttributeComponent<TData> Visit(
            VariableAttributeComponent<TData> first,
            VariableAttributeComponent<TData> second,
            IAttributeComponentFactory<TData> factory)
        {
            first.CurrentValue = (AttributeComponent<TData>)
                (first.CurrentValue & second.CurrentValue);

            return first;
        }
        */

        public IAttributeComponent<TData> Visit(
            VariableAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second,
            IAttributeComponentFactory<TData> factory)
        {
            first.CurrentValue = (AttributeComponent<TData>)
                (first.CurrentValue & second);

            return first.CurrentValue;
        }
    }
}
