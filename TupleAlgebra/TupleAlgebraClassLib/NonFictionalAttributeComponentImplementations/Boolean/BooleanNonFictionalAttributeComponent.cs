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

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    using static AttributeComponentHelper;

    public class BooleanNonFictionalAttributeComponent
        : NonFictionalAttributeComponent<bool>
    {
        #region Static properties

        public static BooleanNonFictionalAttributeComponent False { get; private set; }

        public static BooleanNonFictionalAttributeComponent True { get; private set; }

        #endregion

        #region Instance properties

        public bool Value { get; private set; }

        #endregion

        #region Constructors

        static BooleanNonFictionalAttributeComponent()
        {
            Helper.RegisterType<bool, BooleanNonFictionalAttributeComponent>(
                acFactory: new BooleanAttributeComponentFactory(),
                setOperations: new BooleanAttributeComponentOperationExecutorsContainer());

            AttributeComponentPower power = AtomicNonFictionalAttributeComponentPower<bool>.Instance;
            False = new BooleanNonFictionalAttributeComponent(power, false);
            True = new BooleanNonFictionalAttributeComponent(power, true);

            return;
        }

        public BooleanNonFictionalAttributeComponent(
            AttributeComponentPower power,
            bool value,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(power, queryProvider, queryExpression)
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

        #region Nested types

        /// <summary>
        /// Контейнер исполнителей операций над упорядоченной конечной перечислимой компонентой аттрибута.
        /// </summary>
        private class BooleanAttributeComponentOperationExecutorsContainer
            : NonFictionalAttributeComponentOperationExecutorsContainer<BooleanNonFictionalAttributeComponent>
        {
            #region Constructors

            public BooleanAttributeComponentOperationExecutorsContainer() : base(
                () => new ComplementionOperator(),
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

    /// <summary>
    /// Мощность компоненты, которая может включать 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public sealed class AtomicNonFictionalAttributeComponentPower<TData> 
        : NonFictionalAttributeComponentPower<TData>
    {
        #region Static properties

        public static AtomicNonFictionalAttributeComponentPower<TData> Instance { get; private set; }

        #endregion

        #region Instance properties

        protected override NonFictionalAttributeComponent<TData> Component { get => null; }

        #endregion

        #region Constructors

        static AtomicNonFictionalAttributeComponentPower()
        {
            Instance = new AtomicNonFictionalAttributeComponentPower<TData>();

            return;
        }

        private AtomicNonFictionalAttributeComponentPower()
        { 
            return; 
        }

        #endregion

        #region Instance methods

        public override bool EqualsZero()
        {
            return false;
        }

        public override bool EqualsContinuum()
        {
            return false;
        }

        public override void InitAttributeComponent(NonFictionalAttributeComponent<TData> component)
        {
            return;
        }

        protected override int CompareToZero()
        {
            return 1;
        }

        protected override int CompareToSame(dynamic second)
        {
            if (second is AtomicNonFictionalAttributeComponentPower<TData> second2)
                return 0;
            else
                throw new InvalidCastException("Непустая булевая компонента сравнивается с непустой компонентой другого вида, в данный момент эта операция не поддерживается.");
        }

        #endregion
    }
}
