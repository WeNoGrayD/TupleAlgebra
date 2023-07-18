using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.HierarchicallyPolymorphicOperators
{
    public abstract class FactoryUnaryOperator<TOperand, TOperationResultFactory, TOperationResult>
    {
        public abstract TOperationResult Accept(TOperand first, TOperationResultFactory factory);
    }
}
