using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class InstantUnaryOperator<TOperand, TOperationResult>
    {
        public TOperationResult Accept(TOperand first)
        {
            return AcceptImpl((dynamic)first);
        }

        protected TOperationResult AcceptImpl<DTOperand>(DTOperand first)
        {
            return (this as IInstantUnaryOperator<DTOperand, TOperationResult>).Accept(first);
        }
    }

    public abstract class InstantUnaryAttributeComponentAcceptor<TData, TOperationResult>
        : InstantUnaryOperator<AttributeComponent<TData>, TOperationResult>
    { 

        /*
        public TOperationResult Accept(AttributeComponent<TData> first)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = DowncastOperandToContentType((dynamic)first);
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
            return (this as IInstantBinaryAttributeComponentAcceptor<TData, TOperand1, TOperationResult>).Accept(first);
        }
            */

        /*
        protected TOperationResult DowncastOperandToContentType<TOperand1>(TOperand1 first)
            where TOperand1 : AttributeComponent<TData>
        {
            var data = (this as IInstantBinaryAttributeComponentAcceptor<TData, TOperand1, TOperationResult>).Accept(first);
            return data;
        }
        */
    }
}
