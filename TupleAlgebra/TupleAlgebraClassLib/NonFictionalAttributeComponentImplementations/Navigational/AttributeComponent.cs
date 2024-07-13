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
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Complex;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational
{
    using static AttributeComponentHelper;

    internal static class NavigationalAttributeComponentHelper
    {
        private static TupleObjectFactory _toFactory = new(null);

        public static NavigationByKeyHandler<TKey, TData>
            CreateNavigationBySimpleForeignKey<TKey, TData>(
            TupleObject<TData> source,
            Func<TupleObjectFactory, AttributeComponent<TKey>, TupleObject<TData>> toEntityTuple,
            Func<TupleObject<TData>, AttributeComponent<TData>> toEntityComponent)
            where TKey : new()
            where TData : new()
        {
            return (AttributeComponent<TKey> foreignKey) =>
            {
                TupleObject<TData> entityTuple = toEntityTuple(_toFactory, foreignKey),
                                   resTuple = source & entityTuple;
                return toEntityComponent(resTuple);
            };
        }

        public static NavigationByKeyHandler<TKey, TData>
            CreateNavigationByComplexForeignKey<TKey, TData>(
            TupleObject<TData> source,
            Func<TupleObjectFactory, ComplexAttributeComponent<TKey>, TupleObject<TData>> toEntityTuple,
            Func<TupleObject<TData>, AttributeComponent<TData>> toEntityComponent)
            where TKey : new()
            where TData : new()
        {
            return (AttributeComponent<TKey> foreignKey) =>
            {
                ComplexAttributeComponent<TKey> complexForeignKey =
                    foreignKey as ComplexAttributeComponent<TKey>;
                TupleObject<TData> entityTuple = toEntityTuple(_toFactory, complexForeignKey),
                                   resTuple = source & entityTuple;
                return toEntityComponent(resTuple);
            };
        }
    }

    public class NavigationalAttributeComponent<TKey, TData>
        : NonFictionalAttributeComponent<TData>
        where TKey : new()
        where TData : new()
    {
        private AttributeComponent<TData> _navigationalAttribute;

        private Func<AttributeComponent<TData>> _navigationalAttributeGetter;

        public AttributeComponent<TKey> ForeignKey { get; private set; }

        public AttributeComponent<TData> NavigationalAttribute 
        { get => _navigationalAttribute ??= _navigationalAttributeGetter(); }

        internal NavigationByKeyHandler<TKey, TData> NavigateByKey 
        {
            get => Helper.GetNavigationByKeyHandler<TKey, TData>(this);
        }

        #region Constructors

        static NavigationalAttributeComponent()
        {
            Helper.RegisterType<TData, NavigationalAttributeComponent<TKey, TData>>(
                setOperations: (factory) => null);

            return;
        }

        public NavigationalAttributeComponent(
            AttributeComponentPower power,
            AttributeComponent<TKey> foreignKey,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            ForeignKey = foreignKey;
            _navigationalAttributeGetter = () => NavigateByKey(ForeignKey);

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
