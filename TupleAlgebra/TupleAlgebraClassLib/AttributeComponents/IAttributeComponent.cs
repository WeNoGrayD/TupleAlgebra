using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AttributeComponents
{
    public interface IAttributeComponent : IAlgebraicSetObject, IEnumerable, IQueryable
    {
        AttributeComponentPower Power { get; }

        /// <summary>
        /// Получение буферизированного перечислителя данных, т.е. такого, чей источник данных
        /// буферизирован.
        /// Требуется для оптимизации по времени вычисления декартова произведения компонент атрибутов.
        /// </summary>
        /// <returns></returns>
        IEnumerator GetBufferizedEnumerator();
    }

    public interface IAttributeComponent<TData> : IAttributeComponent, IEnumerable<TData>, IQueryable<TData>
    {
        IEnumerator<TData> GetBufferizedEnumerator();

        IEnumerator IAttributeComponent.GetBufferizedEnumerator() => GetBufferizedEnumerator();
    }
}
