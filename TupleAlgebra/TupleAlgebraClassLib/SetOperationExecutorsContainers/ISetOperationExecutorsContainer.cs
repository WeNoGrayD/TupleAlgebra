using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.SetOperationExecutorsContainers
{
    public interface ISetOperationExecutorsContainer<TOperand>
    {
        #region Instance methods

        TOperand Complement(TOperand first);

        TOperand Intersect(TOperand first, TOperand second);

        TOperand Union(TOperand first, TOperand second);

        TOperand Except(TOperand first, TOperand second);

        TOperand SymmetricExcept(TOperand first, TOperand second);

        bool Include(TOperand first, TOperand second);

        bool Equal(TOperand first, TOperand second);

        bool IncludeOrEqual(TOperand first, TOperand second);

        #endregion
    }
}
