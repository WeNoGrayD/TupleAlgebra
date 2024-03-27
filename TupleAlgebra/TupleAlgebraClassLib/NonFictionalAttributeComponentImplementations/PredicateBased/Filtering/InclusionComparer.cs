using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using UniversalClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering
{
    public class InclusionComparer<TData>
        : NonFictionalAttributeComponentInclusionComparer<
            TData,
            FilteringAttributeComponent<TData>>,
          IInstantBinaryAttributeComponentAcceptor<
            TData,
            FilteringAttributeComponent<TData>,
            FilteringAttributeComponent<TData>,
            bool>
    {
        bool IInstantBinaryOperator<
            FilteringAttributeComponent<TData>,
            FilteringAttributeComponent<TData>,
            bool>
            .Accept(
                 FilteringAttributeComponent<TData>  greater,
                 FilteringAttributeComponent<TData> lower)
        {
            return greater.ToIterableAttributeComponent()
                .Includes(lower.ToIterableAttributeComponent());
        }
    }
}
