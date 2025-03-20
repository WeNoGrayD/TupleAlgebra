using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using UniversalClassLib;

namespace TupleAlgebraClassLib.AttributeComponents
{
    public interface IAttributeComponent 
        : IAlgebraicSetObject, 
          IEnumerable, 
          IQueryable,
          IBufferizedEnumerable
    {
        AttributeComponentPower Power { get; }

        bool IsLazy { get; }

        bool IsEmpty { get; }

        bool IsFull { get; }

        bool IsDefault { get => IsEmpty || IsFull; }

        bool IsFalse()
        {
            if (IsEmpty) return true;
            if (IsFull) return false;
            if (!IsLazy) return false;
            if (GetEnumerator().MoveNext()) return false;

            return true;
        }

        new IAttributeComponent ComplementThe();

        IAttributeComponent IntersectWith(IAttributeComponent second);

        IAttributeComponent UnionWith(IAttributeComponent second);

        IAttributeComponent ExceptWith(IAttributeComponent second);

        IAttributeComponent SymmetricExceptWith(IAttributeComponent second);


        IAlgebraicSetObject IAlgebraicSetObject.ComplementThe()
            => ComplementThe();

        IAlgebraicSetObject IAlgebraicSetObject.IntersectWith(
            IAlgebraicSetObject second)
            => IntersectWith(second);

        IAlgebraicSetObject IAlgebraicSetObject.UnionWith(
            IAlgebraicSetObject second)
            => UnionWith(second);

        IAlgebraicSetObject IAlgebraicSetObject.ExceptWith(
            IAlgebraicSetObject second)
            => ExceptWith(second);

        IAlgebraicSetObject IAlgebraicSetObject.SymmetricExceptWith(
            IAlgebraicSetObject second)
            => SymmetricExceptWith(second);
    }

    public interface IAttributeComponent<TData> 
        : IAttributeComponent,
          IEnumerable<TData>,
          IQueryable<TData>
    {
        IEnumerator<TData> GetBufferizedEnumerator();

        IEnumerator IBufferizedEnumerable.GetBufferizedEnumerator() => GetBufferizedEnumerator();

        IAttributeComponent<TData> IntersectWith(IAttributeComponent<TData> second);

        IAttributeComponent<TData> UnionWith(IAttributeComponent<TData> second);

        IAttributeComponent<TData> ExceptWith(IAttributeComponent<TData> second);

        IAttributeComponent<TData> SymmetricExceptWith(IAttributeComponent<TData> second);

        public bool Includes(
            IAttributeComponent<TData> second);

        public bool EqualsTo(
            IAttributeComponent<TData> second);

        public bool IncludesOrEqualsTo(
            IAttributeComponent<TData> second);

        /*
        ISetOperationExecutorsContainer<IAttributeComponent<TData>> SetOperations
        { get; }

        static IAttributeComponent<TData> operator &(
            AttributeComponent<TData> left,
            IAttributeComponent<TData> right)
        {
            return left.IntersectWith(right);
        }

        static IAttributeComponent<TData> operator |(
            AttributeComponent<TData> left,
            IAttributeComponent<TData> right)
        {
            return left.UnionWith(right);
        }

        static IAttributeComponent<TData> operator ^(
            AttributeComponent<TData> left,
            IAttributeComponent<TData> right)
        {
            return left.SymmetricExceptWith(right);
        }

        static IAttributeComponent<TData> operator /(
            AttributeComponent<TData> left,
            IAttributeComponent<TData> right)
        {
            return left.ExceptWith(right);
        }

        static bool operator <(
            AttributeComponent<TData> left,
            IAttributeComponent<TData> right)
        {
            return right.Includes(left);
        }

        static bool operator >(
            AttributeComponent<TData> left,
            IAttributeComponent<TData> right)
        {
            return left.Includes(right);
        }

        static bool operator <=(
            AttributeComponent<TData> left,
            IAttributeComponent<TData> right)
        {
            return right.IncludesOrEqualsTo(left);
        }

        static bool operator >=(
            AttributeComponent<TData> left,
            IAttributeComponent<TData> right)
        {
            return left.IncludesOrEqualsTo(right);
        }
        */
    }
}
