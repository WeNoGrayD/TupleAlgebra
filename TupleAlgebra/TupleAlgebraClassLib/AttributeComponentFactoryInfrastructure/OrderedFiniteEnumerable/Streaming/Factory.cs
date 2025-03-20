using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.TupleBased;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using static UniversalClassLib.ReadOnlySequenceHelper;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming
{
    public interface IStreamingOrderedFiniteEnumerableAttributeComponentFactory<TData>
        : IOrderedFiniteEnumerableAttributeComponentFactory<
              TData,
              StreamingOrderedFiniteEnumerableAttributeComponent<TData>,
              StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IVariableAttributeComponentFactory<TData>,
          ITupleBasedAttributeComponentFactory<TData>,
          IFilteringAttributeComponentFactory<TData>
    {
        StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
            ISetOperationResultFactory<
                StreamingOrderedFiniteEnumerableAttributeComponent<TData>,
                IEnumerable<TData>,
                StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
                IEnumerable<TData> resultElements)
        {
            return new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(
                resultElements,
                valuesAreOrdered: true);
        }

        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictional(
                StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
                    args)
        {
            return new StreamingOrderedFiniteEnumerableAttributeComponent<TData>(
                args.Power,
                args.Values,
                args.OrderingComparer,
                args.ValuesAreOrdered,
                args.QueryProvider,
                args.QueryExpression);
        }
    }
}
