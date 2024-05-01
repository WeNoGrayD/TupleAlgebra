using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations
{
    /// <summary>
    /// Сей интерфейс потребен для того, чтобы показывать, какой результат
    /// передают на выход различные операторы: сортированный или нет.
    /// В будущем надо от него избавиться и добиться передачи "несортированности"
    /// по умолчанию (для этого ничего делать не нужно) и "сортированности"
    /// в операторах упорядоченной компоненты (для этого нужно потрудиться).
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IOperationEnumerableResultProvider<TData>
        : IEnumerable<TData>
    {
        IEnumerable<TData> Result { get => this; }

        bool KeepsOrder { get; }

        IEnumerator<TData> IEnumerable<TData>.GetEnumerator() => Result.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Result.GetEnumerator();
    }

    public record OperationResultEnumerableResultProvider<TData>
        (IEnumerable<TData> Result, bool KeepsOrder) 
        : IOperationEnumerableResultProvider<TData>
    { }
}
