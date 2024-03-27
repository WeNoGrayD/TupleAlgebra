using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace UniversalClassLib.HierarchicallyPolymorphicOperators
{
    using static OperatorHelper;

    public abstract class FactoryBinaryOperator<
        TOperand1,
        TOperand2, 
        TOperationResultFactory, 
        TOperationResult>
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
            MethodInfo binaryOperatorMI = FactoryBinaryOperatorAcceptImplMIPattern
                .MakeGenericMethod(
                    typeof(TOperand1),
                    second.GetType(),
                    typeof(TOperationResultFactory),
                    typeof(TOperationResult));

            if (binaryOperatorMI is not null)
            {
                return (TOperationResult)binaryOperatorMI
                    .Invoke(null, new object[] { this, first, second, factory });
            }

            throw new NotImplementedException();
        }

        #endregion
    }
}
