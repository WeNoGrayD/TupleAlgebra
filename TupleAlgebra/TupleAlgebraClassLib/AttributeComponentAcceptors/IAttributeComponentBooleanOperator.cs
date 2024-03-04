using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public interface IAttributeComponentBooleanOperator<
        TData, 
        in CTOperand1, 
        in CTOperand2>
        : IInstantBinaryAttributeComponentAcceptor<
              TData, 
              CTOperand1, 
              CTOperand2,
              bool>
        where CTOperand1 : IAttributeComponent<TData>
        where CTOperand2 : IAttributeComponent<TData>
    { }

    public interface IAttributeComponentBooleanOperator<
        TData,
        in CTOperand1>
        : IAttributeComponentBooleanOperator<
              TData,
              CTOperand1,
              AttributeComponent<TData>>
        where CTOperand1 : IAttributeComponent<TData>
    { }
}
