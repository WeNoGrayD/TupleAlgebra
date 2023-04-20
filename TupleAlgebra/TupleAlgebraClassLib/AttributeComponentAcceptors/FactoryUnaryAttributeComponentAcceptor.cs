using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Diagnostics;

namespace TupleAlgebraClassLib.AttributeComponentAcceptors
{
    public abstract class FactoryUnaryOperator<TOperand, TOperationResultFactory, TOperationResult>
    {
        public TOperationResult Accept(TOperand first, TOperationResultFactory factory)
        {
            return AcceptImpl((dynamic)first, factory);
        }

        protected TOperationResult AcceptImpl<DTOperand1, DTOperand2>(DTOperand1 first, TOperationResultFactory factory)
        {
            var data = (this as IFactoryUnaryOperator<DTOperand1, TOperationResultFactory, TOperationResult>)
                .Accept(first, factory);
            return data;
        }
    }

    public abstract class FactoryUnaryAttributeComponentAcceptor<TData, TOperand, TOperationResult>
        : FactoryUnaryOperator<AttributeComponent<TData>, AttributeComponentFactory, TOperationResult>
    {
        /*
        public TOperationResult Accept(
            AttributeComponent<TData> first,
            AttributeComponentFactory factory)
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
            AttributeComponentFactory factory)
            where TOperand1 : AttributeComponent<TData>
        {
            var data = (this as IFactoryBinaryAttributeComponentAcceptor<TData, TOperand1, TOperationResult>).Accept(first, factory);
            return data;
        }
        */
    }
}
