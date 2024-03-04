using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable
{
    public interface IOrderedFiniteEnumerableAttributeComponentBinaryOperator<TData, in TAttributeComponent, in TOperand2, in TFactory, in TFactoryArgs>
        : IFactoryBinaryAttributeComponentAcceptor<TData, TAttributeComponent, TOperand2, TFactory, TFactoryArgs, AttributeComponent<TData>>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IOrderedFiniteEnumerableAttributeComponent<TData>
        where TOperand2 : IAttributeComponent<TData>
        where TFactoryArgs : OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
        where TFactory : IOrderedFiniteEnumerableAttributeComponentFactory<TData, TAttributeComponent, TFactoryArgs>
    { }

    public interface IOrderedFiniteEnumerableAttributeComponentBinaryOperator<TData, in TAttributeComponent, in TFactory, in TFactoryArgs>
        : IOrderedFiniteEnumerableAttributeComponentBinaryOperator<TData, TAttributeComponent, IOrderedFiniteEnumerableAttributeComponent<TData>, TFactory, TFactoryArgs>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IOrderedFiniteEnumerableAttributeComponent<TData>
        where TFactoryArgs : OrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>
        where TFactory : IOrderedFiniteEnumerableAttributeComponentFactory<TData, TAttributeComponent, TFactoryArgs>
    { }

    public interface IOrderedFiniteEnumerableAttributeComponentBooleanOperator<TData, in TAttributeComponent, in TOperand2>
        : IAttributeComponentBooleanOperator<TData, TAttributeComponent, TOperand2>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IOrderedFiniteEnumerableAttributeComponent<TData>
        where TOperand2 : IAttributeComponent<TData>
    { }

    public interface IOrderedFiniteEnumerableAttributeComponentBooleanOperator<TData, in TAttributeComponent>
        : IOrderedFiniteEnumerableAttributeComponentBooleanOperator<TData, TAttributeComponent, IOrderedFiniteEnumerableAttributeComponent<TData>>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IOrderedFiniteEnumerableAttributeComponent<TData>
    { }
}
