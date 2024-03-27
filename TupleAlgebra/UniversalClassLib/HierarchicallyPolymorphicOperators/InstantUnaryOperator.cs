using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalClassLib.HierarchicallyPolymorphicOperators
{
    public abstract class InstantUnaryOperator<TOperand, TOperationResult>
    {
        public abstract TOperationResult Accept(TOperand first);
    }
}
