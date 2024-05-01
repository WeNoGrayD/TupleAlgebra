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
using UniversalClassLib.HierarchicallyPolymorphicOperators;

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
            return;
        }

        #endregion

        #region Instance methods

        protected override AttributeComponent<TReproducedData> ReproduceImpl<TReproducedData>(
            AttributeComponentFactoryArgs factoryArgs)
        {
            return null;

            //return Factory.CreateNonFictional<TReproducedData>(factoryArgs);
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над нефиктивными компонентами.
        /// </summary>
        protected abstract class NonFictionalAttributeComponentOperationExecutorsContainer<
            CTOperand, 
            TIntermediateResult,
            CTFactory, 
            CTFactoryArgs>
            : FactorySetOperationExecutorsContainer<
                IAttributeComponent<TData>,
                CTOperand,
                TIntermediateResult,
                CTFactoryArgs,
                CTFactory>
            where CTOperand : NonFictionalAttributeComponent<TData>
            where CTFactory : INonFictionalAttributeComponentFactory<TData, TIntermediateResult, CTOperand, CTFactoryArgs>
            where CTFactoryArgs : AttributeComponentFactoryArgs
        {
            #region Constructors

            /// <summary>
            /// Конструктор экземпляра.
            /// </summary>
            /// <param name="complementationOperator"></param>
            /// <param name="intersectionOperator"></param>
            /// <param name="unionOperator"></param>
            /// <param name="differenceOperator"></param>
            /// <param name="symmetricExceptionOperator"></param>
            /// <param name="inclusionComparer"></param>
            /// <param name="equalityComparer"></param>
            /// <param name="inclusionOrEquationComparer"></param>
            public NonFictionalAttributeComponentOperationExecutorsContainer(
                CTFactory factory,
                Func<FactoryUnaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, CTFactory, CTFactoryArgs, IAttributeComponent<TData>>> 
                    complementationOperator,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, CTFactory, CTFactoryArgs, IAttributeComponent<TData>>>
                    intersectionOperator,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, CTFactory, CTFactoryArgs, IAttributeComponent<TData>>>
                    unionOperator,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, CTFactory, CTFactoryArgs, IAttributeComponent<TData>>>
                    differenceOperator,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, CTFactory, CTFactoryArgs, IAttributeComponent<TData>>>
                    symmetricExceptionOperator,
                Func<InstantBinaryAttributeComponentAcceptor<TData, CTOperand, bool>>
                    inclusionComparer,
                Func<InstantBinaryAttributeComponentAcceptor<TData, CTOperand, bool>>
                    equalityComparer,
                Func<InstantBinaryAttributeComponentAcceptor<TData, CTOperand, bool>>
                    inclusionOrEquationComparer)
                : base(
                      factory,
                      complementationOperator,
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
                CTFactory factory,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, CTFactory, CTFactoryArgs, IAttributeComponent<TData>>>
                    intersectionOperator,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, CTFactory, CTFactoryArgs, IAttributeComponent<TData>>>
                    unionOperator,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, CTFactory, CTFactoryArgs, IAttributeComponent<TData>>>
                    differenceOperator,
                Func<FactoryBinaryAttributeComponentAcceptor<TData, TIntermediateResult, CTOperand, CTFactory, CTFactoryArgs, IAttributeComponent<TData>>>
                    symmetricExceptionOperator,
                Func<InstantBinaryAttributeComponentAcceptor<TData, CTOperand, bool>>
                    inclusionComparer,
                Func<InstantBinaryAttributeComponentAcceptor<TData, CTOperand, bool>>
                    equalityComparer,
                Func<InstantBinaryAttributeComponentAcceptor<TData, CTOperand, bool>>
                    inclusionOrEquationComparer)
                : this(
                      factory,
                      () => new NonFictionalAttributeComponentComplementationOperator<TData, TIntermediateResult, CTOperand, CTFactory, CTFactoryArgs>(),
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
