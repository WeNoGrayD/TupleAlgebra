using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponents;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering
{
    /// <summary>
    /// Только для двухсторонних операций.
    /// no except, include or includeorequal
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /*
    public interface IFilteringFiniteEnumerableAttributeComponentBinaryOperator<TData>
        : IFactoryBinaryOperator<
            FilteringAttributeComponent<TData>,
            NonFictionalAttributeComponent<TData>,
            IAttributeComponentFactory<TData>,
            IAttributeComponent<TData>>
    {
        IAttributeComponent<TData> IFactoryBinaryOperator<
            FilteringAttributeComponent<TData>,
            NonFictionalAttributeComponent<TData>,
            IAttributeComponentFactory<TData>,
            IAttributeComponent<TData>>.Visit(
            FilteringAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second,
            IAttributeComponentFactory<TData> factory)
        {
            return second

            IFactoryBinaryOperator<
                NonFictionalAttributeComponent<TData>,
                FilteringAttributeComponent<TData>,
                IAttributeComponentFactory<TData>,
                IAttributeComponent> binOp = (dynamic)this;

            return binOp.Visit(
                second,
                first,
                factory) as IAttributeComponent<TData>;
        }
    }
    */

    public interface IFiniteEnumerableXFilteringAttributeComponentBinaryOperator<
        TData,
        CTOperand1,
        CTFactory,
        CTFactoryArgs>
        : IFactoryBinaryAttributeComponentVisitor<
            TData,
            IEnumerable<TData>,
            CTOperand1,
            FilteringAttributeComponent<TData>,
            CTFactory,
            CTFactoryArgs,
            IAttributeComponent<TData>>
        where CTOperand1 : NonFictionalAttributeComponent<TData>, IFiniteEnumerableAttributeComponent<TData>
        where CTFactory : INonFictionalAttributeComponentFactory<TData, IEnumerable<TData>, CTFactoryArgs>
        where CTFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
    {
    }

    public interface IFiniteEnumerableXFilteringAttributeComponentBooleanOperator<
        TData,
        CTOperand1>
        : IInstantBinaryAttributeComponentVisitor<
            TData,
            CTOperand1,
            FilteringAttributeComponent<TData>,
            bool>
        where CTOperand1 : IFiniteEnumerableAttributeComponent<TData>
    { }

    public interface IFilteringXFiniteEnumerableAttributeComponentBooleanOperator<
        TData>
        : IInstantBinaryAttributeComponentVisitor<
            TData,
            FilteringAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>,
            bool>
    { }
}
