using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    public class CountableEnumerableAttributeComponentPower<
        TData, TAttributeComponent>
        : FiniteEnumerableAttributeComponentPower<TData, TAttributeComponent>
        where TAttributeComponent : NonFictionalAttributeComponent<TData>, ICountableAttributeComponent<TData>
    {
        public override int NumericalRepresentation
        { get => Component.Count; }
    }
}
