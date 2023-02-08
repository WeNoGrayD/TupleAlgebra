using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class InstantUnaryAttributeComponentAcceptor<TValue, TOperationResult>
    {
        public TOperationResult Accept(AttributeComponent<TValue> first)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = DowncastOperandToContentType((dynamic)first);
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
        }

        protected TOperationResult DowncastOperandToContentType<TOperand1>(TOperand1 first)
            where TOperand1 : AttributeComponent<TValue>
        {
            var data = (this as IInstantAttributeComponentAcceptor<TValue, TOperand1, TOperationResult>).Accept(first);
            return data;
        }
    }
}
