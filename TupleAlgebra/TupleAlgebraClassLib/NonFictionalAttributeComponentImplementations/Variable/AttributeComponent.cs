using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable
{
    using static AttributeComponentHelper;

    public interface IVariableAttributeComponent : IAttributeComponent
    {
        public string Name { get; }

        public IAttributeComponent CurrentValue { get; set; }

        public void Reset();
    }

    public class VariableAttributeComponent<TData>
        : NonFictionalAttributeComponent<TData>, IVariableAttributeComponent
    {
        #region Instance fields

        private AttributeComponent<TData> _currentValue;

        #endregion

        #region Instance properties

        public override bool IsLazy { get => true; }

        public string Name { get; private set; }

        public AttributeComponent<TData> CurrentValue 
        {
            get
            {
                if (_currentValue is null) Reset();
                return _currentValue;
            }
            set => _currentValue = value;
        }

        IAttributeComponent IVariableAttributeComponent.CurrentValue
        { 
            get => CurrentValue;
            set => CurrentValue = (value as AttributeComponent<TData>);
        }

        #endregion

        #region Constructors

        static VariableAttributeComponent()
        {
            Helper.RegisterType<TData, VariableAttributeComponent<TData>>(
                acFactory: (domain) => new VariableAttributeComponentFactory<TData>(domain),
                setOperations: (factory) => new VariableAttributeComponentOperationExecutorsContainer((IVariableAttributeComponentFactory<TData>)factory));

            return;
        }

        public VariableAttributeComponent(
            AttributeComponentPower power,
            string name,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            Name = name;

            return;
        }

        #endregion

        #region Instance methods

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(
            IEnumerable<TReproducedData> populatingData)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            /*
            foreach (TData value in CurrentValue)
            { 

            }

            yield break;
            */
            return CurrentValue.GetEnumerator();
        }

        public void Reset()
        {
            CurrentValue = Factory.CreateFull();

            return;
        }

        #endregion

        #region Nested types

        private class VariableAttributeComponentOperationExecutorsContainer
            : NonFictionalAttributeComponentOperationExecutorsContainer<
                VariableAttributeComponent<TData>,
                VariableAttributeComponentFactoryArgs<TData>,
                IVariableAttributeComponentFactory<TData>,
                VariableAttributeComponentFactoryArgs<TData>>
        {
            #region Constructors

            public VariableAttributeComponentOperationExecutorsContainer(
                IVariableAttributeComponentFactory<TData> factory)
                : base(
                      factory,
                      () => null,
                      () => new IntersectionOperator<TData>(),
                      () => null,
                      () => null,
                      () => null,
                      () => null,
                      () => null,
                      () => null)
            { }

            #endregion
        }

        #endregion
    }

    public class VariableAttributeComponentPower
        : NonFictionalAttributeComponentPower
    {
        #region Constructors

        static VariableAttributeComponentPower()
        {
            return;
        }

        #endregion

        #region Instance methods

        protected override int CompareToZero<TData>(AttributeComponent<TData> ac)
        {
            AttributeComponent<TData> inner =
                (ac as VariableAttributeComponent<TData>)!.CurrentValue;

            return inner.Power.EqualsZero(inner) ? 0 : 1;
        }

        public override int CompareToNonFictional<TData>(
            AttributeComponentPower second,
            AttributeComponent<TData> ac1,
            AttributeComponent<TData> ac2)
        {
            /*
             * Если вызывается этот метод, то мощность заведомо равняется
             * мощности некоторой нефиктивной компоненты, не пустой и 
             * не полной. Определить более точно, какая мощность больше,
             * можно лишь с затратой времени и ресурсов.
             */
            return 0;
        }

        public override bool EqualsContinuum<TData>(AttributeComponent<TData> ac)
        {
            return false;
        }

        #endregion}
    }
}
