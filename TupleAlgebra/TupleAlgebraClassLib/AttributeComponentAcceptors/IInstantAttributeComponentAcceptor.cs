using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public interface IInstantAttributeComponentAcceptor<TValue, in TOperand, out TOperationResult>
        where TOperand : AttributeComponent<TValue>
    {
        TOperationResult Accept(TOperand first);
    }
    public interface IFactoryAttributeComponentAcceptor<TValue, in TOperand1, in TOperand2, out TOperationResult>
        where TOperand1 : AttributeComponent<TValue>
        where TOperand2 : AttributeComponent<TValue>
    {
        TOperationResult Accept(
            TOperand1 first,
            TOperand2 second,
            AttributeComponentFactory<TValue> factory);
    }
}
