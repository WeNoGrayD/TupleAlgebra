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
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational.WithComplexKey
{
    using static AttributeComponentHelper;

    /*
    public class NavigationalAttributeComponentWithComplexKey<TKey, TData>
        : NavigationalAttributeComponent<TKey, TData, TupleObject<TKey>>
        where TKey : new()
        where TData : new()
    {
        #region Constructors

        static NavigationalAttributeComponentWithComplexKey()
        {
            Helper.RegisterType<TData, NavigationalAttributeComponentWithComplexKey<TKey, TData>>(
                acFactory: (domain) => new NavigationalAttributeComponentWithComplexKeyFactory<TKey, TData>(domain),
                setOperations: (factory) => new NavigationalAttributeComponentOperationExecutorsContainer((NavigationalAttributeComponentWithComplexKeyFactory<TKey, TData>)factory));

            return;
        }

        public NavigationalAttributeComponentWithComplexKey(
            AttributeComponentPower power,
            TupleObject<TKey> foreignKey,
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

        public NavigationalAttributeComponentWithComplexKey(
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
                NavigationalAttributeComponentWithComplexKey<TKey, TData>,
                NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>,
                NavigationalAttributeComponentWithComplexKeyFactory<TKey, TData>,
                NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>>
        {
            #region Constructors

            public NavigationalAttributeComponentOperationExecutorsContainer(
                NavigationalAttributeComponentWithComplexKeyFactory<TKey, TData> factory)
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
