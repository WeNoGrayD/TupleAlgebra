using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace UniversalClassLib.HierarchicallyPolymorphicOperators
{
    using static OperatorHelper;

    public abstract class InstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
    {
        #region Methods

        public TOperationResult Visit(TOperand1 first, TOperand2 second)
        {
            var v = (this.GetType()).GetInterfaces();

            return (TOperationResult)InstantBinaryOperatorVisitImplMIPattern
                .MakeGenericMethod(typeof(TOperand1), second.GetType(), typeof(TOperationResult))
                .Invoke(null, new object[] { this, first, second });
        }

        #endregion
    }
}
