using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    internal static class AttributeComponentComplementionRules
    {
        public static FullAttributeComponent<TValue> Complement<TValue>(EmptyAttributeComponent<TValue> first)
            where TValue : IComparable<TValue>
        {
            return FullAttributeComponent<TValue>.Instance;
        }

        public static AttributeComponent<TValue> Complement<TValue>(NonFictionalAttributeComponent<TValue> first)
            where TValue : IComparable<TValue>
        {
            return first.Domain.Universum ^ first;
        }

        public static EmptyAttributeComponent<TValue> Complement<TValue>(FullAttributeComponent<TValue> first)
            where TValue : IComparable<TValue>
        {
            return EmptyAttributeComponent<TValue>.Instance;
        }
    }
}
