using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.TupleBased;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased
{
    public class IntersectionOperator<TData>
        : NonFictionalAttributeComponentIntersectionOperator<
            TData,
            TupleBasedAttributeComponentFactoryArgs<TData>,
            TupleBasedAttributeComponent<TData>,
            ITupleBasedAttributeComponentFactory<TData>,
            TupleBasedAttributeComponentFactoryArgs<TData>>,
          IFactoryBinaryOperator<
              TupleBasedAttributeComponent<TData>,
              NonFictionalAttributeComponent<TData>,
              IAttributeComponentFactory<TData>,
              IAttributeComponent<TData>>,
          IFactoryBinaryOperator<
              TupleBasedAttributeComponent<TData>,
              TupleBasedAttributeComponent<TData>,
              IAttributeComponentFactory<TData>,
              IAttributeComponent<TData>>
    {
        public IAttributeComponent<TData> Visit(
            TupleBasedAttributeComponent<TData> first,
            NonFictionalAttributeComponent<TData> second,
            IAttributeComponentFactory<TData> factory)
        {
            return first.Unfold() & second;
        }

        public IAttributeComponent<TData> Visit(
            TupleBasedAttributeComponent<TData> first,
            TupleBasedAttributeComponent<TData> second,
            IAttributeComponentFactory<TData> factory)
        {
            /*
            if (first.Predicate.ElementType != second.Predicate.ElementType)
            {
                // В будущем можно изменить
                throw new NotImplementedException();
            }

            return (dynamic)first.Predicate & (dynamic)second.Predicate;
            */

            return first.Unfold() & second.Unfold();
        }
    }
}
