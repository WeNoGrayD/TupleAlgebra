﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib
{
    internal static class AttributeComponentUnionRules
    {
        public static EmptyAttributeComponent<TValue> Union<TValue>(
            EmptyAttributeComponent<TValue> first,
            EmptyAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }

        public static NonFictionalAttributeComponent<TValue> Union<TValue>(
            EmptyAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return second;
        }

        public static FullAttributeComponent<TValue> Union<TValue>(
            EmptyAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return second;
        }

        public static AttributeComponent<TValue> Union<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            NonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first.UnionWith(second);
        }

        public static FullAttributeComponent<TValue> Union<TValue>(
            NonFictionalAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return second;
        }

        public static FullAttributeComponent<TValue> Union<TValue>(
            FullAttributeComponent<TValue> first,
            FullAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            return first;
        }
    }
}
