using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class InstantUnaryAttributeComponentAcceptor<TData, TOperationResult>
    {
        public TOperationResult Accept(AttributeComponent<TData> first)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = DowncastOperandToContentType((dynamic)first);
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
        }

        protected TOperationResult DowncastOperandToContentType<TOperand1>(TOperand1 first)
            where TOperand1 : AttributeComponent<TData>
        {
            var data = (this as IInstantAttributeComponentAcceptor<TData, TOperand1, TOperationResult>).Accept(first);
            return data;
        }
    }
}
