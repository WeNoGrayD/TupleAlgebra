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

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational.WithSimpleKey
{
    using static AttributeComponentHelper;

    /*
    public class NavigationalAttributeComponentWithSimpleKey<TKey, TData>
        : NavigationalAttributeComponent<TKey, TData, AttributeComponent<TKey>>
        where TData : new()
    {
        #region Constructors

        static NavigationalAttributeComponentWithSimpleKey()
        {
            Helper.RegisterType<TData, NavigationalAttributeComponentWithSimpleKey<TKey, TData>>(
                acFactory: (domain) => new NavigationalAttributeComponentWithSimpleKeyFactory<TKey, TData>(domain),
                setOperations: (factory) => new NavigationalAttributeComponentOperationExecutorsContainer((NavigationalAttributeComponentWithSimpleKeyFactory<TKey, TData>)factory));

            return;
        }

        public NavigationalAttributeComponentWithSimpleKey(
            AttributeComponentPower power,
            AttributeComponent<TKey> foreignKey,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power,
                  foreignKey,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            return;
        }

        public NavigationalAttributeComponentWithSimpleKey(
            AttributeComponentPower power,
            AttributeComponent<TData> navigationalAttribute,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power,
                  navigationalAttribute,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
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

        #region Nested types

        private class NavigationalAttributeComponentOperationExecutorsContainer
            : NonFictionalAttributeComponentOperationExecutorsContainer<
                NavigationalAttributeComponentWithSimpleKey<TKey, TData>,
                NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>,
                NavigationalAttributeComponentWithSimpleKeyFactory<TKey, TData>,
                NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>>
        {
            #region Constructors

            public NavigationalAttributeComponentOperationExecutorsContainer(
                NavigationalAttributeComponentWithSimpleKeyFactory<TKey, TData> factory)
                : base(
                      factory,
                      () => null,
                      () => null,
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
    */
}
