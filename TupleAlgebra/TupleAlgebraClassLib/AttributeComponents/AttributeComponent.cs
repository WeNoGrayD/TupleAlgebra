using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using System.Numerics;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponentAcceptors;

namespace TupleAlgebraClassLib.AttributeComponents
{
    using static AttributeComponentHelper;

    /// <summary>
    /// Параметризированная компонента атрибута.
    /// Определяет операторы и методы для операций пересечения, объединения,
    /// дополнения, сравнения на равенство и включения.
    /// </summary>
    /// <typeparam name="TData">Тип значения, которое может принимать атрибут.</typeparam>
    public abstract class AttributeComponent<TData>
        : IEnumerable<TData>,
          IQueryable<TData>,
          IReproducingQueryable<TData>,
          IReproducingQueryable<AttributeComponentFactoryArgs, TData>,
          IAlgebraicSetObject,
          IBitwiseOperators<AttributeComponent<TData>, AttributeComponent<TData>, AttributeComponent<TData>>,
          IDivisionOperators<AttributeComponent<TData>, AttributeComponent<TData>, AttributeComponent<TData>>,
          IEqualityOperators<AttributeComponent<TData>, AttributeComponent<TData>, bool>,
          IComparisonOperators<AttributeComponent<TData>, AttributeComponent<TData>, bool>,
          IAttributeComponent
    {
        #region Instance properties

        public AttributeComponentPower Power { get; private set; }

        public AttributeDomain<TData> Domain { get => GetDomain(); }

        public virtual Func<AttributeDomain<TData>> GetDomain { get;  set; }

        public AttributeComponentFactory Factory { get => Helper.GetFactory(this); }

        public ISetOperationExecutorsContainer<AttributeComponent<TData>> SetOperations
            { get => Helper.GetSetOperations<TData>(this); }

        #endregion

        #region IQueryable implemented properties

        public virtual Expression Expression { get; protected set; }

        public virtual Type ElementType { get => typeof(TData); }

        public virtual IQueryProvider Provider { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static AttributeComponent()
        {
            Helper.RegisterType<TData, AttributeComponent<TData>>(
                acFactory: new AttributeComponentFactory());

            return;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="power"></param>
        public AttributeComponent(
            AttributeComponentPower power,
            IQueryProvider queryProvider,
            Expression queryExpression)
        {
            Power = power;
            Provider = queryProvider;
            Expression = queryExpression ?? Expression.Constant(this);
        }

        #endregion

        #region Instance methods

        public AttributeComponentFactoryArgs ZipInfo<TReproducedData>(
            IEnumerable<TReproducedData> populatingData,
            bool includeDomain = false)
        {
            AttributeComponentFactoryArgs factoryArgs = ZipInfoImpl(populatingData);
            if (includeDomain) IncludeDomain(factoryArgs);

            return factoryArgs;
        }

        public abstract AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(
            IEnumerable<TReproducedData> populatingData);

        public void IncludeDomain(AttributeComponentFactoryArgs factoryArgs)
        {
            factoryArgs.SetDomainGetter(GetDomain);

            return;
        }

        protected abstract AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs);

        public IReproducingQueryable<TReproducedData> Reproduce<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return ReproduceImpl<TReproducedData>(factoryArgs);// as IReproducingQueryable<TReproducedData>;
        }

        public IReproducingQueryable<TReproducedData> Reproduce<TReproducedData>(
            IEnumerable<TReproducedData> reproducedData,
            bool includeDomain = false)
        {
            /*
            if (typeof(TData) != typeof(TReproducedData))
            {
                throw new ArgumentException($"Тип {this.GetType()} не может воспроизводить " +
                    $"компоненты атрибутов, чей тип данных отличается от типа данных {this.GetType()}.\n" +
                    $"Тип данных воспроизводимой компоненты атрибута: {typeof(TReproducedData)}.");
            }
            */

            return ReproduceImpl<TReproducedData>(ZipInfo(reproducedData, includeDomain));// as IEnumerable<TData>))
                                                                           //as IReproducingQueryable<TReproducedData>;
        }

        #endregion

        public IEnumerator<TData> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<TData>>(Expression) as AttributeComponent<TData>).GetEnumeratorImpl();
        }

        public abstract IEnumerator<TData> GetEnumeratorImpl();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IAttributeComponent.GetBufferizedEnumerator()
        {
            /*
             * 1) Сначала компонента превращается в массив.
             * 2) Затем массив приводится к типу IEnumerable<TData>, чтобы вернулся обобщённый
             * IEnumerator, а не необобщённый, как по умолчанию у массивов.
             * 3) И вот от приведённого массива передаётся GetEnumerator().
             */
            return ((this.ToArray()) as IEnumerable<TData>).GetEnumerator();
        }

        #region Set operations

        protected AttributeComponent<TData> ComplementThe()
        {
            return SetOperations.Complement(this);
        }

        protected AttributeComponent<TData> IntersectWith(AttributeComponent<TData> second)
        {
            return SetOperations.Intersect(this, second);
        }

        protected AttributeComponent<TData> UnionWith(AttributeComponent<TData> second)
        {
            return SetOperations.Union(this, second);
        }

        protected AttributeComponent<TData> ExceptWith(AttributeComponent<TData> second)
        {
            return SetOperations.Except(this, second);
        }

        protected AttributeComponent<TData> SymmetricExceptWith(AttributeComponent<TData> second)
        {
            return SetOperations.SymmetricExcept(this, second);
        }

        protected bool Includes(AttributeComponent<TData> second)
        {
            return SetOperations.Include(this, second);
        }

        protected bool EqualsTo(AttributeComponent<TData> second)
        {
            return SetOperations.Equal(this, second);
        }

        protected bool IncludesOrEqualsTo(AttributeComponent<TData> second)
        {
            return SetOperations.IncludeOrEqual(this, second);
        }

        #endregion

        #region IAlgebraicSetObject implementation

        IAlgebraicSetObject IAlgebraicSetObject.ComplementThe()
        {
            return this.ComplementThe();
        }

        IAlgebraicSetObject IAlgebraicSetObject.IntersectWith(IAlgebraicSetObject second)
        {
            return this.IntersectWith(second as AttributeComponent<TData>);
        }

        IAlgebraicSetObject IAlgebraicSetObject.UnionWith(IAlgebraicSetObject second)
        {
            return this.UnionWith(second as AttributeComponent<TData>);
        }

        IAlgebraicSetObject IAlgebraicSetObject.ExceptWith(IAlgebraicSetObject second)
        {
            return this.ExceptWith(second as AttributeComponent<TData>);
        }

        IAlgebraicSetObject IAlgebraicSetObject.SymmetricExceptWith(IAlgebraicSetObject second)
        {
            return this.SymmetricExceptWith(second as AttributeComponent<TData>);
        }

        bool IAlgebraicSetObject.Includes(IAlgebraicSetObject second)
        {
            return this.Includes(second as AttributeComponent<TData>);
        }

        bool IAlgebraicSetObject.EqualsTo(IAlgebraicSetObject second)
        {
            return this.EqualsTo(second as AttributeComponent<TData>);
        }

        bool IAlgebraicSetObject.IncludesOrEqualsTo(IAlgebraicSetObject second)
        {
            return this.IncludesOrEqualsTo(second as AttributeComponent<TData>);
        }

        #endregion

        #region Operators

        public static AttributeComponent<TData> operator ~(AttributeComponent<TData> first)
        {
            return first.ComplementThe();
        }

        public static AttributeComponent<TData> operator &(
            AttributeComponent<TData> first,
            AttributeComponent<TData> second)
        {
            return first.IntersectWith(second);
        }

        public static AttributeComponent<TData> operator |(
            AttributeComponent<TData> first,
            AttributeComponent<TData> second)
        {
            return first.UnionWith(second);
        }

        public static AttributeComponent<TData> operator /(
            AttributeComponent<TData> first,
            AttributeComponent<TData> second)
        {
            return first.ExceptWith(second);
        }

        public static AttributeComponent<TData> operator ^(
            AttributeComponent<TData> first,
            AttributeComponent<TData> second)
        {
            return first.SymmetricExceptWith(second);
        }

        public static bool operator ==(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return first.EqualsTo(second);
        }

        public static bool operator !=(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return !(first == second);
        }

        public static bool operator >(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return first.Includes(second);
        }

        public static bool operator >=(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return first.IncludesOrEqualsTo(second);
        }

        public static bool operator <(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return second.Includes(first);
        }

        public static bool operator <=(AttributeComponent<TData> first, AttributeComponent<TData> second)
        {
            return second.IncludesOrEqualsTo(first);
        }

        #endregion

        #region Nested types

        public abstract class InstantAttributeComponentOperationExecutorsContainer<CTOperand>
            : InstantSetOperationExecutorsContainer<AttributeComponent<TData>, CTOperand, AttributeComponentFactory>
            where CTOperand : AttributeComponent<TData>
        {
            #region Instance properties

            protected override AttributeComponentFactory Factory
            { get => Helper.GetFactory(typeof(CTOperand)); }

            #endregion

            #region Constructors

            public InstantAttributeComponentOperationExecutorsContainer(
                Func<IFactoryUnaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>>>
                    complementionOperator,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    intersectionOperator,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    unionOperator,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    differenceOperator,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    symmetricExceptionOperator,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, bool>>
                    inclusionComparer,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, bool>>
                    equalityComparer,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, bool>>
                    inclusionOrEquationComparer)
                : base(complementionOperator,
                       intersectionOperator,
                       unionOperator,
                       differenceOperator,
                       symmetricExceptionOperator,
                       inclusionComparer,
                       equalityComparer,
                       inclusionOrEquationComparer)
            {
                return;
            }

            #endregion
        }

        #endregion
    }
}
