using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutersContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework;
using System.Reflection;

namespace TupleAlgebraClassLib
{
    /// <summary>
    /// Тип непустой компоненты атрибута.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public abstract class NonFictionalAttributeComponent<TValue> : AttributeComponent<TValue>, IQueryable<TValue>
    {
        #region Constants

        /// <summary>
        /// Константное значение типа наполнения компоненты атрибута - нефиктивный.
        /// </summary>
        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.NonFictional;

        #endregion

        #region Static properties

        /// <summary>
        /// Словарь контейнеров исполнителей операций над нефиктивной компонентой атрибута.
        /// Ключ: строковое представление математического вида нефиктивной компоненты.
        /// Значение: фабричный контейнер исполнителей операций над компонентов атрибута.
        /// </summary>
        private static Dictionary<string, FactorySetOperationExecutersContainer<TValue>> _nonFictionalSpecificSetOperations;

        #endregion

        #region Instance properties

        /// <summary>
        /// Тип наполнения компоненты атрибута.
        /// </summary>
        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }
        /// <summary>
        /// Домен атрибута.
        /// </summary>
        public AttributeDomain<TValue> Domain { get; private set; }

        #endregion

        #region Abstract instance properties

        /// <summary>
        /// Строковое представление математического вида нефиктивной компоненты, то есть 
        /// обозначение для типа конкретной её реализации.
        /// </summary>
        protected abstract string NatureType { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static NonFictionalAttributeComponent()
        {
            AttributeComponent<TValue>.InitSetOperations(
                CONTENT_TYPE, new NonFictionalAttributeComponentOperationExecutersContainer());

            _nonFictionalSpecificSetOperations = new Dictionary<string, FactorySetOperationExecutersContainer<TValue>>();
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="power"></param>
        /// <param name="queryExpression"></param>
        /// <param name="queryProvider"></param>
        public NonFictionalAttributeComponent(
            AttributeDomain<TValue> domain,
            NonFictionalAttributeComponentPower power,
            NonFictionalAttributeComponentQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(power)
        {
            Domain = domain;
            Expression = queryExpression ?? Expression.Constant(this);
            Provider = queryProvider;
            queryProvider.AppendDataSource(this);
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="power"></param>
        /// <param name="setDomainCallback"></param>
        /// <param name="queryExpression"></param>
        /// <param name="queryProvider"></param>
        public NonFictionalAttributeComponent(
            NonFictionalAttributeComponentPower power,
            out Action<AttributeDomain<TValue>> setDomainCallback,
            NonFictionalAttributeComponentQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(power)
        {
            setDomainCallback = (domain) => Domain = domain;
            Expression = queryExpression ?? Expression.Constant(this);
            Provider = queryProvider;
            queryProvider.AppendDataSource(this);
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Установка контейнера исполнителей операций над нефиктивной компонентой.
        /// </summary>
        /// <param name="natureType"></param>
        /// <param name="setOperations"></param>
        protected static void InitSetOperations(
            string natureType,
            FactorySetOperationExecutersContainer<TValue> setOperations)
        {
            _nonFictionalSpecificSetOperations[natureType] = setOperations;
        }

        #endregion

        #region Instance methods

        public AttributeComponent<TValueNew> ProduceNewOfSameNatureType<TValueNew>(
            AttributeComponentFactoryArgs<TValueNew> factoryArgs)
        {
            return _nonFictionalSpecificSetOperations[NatureType]
                .ProduceNonFictionalAttributeComponent(factoryArgs);
        }

        #endregion

        #region  Abstract instance methods

        /// <summary>
        /// Проверка нефиктивной компоненты на пустоту.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsEmpty();

        /// <summary>
        /// Проверка нефиктивной компоненты на полноту.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsFull();

        #endregion

        #region Set operations

        /// <summary>
        /// Операция пересечения двух нефиктивных компонент (AND).
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        internal AttributeComponent<TValue> IntersectWith(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].Intersect(this, second);
        }

        /// <summary>
        /// Операция объединения двух нефиктивных компонент (OR).
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        internal AttributeComponent<TValue> UnionWith(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].Union(this, second);
        }

        /// <summary>
        /// Операция исключения второй нефиктивной компоненты из первой (DIFF).
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        internal AttributeComponent<TValue> ExceptWith(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].Except(this, second);
        }

        /// <summary>
        /// Операция симметричного исключения двух нефиктивных компонент (XOR).
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        internal AttributeComponent<TValue> SymmetricExceptWith(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].SymmetricExcept(this, second);
        }

        /// <summary>
        /// Операция проверки строгого включения второй нефиктивной компоненты в первую.
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        internal bool Includes(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].Include(this, second);
        }

        /// <summary>
        /// Операция сравнения двух нефиктивных компонент на равенство.
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        internal bool EqualsTo(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].Equal(this, second);
        }

