using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalClassLib.HierarchicallyPolymorphicOperators
{
    public interface IFactoryUnaryOperator<
        in TOperand, 
        in TOperationResultFactory,
        out TOperationResult>
    {
        TOperationResult Visit(TOperand first, TOperationResultFactory factory);
    }

    public interface IFactoryBinaryOperator<
        in TOperand1, 
        in TOperand2, 
        in TOperationResultFactory, 
        out TOperationResult>
    {
        public abstract TOperationResult Visit(
            TOperand1 first, 
            TOperand2 second, 
            TOperationResultFactory factory);
    }
}
