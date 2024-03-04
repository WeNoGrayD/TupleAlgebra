using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure
{
    public abstract class NonFictionalAttributeComponentEqualityComparer<TData, CTOperand1>
        : NonFictionalAttributeComponentBooleanBinaryOperator<TData, CTOperand1>
        where CTOperand1: NonFictionalAttributeComponent<TData>
    {
        public override bool Accept(
            NonFictionalAttributeComponent<TData> first, 
            EmptyAttributeComponent<TData> second)
        {
            return false;
        }

        public override bool Accept(
            NonFictionalAttributeComponent<TData> first, 
            FullAttributeComponent<TData> second)
        {
            return false;
        }
    }
}
