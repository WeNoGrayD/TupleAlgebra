using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TupleAlgebraClassLib.HierarchicallyPolymorphicOperators
{
    public abstract class InstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
        : IInstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
    {
        #region Methods

        public TOperationResult Accept(TOperand1 first, TOperand2 second)
        {
            //return AcceptImpl(first, (dynamic)second);
            return (TOperationResult)typeof(InstantBinaryOperator<TOperand1, TOperand2, TOperationResult>)
                .GetMethod(nameof(InstantBinaryOperator<TOperand1, TOperand2, TOperationResult>.AcceptImpl),
                           BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(second.GetType())
                .Invoke(this, new object[] { first, second });
        }

        protected TOperationResult AcceptImpl<DTOperand2>(TOperand1 first, DTOperand2 second)
        {
            var data = (this as IInstantBinaryOperator<TOperand1, DTOperand2, TOperationResult>).Accept(first, second);
            return data;
        }

        #endregion
    }
}
