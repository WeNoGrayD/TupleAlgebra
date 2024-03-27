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
    public interface IFiniteEnumerableXFilteringExceptionOperator<
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
        where CTFactory : INonFictionalAttributeComponentFactory<TData, IEnumerable<TData>, CTOperand1, CTFactoryArgs>
        where CTFactoryArgs : AttributeComponentFactoryArgs
    {
        IAttributeComponent<TData> IFactoryBinaryOperator<
            CTOperand1,
            FilteringAttributeComponent<TData>,
            CTFactory,
            IAttributeComponent<TData>>
            .Accept(
                CTOperand1 first,
                FilteringAttributeComponent<TData> second,
                CTFactory factory)
        {
            return first & ~second;
        }
    }
}
