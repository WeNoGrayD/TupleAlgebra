using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib
{
    public interface IAttributeComponentAcceptor<TValue, in TOperand1, in TOperand2, out TOperationResult>
        where TValue : IComparable<TValue>
        where TOperand1 : AttributeComponent<TValue>
        where TOperand2 : AttributeComponent<TValue>
    {
        TOperationResult Accept(TOperand1 first, TOperand2 second);
    }

    public interface IFactoryAttributeComponentAcceptor<TValue, in TOperand1, in TOperand2, out TOperationResult>
        where TValue : IComparable<TValue>
        where TOperand1 : AttributeComponent<TValue>
        where TOperand2 : AttributeComponent<TValue>
    {
        TOperationResult Accept(
            TOperand1 first, 
            TOperand2 second, 
            AttributeComponentFactory<TValue> factory);
    }

    /// <summary>
    /// Абстрактный класс для операторов и компараторов, которые способны принимать
    /// две компоненты атрибута, одна из которых типизированная.
    /// Почти вырожденный интерфейс, который предоставляет метод
    /// для вызова методов потомков с динамическим приведением второго параметра.
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public abstract class AttributeComponentAcceptor<TOperationResult>
    {
        #region Methods

        /// <summary>
        /// Метод для приёма любых двух компонент.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        
        public TOperationResult Accept<TValue>(
            dynamic first, 
            dynamic second)
            where TValue : IComparable<TValue>
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = UpcastSecondOperandToContentType(first, second);
            sw.Stop();
            var(ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
        }

        protected TOperationResult UpcastSecondOperandToContentType<TValue, TOperand1, TOperand2>(
            TOperand1 first, 
            TOperand2 second)
            where TValue : IComparable<TValue>
            where TOperand1 : AttributeComponent<TValue>
            where TOperand2 : AttributeComponent<TValue>
        {
            var data = (this as IAttributeComponentAcceptor<TValue, TOperand1, TOperand2, TOperationResult>).Accept(first, second);
            return data;
        }

        #endregion
    }

    public abstract class FactoryAttributeComponentAcceptor<TOperationResult>
    {
        #region Methods

        /// <summary>
        /// Метод для приёма любых двух компонент.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>

        public TOperationResult Accept<TValue>(
            dynamic first, 
            dynamic second,
            AttributeComponentFactory<TValue> factory)
            where TValue : IComparable<TValue>
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = UpcastSecondOperandToContentType(first, second, factory);
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
        }

        protected TOperationResult UpcastSecondOperandToContentType<TValue, TOperand1, TOperand2>(
            TOperand1 first, 
            TOperand2 second,
            AttributeComponentFactory<TValue> factory)
            where TValue : IComparable<TValue>
            where TOperand1 : AttributeComponent<TValue>
            where TOperand2 : AttributeComponent<TValue>
        {
            var data = (this as IAttributeComponentAcceptor<TValue, TOperand1, TOperand2, TOperationResult>).Accept(first, second);
            return data;
        }

        #endregion
    }
    
    /// <summary>
    /// Интерфейс для операторов и компараторов, которые способны принимать
    /// две типизированных компоненты атрибута.
    /// Реализует паттерн, обратный паттерну посетителя - 
    /// паттерн "приниматель" (название временное).
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public abstract class CrossContentTypesAttributeComponentAcceptor<TValue, TOperand1, TOperationResult> 
        : AttributeComponentAcceptor<TOperationResult>,
          IAttributeComponentAcceptor<TValue, TOperand1, EmptyAttributeComponent<TValue>, TOperationResult>, 
          IAttributeComponentAcceptor<TValue, TOperand1, NonFictionalAttributeComponent<TValue>, TOperationResult>, 
          IAttributeComponentAcceptor<TValue, TOperand1, FullAttributeComponent<TValue>, TOperationResult>
        where TValue : IComparable<TValue>
        where TOperand1 : AttributeComponent<TValue>
    {
        public abstract TOperationResult Accept(
            TOperand1 first, 
            EmptyAttributeComponent<TValue> second);

        public abstract TOperationResult Accept(
            TOperand1 first, 
            NonFictionalAttributeComponent<TValue> second);

        public abstract TOperationResult Accept(
            TOperand1 first,
            FullAttributeComponent<TValue> second);
    }
    
    public abstract class CrossContentTypesFactoryAttributeComponentAcceptor<TValue, TOperand1, TOperationResult>
        : FactoryAttributeComponentAcceptor<TOperationResult>,
          IFactoryAttributeComponentAcceptor<TValue, TOperand1, EmptyAttributeComponent<TValue>, TOperationResult>,
          IFactoryAttributeComponentAcceptor<TValue, TOperand1, NonFictionalAttributeComponent<TValue>, TOperationResult>,
          IFactoryAttributeComponentAcceptor<TValue, TOperand1, FullAttributeComponent<TValue>, TOperationResult>
        where TValue : IComparable<TValue>
        where TOperand1 : AttributeComponent<TValue>
    {
        public abstract TOperationResult Accept(
            TOperand1 first,
            EmptyAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            NonFictionalAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory);

        public abstract TOperationResult Accept(
            TOperand1 first,
            FullAttributeComponent<TValue> second,
            AttributeComponentFactory<TValue> factory);
    }

    /*
    /// <summary>
    /// Интерфейс для операторов и компараторов, которые способны принимать
    /// первую любую непустую и вторую конечную перечислимую непустую 
    /// типизированные компоненты атрибута.
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public abstract class IFiniteEnumerableNonFictionalAttributeComponentAcceptor<TValue, TOperationResult>
        : AttributeComponentAcceptor<TOperationResult>,
          IAttributeComponentAcceptor<FiniteEnumerableNonFictionalAttributeComponent<TValue>, TOperationResult>
        where TValue : IComparable<TValue>
    {
        #region Methods

        /// <summary>
        /// Метод для приёма первой любой непустой и 
        /// второй конечноей перечислимой непустой компоненты.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="FiniteEnumerable"></param>
        /// <returns></returns>
        public abstract TOperationResult Accept(
            AttributeComponent firstOfBaseType, 
            FiniteEnumerableNonFictionalAttributeComponent<TValue> second);

        #endregion
    }
    */
}
