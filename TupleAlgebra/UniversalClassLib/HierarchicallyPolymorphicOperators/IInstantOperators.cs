using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalClassLib.HierarchicallyPolymorphicOperators
{
    public interface IInstantUnaryOperator<in TOperand, out TOperationResult>
    {
        TOperationResult Accept(TOperand first);
    }

    public interface IInstantBinaryOperator<in TOperand1, in TOperand2, out TOperationResult>
    {
        TOperationResult Accept(TOperand1 first, TOperand2 second);
    }
}
