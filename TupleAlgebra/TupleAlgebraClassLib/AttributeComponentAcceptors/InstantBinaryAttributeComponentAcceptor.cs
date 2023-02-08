using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    /// <summary>
    /// Абстрактный класс для операторов и компараторов, которые способны принимать
    /// две компоненты атрибута, одна из которых типизированная.
    /// Почти вырожденный интерфейс, который предоставляет метод
    /// для вызова методов потомков с динамическим приведением второго параметра.
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public abstract class InstantBinaryAttributeComponentAcceptor<TValue, TOperationResult>
    {
        #region Methods

        /// <summary>
        /// Метод для приёма любых двух компонент.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>

        public TOperationResult Accept(
            AttributeComponent<TValue> first,
            AttributeComponent<TValue> second)
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
            where TOperand1 : AttributeComponent<TValue>
            where TOperand2 : AttributeComponent<TValue>
        {
            var data = (this as IInstantAttributeComponentAcceptor<TValue, TOperand1, TOperand2, TOperationResult>).Accept(first, second);
            return data;
        }

        #endregion
    }
}
