using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentVisitors
{
    public interface IAttributeComponentBooleanOperator<
        TData, 
        in CTOperand1, 
        in CTOperand2>
        : IInstantBinaryAttributeComponentVisitor<
              TData, 
              CTOperand1, 
              CTOperand2,
              bool>
        where CTOperand1 : IAttributeComponent<TData>
        where CTOperand2 : IAttributeComponent<TData>
    { }
}
