﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.AttributeComponentVisitors
{
    /// <summary>
    /// Абстрактный класс для операторов и компараторов, которые способны принимать
    /// две компоненты атрибута, одна из которых типизированная.
    /// Почти вырожденный интерфейс, который предоставляет метод
    /// для вызова методов потомков с динамическим приведением второго параметра.
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public abstract class InstantBinaryAttributeComponentVisitor<TData, TOperand1, TOperationResult>
        : InstantBinaryOperator<TOperand1, IAttributeComponent<TData>, TOperationResult>
        where TOperand1: IAttributeComponent<TData>
    { }
}
