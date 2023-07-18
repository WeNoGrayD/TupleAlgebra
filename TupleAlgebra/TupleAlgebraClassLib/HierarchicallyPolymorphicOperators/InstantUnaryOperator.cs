using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.HierarchicallyPolymorphicOperators
{
    public abstract class InstantUnaryOperator<TOperand, TOperationResult>
    {
        public abstract TOperationResult Accept(TOperand first);
    }
}
