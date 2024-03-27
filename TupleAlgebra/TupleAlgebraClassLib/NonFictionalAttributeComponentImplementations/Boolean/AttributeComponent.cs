using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    using static AttributeComponentHelper;

    public class BooleanNonFictionalAttributeComponent
        : NonFictionalAttributeComponent<bool>
    {
        #region Static fields

        private static Lazy<BooleanNonFictionalAttributeComponent> _false
            = new Lazy<BooleanNonFictionalAttributeComponent>(
                () => InitConstant(false));

        private static Lazy<BooleanNonFictionalAttributeComponent> _true
            = new Lazy<BooleanNonFictionalAttributeComponent>(
                () => InitConstant(true));

        #endregion

        #region Static properties

        public static BooleanNonFictionalAttributeComponent False 
        { get => _false.Value; }

        public static BooleanNonFictionalAttributeComponent True 
        { get => _true.Value; }

        #endregion

        #region Instance properties

        public bool Value { get; private set; }

        #endregion

        #region Constructors

        static BooleanNonFictionalAttributeComponent()
        {
            Helper.RegisterType<bool, BooleanNonFictionalAttributeComponent>(
                acFactory: (_) => BooleanAttributeComponentFactory.Instance,
                setOperations: (factory) => new BooleanAttributeComponentOperationExecutorsContainer((IBooleanAttributeComponentFactory)factory));

            return;
        }

        public BooleanNonFictionalAttributeComponent(
            AttributeComponentPower power,
            bool value,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power, 
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(), 
                  queryExpression)
        {
            Value = value;

            return;
        }

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(IEnumerable<TReproducedData> populatingData)
        {
            if (populatingData is IEnumerable<bool> values)
                return new BooleanAttributeComponentFactoryArgs(values.First(), queryProvider: Provider);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Перечисление значений компоненты.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<bool> GetEnumeratorImpl()
        {
            yield return Value;
        }

        #endregion

        #region Static methods

        private static BooleanNonFictionalAttributeComponent 
            InitConstant(bool value)
        {
            return BooleanAttributeComponentFactory.Instance
                .CreateConstantNonFictional<BooleanNonFictionalAttributeComponent, BooleanAttributeComponentFactoryArgs>
                (new BooleanAttributeComponentFactoryArgs(value));
        }

        #endregion

        #region Operators

        public static bool operator true(BooleanNonFictionalAttributeComponent b) => b.Value;

        public static bool operator false(BooleanNonFictionalAttributeComponent b) => !b.Value;

        #endregion

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой аттрибута.
        /// </summary>
        private class BooleanAttributeComponentOperationExecutorsContainer
            : NonFictionalAttributeComponentOperationExecutorsContainer<
                BooleanNonFictionalAttributeComponent,
                bool,
                IBooleanAttributeComponentFactory,
                BooleanAttributeComponentFactoryArgs>
        {
            #region Constructors

            public BooleanAttributeComponentOperationExecutorsContainer(
                IBooleanAttributeComponentFactory factory) 
                : base(
                      factory,
                      () => new ComplementationOperator(),
                      () => new IntersectionOperator(),
                      () => new UnionOperator(),
                      () => new ExceptionOperator(),
                      () => new SymmetricExceptionOperator(),
                      () => new InclusionComparer(),
                      () => new EqualityComparer(),
                      () => new InclusionOrEqualityComparer())
            {
                return;
            }

            #endregion
        }

        #endregion
    }
}
