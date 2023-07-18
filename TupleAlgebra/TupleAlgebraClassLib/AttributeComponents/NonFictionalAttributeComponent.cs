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
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.AttributeComponents
{
    /// <summary>
    /// Тип непустой компоненты атрибута.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class NonFictionalAttributeComponent<TData>
        : AttributeComponent<TData>, IQueryable<TData>
    {
        #region Constants

        /// <summary>
        /// Константное значение типа наполнения компоненты атрибута - нефиктивный.
        /// </summary>
        private const AttributeComponentContentType CONTENT_TYPE = AttributeComponentContentType.NonFictional;

        #endregion

        #region Instance properties

        /// <summary>
        /// Тип наполнения компоненты атрибута.
        /// </summary>
        protected override AttributeComponentContentType ContentType { get => CONTENT_TYPE; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="power"></param>
        /// <param name="queryExpression"></param>
        /// <param name="queryProvider"></param>
        public NonFictionalAttributeComponent(
            NonFictionalAttributeComponentPower power,
            IQueryProvider queryProvider,
            Expression queryExpression = null)
            : base(power, queryProvider, queryExpression)
        {
            return;
        }

        #endregion

        #region Instance methods

        /// <summary>
        /// Проверка компоненты на пустоту.
        /// </summary>
        /// <returns></returns>
        public override bool IsEmpty()
        {
            return Power.IsZero();
        }

        /// <summary>
        /// Проверка компоненты на полноту.
        /// </summary>
        /// <returns></returns>
        public override bool IsFull()
        {
            return Domain == this;
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над нефиктивными компонентами.
        /// </summary>
        protected class NonFictionalAttributeComponentOperationExecutersContainer<CTOperand1>
            : FactoryAttributeComponentOperationExecutersContainer<TData, CTOperand1>
            where CTOperand1 : NonFictionalAttributeComponent<TData>
        {
            #region Constructors

            /// <summary>
            /// Конструктор экземпляра.
            /// </summary>
            public NonFictionalAttributeComponentOperationExecutersContainer(
                AttributeComponentFactory componentFactory,
                IFactoryBinaryOperator<CTOperand1, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>> intersectionOperator,
                IFactoryBinaryOperator<CTOperand1, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>> unionOperator,
                IFactoryBinaryOperator<CTOperand1, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>> differenceOperator,
                IFactoryBinaryOperator<CTOperand1, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>> symmetricExceptionOperator,
                IInstantBinaryOperator<CTOperand1, AttributeComponent<TData>, bool> inclusionComparer,
                IInstantBinaryOperator<CTOperand1, AttributeComponent<TData>, bool> equalityComparer,
                IInstantBinaryOperator<CTOperand1, AttributeComponent<TData>, bool> inclusionOrEquationComparer)
                : base(
                      componentFactory,
                      new NonFictionalAttributeComponentComplementionOperator<TData, CTOperand1>(),
                      intersectionOperator,
                      unionOperator,
                      differenceOperator,
                      symmetricExceptionOperator,
                      inclusionComparer,
                      equalityComparer,
                      inclusionOrEquationComparer)
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
                    comparisonResult = CompareToSame(second);

                return comparisonResult;
            }
        }


        protected class AttributeComponentQueryContext
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
