using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework;
using System.Reflection;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.HierarchicallyPolymorphicOperators;

namespace TupleAlgebraClassLib.AttributeComponents
{
    using static AttributeComponentHelper;

    /// <summary>
    /// Тип непустой компоненты атрибута.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class NonFictionalAttributeComponent<TData>
        : AttributeComponent<TData>
    {
        #region Instance properties

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
            AttributeComponentPower power,
            IQueryProvider queryProvider,
            Expression queryExpression = null)
            : base(power, queryProvider, queryExpression)
        {
            /*
             * Нефиктивная компонента допускает приём как мощности нефиктивной компоненты,
             * так и мощности фиктивной полной. Последняя используется доменами атрибутов
             * для создания универсумов.
             */
            power.As<NonFictionalAttributeComponentPower<TData>>().InitAttributeComponent(this);

            return;
        }

        #endregion

        #region Instance methods

        protected override AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return Factory.CreateNonFictional<TReproducedData>(factoryArgs);
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над нефиктивными компонентами.
        /// </summary>
        protected class NonFictionalAttributeComponentOperationExecutorsContainer<CTOperand>
            : FactorySetOperationExecutorsContainer<
                AttributeComponent<TData>,
                CTOperand,
                AttributeComponentFactory>
            where CTOperand : NonFictionalAttributeComponent<TData>
        {
            #region Instance properties

            protected override AttributeComponentFactory Factory
            { get => Helper.GetFactory(typeof(CTOperand)); }

            #endregion

            #region Constructors

            /// <summary>
            /// Конструктор экземпляра.
            /// </summary>
            /// <param name="complementionOperator"></param>
            /// <param name="intersectionOperator"></param>
            /// <param name="unionOperator"></param>
            /// <param name="differenceOperator"></param>
            /// <param name="symmetricExceptionOperator"></param>
            /// <param name="inclusionComparer"></param>
            /// <param name="equalityComparer"></param>
            /// <param name="inclusionOrEquationComparer"></param>
            public NonFictionalAttributeComponentOperationExecutorsContainer(
                Func<IFactoryUnaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>>> 
                    complementionOperator,
                Func<IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    intersectionOperator,
                Func<IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    unionOperator,
                Func<IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    differenceOperator,
                Func<IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    symmetricExceptionOperator,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, bool>>
                    inclusionComparer,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, bool>>
                    equalityComparer,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, bool>>
                    inclusionOrEquationComparer)
                : base(
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

            /// <summary>
            /// Конструктор экземпляра.
            /// </summary>
            /// <param name="intersectionOperator"></param>
            /// <param name="unionOperator"></param>
            /// <param name="differenceOperator"></param>
            /// <param name="symmetricExceptionOperator"></param>
            /// <param name="inclusionComparer"></param>
            /// <param name="equalityComparer"></param>
            /// <param name="inclusionOrEquationComparer"></param>
            public NonFictionalAttributeComponentOperationExecutorsContainer(
                Func<IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    intersectionOperator,
                Func<IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    unionOperator,
                Func<IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    differenceOperator,
                Func<IFactoryBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, AttributeComponent<TData>>>
                    symmetricExceptionOperator,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, bool>>
                    inclusionComparer,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, bool>>
                    equalityComparer,
                Func<IInstantBinaryAttributeComponentAcceptor<TData, CTOperand, AttributeComponent<TData>, bool>>
                    inclusionOrEquationComparer)
                : this(
                      () => new NonFictionalAttributeComponentComplementionOperator<TData, CTOperand>(),
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
