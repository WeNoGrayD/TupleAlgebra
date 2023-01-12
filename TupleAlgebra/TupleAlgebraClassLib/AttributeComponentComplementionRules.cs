using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    internal static class AttributeComponentComplementionRules
    {
        public static Func<AttributeComponentFactory<TValue>, FullAttributeComponent<TValue>> Complement<TValue>(EmptyAttributeComponent<TValue> first)
            where TValue : IComparable<TValue>
        {
            return (factory) => factory.CreateFull();
        }

        public static Func<AttributeComponentFactory<TValue>, AttributeComponent<TValue>> Complement<TValue>(NonFictionalAttributeComponent<TValue> first)
            where TValue : IComparable<TValue>
        {
            return first.ComplementThe();
        }

        public static Func<AttributeComponentFactory<TValue>, EmptyAttributeComponent<TValue>> Complement<TValue>(FullAttributeComponent<TValue> first)
            where TValue : IComparable<TValue>
        {
            return (factory) => factory.CreateEmpty();
        }
    }
}
