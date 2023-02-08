using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Diagnostics;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class FactoryUnaryAttributeComponentAcceptor<TValue, TOperationResult>
    {
        public TOperationResult Accept(
            AttributeComponent<TValue> first,
            AttributeComponentFactory<TValue> factory)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = DowncastOperandToContentType((dynamic)first, factory);
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
        }

        protected TOperationResult DowncastOperandToContentType<TOperand1>(
            TOperand1 first,
            AttributeComponentFactory<TValue> factory)
            where TOperand1 : AttributeComponent<TValue>
        {
            var data = (this as IFactoryAttributeComponentAcceptor<TValue, TOperand1, TOperationResult>).Accept(first, factory);
            return data;
        }
    }
}
