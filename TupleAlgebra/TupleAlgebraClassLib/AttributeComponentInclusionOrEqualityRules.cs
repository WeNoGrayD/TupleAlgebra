using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    internal static class AttributeComponentInclusionOrEqualityRules
    {
        public static bool IncludeOrEqual<TValue>(
            EmptyAttributeComponent<TValue> greater,
            EmptyAttributeComponent<TValue> lesser)
            where TValue : IComparable<TValue>
        {
            return true;
        }

        public static bool IncludeOrEqual<TValue>(
            EmptyAttributeComponent<TValue> greater,
            NonFictionalAttributeComponent<TValue> lesser)
            where TValue : IComparable<TValue>
        {
            return false;
        }

        public static bool IncludeOrEqual<TValue>(
            EmptyAttributeComponent<TValue> greater,
            FullAttributeComponent<TValue> lesser)
            where TValue : IComparable<TValue>
        {
            return false;
        }

        public static bool IncludeOrEqual<TValue>(
            NonFictionalAttributeComponent<TValue> greater,
            NonFictionalAttributeComponent<TValue> lesser)
            where TValue : IComparable<TValue>
        {
            return greater.IncludesOrEqualsTo(lesser);
        }

        public static bool IncludeOrEqual<TValue>(
            NonFictionalAttributeComponent<TValue> greater,
            FullAttributeComponent<TValue> lesser)
            where TValue : IComparable<TValue>
        {
            return false;
        }

        public static bool IncludeOrEqual<TValue>(
            FullAttributeComponent<TValue> greater,
            FullAttributeComponent<TValue> lesser)
            where TValue : IComparable<TValue>
        {
            return true;
        }
    }
}
