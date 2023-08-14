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
    using static AttributeComponentHelper;

    /// <summary>
    /// Тип непустой компоненты атрибута.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class NonFictionalAttributeComponent<TData>
        : AttributeComponent<TData>, IQueryable<TData>
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
            return GetFactory(this).CreateNonFictional<TReproducedData>(factoryArgs);
        }

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над нефиктивными компонентами.
        /// </summary>
        protected class NonFictionalAttributeComponentOperationExecutersContainer<CTOperand>
            : FactoryAttributeComponentOperationExecutersContainer<TData, CTOperand>
            where CTOperand : NonFictionalAttributeComponent<TData>
        {
            #region Constructors

            /// <summary>
            /// Конструктор экземпляра.
            /// </summary>
            public NonFictionalAttributeComponentOperationExecutersContainer(
                Func<AttributeComponentFactory> componentFactory,
                Func<IFactoryBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>>>
                    intersectionOperator,
                Func<IFactoryBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>>>
                    unionOperator,
                Func<IFactoryBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>>>
                    differenceOperator,
                Func<IFactoryBinaryOperator<CTOperand, AttributeComponent<TData>, AttributeComponentFactory, AttributeComponent<TData>>>
                    symmetricExceptionOperator,
                Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool>>
                    inclusionComparer,
                Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool>>
                    equalityComparer,
                Func<IInstantBinaryOperator<CTOperand, AttributeComponent<TData>, bool>>
                    inclusionOrEquationComparer)
                : base(
                      componentFactory,
                      () => new NonFictionalAttributeComponentComplementionOperator<TData, CTOperand>(),
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
