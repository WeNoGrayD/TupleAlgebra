using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraClassLib
{
    public interface IAttributeComponentAcceptor<in TComponent, TOperationResult>
        where TComponent : AttributeComponent
    {
        TOperationResult Accept(AttributeComponent first, TComponent second);
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
        
        public TOperationResult Accept<TValue>(AttributeComponent<TValue> first, dynamic second)
            where TValue : IComparable<TValue>
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = CastUpToContentType(first, second);
            sw.Stop();
            var(ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
        }

        /*
        public TCast CastUpToContentType<TValue, TCast>(TCast castedCompomnent)
            where TValue : IComparable<TValue> 
            where TCast : AttributeComponent<TValue>
        {

        }*/

        public TOperationResult CastUpToContentType<TValue, TCast>(AttributeComponent<TValue> first, TCast second)
            where TValue : IComparable<TValue>
            where TCast : AttributeComponent<TValue>
        {
            var data = (this as IAttributeComponentAcceptor<TCast, TOperationResult>).Accept(first, second);
            return data;
        }

        /*
        public NonFictionalAttributeComponent<TValue> CastUpToContentType<TValue, TCast>(NonFictionalAttributeComponent<TValue> castedComponent)
            where TValue : IComparable<TValue>
        {
            return castedComponent;
        }

        public EmptyAttributeComponent<TValue> CastUpToContentType<TValue, TCast>(EmptyAttributeComponent<TValue> castedComponent)
            where TValue : IComparable<TValue>
        {
            return castedComponent;
        }
        */

        /*
    public TOperationResult Accept<TValue>(AttributeComponent<TValue> first, AttributeComponent<TValue> second)
    where TValue : IComparable<TValue>
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        //var data = default(TOperationResult);
        var data = Accept(first, second);
        //var data = Accept(first, (NonFictionalAttributeComponent<TValue>)second);
        sw.Stop();
        var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
        return data;
    }
    */

        /*
        public TOperationResult Accept<TValue>(NonFictionalAttributeComponent<TValue> first, AttributeComponent<TValue> nonFictional)
            where TValue : IComparable<TValue>
        {
            return AttributeComponentEqualityRules.Equal(first as NonFictionalAttributeComponent<TValue>, nonFictional);
        }
        */

        /*
    public TOperationResult Accept<TComponent, TValue>(AttributeComponent<TValue> first, TComponent second)
        where TComponent : AttributeComponent<TValue>
        where TValue : IComparable<TValue>
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        //var data = default(TOperationResult);
        var data = (this as IAttributeComponentAcceptor<TComponent, TOperationResult>).Accept(first, second);
        //var data = Accept(first, (NonFictionalAttributeComponent<TValue>)second);
        sw.Stop();
        var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
        return data;
    }*/

        /*
        public TOperationResult Accept<TValue>(AttributeComponent<TValue> first, FiniteEnumerableNonFictionalAttributeComponent<TValue> second)
            where TValue : IComparable<TValue>
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var data = default(TOperationResult);
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            return data;
        }*/

        #endregion
    }

    /// <summary>
    /// Интерфейс для операторов и компараторов, которые способны принимать
    /// две типизированных компоненты атрибута.
    /// Реализует паттерн, обратный паттерну посетителя - 
    /// паттерн "приниматель" (название временное).
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public interface ICrossContentTypesAttributeComponentAcceptor<TValue, TOperationResult> 
        : IAttributeComponentAcceptor<EmptyAttributeComponent<TValue>, AttributeComponent>, 
          IAttributeComponentAcceptor<NonFictionalAttributeComponent<TValue>, AttributeComponent>, 
          IAttributeComponentAcceptor<FullAttributeComponent<TValue>, AttributeComponent>
        where TValue : IComparable<TValue>
    {
        #region Methods

        /// <summary>
        /// Метод для приёма первой любой и второй пустой компоненты.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="empty"></param>
        /// <returns></returns>
        TOperationResult Accept(AttributeComponent<TValue> first, EmptyAttributeComponent<TValue> second);

        /// <summary>
        /// Метод для приёма первой любой и второй непустой компоненты.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="nonFictional"></param>
        /// <returns></returns>
        TOperationResult Accept<(AttributeComponent<TValue> first, NonFictionalAttributeComponent<TValue> second);

        /// <summary>
        /// Метод для приёма первой любой и второй полной компоненты.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="first"></param>
        /// <param name="full"></param>
        /// <returns></returns>
        TOperationResult Accept(AttributeComponent<TValue> first, FullAttributeComponent<TValue> second);

        #endregion
    }

    /// <summary>
    /// Интерфейс для операторов и компараторов, которые способны принимать
    /// первую любую непустую и вторую конечную перечислимую непустую 
    /// типизированные компоненты атрибута.
    /// </summary>
    /// <typeparam name="TOperationResult"></typeparam>
    public interface IFiniteEnumerableNonFictionalAttributeComponentAcceptor<TValue, TOperationResult>
        : IAttributeComponentAcceptor<FiniteEnumerableNonFictionalAttributeComponent<TValue>, AttributeComponent>
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
        TOperationResult Accept(
            AttributeComponent<TValue> first, 
            FiniteEnumerableNonFictionalAttributeComponent<TValue> second);

        #endregion
    }
}
