using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AttributeComponents
{
    /// <summary>
    /// Перечисление типов содержимого компоненты атрибута.
    /// </summary>
    public enum AttributeComponentContentType : byte
    {
        /// <summary>
        /// Пустая фиктивная компонента.
        /// </summary>
        Empty = 0,
        /// <summary>
        /// Непустая компонента.
        /// </summary>
        NonFictional = 1,
        /// <summary>
        /// Полная фиктивная компонента.
        /// </summary>
        Full = 2
    }

    public static class AttributeComponentContentTypeHelper
    {
        public static int CompareToZero(
            this AttributeComponentContentType contentType)
        {
            return (int)contentType;
        }

        public static int CompareToContinuum(
            this AttributeComponentContentType contentType)
        {
            return ((int)contentType) - 2;
        }

        public static AttributeComponentContentType ComplementThe(
            this AttributeComponentContentType ct)
        {
            return ct switch
            {
                AttributeComponentContentType.Empty =>
                    AttributeComponentContentType.Full,
                AttributeComponentContentType.NonFictional => ct,
                _ => AttributeComponentContentType.Empty
            };
        }

        public static AttributeComponentContentType IntersectWith(
            this AttributeComponentContentType ct1,
            AttributeComponentContentType ct2)
        {
            return Enumerable.Min([ct1, ct2]);
        }

        public static AttributeComponentContentType UnionWith(
            this AttributeComponentContentType ct1,
            AttributeComponentContentType ct2)
        {
            return Enumerable.Max([ct1, ct2]);
        }

        public static AttributeComponentContentType ExceptOf(
            this AttributeComponentContentType ct1,
            AttributeComponentContentType ct2)
        {
            return ct1 switch
            {
                AttributeComponentContentType.NonFictional when ct2 <= ct1 => ct1,
                AttributeComponentContentType.Full => 
                    (AttributeComponentContentType)(ct1 - ct2),
                _ => AttributeComponentContentType.Empty
            };
        }

        public static AttributeComponentContentType SymmetricExceptOf(
            this AttributeComponentContentType ct1,
            AttributeComponentContentType ct2)
        {
            return ct1 switch
            {
                AttributeComponentContentType.Empty => ct2,
                AttributeComponentContentType.NonFictional => ct1,
                _ => (AttributeComponentContentType)(ct1 - ct2)
            };
        }
    }
}
