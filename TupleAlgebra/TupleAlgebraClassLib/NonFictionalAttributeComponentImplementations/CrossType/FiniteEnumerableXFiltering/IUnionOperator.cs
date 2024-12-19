using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering
{
    public interface IFiniteEnumerableXFilteringUnionOperator<
        TData,
        CTOperand1,
        CTFactory,
        CTFactoryArgs>
        : IFiniteEnumerableXFilteringAttributeComponentBinaryOperator<
            TData,
            CTOperand1,
            CTFactory,
            CTFactoryArgs>
        where CTOperand1 : NonFictionalAttributeComponent<TData>, IFiniteEnumerableAttributeComponent<TData>
        where CTFactory : INonFictionalAttributeComponentFactory<TData, IEnumerable<TData>, CTFactoryArgs>
        where CTFactoryArgs : NonFictionalAttributeComponentFactoryArgs<TData>
    {
        IAttributeComponent<TData> IFactoryBinaryOperator<
            CTOperand1,
            FilteringAttributeComponent<TData>,
            CTFactory,
            IAttributeComponent<TData>>
            .Visit(
                CTOperand1 first,
                FilteringAttributeComponent<TData> second,
                CTFactory factory)
        {
            return first | second.ToIterableAttributeComponent();
        }
    }
}
