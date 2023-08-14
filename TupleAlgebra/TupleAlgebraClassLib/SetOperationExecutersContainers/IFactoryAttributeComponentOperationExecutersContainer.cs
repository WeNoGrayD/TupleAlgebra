using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public interface IFactoryAttributeComponentOperationExecutersContainer<TData, CTOperand>
        : ISetOperationExecutersContainer<AttributeComponent<TData>, CTOperand>
        where CTOperand : AttributeComponent<TData>
    {
        public AttributeComponent<TProducedData> Produce<TProducedData>(
            AttributeComponentFactoryArgs factoryArgs);
    }
}
