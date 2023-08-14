using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TupleAlgebraClassLib.HierarchicallyPolymorphicOperators
{
    using static OperatorHelper;

    public abstract class FactoryBinaryOperator<TOperand1, TOperand2, TOperationResultFactory, TOperationResult>
    {
        #region Methods

        /// <summary>
        /// Метод для приёма любых двух компонент.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>

        public TOperationResult Accept(
            TOperand1 first,
            TOperand2 second,
            TOperationResultFactory factory)
        {
            return (TOperationResult)FactoryBinaryOperatorAcceptImplMIPattern
                .MakeGenericMethod(
                    typeof(TOperand1), 
                    second.GetType(), 
                    typeof(TOperationResultFactory),
                    typeof(TOperationResult))
                .Invoke(null, new object[] { this, first, second, factory });
        }

        #endregion
    }
}
