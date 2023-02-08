using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AlgebraicTupleInfrastructure
{
    public interface IAlgebraicTupleSchemaProvider
    {
        AttributeInfo? this[string attributeName] { get; set; }
    }
}
