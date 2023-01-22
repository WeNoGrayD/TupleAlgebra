using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    internal static class AttributeComponentExceptRules
    {
        public static EmptyAttributeComponent<TValue> Except<TValue>(
            EmptyAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static EmptyAttributeComponent<TValue> Except<TValue>(
            EmptyAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static EmptyAttributeComponent<TValue> Except<TValue>(
            EmptyAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static NonFictionalAttributeComponent<TValue> Except<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static AttributeComponent<TValue> Except<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first.ExceptWith(second);
        }

        public static EmptyAttributeComponent<TValue> Except<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return EmptyAttributeComponent<TValue>.Instance;
        }

        public static FullAttributeComponent<TValue> Except<TValue>(
            FullAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static NonFictionalAttributeComponent<TValue> Except<TValue>(
            FullAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return !second as NonFictionalAttributeComponent<TValue>;
        }

        public static EmptyAttributeComponent<TValue> Except<TValue>(
            FullAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return EmptyAttributeComponent<TValue>.Instance;
        }
    }
}
