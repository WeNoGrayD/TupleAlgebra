using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
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
    public interface IFilteringFiniteEnumerableAttributeComponentBinaryOperator<
        TData,
        CTFactory,
        CTFactoryArgs>
        : IFactoryBinaryAttributeComponentAcceptor<
            TData,
            IEnumerable<TData>,
            FilteringAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>,
            CTFactory,
            CTFactoryArgs,
            IAttributeComponent<TData>>
        where CTFactory : INonFictionalAttributeComponentFactory<TData, IEnumerable<TData>, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    { }

    public interface IFiniteEnumerableXFilteringAttributeComponentBinaryOperator<
        TData,
        CTOperand1,
        CTFactory,
        CTFactoryArgs>
        : IFactoryBinaryAttributeComponentAcceptor<
            TData,
            IEnumerable<TData>,
            CTOperand1,
            FilteringAttributeComponent<TData>,
            CTFactory,
            CTFactoryArgs,
            IAttributeComponent<TData>>
        where CTOperand1 : NonFictionalAttributeComponent<TData>, IFiniteEnumerableAttributeComponent<TData>
        where CTFactory : INonFictionalAttributeComponentFactory<TData, IEnumerable<TData>, CTOperand1, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
    }

    public interface IFiniteEnumerableXFilteringAttributeComponentBooleanOperator<
        TData,
        CTOperand1>
        : IInstantBinaryAttributeComponentAcceptor<
            TData,
            CTOperand1,
            FilteringAttributeComponent<TData>,
            bool>
        where CTOperand1 : IFiniteEnumerableAttributeComponent<TData>
    { }

    public interface IFilteringXFiniteEnumerableAttributeComponentBooleanOperator<
        TData>
        : IInstantBinaryAttributeComponentAcceptor<
            TData,
            FilteringAttributeComponent<TData>,
            IFiniteEnumerableAttributeComponent<TData>,
            bool>
    { }
}
