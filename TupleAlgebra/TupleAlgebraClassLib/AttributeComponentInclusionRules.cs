using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    internal static class AttributeComponentInclusionRules
    {
        public static bool Include<TValue>(
            EmptyAttributeComponent<TValue> equal1,
            EmptyAttributeComponent<TValue> equal2)
            where TValue : IComparable<TValue>
        {
            return false;
        }

        public static bool Include<TValue>(
            EmptyAttributeComponent<TValue> lesser,
            NonFictionalAttributeComponent<TValue> greater)
            where TValue : IComparable<TValue>
        {
            return false;
        }

        public static bool Include<TValue>(
            EmptyAttributeComponent<TValue> lesser,
            FullAttributeComponent<TValue> greater)
            where TValue : IComparable<TValue>
        {
            return false;
        }

        public static bool Include<TValue>(
            NonFictionalAttributeComponent<TValue> greater,
            NonFictionalAttributeComponent<TValue> lesser)
            where TValue : IComparable<TValue>
        {
            return greater.Power > lesser.Power && AttributeComponentInclusionOrEqualityRules.IncludeOrEqual(greater, lesser);
        }

        public static bool Include<TValue>(
            NonFictionalAttributeComponent<TValue> lesser,
            FullAttributeComponent<TValue> greater)
            where TValue : IComparable<TValue>
        {
            return false;
        }

        public static bool Include<TValue>(
            FullAttributeComponent<TValue> equal1,
            FullAttributeComponent<TValue> equal2)
            where TValue : IComparable<TValue>
        {
            return false;
        }
    }
}
