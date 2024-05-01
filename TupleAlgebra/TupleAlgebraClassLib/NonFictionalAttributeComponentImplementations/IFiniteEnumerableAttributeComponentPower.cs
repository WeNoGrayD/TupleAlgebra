using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    public interface IFiniteEnumerableAttributeComponentPower
    {
        int CompareTo<TData>(
            IFiniteEnumerableAttributeComponentPower second,
            IFiniteEnumerableAttributeComponent<TData> ac1,
            IFiniteEnumerableAttributeComponent<TData> ac2)
        {
            return GetNumericalRepresentation(ac1)
                .CompareTo(second.GetNumericalRepresentation(ac2));
        }

        int CompareToZero<TData>(IFiniteEnumerableAttributeComponent<TData> ac)
        {
            return GetNumericalRepresentation(ac).CompareTo(0);
        }

        int GetNumericalRepresentation<TData>(
            IFiniteEnumerableAttributeComponent<TData> ac)
        {
            return ac.GetCount();
        }
    }
}
