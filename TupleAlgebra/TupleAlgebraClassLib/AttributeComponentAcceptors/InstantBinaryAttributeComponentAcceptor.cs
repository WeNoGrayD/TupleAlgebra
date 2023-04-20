using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class InstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
    {
        #region Methods

        public TOperationResult Accept(TOperand1 first, TOperand2 second)
        {
            return AcceptImpl((dynamic)first, (dynamic)second);
        }

        protected TOperationResult AcceptImpl<DTOperand1, DTOperand2>(DTOperand1 first, DTOperand2 second)
        {
            var data = (this as IInstantBinaryOperator<DTOperand1, DTOperand2, TOperationResult>).Accept(first, second);
            return data;
        }

        #endregion
    }

    /// <summary>
    /// Абстрактный класс для операторов и компараторов, которые способны принимать
    /// две компоненты атрибута, одна из которых типизированная.
    /// Почти вырожденный интерфейс, который предоставляет метод
    /// для вызова методов потомков с динамическим приведением второго параметра.
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public abstract class InstantBinaryAttributeComponentAcceptor<TData, TOperationResult>
        : InstantBinaryOperator<AttributeComponent<TData>, AttributeComponent<TData>, TOperationResult>
    {
        #region Methods

        /// <summary>
        /// Метод для приёма любых двух компонент.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>

        /*
        public TOperationResult Accept(
            AttributeComponent<TData> first,
            AttributeComponent<TData> second)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = DowncastOperandsToContentType((dynamic)first, (dynamic)second);
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
        }

        protected TOperationResult DowncastOperandsToContentType<TOperand1, TOperand2>(
            TOperand1 first,
            TOperand2 second)
            where TOperand1 : AttributeComponent<TData>
            where TOperand2 : AttributeComponent<TData>
        {
            var data = (this as IInstantBinaryAttributeComponentAcceptor<TData, TOperand1, TOperand2, TOperationResult>).Accept(first, second);
            return data;
        }
        */

        #endregion
    }
}
