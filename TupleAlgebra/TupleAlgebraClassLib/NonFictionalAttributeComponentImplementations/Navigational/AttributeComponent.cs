using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
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

        public static NavigationHandler<TKey, TData>
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

        public static NavigationHandler<TKey, TData>
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

        public static NavigationHandler<TData, TKey>
            CreateSimplePrincipalKeySelection<TKey, TData>(
            IEnumerableNonFictionalAttributeComponentFactory<TKey> foreignKeyComponentFactory,
            Func<TData, TKey> principalKeySelector)
            where TKey : new()
            where TData : new()
        {
            return (AttributeComponent<TData> navigationalAttr) =>
            {
                return foreignKeyComponentFactory
                    .CreateNonFictional(navigationalAttr.Select(principalKeySelector));
            };
        }
    }

    internal class NavigationalAttributeComponent<TKey, TData>
        : NonFictionalAttributeComponent<KeyValuePair<TKey, TData>>
        where TKey : new()
        where TData : new()
    {
        private AttributeComponent<TKey> _foreignKey;

        private AttributeComponent<TData> _navigationalAttribute;

        public AttributeComponent<TKey> ForeignKey 
        {
            get => _foreignKey ??= SelectPrincipalKey(_navigationalAttribute);
            private set => _foreignKey = value;
        }

        public AttributeComponent<TData> NavigationalAttribute 
        { 
            get => _navigationalAttribute ??= NavigateByKey(_foreignKey);
            private set => _navigationalAttribute = value;
        }

        public NavigationHandler<TKey, TData> NavigateByKey 
        {
            get => Helper.GetNavigationByKeyHandler<TKey, TData>(this);
        }

        public Func<TData, TKey> PrincipleKeySelector
        {
            get => Helper.GetPrincipleKeySelector<TKey, TData>(this);
        }

        public NavigationHandler<TData, TKey> SelectPrincipalKey
        {
            get => Helper.GetKeySelectionHandler<TKey, TData>(this);
        }

        #region Constructors

        static NavigationalAttributeComponent()
        {
            Helper.RegisterType<
                KeyValuePair<TKey, TData>, 
                NavigationalAttributeComponent<TKey, TData>>(
                acFactory: (domain) => new NavigationalAttributeComponentFactory<TKey, TData>(domain),
                setOperations: (factory) => null);

            return;
        }

        public NavigationalAttributeComponent(
            AttributeComponentPower power,
            AttributeComponent<TKey> foreignKey,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : this(
                  power,
                  foreignKey,
                  null,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            return;
        }

        public NavigationalAttributeComponent(
            AttributeComponentPower power,
            AttributeComponent<TData> navigationalAttribute,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : this(
                  power,
                  null,
                  navigationalAttribute,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            return;
        }

        public NavigationalAttributeComponent(
            AttributeComponentPower power,
            AttributeComponent<TKey> foreignKey,
            AttributeComponent<TData> navigationalAttribute,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            ForeignKey = foreignKey;
            NavigationalAttribute = navigationalAttribute;

            return;
        }

        #endregion

        #region Instance methods

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(
            IEnumerable<TReproducedData> populatingData)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<KeyValuePair<TKey, TData>> GetEnumeratorImpl()
        {
            return (_foreignKey, _navigationalAttribute) switch
            { 
                (not null, not null) => Enumerable
                    .Zip(_foreignKey, _navigationalAttribute)
                    .Select(kvp => new KeyValuePair<TKey, TData>(kvp.First, kvp.Second))
                    .GetEnumerator(),
                (not null, null) => Enumerable
                    .Zip(_foreignKey, NavigationalAttribute)
                    .Select(kvp => new KeyValuePair<TKey, TData>(kvp.First, kvp.Second))
                    .GetEnumerator(),
                (null, not null) => _navigationalAttribute
                    .Select(na => new KeyValuePair<TKey, TData>(PrincipleKeySelector(na), na))
                    .GetEnumerator(),
                _ => throw new Exception("Компонента навигационного атрибута должна содержать хотя бы ключ или хотя бы значение.")
            };
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
