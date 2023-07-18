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

namespace TupleAlgebraClassLib.AttributeComponents
{
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
          IAlgebraicSetObject
    {
        public virtual Expression Expression { get; protected set; }

        public virtual Type ElementType { get => typeof(TData); }

        public virtual IQueryProvider Provider { get; protected set; }

        public AttributeComponentPower Power { get; private set; }

        protected abstract AttributeComponentContentType ContentType { get; }

        public AttributeDomain<TData> Domain { get => _getDomain(); }

        public static readonly AttributeComponentFactory FictionalAttributeComponentFactory
            = new AttributeComponentFactory();

        private event Func<AttributeDomain<TData>> _getDomain;

        public event Func<AttributeDomain<TData>> GetDomain
        {
            add
            {
                if (_getDomain is null)
                    _getDomain += value;
                else
                {
                    throw new InvalidOperationException("К событию получения домента атрибута нельзя подключить больше одного обработчика.");
                }
            }
            remove
            {
                throw new InvalidOperationException("От события получения домента атрибута нельзя отключить обработчик.");
            }
        }

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static AttributeComponent()
        { }

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
            Power.InitAttributeComponent(this);
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

        public virtual AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(IEnumerable<TReproducedData> populatingData)
        {
            return new AttributeComponentFactoryArgs();
        }

        public void IncludeDomain(AttributeComponentFactoryArgs factoryArgs)
        {
            factoryArgs.SetAttributeDomainGetter(_getDomain.GetInvocationList()[0] as Func<AttributeDomain<TData>>);
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

        public abstract bool IsEmpty();

        public abstract bool IsFull();

        #endregion

        #region Operators

        public static AttributeComponent<TData> operator !(AttributeComponent<TData> first)
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

        #region InnerTypes

        /// <summary>
        /// Перечисление типов содержимого компоненты атрибута.
        /// </summary>
        protected internal enum AttributeComponentContentType : byte
        {
            /// <summary>
            /// Пустая фиктивная компонента.
            /// </summary>
            Empty = 0,
            /// <summary>
            /// Непустая компонента.
            /// </summary>
            NonFictional = 1,
            /// <summary>
            /// Полная фиктивная компонента.
            /// </summary>
            Full = 2
        }

        /// <summary>
        /// Абстрактный тип мощности компоненты атрибута.
        /// </summary>
        public abstract class AttributeComponentPower : IComparable<AttributeComponentPower>
        {
            internal abstract AttributeComponentContentType ContentType { get; }

            public abstract void InitAttributeComponent(AttributeComponent<TData> component);

            public abstract bool IsZero();

            public virtual int CompareTo(AttributeComponentPower second)
            {
                return ContentType.CompareTo(second.ContentType);
            }

            #region Operators

            /// <summary>
            /// Оператор сравнения на равенство двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator ==(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) == 0;
            }

            /// <summary>
            /// Оператор сравнения на неравенство двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator !=(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) != 0;
            }

            /// <summary>
            /// Оператор сравнения "больше" двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator >(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) == 1;
            }

            /// <summary>
            /// Оператор сравнения "больше или равно" двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator >=(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) > -1;
            }

            /// <summary>
            /// Оператор сравнения "меньше" двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator <(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) == -1;
            }

            /// <summary>
            /// Оператор сравнения "меньше или равно" двух мощностей.
            /// </summary>
            /// <param name="first"></param>
            /// <param name="second"></param>
            /// <returns></returns>
            public static bool operator <=(AttributeComponentPower first, AttributeComponentPower second)
            {
                return first.CompareTo(second) < 1;
            }

            #endregion
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
        protected abstract ISetOperationExecutersContainer<AttributeComponent<TData>, CTAttributeComponent> _setOperations
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
            return _setOperations.Complement(this as CTAttributeComponent);
        }

        protected override AttributeComponent<TData> IntersectWith(AttributeComponent<TData> second)
        {
            return _setOperations.Intersect(this as CTAttributeComponent, second);
        }

        protected override AttributeComponent<TData> UnionWith(AttributeComponent<TData> second)
        {
            return _setOperations.Union(this as CTAttributeComponent, second);
        }

        protected override AttributeComponent<TData> ExceptWith(AttributeComponent<TData> second)
        {
            return _setOperations.Except(this as CTAttributeComponent, second);
        }

        protected override AttributeComponent<TData> SymmetricExceptWith(AttributeComponent<TData> second)
        {
            return _setOperations.SymmetricExcept(this as CTAttributeComponent, second);
        }

        protected override bool Includes(AttributeComponent<TData> second)
        {
            return _setOperations.Include(this as CTAttributeComponent, second);
        }

        protected override bool EqualsTo(AttributeComponent<TData> second)
        {
            return _setOperations.Equal(this as CTAttributeComponent, second);
        }

        protected override bool IncludesOrEqualsTo(AttributeComponent<TData> second)
        {
            return _setOperations.IncludeOrEqual(this as CTAttributeComponent, second);
        }
    }
}
