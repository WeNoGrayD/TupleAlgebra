using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace TupleAlgebraClassLib.AttributeComponents
{
    /// <summary>
    /// Абстрактный тип мощности компоненты атрибута.
    /// </summary>
    public abstract class AttributeComponentPower
        : IComparable<AttributeComponentPower>,
          IEqualityOperators<AttributeComponentPower, AttributeComponentPower, bool>,
          IComparisonOperators<AttributeComponentPower, AttributeComponentPower, bool>
    {
        #region Instance properties

        public abstract AttributeComponentContentType ContentType { get; }

        #endregion

        #region IAttributeComponentPower implementation

        /// <summary>
        /// Проверка мощности на равенство нулю.
        /// </summary>
        /// <returns></returns>
        public abstract bool EqualsZero();

        /// <summary>
        /// Проверка мощности на равенство мощности континуума.
        /// </summary>
        /// <returns></returns>
        public abstract bool EqualsContinuum();

        #endregion

        public virtual int CompareTo(AttributeComponentPower second)
        {
            return ContentType.CompareTo(second.ContentType);
        }

        /// <summary>
        /// Преобраование мощности к требуемому виду мощности.
        /// Строго рекомендуется использовать этот метод вместо ключевого слова as при приведении
        /// к мощности нефиктивной компоненты, поскольку нефиктивная компонента как универсум домена 
        /// будет содержать иной тип мощности (но с вызовом данного метода предоставит нефиктивную мощность).
        /// </summary>
        /// <typeparam name="TConverting"></typeparam>
        /// <returns></returns>
        public virtual TConverting As<TConverting>()
            where TConverting : AttributeComponentPower => (this as TConverting)!;

        #region Operators

        /// <summary>
        /// Оператор сравнения на равенство двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator ==(AttributeComponentPower first, AttributeComponentPower second)
        {
            return first.CompareTo(second) == 0;
        }

        /// <summary>
        /// Оператор сравнения на неравенство двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator !=(AttributeComponentPower first, AttributeComponentPower second)
        {
            return first.CompareTo(second) != 0;
        }

        /// <summary>
        /// Оператор сравнения "больше" двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator >(AttributeComponentPower first, AttributeComponentPower second)
        {
            return first.CompareTo(second) == 1;
        }

        /// <summary>
        /// Оператор сравнения "больше или равно" двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator >=(AttributeComponentPower first, AttributeComponentPower second)
        {
            return first.CompareTo(second) > -1;
        }

        /// <summary>
        /// Оператор сравнения "меньше" двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator <(AttributeComponentPower first, AttributeComponentPower second)
        {
            return first.CompareTo(second) == -1;
        }

        /// <summary>
        /// Оператор сравнения "меньше или равно" двух мощностей.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator <=(AttributeComponentPower first, AttributeComponentPower second)
        {
            return first.CompareTo(second) < 1;
        }

        #endregion
    }
}
