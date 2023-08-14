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
}
