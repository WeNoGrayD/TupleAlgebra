using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.HierarchicallyPolymorphicOperators
{
    public interface IFactoryUnaryOperator<in TOperand, in TOperationResultFactory, out TOperationResult>
    {
        TOperationResult Accept(TOperand first, TOperationResultFactory factory);
    }

    public interface IFactoryBinaryOperator<in TOperand1, in TOperand2, in TOperationResultFactory, out TOperationResult>
    {
        TOperationResult Accept(TOperand1 first, TOperand2 second, TOperationResultFactory factory);
    }
}
