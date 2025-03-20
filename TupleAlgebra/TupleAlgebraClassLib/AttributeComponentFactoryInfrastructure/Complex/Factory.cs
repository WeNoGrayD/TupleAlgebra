using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Complex
{
    public interface IComplexAttributeComponentFactory<TData>
        : /*IEnumerableNonFictionalAttributeComponentFactory<
              TData,
              ComplexAttributeComponent<TData>,
              ComplexAttributeComponentFactoryArgs<TData>>,*/
          INonFictionalAttributeComponentFactory<
              TData,
              TupleObject<TData>,
              ComplexAttributeComponent<TData>,
              ComplexAttributeComponentFactoryArgs<TData>>,
          IFiniteIterableAttributeComponentFactory<TData>
        where TData : new()
    {
        TupleObjectFactory TupleFactory { get; }

        /*ComplexAttributeComponentFactoryArgs<TData>
            INonFictionalAttributeComponentFactory<
                TData,
                IEnumerable<TData>,
                ComplexAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictionalFactoryArgs(
                IEnumerable<TData> resultElements)
        {
            return new ComplexAttributeComponentFactoryArgs<TData>(
                resultElements is TupleObject<TData> to ?
                to :
                TupleFactory.CreateConjunctiveTupleSystem(resultElements));
        }*/

        ComplexAttributeComponentFactoryArgs<TData>
            INonFictionalAttributeComponentFactory<
                TData,
                TupleObject<TData>,
                ComplexAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictionalFactoryArgs(
                TupleObject<TData> resultElements)
        {
            return new ComplexAttributeComponentFactoryArgs<TData>(
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

        /*AttributeComponent<TData>
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
        }*/

        AttributeComponent<TData>
            ISetOperationResultFactory<
                ComplexAttributeComponent<TData>,
                TupleObject<TData>,
                ComplexAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .ProduceOperationResult(
                    ComplexAttributeComponent<TData> first,
                    TupleObject<TData> resultElements)
        {
            return CreateSpecificNonFictional(
                new ComplexAttributeComponentFactoryArgs<TData>(
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
                _tupleFactory.CreateFullConjunctive<TData>()))
        { }

        public ComplexAttributeComponentFactory(
            TupleObject<TData> referencedKb)
            : base(new ComplexAttributeComponentFactoryArgs<TData>(
                referencedKb))
        { }

        /*
        AttributeComponent<TData>
            INonFictionalAttributeComponentFactory<
                TData,
                IEnumerable<TData>>
            .CreateNonFictional(IEnumerable<TData> values)
        {
            IFiniteIterableAttributeComponentFactory<TData>
                iterableFactory = this;

            return iterableFactory.ProduceOperationResult(values);
        }

        NonFictionalAttributeComponentFactoryArgs<TData>
            INonFictionalAttributeComponentFactory<
                TData,
                IEnumerable<TData>>
            .CreateNonFictionalFactoryArgs(IEnumerable<TData> values)
        {
            IFiniteIterableAttributeComponentFactory<TData>
                iterableFactory = this;

            return iterableFactory.CreateFactoryArgs(values);
        }
        */
    }
}
