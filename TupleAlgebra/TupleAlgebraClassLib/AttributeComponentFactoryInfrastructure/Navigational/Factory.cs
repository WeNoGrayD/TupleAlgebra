using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational.WithComplexKey;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational.WithSimpleKey;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Navigational
{
    /*
    public class NavigationalAttributeComponentFactory<TKey, TData>
        : AttributeComponentFactory<TData>,
          INonFictionalAttributeComponentFactory<
            TData,
            NavigationalAttributeComponentFactoryArgs<TKey, TData>,
            NavigationalAttributeComponent<TKey, TData>,
            NavigationalAttributeComponentFactoryArgs<TKey, TData>>
    {
        public NavigationalAttributeComponentFactory(
            AttributeDomain<TData> domain)
            : base(domain)
        { 
            return;
        }

        NavigationalAttributeComponentFactoryArgs<TKey, TData>
            ISetOperationResultFactory<
                NavigationalAttributeComponent<TKey, TData>,
                NavigationalAttributeComponentFactoryArgs<TKey, TData>,
                NavigationalAttributeComponentFactoryArgs<TKey, TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
                NavigationalAttributeComponentFactoryArgs<TKey, TData>
                opResultFactoryArgs)
        {
            return opResultFactoryArgs;
        }

        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                NavigationalAttributeComponentFactoryArgs<TKey, TData>>
            .CreateSpecificNonFictional(
                NavigationalAttributeComponentFactoryArgs<TKey, TData> args)
        {
            return args;
        }

        NavigationalAttributeComponentFactoryArgs<TKey, TData>
            ISetOperationResultFactory<
                NavigationalAttributeComponent<TKey, TData>,
                NavigationalAttributeComponentFactoryArgs<TKey, TData>,
                NavigationalAttributeComponentFactoryArgs<TKey, TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
            NavigationalAttributeComponent<TKey, TData> first,
            NavigationalAttributeComponentFactoryArgs<TKey, TData> resultElements)
        {
            throw new NotImplementedException();
        }
    }
    */

    public class NavigationalAttributeComponentWithSimpleKeyFactory<TKey, TData>
        : AttributeComponentFactory<TData>,
          INonFictionalAttributeComponentFactory<
            TData,
            NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>,
            NavigationalAttributeComponentWithSimpleKey<TKey, TData>,
            NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>>//,
          //IUnorderedFiniteEnumerableAttributeComponentFactory<TKey>
        where TData : new()
    {
        private UnorderedFiniteEnumerableAttributeComponentFactory<TKey>
            _keyComponentFactory;

        private IAttributeComponentFactory<TData> _navAttrComponentFactory;

        private Func<TData, TKey> _principalKeySelector;

        public IAttributeComponentFactory<TData> NavigationalAttributeComponentFactory
        {
            get => _navAttrComponentFactory;
            set => _navAttrComponentFactory = value;
        }

        public NavigationalAttributeComponentWithSimpleKeyFactory(
            AttributeDomain<TKey> keyAttrDomain,
            TupleObject<TData> navAttrDomain,
            Func<TData, TKey> principalKeySelector)
            : base(null)
        {
            //_principalKeySelector = principalKeySelector;
            _keyComponentFactory = new(keyAttrDomain);
            _navAttrComponentFactory = new UnorderedFiniteEnumerableAttributeComponentFactory<TData>(
                navAttrDomain);
            Domain = _navAttrComponentFactory.Domain;

            return;
        }

        public void SetPrincipalKeySelector()
        {
            
        }

        public AttributeComponent<TKey> SelectKey(
            NavigationalAttributeComponentWithSimpleKey<TKey, TData> values)
        {
            UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TKey> factoryArgs =
                new UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TKey>(
                Enumerable.Select<TData, TKey>(values, _principalKeySelector).ToHashSet());

            return _keyComponentFactory.CreateNonFictional(factoryArgs);
        }

        public AttributeComponent<TData> SelectValue(
            TupleObject<TData> tuple)
        {
            UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData> factoryArgs =
                new UnorderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(
                    tuple);

            return _navAttrComponentFactory.CreateNonFictional(factoryArgs);
        }

        NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>
            ISetOperationResultFactory<
                NavigationalAttributeComponentWithSimpleKey<TKey, TData>,
                NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>,
                NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
                NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>
                opResultFactoryArgs)
        {
            return opResultFactoryArgs;
        }

        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>>
            .CreateSpecificNonFictional(
                NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData> args)
        {
            return null;
        }

        NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>
            ISetOperationResultFactory<
                NavigationalAttributeComponentWithSimpleKey<TKey, TData>,
                NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>,
                NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
            NavigationalAttributeComponentWithSimpleKey<TKey, TData> first,
            NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData> resultElements)
        {
            throw new NotImplementedException();
        }
    }

    public class NavigationalAttributeComponentWithComplexKeyFactory<TKey, TData>
        : AttributeComponentFactory<TData>,
          INonFictionalAttributeComponentFactory<
            TData,
            NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>,
            NavigationalAttributeComponentWithComplexKey<TKey, TData>,
            NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>>//,
          //IUnorderedFiniteEnumerableAttributeComponentFactory<TKey>
        where TKey : new()
        where TData : new()
    {
        private UnorderedFiniteEnumerableAttributeComponentFactory<TKey> _keyFactory;

        public NavigationalAttributeComponentWithComplexKeyFactory(
            AttributeDomain<TData> domain)
            : base(domain)
        {
            return;
        }

        NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>
            ISetOperationResultFactory<
                NavigationalAttributeComponentWithComplexKey<TKey, TData>,
                NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>,
                NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
                NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>
                opResultFactoryArgs)
        {
            return opResultFactoryArgs;
        }

        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>>
            .CreateSpecificNonFictional(
                NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData> args)
        {
            return null;
        }

        NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>
            ISetOperationResultFactory<
                NavigationalAttributeComponentWithComplexKey<TKey, TData>,
                NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>,
                NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
            NavigationalAttributeComponentWithComplexKey<TKey, TData> first,
            NavigationalAttributeComponentWithComplexKeyFactoryArgs<TKey, TData> resultElements)
        {
            throw new NotImplementedException();
        }
    }
}
