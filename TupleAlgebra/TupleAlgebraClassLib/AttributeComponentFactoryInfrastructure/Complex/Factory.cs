using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Complex
{
    public interface IComplexAttributeComponentFactory<TData>
        : IEnumerableNonFictionalAttributeComponentFactory<
              TData,
              ComplexAttributeComponent<TData>,
              ComplexAttributeComponentFactoryArgs<TData>>
        where TData : new()
    {
        TupleObjectFactory TupleFactory { get; }

        ComplexAttributeComponentFactoryArgs<TData>
            ISetOperationResultFactory<
                ComplexAttributeComponent<TData>,
                IEnumerable<TData>,
                ComplexAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
                IEnumerable<TData> resultElements)
        {
            return new ComplexAttributeComponentFactoryArgs<TData>(
                resultElements is TupleObject<TData> to ? 
                to :
                TupleFactory.CreateConjunctiveTupleSystem(resultElements));
        }

        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                ComplexAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictional(
                ComplexAttributeComponentFactoryArgs<TData> args)
        {
            return new ComplexAttributeComponent<TData>(
                args.Power,
                args.Values,
                args.QueryProvider,
                args.QueryExpression);
        }

        AttributeComponent<TData>
            ISetOperationResultFactory<
                ComplexAttributeComponent<TData>,
                IEnumerable<TData>,
                ComplexAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .ProduceOperationResult(
                    ComplexAttributeComponent<TData> first,
                    IEnumerable<TData> resultElements)
        {
            return CreateSpecificNonFictional(
                new ComplexAttributeComponentFactoryArgs<TData>(
                    resultElements is TupleObject<TData> to ?
                    to :
                    TupleFactory.CreateConjunctiveTupleSystem(resultElements)));
        }
    }

    public class ComplexAttributeComponentFactory<TData>
        : AttributeComponentFactory<TData, ComplexAttributeComponentFactoryArgs<TData>>,
          IComplexAttributeComponentFactory<TData>
        where TData : new()
    {
        private static TupleObjectFactory _tupleFactory = new(null);

        public TupleObjectFactory TupleFactory { get => _tupleFactory; }

        public ComplexAttributeComponentFactory(AttributeDomain<TData> domain)
            : base(domain)
        { }

        public ComplexAttributeComponentFactory()
            : base(new ComplexAttributeComponentFactoryArgs<TData>(
                _tupleFactory.CreateFull<TData>()))
        { }
    }
}
