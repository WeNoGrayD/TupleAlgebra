using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Complex;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational.WithComplexKey;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational.WithSimpleKey;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using static TupleAlgebraClassLib.AttributeComponents.AttributeComponentHelper;
using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Navigational
{
    internal interface INavigationalAttributeComponentFactory<TKey, TData>
        : INonFictionalAttributeComponentFactory2<
              KeyValuePair<TKey, TData>,
              NavigationalAttributeDomainFactoryArgs<TKey, TData>>,
          IEnumerableNonFictionalAttributeComponentFactory<
              KeyValuePair<TKey, TData>,
              NavigationalAttributeComponent<TKey, TData>,
              NavigationalAttributeComponentFactoryArgs<TKey, TData>>
        where TKey : new()
        where TData : new()
    {
        public IAttributeComponentFactory<TKey> KeyAttributeComponentFactory
        { get; }

        public IAttributeComponentFactory<TData> ValueAttributeComponentFactory
        { get; }

        public Func<TData, TKey> PrincipalKeySelector { get; set; }

        public NavigationHandler<TKey, TData> NavigateByKey { get; set; }

        private KeyValuePair<TKey, TData>? AutoFillKeyValuePair(
            KeyValuePair<TKey, TData> kvp,
            Func<TKey, AttributeComponent<TKey>> keyComponentCtor)
        {
            if (kvp.Key is null)
            {
                if (kvp.Value is null) return null;

                return new(PrincipalKeySelector(kvp.Value), kvp.Value);
            }

            if (kvp.Value is null)
            {
                return new(kvp.Key, NavigateByKey(keyComponentCtor(kvp.Key)).Single());
            }

            return kvp;
        }

        private KeyValuePair<TKey, TData>? AutoFillKeyValuePairWithValue(
            KeyValuePair<TKey, TData> kvp,
            Func<TKey, AttributeComponent<TKey>> keyComponentCtor)
        {
            if (kvp.Value is null)
            {
                if (kvp.Key is null) return null;

                return new(kvp.Key, Enumerable.Single(NavigateByKey(keyComponentCtor(kvp.Key))));
            }

            return kvp;
        }

        private KeyValuePair<TKey, TData>? AutoFillKeyValuePairWithKey(
            KeyValuePair<TKey, TData> kvp)
        {
            if (kvp.Key is null)
            {
                if (kvp.Value is null) return null;

                return new(PrincipalKeySelector(kvp.Value), kvp.Value);
            }

            return kvp;
        }

        NavigationalAttributeComponentFactoryArgs<TKey, TData>
            INonFictionalAttributeComponentFactory<
            KeyValuePair<TKey, TData>,
            IEnumerable<KeyValuePair<TKey, TData>>,
            NavigationalAttributeComponentFactoryArgs<TKey, TData>>
            .CreateSpecificNonFictionalFactoryArgs(
            IEnumerable<KeyValuePair<TKey, TData>> resultElements)
        {
            return
                (KeyAttributeComponentFactory, ValueAttributeComponentFactory) switch
                {
                    (IEnumerableNonFictionalAttributeComponentFactory<TKey> keyFactory,
                     IEnumerableNonFictionalAttributeComponentFactory<TData> valueFactory) =>
                     CreateFactoryArgs(keyFactory, valueFactory),
                    (IEnumerableNonFictionalAttributeComponentFactory<TKey> keyFactory,
                     _) =>
                     CreateKeyFactoryArgs(keyFactory),
                    (_,
                     IEnumerableNonFictionalAttributeComponentFactory<TData> valueFactory) =>
                     CreateValueFactoryArgs(valueFactory)
                };

            NavigationalAttributeComponentFactoryArgs<TKey, TData> CreateFactoryArgs(
                IEnumerableNonFictionalAttributeComponentFactory<TKey> keyFactory,
                IEnumerableNonFictionalAttributeComponentFactory<TData> valueFactory)
            {
                return new NavigationalAttributeComponentFactoryArgs<TKey, TData>(
                    keyFactory.CreateNonFictionalFactoryArgs(
                        resultElements
                        .Select(AutoFillKeyValuePairWithKey)
                        .OfType<KeyValuePair<TKey, TData>>()
                        .Select(kvp => kvp.Key)),
                    valueFactory.CreateNonFictionalFactoryArgs(
                        resultElements
                        .Select(kvp =>
                            AutoFillKeyValuePairWithValue(kvp, k => keyFactory.CreateNonFictional([k])))
                        .OfType<KeyValuePair<TKey, TData>>()
                        .Select(kvp => kvp.Value)));
            }

            NavigationalAttributeComponentFactoryArgs<TKey, TData> CreateKeyFactoryArgs(
                IEnumerableNonFictionalAttributeComponentFactory<TKey> keyFactory)
            {
                return new NavigationalAttributeComponentFactoryArgs<TKey, TData>(
                    keyFactory.CreateNonFictionalFactoryArgs(resultElements.Select(kvp => kvp.Key)));
            }

            NavigationalAttributeComponentFactoryArgs<TKey, TData> CreateValueFactoryArgs(
                IEnumerableNonFictionalAttributeComponentFactory<TData> valueFactory)
            {
                return new NavigationalAttributeComponentFactoryArgs<TKey, TData>(
                    valueFactory.CreateNonFictionalFactoryArgs(resultElements.Select(kvp => kvp.Value)));
            }
        }

        AttributeComponent<KeyValuePair<TKey, TData>>
            INonFictionalAttributeComponentFactory<
            KeyValuePair<TKey, TData>,
            IEnumerable<KeyValuePair<TKey, TData>>>
            .CreateNonFictional(
            IEnumerable<KeyValuePair<TKey, TData>> resultElements)
        {
            return CreateSpecificNonFictional(
                CreateSpecificNonFictionalFactoryArgs(resultElements));
        }

        NonFictionalAttributeComponent<KeyValuePair<TKey, TData>>
            INonFictionalAttributeComponentFactory2<
                KeyValuePair<TKey, TData>,
                NavigationalAttributeDomainFactoryArgs<TKey, TData>>
            .CreateSpecificNonFictional(
                NavigationalAttributeDomainFactoryArgs<TKey, TData> args)
        {
            return new NavigationalAttributeComponent<TKey, TData>(
                args.Power,
                args.Keys,
                args.Values,
                args.QueryProvider,
                args.QueryExpression);
        }

        /*
        NavigationalAttributeComponentFactoryArgs<TKey, TData>
            ISetOperationResultFactory<
                NavigationalAttributeComponent<TKey, TData>,
                NavigationalAttributeComponentFactoryArgs<TKey, TData>,
                NavigationalAttributeComponentFactoryArgs<TKey, TData>,
                AttributeComponent<KeyValuePair<TKey, TData>>>
            .CreateFactoryArgs(
                NavigationalAttributeComponentFactoryArgs<TKey, TData>
                opResultFactoryArgs)
        {
            return opResultFactoryArgs;
        }
        */

        NonFictionalAttributeComponent<KeyValuePair<TKey, TData>>
            INonFictionalAttributeComponentFactory2<
                KeyValuePair<TKey, TData>,
                NavigationalAttributeComponentFactoryArgs<TKey, TData>>
            .CreateSpecificNonFictional(
                NavigationalAttributeComponentFactoryArgs<TKey, TData> args)
        {
            return args.Member switch
            {
                NavigationalPropertyMember.Key =>
                    new NavigationalAttributeComponent<TKey, TData>(
                        args.Power,
                        CreateKeyAttributeComponent(args),
                        args.QueryProvider,
                        args.QueryExpression)
                    { Domain = Domain },
                NavigationalPropertyMember.Value =>
                    new NavigationalAttributeComponent<TKey, TData>(
                        args.Power,
                        CreateValueAttributeComponent(args),
                        args.QueryProvider,
                        args.QueryExpression)
                    { Domain = Domain },
                NavigationalPropertyMember.Both =>
                    new NavigationalAttributeComponent<TKey, TData>(
                        args.Power,
                        CreateKeyAttributeComponent(args),
                        CreateValueAttributeComponent(args),
                        args.QueryProvider,
                        args.QueryExpression)
                    { Domain = Domain },
                _ => throw new Exception()
            };
        }

        private AttributeComponent<TKey> CreateKeyAttributeComponent(
                NavigationalAttributeComponentFactoryArgs<TKey, TData> args)
        {
            return args.KeyFactoryArgs.ProvideTo(KeyAttributeComponentFactory);
        }

        public AttributeComponent<TData> CreateValueAttributeComponent(
                NavigationalAttributeComponentFactoryArgs<TKey, TData> args)
        {
            return args.Values is not null ?
                (ValueAttributeComponentFactory as IEnumerableNonFictionalAttributeComponentFactory<TData>)
                    .CreateNonFictional(args.Values) :
                args.ValueFactoryArgs.ProvideTo(ValueAttributeComponentFactory);
        }

        /*
        NavigationalAttributeComponentFactoryArgs<TKey, TData>
            ISetOperationResultFactory<
                NavigationalAttributeComponent<TKey, TData>,
                NavigationalAttributeComponentFactoryArgs<TKey, TData>,
                NavigationalAttributeComponentFactoryArgs<TKey, TData>,
                AttributeComponent<KeyValuePair<TKey, TData>>>
            .CreateFactoryArgs(
            NavigationalAttributeComponent<TKey, TData> first,
            NavigationalAttributeComponentFactoryArgs<TKey, TData> resultElements)
        {
            throw new NotImplementedException();
        }
        */

        /*
        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                NonFictionalAttributeComponentFactoryArgs<TKey>>
            .CreateSpecificNonFictional(
            NonFictionalAttributeComponentFactoryArgs<TKey> keyArgs)
        {
            NavigationalAttributeComponentFactoryArgs<TKey, TData> navArgs =
                new NavigationalAttributeComponentFactoryArgs<TKey, TData>(
                    keyArgs);

            return CreateSpecificNonFictional(navArgs);
        }
        */

        /*
        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                NonFictionalAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictional(
            NonFictionalAttributeComponentFactoryArgs<TData> valueArgs)
        {
            NavigationalAttributeComponentFactoryArgs<TKey, TData> navArgs =
                new NavigationalAttributeComponentFactoryArgs<TKey, TData>(
                    valueArgs);

            return CreateSpecificNonFictional(navArgs);
        }
        */
    }

    internal class NavigationalAttributeComponentFactory<TKey, TData>
        : AttributeComponentFactory<
            KeyValuePair<TKey, TData>,
            NavigationalAttributeDomainFactoryArgs<TKey, TData>>,
          INavigationalAttributeComponentFactory<TKey, TData>
        where TKey : new()
        where TData : new()
    {
        public IAttributeComponentFactory<TKey> KeyAttributeComponentFactory 
        { get; set; }

        public IAttributeComponentFactory<TData> ValueAttributeComponentFactory
        { get; set; }

        public Func<TData, TKey> PrincipalKeySelector { get; set; }

        public NavigationHandler<TKey, TData> NavigateByKey { get; set; }

        public NavigationalAttributeComponentFactory(
            AttributeDomain<KeyValuePair<TKey, TData>> domain)
            : base(domain)
        { 
            return;
        }

        public NavigationalAttributeComponentFactory(
            IAttributeComponentFactory<TKey> keyAttributeComponentFactory,
            IAttributeComponentFactory<TData> valueAttributeComponentFactory)
            : base(null as AttributeDomain<KeyValuePair<TKey, TData>>)
        {
            KeyAttributeComponentFactory = keyAttributeComponentFactory;
            ValueAttributeComponentFactory = valueAttributeComponentFactory;
            InitDomainFrom(CreateDomainUniverseFactoryArgs());

            return;
        }

        public NavigationalAttributeComponentFactory(
            IAttributeComponentFactory<TKey> keyAttributeComponentFactory,
            TupleObject<TData> referencedKb)
            : base(null as AttributeDomain<KeyValuePair<TKey, TData>>)
        {
            KeyAttributeComponentFactory = keyAttributeComponentFactory;
            ValueAttributeComponentFactory =
                new ComplexAttributeComponentFactory<TData>(referencedKb);
            InitDomainFrom(CreateDomainUniverseFactoryArgs());

            return;
        }

        private NavigationalAttributeDomainFactoryArgs<TKey, TData>
            CreateDomainUniverseFactoryArgs()
        {
            return new NavigationalAttributeDomainFactoryArgs<TKey, TData>(
                KeyAttributeComponentFactory.Domain.Universe,
                ValueAttributeComponentFactory.Domain.Universe);
        }
    }

    /*
    public class NavigationalAttributeComponentWithSimpleKeyFactory<TKey, TData>
        : AttributeComponentFactory<TData>,
          INonFictionalAttributeComponentFactory<
            TData,
            NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>,
            NavigationalAttributeComponentWithSimpleKey<TKey, TData>,
            NavigationalAttributeComponentWithSimpleKeyFactoryArgs<TKey, TData>>
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
    */
}
