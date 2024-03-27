using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable
{
    public interface IOrderedFiniteEnumerableAttributeComponentFactory<TData, in TAttributeComponent, TFactoryArgs>
        : IEnumerableNonFictionalAttributeComponentFactory<
              TData,
              TAttributeComponent,
              TFactoryArgs>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IOrderedFiniteEnumerableAttributeComponent<TData>
        where TFactoryArgs : OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
    {
        TFactoryArgs
            ISetOperationResultFactory<
                TAttributeComponent,
                IEnumerable<TData>,
                TFactoryArgs,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
                    TAttributeComponent first,
                    IEnumerable<TData> resultElements)
        {
            var factoryArgs =
                CreateFactoryArgs_DefaultImpl(first, resultElements);
            factoryArgs.ValuesAreOrdered = true;

            return factoryArgs;
        }
    }

    public class OrderedFiniteEnumerableAttributeComponentFactory<TData>
        : AttributeComponentFactory<TData, BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IStreamingOrderedFiniteEnumerableAttributeComponentFactory<TData>,
          IBufferedOrderedFiniteEnumerableAttributeComponentFactory<TData>
    {
        public OrderedFiniteEnumerableAttributeComponentFactory(AttributeDomain<TData> domain)
            : base(domain)
        { }

        public OrderedFiniteEnumerableAttributeComponentFactory(
            BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData> factoryArgs)
            : base(factoryArgs)
        { }

        public OrderedFiniteEnumerableAttributeComponentFactory(
            IEnumerable<TData> universeData)
            : this(new BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(values: universeData))
        { }

        AttributeComponent<TData>
            INonFictionalAttributeComponentFactory<
                TData,
                IEnumerable<TData>>
            .CreateNonFictional(IEnumerable<TData> values)
        {
            IStreamingOrderedFiniteEnumerableAttributeComponentFactory<TData>
                streamingFactory = this;

            return streamingFactory.ProduceOperationResult(values);
        }
    }
}
