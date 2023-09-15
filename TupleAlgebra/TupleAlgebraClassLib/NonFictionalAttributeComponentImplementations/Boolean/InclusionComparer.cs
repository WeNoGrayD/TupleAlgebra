using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    public class InclusionComparer
        : NonFictionalAttributeComponentInclusionComparer<bool, BooleanNonFictionalAttributeComponent>,
          IInstantBinaryAttributeComponentAcceptor<bool, BooleanNonFictionalAttributeComponent, BooleanNonFictionalAttributeComponent, bool>
    {
        public bool Accept(
            BooleanNonFictionalAttributeComponent first,
            BooleanNonFictionalAttributeComponent second)
        {
            return false;
        }
    }
}
