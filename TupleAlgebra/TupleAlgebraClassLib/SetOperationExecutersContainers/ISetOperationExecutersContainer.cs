using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.SetOperationExecutersContainers
{
    public interface ISetOperationExecutersContainer<BTOperand, CTOperand>
        where CTOperand : BTOperand
    {
        public BTOperand Complement(CTOperand first);

        public BTOperand Intersect(CTOperand first, BTOperand second);

        public BTOperand Union(CTOperand first, BTOperand second);

        public BTOperand Except(CTOperand first, BTOperand second);

        public BTOperand SymmetricExcept(CTOperand first, BTOperand second);

        public bool Include(CTOperand first, BTOperand second);

        public bool Equal(CTOperand first, BTOperand second);

        public bool IncludeOrEqual(CTOperand first, BTOperand second);
    }
}
