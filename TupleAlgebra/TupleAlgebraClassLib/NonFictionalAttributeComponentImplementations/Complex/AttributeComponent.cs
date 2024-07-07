using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Complex;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex
{
    using static AttributeComponentHelper;

    internal interface IComplexAttributeComponent
    {
        public bool IsInnerTupleEmpty();

        public bool IsInnerTupleFull();
    }

    /// <summary>
    /// Специальный тип компоненты атрибута для классов,
    /// экземпляры которых нельзя напрямую перечислить
    /// и передать как IEnumerable<T>, а также для которых нельзя
    /// указать домен атрибута.
    /// Пример: анонимные классы, представляющие ключи.
    /// Их домены - это декартово произведение доменов атрибутов.
    /// Поэтому резонно "архивировать" универсумы таких доменов
    /// в компоненты из кортежей, а также сами перечисления объектов.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class ComplexAttributeComponent<TData>
        : NonFictionalAttributeComponent<TData>,
          IComplexAttributeComponent
        where TData : new()
    {
        #region Instance fields

        #endregion

        #region Instance properties

        //public override bool IsLazy { get => true; }

        public TupleObject<TData> Tuple { get; init; }

        #endregion

        #region Constructors

        static ComplexAttributeComponent()
        {
            Helper.RegisterType<TData, ComplexAttributeComponent<TData>>(
                acFactory: (domain) => new ComplexAttributeComponentFactory<TData>(domain),
                setOperations: (factory) => new ComplexAttributeComponentOperationExecutorsContainer((IComplexAttributeComponentFactory<TData>)factory));

            return;
        }

        public ComplexAttributeComponent(
            AttributeComponentPower power,
            TupleObject<TData> tuple,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            Tuple = tuple;

            return;
        }

        #endregion

        #region Instance methods

        public bool IsInnerTupleEmpty() => Tuple.IsEmpty();

        public bool IsInnerTupleFull() => Tuple.IsFull();

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(
            IEnumerable<TReproducedData> populatingData)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            return Tuple.GetEnumerator();
        }

        #endregion

        #region Nested types

        private class ComplexAttributeComponentOperationExecutorsContainer
            : NonFictionalAttributeComponentOperationExecutorsContainer<
                ComplexAttributeComponent<TData>,
                IEnumerable<TData>,
                IComplexAttributeComponentFactory<TData>,
                ComplexAttributeComponentFactoryArgs<TData>>
        {
            #region Constructors

            public ComplexAttributeComponentOperationExecutorsContainer(
                IComplexAttributeComponentFactory<TData> factory)
                : base(
                      factory,
                      () => null,
                      () => null,
                      () => null,
                      () => null,
                      () => null,
                      () => null,
                      () => null,
                      () => null)
            { }

            #endregion
        }

        #endregion
    }

    public class ComplexAttributeComponentPower
        : NonFictionalAttributeComponentPower
    {
        #region Static properties

        public static ComplexAttributeComponentPower Instance { get; } 

        #endregion

        #region Constructors

        static ComplexAttributeComponentPower()
        {
            Instance = new();

            return;
        }

        #endregion

        #region Instance methods

        protected override int CompareToZero<TData>(AttributeComponent<TData> ac)
        {
            return (ac as IComplexAttributeComponent).IsInnerTupleEmpty() ? 0 : 1;
        }

        public override int CompareToNonFictional<TData>(
            AttributeComponentPower second,
            AttributeComponent<TData> ac1,
            AttributeComponent<TData> ac2)
        {
            return 0;
        }

        public override bool EqualsContinuum<TData>(AttributeComponent<TData> ac)
        {
            return (ac as IComplexAttributeComponent).IsInnerTupleFull();
        }

        #endregion}
    }
}
