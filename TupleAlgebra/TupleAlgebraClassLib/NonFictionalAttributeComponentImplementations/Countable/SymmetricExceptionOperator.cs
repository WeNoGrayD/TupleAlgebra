﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Countable
{
    public interface ICountableAttributeComponentSymmetricExceptionOperator<
        TData,
        TAttributeComponent,
        TFactory,
        TFactoryArgs>
        : IAttributeComponentSymmetricExceptionOperator<
            TData,
            TAttributeComponent,
            ICountableAttributeComponent<TData>,
            TFactory,
            TFactoryArgs>,
          ICountableSequenceOperator<
              TData, 
              TAttributeComponent>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, ICountableAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, TAttributeComponent, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    { }
}
