using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure
{
    public interface IAttributeComponentFactory<TData>
    {
        AttributeDomain<TData> Domain { get; }

        EmptyAttributeComponent<TData> CreateEmpty(
            AttributeComponentFactoryArgs factoryArgs);

        EmptyAttributeComponent<TData> CreateEmpty();

        AttributeComponent<TData> CreateNonFictional(
            AttributeComponentFactoryArgs factoryArgs);

        AttributeComponent<TData> CreateNonFictional<TFactoryArgs>(
            TFactoryArgs factoryArgs)
            where TFactoryArgs : AttributeComponentFactoryArgs;

        FullAttributeComponent<TData> CreateFull(
            AttributeComponentFactoryArgs factoryArgs);

        FullAttributeComponent<TData> CreateFull();
    }
}
