using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    public interface IAlgebraicSetObject
    {
        IAlgebraicSetObject ComplementThe();

        IAlgebraicSetObject IntersectWith(IAlgebraicSetObject second);

        IAlgebraicSetObject UnionWith(IAlgebraicSetObject second);

        IAlgebraicSetObject ExceptWith(IAlgebraicSetObject second);

        IAlgebraicSetObject SymmetricExceptWith(IAlgebraicSetObject second);

        bool Includes(IAlgebraicSetObject second);

        bool EqualsTo(IAlgebraicSetObject second);

        bool IncludesOrEqualsTo(IAlgebraicSetObject second);
    }
}
