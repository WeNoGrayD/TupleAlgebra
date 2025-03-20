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
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.AttributeComponentVisitors;

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
        : IReproducingQueryable<TData>,
          IReproducingQueryable<AttributeComponentFactoryArgs, TData>,
          IAttributeComponent<TData>,
          IBitwiseOperators<AttributeComponent<TData>, IAttributeComponent<TData>, IAttributeComponent<TData>>,
          IDivisionOperators<AttributeComponent<TData>, IAttributeComponent<TData>, IAttributeComponent<TData>>,
          IEqualityOperators<AttributeComponent<TData>, IAttributeComponent<TData>, bool>,
          IComparisonOperators<AttributeComponent<TData>, IAttributeComponent<TData>, bool>
    
    {
        #region Instance fields

        protected AttributeDomain<TData> _domain;

        #endregion

        #region Instance properties

        public virtual bool IsLazy { get => false; }

        public AttributeComponentPower Power { get; private set; }

        public virtual AttributeDomain<TData> Domain 
        { get => _domain; set => _domain = value; }

        public IAttributeComponentFactory<TData> Factory { get => Helper.GetFactory(this); }

        public ISetOperationExecutorsContainer<IAttributeComponent<TData>> SetOperations
            { get => Helper.GetSetOperations<TData>(this); }

        #region IQueryable implemented properties

        public virtual Expression Expression { get; protected set; }

        public virtual Type ElementType { get => typeof(TData); }

        public virtual IQueryProvider Provider { get; protected set; }

        bool IAttributeComponent.IsEmpty { get => Power.EqualsZero(this); }

        bool IAttributeComponent.IsFull { get => Power.EqualsContinuum(this); }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static AttributeComponent()
        {
            Helper.RegisterType<TData, AttributeComponent<TData>>(
                acFactory: (domain) => new AttributeComponentFactory<TData>(domain));

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
            //factoryArgs.SetDomainGetter(GetDomain);

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
            return ExecuteQueryExpression().GetEnumeratorImpl();
        }

        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <returns></returns>
        protected internal AttributeComponent<TData> ExecuteQueryExpression()
        {
            AttributeComponent<TData> result = 
                (Provider.Execute<IEnumerable<TData>>(Expression) as AttributeComponent<TData>);
            this.Expression = result.Expression;

            return result;
        }

        public abstract IEnumerator<TData> GetEnumeratorImpl();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<TData> IAttributeComponent<TData>.GetBufferizedEnumerator()
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

        public IAttributeComponent<TData> ComplementThe()
        {
            return SetOperations.Complement(this);
        }

        public IAttributeComponent<TData> IntersectWith(
            IAttributeComponent<TData> second)
        {
            return SetOperations.Intersect(this, second);
        }

        public IAttributeComponent<TData> UnionWith(
            IAttributeComponent<TData> second)
        {
            return SetOperations.Union(this, second);
        }

        public IAttributeComponent<TData> ExceptWith(
            IAttributeComponent<TData> second)
        {
            return SetOperations.Except(this, second);
        }

        public IAttributeComponent<TData> SymmetricExceptWith(
            IAttributeComponent<TData> second)
        {
            return SetOperations.SymmetricExcept(this, second);
        }

        public bool Includes(
            IAttributeComponent<TData> second)
        {
            return SetOperations.Include(this, second);
        }

        public bool EqualsTo(
            IAttributeComponent<TData> second)
        {
            return SetOperations.Equal(this, second);
        }

        public bool IncludesOrEqualsTo(
            IAttributeComponent<TData> second)
        {
            return SetOperations.IncludeOrEqual(this, second);
        }

        #endregion

        #region IAttributeComponent implementation

        IAttributeComponent IAttributeComponent.ComplementThe()
        {
            return this.ComplementThe();
        }

        IAttributeComponent IAttributeComponent.IntersectWith(
            IAttributeComponent second)
        {
            return this.IntersectWith(second as AttributeComponent<TData>);
        }

        IAttributeComponent IAttributeComponent.UnionWith(
            IAttributeComponent second)
        {
            return this.UnionWith(second as AttributeComponent<TData>);
        }

        IAttributeComponent IAttributeComponent.ExceptWith(
            IAttributeComponent second)
        {
            return this.ExceptWith(second as AttributeComponent<TData>);
        }

        IAttributeComponent IAttributeComponent.SymmetricExceptWith(
            IAttributeComponent second)
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

        bool IAlgebraicSetObject.IncludesOrEqualsTo(
            IAlgebraicSetObject second)
        {
            return this.IncludesOrEqualsTo(second as AttributeComponent<TData>);
        }

        #endregion

        #region Operators

        public static IAttributeComponent<TData> operator ~(
            AttributeComponent<TData> first)
        {
            return first.ComplementThe();
        }

        public static IAttributeComponent<TData> operator &(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first.IntersectWith(second);
        }

        public static IAttributeComponent<TData> operator |(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first.UnionWith(second);
        }

        public static IAttributeComponent<TData> operator /(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first.ExceptWith(second);
        }

        public static IAttributeComponent<TData> operator ^(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first.SymmetricExceptWith(second);
        }

        public static bool operator ==(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first.EqualsTo(second);
        }

        public static bool operator !=(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return !(first == second);
        }

        public static bool operator >(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first.Includes(second);
        }

        public static bool operator >=(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return first.IncludesOrEqualsTo(second);
        }

        public static bool operator <(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return second.Includes(first);
        }

        public static bool operator <=(
            AttributeComponent<TData> first,
            IAttributeComponent<TData> second)
        {
            return second.IncludesOrEqualsTo(first);
        }

        #endregion

        #region Nested types

        public abstract class InstantAttributeComponentOperationExecutorsContainer<CTOperand>
            : InstantSetOperationExecutorsContainer<IAttributeComponent<TData>, CTOperand, IAttributeComponentFactory<TData>>
            where CTOperand : AttributeComponent<TData>
        {
            #region Constructors

            public InstantAttributeComponentOperationExecutorsContainer(
                IAttributeComponentFactory<TData> factory,
                Func<AttributeComponentFactoryUnarySetOperator<TData, CTOperand>>
                    complementionOperator,
                Func<InstantBinaryAttributeComponentVisitor<TData, CTOperand, IAttributeComponent<TData>>>
                    intersectionOperator,
                Func<InstantBinaryAttributeComponentVisitor<TData, CTOperand, IAttributeComponent<TData>>>
                    unionOperator,
                Func<InstantBinaryAttributeComponentVisitor<TData, CTOperand, IAttributeComponent<TData>>>
                    differenceOperator,
                Func<InstantBinaryAttributeComponentVisitor<TData, CTOperand, IAttributeComponent<TData>>>
                    symmetricExceptionOperator,
                Func<InstantBinaryAttributeComponentVisitor<TData, CTOperand, bool>>
                    inclusionComparer,
                Func<InstantBinaryAttributeComponentVisitor<TData, CTOperand, bool>>
                    equalityComparer,
                Func<InstantBinaryAttributeComponentVisitor<TData, CTOperand, bool>>
                    inclusionOrEquationComparer)
                : base(factory,
                       complementionOperator,
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
