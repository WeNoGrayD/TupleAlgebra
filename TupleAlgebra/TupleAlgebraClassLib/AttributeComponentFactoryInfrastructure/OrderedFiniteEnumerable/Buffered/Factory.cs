using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.TupleBased;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered
{
    public interface IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>
        : IOrderedFiniteEnumerableAttributeComponentFactory<
              TData,
              BufferedOrderedFiniteEnumerableAttributeComponent<TData>,
              BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IVariableAttributeComponentFactory<TData>,
          ITupleBasedAttributeComponentFactory<TData>,
          IFilteringAttributeComponentFactory<TData>
    {
        BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
            INonFictionalAttributeComponentFactory<
                TData,
                IEnumerable<TData>,
                BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictionalFactoryArgs(
                IEnumerable<TData> resultElements)
        {
            return new BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(
                resultElements,
                valuesAreOrdered: true);
        }


        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictional(
                BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
                    args)
        {
            return new BufferedOrderedFiniteEnumerableAttributeComponent<TData>(
                args.Power,
                args.Values,
                args.OrderingComparer,
                args.ValuesAreOrdered,
                args.QueryProvider,
                args.QueryExpression);
        }
    }
}
