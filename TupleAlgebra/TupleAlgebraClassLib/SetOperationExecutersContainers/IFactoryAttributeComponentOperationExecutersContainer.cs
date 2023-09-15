using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.SetOperationExecutorsContainers
{
    public interface IFactoryAttributeComponentOperationExecutorsContainer<TData>
        : ISetOperationExecutorsContainer<AttributeComponent<TData>>
    {
        public AttributeComponent<TProducedData> Produce<TProducedData>(
            AttributeComponentFactoryArgs factoryArgs);
    }
}
