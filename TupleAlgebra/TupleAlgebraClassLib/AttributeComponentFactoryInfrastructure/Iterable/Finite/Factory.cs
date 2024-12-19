using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.TupleBased;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Iterable.Finite;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite
{
    public interface IFiniteIterableAttributeComponentFactory<TData>
        : IEnumerableNonFictionalAttributeComponentFactory<
              TData,
              FiniteIterableAttributeComponent<TData>,
              FiniteIterableAttributeComponentFactoryArgs<TData>>,
          IVariableAttributeComponentFactory<TData>,
          ITupleBasedAttributeComponentFactory<TData>,
          IFilteringAttributeComponentFactory<TData>
    {
        FiniteIterableAttributeComponentFactoryArgs<TData>
            INonFictionalAttributeComponentFactory<
                TData,
                IEnumerable<TData>,
                FiniteIterableAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictionalFactoryArgs(
            IEnumerable<TData> resultElements)
        {
            return new FiniteIterableAttributeComponentFactoryArgs<TData>(
                resultElements);
        }

        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                FiniteIterableAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictional(
                FiniteIterableAttributeComponentFactoryArgs<TData>
                    args)
        {
            return new FiniteIterableAttributeComponent<TData>(
                args.Power,
                args.Values,
                args.QueryProvider,
                args.QueryExpression);
        }
    }

    public class FiniteIterableAttributeComponentFactory<TData>
        : AttributeComponentFactory<TData, FiniteIterableAttributeComponentFactoryArgs<TData>>,
          IFiniteIterableAttributeComponentFactory<TData>
    {
        public FiniteIterableAttributeComponentFactory(AttributeDomain<TData> domain)
            : base(domain)
        { }

        public FiniteIterableAttributeComponentFactory(
            FiniteIterableAttributeComponentFactoryArgs<TData> factoryArgs)
            : base(factoryArgs)
        { }

        public FiniteIterableAttributeComponentFactory(
            IEnumerable<TData> universeData)
            : this(new FiniteIterableAttributeComponentFactoryArgs<TData>(universeData))
        { }
    }
}
