﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable
{
    public interface IUnorderedFiniteEnumerableAttributeComponentBinaryOperator<TData>
        : IFactoryBinaryAttributeComponentAcceptor<
            TData,
            IEnumerable<TData>,
            UnorderedFiniteEnumerableNonFictionalAttributeComponent<TData>, 
            IFiniteEnumerableAttributeComponent<TData>, 
            IUnorderedFiniteEnumerableAttributeComponentFactory<TData>,
            UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>,
            IAttributeComponent<TData>>
    { }
}
