using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Navigational;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.CrossType.FiniteEnumerableXFiltering;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational.WithSimpleKey
{
    public class IntersectionOperator<TKey, TData>
        : NonFictionalAttributeComponentIntersectionOperator<
            TData,
            NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>,
            NavigationalAttributeComponentWithSimpleKey<TKey, TData>,
            NavigationalAttributeComponentWithSimpleKeyFactory<TKey, TData>,
            NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>>
        where TData : new()
    {
        public IAttributeComponent<TData> Visit(
            NavigationalAttributeComponentWithSimpleKey<TKey, TData> first,
            NavigationalAttributeComponentWithSimpleKey<TKey, TData> second,
            NavigationalAttributeComponentWithSimpleKeyFactory<TKey, TData> factory)
        {
            NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>
                factoryArgs = (first.ForeignKey, second.ForeignKey) switch
                {
                    (not null, not null) => new NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>(
                        foreignKeyComponent: first.ForeignKey & second.ForeignKey),
                    (null, null) => new NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>(
                        navigationalComponent: first.NavigationalAttribute & second.NavigationalAttribute)
                };

            return factory.CreateNonFictional(factoryArgs);
        }
    }
}
