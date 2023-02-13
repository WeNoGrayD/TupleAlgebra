using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Diagnostics;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class FactoryBinaryAttributeComponentAcceptor<TData, TOperationResult>
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
            AttributeComponent<TData> first,
            AttributeComponent<TData> second,
            AttributeComponentFactory<TData> factory)
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
            AttributeComponentFactory<TData> factory)
            where TOperand1 : AttributeComponent<TData>
            where TOperand2 : AttributeComponent<TData>
        {
            var data = (this as IFactoryAttributeComponentAcceptor<TData, TOperand1, TOperand2, TOperationResult>).Accept(first, second, factory);
            return data;
        }

        #endregion
    }
}
