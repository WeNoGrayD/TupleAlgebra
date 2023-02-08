using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Diagnostics;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class FactoryBinaryAttributeComponentAcceptor<TValue, TOperationResult>
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
            AttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = DowncastOperandsToContentType((dynamic)first, (dynamic)second, factory);
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
        }

        protected TOperationResult DowncastOperandsToContentType<TOperand1, TOperand2>(
            TOperand1 first,
            TOperand2 second,
            AttributeComponentFactory<TValue> factory)
            where TOperand1 : AttributeComponent<TValue>
            where TOperand2 : AttributeComponent<TValue>
        {
            var data = (this as IFactoryAttributeComponentAcceptor<TValue, TOperand1, TOperand2, TOperationResult>).Accept(first, second, factory);
            return data;
        }

        #endregion
    }
}
