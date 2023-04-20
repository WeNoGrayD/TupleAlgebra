using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Diagnostics;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
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
            return AcceptImpl((dynamic)first, (dynamic)second, factory);
        }

        protected TOperationResult AcceptImpl<DTOperand1, DTOperand2>(
            DTOperand1 first, 
            DTOperand2 second, 
            TOperationResultFactory factory)
        {
            var data = (this as IFactoryBinaryOperator<DTOperand1, DTOperand2, TOperationResultFactory, TOperationResult>)
                .Accept(first, second, factory);
            return data;
        }

        #endregion
    }

    public abstract class FactoryBinaryAttributeComponentAcceptor<TData, TOperationResult>
        : FactoryBinaryOperator<AttributeComponent<TData>, AttributeComponent<TData>, AttributeComponentFactory, TOperationResult>
    {
        #region Methods

        /*
        protected TOperationResult DowncastOperandsToContentType<TOperand1, TOperand2>(
            TOperand1 first,
            TOperand2 second,
            AttributeComponentFactory factory)
            where TOperand1 : AttributeComponent<TData>
            where TOperand2 : AttributeComponent<TData>
        {
            var data = (this as IFactoryBinaryAttributeComponentAcceptor<TData, TOperand1, TOperand2, TOperationResult>)
                .Accept(first, second, factory);
            return data;
        }
        */

        #endregion
    }
}
