using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Navigational;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational
{
    using static AttributeComponentHelper;

    public abstract class NavigationalAttributeComponent<TKey, TData, TKeyContainer>
        : NonFictionalAttributeComponent<TData>
        where TData : new()
    {
        private AttributeComponent<TData> _navigationalAttribute;

        private Func<AttributeComponent<TData>> _navigationalAttributeGetter;

        public TKeyContainer ForeignKey { get; private set; }

        public AttributeComponent<TData> NavigationalAttribute 
        { get => _navigationalAttribute ??= _navigationalAttributeGetter(); }

        internal delegate AttributeComponent<TData> NavigateByKeyHandler(
            TKeyContainer key);

        internal static NavigateByKeyHandler NavigateByKey { private get; set; }

        #region Constructors

        public NavigationalAttributeComponent(
            AttributeComponentPower power,
            TKeyContainer foreignKey,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            ForeignKey = foreignKey;
            _navigationalAttributeGetter = () =>
                Helper.GetNavigationByKeyHandler(this)(ForeignKey);

            return;
        }

        public NavigationalAttributeComponent(
            AttributeComponentPower power,
            AttributeComponent<TData> navigationalAttribute,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            _navigationalAttributeGetter = () => _navigationalAttribute;

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
            return NavigationalAttribute.GetEnumerator();
        }

        #endregion
    }

    public class NavigationalAttributeComponentPower
        : FiniteEnumerableAttributeComponentPower
    {
        #region Constructors

        static NavigationalAttributeComponentPower()
        {
            return;
        }

        #endregion

        #region Instance methods

        protected override int CompareToZero<TData>(AttributeComponent<TData> ac)
        {
            return 1;
            //var tupleBased = (ac as NavigationalAttributeComponent<TData>);
            //return (tupleBased.Sample.IsEmpty() || tupleBased.Mask.IsEmpty()) ? 0 : 1;
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
            //var tupleBased = (ac as NavigationalAttributeComponent<TData>);
            //return tupleBased.Sample.IsFull() && tupleBased.Mask.IsFull();
        }

        #endregion
    }
}
