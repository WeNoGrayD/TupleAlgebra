using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.SetOperationExecutorsContainers
{
    public interface ISetOperationResultFactory<
        in TOperand1,
        in TIntermediateOperationResult,
        out TOperationResultFactoryArgs,
        out TResult>
    {
        abstract TResult ProduceOperationResult(
            TOperand1 first,
            TIntermediateOperationResult intermediaryResult);

        protected abstract TOperationResultFactoryArgs CreateFactoryArgs(
            TOperand1 first,
            TIntermediateOperationResult intermediaryResult);
    }
}
