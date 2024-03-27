using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.FiniteEnumerable
{
    public interface IFiniteEnumerableAttributeComponentBinaryOperator<
        TData,
        TAttributeComponent,
        TFactory,
        TFactoryArgs>
        : IFactoryBinaryAttributeComponentAcceptor<
            TData,
            IEnumerable<TData>,
            TAttributeComponent,
            IFiniteEnumerableAttributeComponent<TData>,
            TFactory,
            TFactoryArgs,
            IAttributeComponent<TData>>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, IFiniteEnumerableAttributeComponent<TData>
        where TFactory : INonFictionalAttributeComponentFactory<TData, IEnumerable<TData>, TAttributeComponent, TFactoryArgs>
        where TFactoryArgs : AttributeComponentFactoryArgs
    { }
}
