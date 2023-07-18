using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TupleAlgebraClassLib.HierarchicallyPolymorphicOperators
{
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
            //return AcceptImpl(first, (dynamic)second, factory);

            return (TOperationResult)typeof(FactoryBinaryOperator<TOperand1, TOperand2, TOperationResultFactory, TOperationResult>)
                .GetMethod(nameof(FactoryBinaryOperator<TOperand1, TOperand2, TOperationResultFactory, TOperationResult>.AcceptImpl), 
                           BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(second.GetType())
                .Invoke(this, new object[] { first, second, factory });
        }

        protected TOperationResult AcceptImpl<DTOperand2>(
            TOperand1 first,
            DTOperand2 second,
            TOperationResultFactory factory)
        {
            var data = (this as IFactoryBinaryOperator<TOperand1, DTOperand2, TOperationResultFactory, TOperationResult>)
                .Accept(first, second, factory);
            return data;
        }

        #endregion
    }
}
