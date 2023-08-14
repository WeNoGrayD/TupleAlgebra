using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using System.Numerics;

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
          IComparisonOperators<AttributeComponent<TData>, AttributeComponent<TData>, bool>
    {
        #region Instance properties

        public AttributeComponentPower Power { get; private set; }

        public AttributeDomain<TData> Domain { get => GetDomain(); }

        public Func<AttributeDomain<TData>> GetDomain { get;  set; }

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
            RegisterType<TData>(
                typeof(AttributeComponent<TData>),
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
        }

        protected abstract AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs);

        public IReproducingQueryable<TReproducedData> Reproduce<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return ReproduceImpl<TReproducedData>(factoryArgs);// as IReproducingQueryable<TReproducedData>;
        }

        public IReproducingQueryable<TReproducedData> Reproduce<TReproducedData>(
            IEnumerable<TReproducedData> reproducedData)
        {
            /*
            if (typeof(TData) != typeof(TReproducedData))
            {
                throw new ArgumentException($"Тип {this.GetType()} не может воспроизводить " +
                    $"компоненты атрибутов, чей тип данных отличается от типа данных {this.GetType()}.\n" +
                    $"Тип данных воспроизводимой компоненты атрибута: {typeof(TReproducedData)}.");
            }
            */

            return ReproduceImpl<TReproducedData>(ZipInfo(reproducedData));// as IEnumerable<TData>))
                                                                           //as IReproducingQueryable<TReproducedData>;
        }

        #endregion

        /*
        protected static void InitSetOperations(
            AttributeComponentContentType contentType,
            InstantAttributeComponentOperationExecutersContainer<TData> setOperations)
        {
            _setOperations[contentType] = setOperations;
        }
        */

        public IEnumerator<TData> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<TData>>(Expression) as AttributeComponent<TData>).GetEnumeratorImpl();
        }

        public abstract IEnumerator<TData> GetEnumeratorImpl();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Set operations

        protected abstract AttributeComponent<TData> ComplementThe();

        protected abstract AttributeComponent<TData> IntersectWith(AttributeComponent<TData> second);

        protected abstract AttributeComponent<TData> UnionWith(AttributeComponent<TData> second);

        protected abstract AttributeComponent<TData> ExceptWith(AttributeComponent<TData> second);

        protected abstract AttributeComponent<TData> SymmetricExceptWith(AttributeComponent<TData> second);

        protected abstract bool Includes(AttributeComponent<TData> second);

        protected abstract bool EqualsTo(AttributeComponent<TData> second);

        protected abstract bool IncludesOrEqualsTo(AttributeComponent<TData> second);

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
    }

    /*
    public interface ISetOperationApplicable<BTOperand>
    {
        public TOperationResult MakeOp<TOperand1, TOperand2, TOperationResult>(
            TOperand1 op1, TOperand2 op2)
            where TOperand1 : BTOperand
            where TOperand2 : BTOperand
        {
            return (this as ISetOperationApplicable<BTOperand, TOperand1>).MakeOp<TOperand2, TOperationResult>(op1, op2);
        }
    }

    public interface ISetOperationApplicable<BTOperand, CTOperand1>
        where CTOperand1 : BTOperand
    {
        public TOperationResult MakeOp<TOperand2, TOperationResult>(
            CTOperand1 op1, TOperand2 op2)
            where TOperand2 : BTOperand
        {

        }
    }
    */

    public abstract class AttributeComponent<TData, CTAttributeComponent> : AttributeComponent<TData>
        where CTAttributeComponent : AttributeComponent<TData>
    {
        protected abstract ISetOperationExecutersContainer<AttributeComponent<TData>, CTAttributeComponent> SetOperations
        { get; }

        public AttributeComponent(
            AttributeComponentPower power,
            IQueryProvider queryProvider,
            Expression queryExpression)
            : base(power, queryProvider, queryExpression)
        {
            return;
        }

        protected override AttributeComponent<TData> ComplementThe()
        {
            return SetOperations.Complement(this as CTAttributeComponent);
        }

        protected override AttributeComponent<TData> IntersectWith(AttributeComponent<TData> second)
        {
            return SetOperations.Intersect(this as CTAttributeComponent, second);
        }

        protected override AttributeComponent<TData> UnionWith(AttributeComponent<TData> second)
        {
            return SetOperations.Union(this as CTAttributeComponent, second);
        }

        protected override AttributeComponent<TData> ExceptWith(AttributeComponent<TData> second)
        {
            return SetOperations.Except(this as CTAttributeComponent, second);
        }

        protected override AttributeComponent<TData> SymmetricExceptWith(AttributeComponent<TData> second)
        {
            return SetOperations.SymmetricExcept(this as CTAttributeComponent, second);
        }

        protected override bool Includes(AttributeComponent<TData> second)
        {
            return SetOperations.Include(this as CTAttributeComponent, second);
        }

        protected override bool EqualsTo(AttributeComponent<TData> second)
        {
            return SetOperations.Equal(this as CTAttributeComponent, second);
        }

        protected override bool IncludesOrEqualsTo(AttributeComponent<TData> second)
        {
            return SetOperations.IncludeOrEqual(this as CTAttributeComponent, second);
        }
    }
}
