using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    public class EqualityComparer
        : NonFictionalAttributeComponentEqualityComparer<bool, BooleanNonFictionalAttributeComponent>,
          IBooleanAttributeComponentBooleanOperator
    {
        public bool Visit(
            BooleanNonFictionalAttributeComponent first,
            BooleanNonFictionalAttributeComponent second)
        {
            return first.Value == second.Value;
        }
    }
}