        /// <summary>
        /// Операция проверки нестрогого включения второй нефиктивной компоненты в первую.
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        internal bool IncludesOrEqualsTo(NonFictionalAttributeComponent<TValue> second)
        {
            return _nonFictionalSpecificSetOperations[NatureType].IncludeOrEqual(this, second);
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над нефиктивными компонентами.
        /// </summary>
        private sealed class NonFictionalAttributeComponentOperationExecutersContainer : InstantSetOperationExecutersContainer<TValue>
        {
            #region Constructors

            /// <summary>
            /// Конструктор экземпляра.
            /// </summary>
            public NonFictionalAttributeComponentOperationExecutersContainer() : base(
                new NonFictionalAttributeComponentComplementionOperator<TValue>(),
                new NonFictionalAttributeComponentIntersectionOperator<TValue>(),
                new NonFictionalAttributeComponentUnionOperator<TValue>(),
                new NonFictionalAttributeComponentExceptionOperator<TValue>(),
                new NonFictionalAttributeComponentSymmetricExceptionOperator<TValue>(),
                new NonFictionalAttributeComponentInclusionComparer<TValue>(),
                new NonFictionalAttributeComponentEqualityComparer<TValue>(),
                new NonFictionalAttributeComponentInclusionOrEqualityComparer<TValue>())
            { }

            #endregion
        }

        /// <summary>
        /// Мощность нефиктивной компоненты.
        /// </summary>
        public abstract class NonFictionalAttributeComponentPower : AttributeComponentPower
        {
            #region Constants

            /// <summary>
            /// Константное значение типа наполнения компоненты - нефиктивный.
            /// </summary>
            internal override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

            #endregion

            protected abstract int CompareToSame(dynamic second);

            public override sealed int CompareTo(AttributeComponentPower second)
            {
                int comparisonResult = base.CompareTo(second);
                if (comparisonResult == 0)
                    comparisonResult = this.CompareToSame(second);

                return comparisonResult;
            }
        }


        protected class NonFictionalAttributeComponentQueryContext
        {
            public object Execute(Expression queryExpression)
            {
                return null;
            }
        }

        public class NonFictionalAttributeComponentQueryInspector : ExpressionVisitor
        {
            protected override sealed Expression VisitMethodCall(MethodCallExpression node)
            {
                Expression node2;

                switch (node.Method.Name)
                {
                    case nameof(Queryable.Any):
                        { break; }
                    case nameof(Queryable.All):
                        { break; }
                    case nameof(Queryable.Select):
                        { break; }
                    case nameof(Queryable.SelectMany):
                        { break; }
                    case nameof(Queryable.Where):
                        { break; }
                    case nameof(Queryable.First):
                        { break; }
                    case nameof(Queryable.FirstOrDefault):
                        { break; }
                    case nameof(Queryable.Single):
                        { break; }
                    case nameof(Queryable.SingleOrDefault):
                        { break; }
                }
                return base.VisitMethodCall(node);
            }
        }

        #endregion
    }
}
