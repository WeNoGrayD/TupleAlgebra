using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public abstract class NonFictionalAttributeComponentInclusionOrEqualityComparer<TData, CTOperand1>
        : NonFictionalAttributeComponentBooleanBinaryOperator<TData, CTOperand1>
        where CTOperand1 : NonFictionalAttributeComponent<TData>
    {
        public override bool Visit(
            NonFictionalAttributeComponent<TData> first, 
            EmptyAttributeComponent<TData> second)
        {
            return true;
        }

        public override bool Visit(
            NonFictionalAttributeComponent<TData> first, 
            FullAttributeComponent<TData> second)
        {
            return false;
        }
    }
}
