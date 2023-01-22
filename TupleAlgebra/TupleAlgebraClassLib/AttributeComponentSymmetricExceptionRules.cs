using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    internal static class AttributeComponentSymmetricExceptionRules
    {
        public static EmptyAttributeComponent<TValue> SymmetricExcept<TValue>(
            EmptyAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static NonFictionalAttributeComponent<TValue> SymmetricExcept<TValue>(
            EmptyAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return second;
        }

        public static FullAttributeComponent<TValue> SymmetricExcept<TValue>(
            EmptyAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return second;
        }

        public static AttributeComponent<TValue> SymmetricExcept<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first.SymmetricExceptWith(second);
        }

        public static NonFictionalAttributeComponent<TValue> SymmetricExcept<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return !first as NonFictionalAttributeComponent<TValue>;
        }

        public static EmptyAttributeComponent<TValue> SymmetricExcept<TValue>(
            FullAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return EmptyAttributeComponent<TValue>.Instance;
        }
    }
}
