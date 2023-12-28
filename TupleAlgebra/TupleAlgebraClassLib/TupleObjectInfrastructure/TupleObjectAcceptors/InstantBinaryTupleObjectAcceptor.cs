using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.TupleObjectAcceptors
{
    /// <summary>
    /// Абстрактный класс для операторов и компараторов, которые способны принимать
    /// две компоненты атрибута, одна из которых типизированная.
    /// Почти вырожденный интерфейс, который предоставляет метод
    /// для вызова методов потомков с динамическим приведением второго параметра.
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public abstract class InstantBinaryTupleObjectAcceptor<TEntity, TOperand1, TOperationResult>
        : InstantBinaryOperator<TOperand1, TupleAlgebraClassLib.TupleObjects.TupleObject<TEntity>, TOperationResult>
        where TEntity : new()
        where TOperand1: TupleAlgebraClassLib.TupleObjects.TupleObject<TEntity>
    { }
}
