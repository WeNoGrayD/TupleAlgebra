using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.TupleBased;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable
{
    public interface IUnorderedFiniteEnumerableAttributeComponentFactory<TData>
        : IEnumerableNonFictionalAttributeComponentFactory<
              TData,
              UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
              UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IVariableAttributeComponentFactory<TData>,
          ITupleBasedAttributeComponentFactory<TData>,
          IFilteringAttributeComponentFactory<TData>
    {
        UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
            INonFictionalAttributeComponentFactory<
                TData,
                IEnumerable<TData>,
                UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictionalFactoryArgs(
                IEnumerable<TData> resultElements)
        {
            return new UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(
                resultElements.ToHashSet());
        }

        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictional(
                UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData> args)
        {
            return new UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>(
                args.Power,
                args.Values,
                args.QueryProvider,
                args.QueryExpression);
        }

        AttributeComponent<TData>
            ISetOperationResultFactory<
                UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>,
                IEnumerable<TData>,
                UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .ProduceOperationResult(
                    UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData> first,
                    IEnumerable<TData> resultElements)
        {
            IEqualityComparer<TData> eqComparer = first.Values.Comparer;
            HashSet<TData> resultHashSet = resultElements.ToHashSet(eqComparer);

            return ProduceOperationResult_DefaultImpl(first, resultHashSet);
        }
    }

    public class UnorderedFiniteEnumerableAttributeComponentFactory<TData>
        : AttributeComponentFactory<TData, UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>>,
          IUnorderedFiniteEnumerableAttributeComponentFactory<TData>
    {
        public UnorderedFiniteEnumerableAttributeComponentFactory(AttributeDomain<TData> domain)
            : base(domain)
        { }

        public UnorderedFiniteEnumerableAttributeComponentFactory(
            UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData> factoryArgs)
            : base(factoryArgs)
        { }

        public UnorderedFiniteEnumerableAttributeComponentFactory(
            HashSet<TData> universeData)
            : this(new UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(universeData))
        { }

        public UnorderedFiniteEnumerableAttributeComponentFactory(
            IEnumerable<TData> universeData)
            : this(universeData.ToHashSet())
        { }
    }
}
