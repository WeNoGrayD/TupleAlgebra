using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib;
using System.Collections.Generic;
using System.Linq;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;

namespace TupleAlgebraFrameworkTests
{
    [TestClass]
    public class AttributeComponentTests
    {
        protected AttributeComponent<int> component1, component2;
        protected MockAttributeComponentFactory<int> intFactory;
        protected MockAttributeComponentFactory<string> stringFactory;
        protected Dictionary<Type, object> factories;

        protected class MockAttributeComponentFactory<TData> : 
            OrderedFiniteEnumerableAttributeComponentFactory<TData>
            where TData : IComparable<TData>
        {
            public MockAttributeComponentFactory(
                AttributeDomain<TData> domain)
                : base(domain)
            { }

            public AttributeComponent<TData> CreateNonFictional(
                IEnumerable<TData> values)
            {
                OrderedFiniteEnumerableAttributeComponentFactoryArgs factoryArgs =
                    OrderedFiniteEnumerableAttributeComponentFactoryArgs
                    .Construct(domainGetter: () => Domain, values: values);
                return CreateNonFictional(factoryArgs);
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            intFactory = new MockAttributeComponentFactory<int>(
                new OrderedFiniteEnumerableAttributeDomain<int>(Enumerable.Range(0, 10)));
            stringFactory = new MockAttributeComponentFactory<string>(
                new OrderedFiniteEnumerableAttributeDomain<string>(
                    new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" }));
            factories = new Dictionary<Type, object>()
            { { typeof(int), intFactory }, { typeof(string), stringFactory } };
        }

        protected IEnumerable<T> SortedComplement<T>(
            HashSet<T> first)
            where T : IComparable<T>
        {
            MockAttributeComponentFactory<T> factory =
                factories[typeof(T)] as MockAttributeComponentFactory<T>;
            first.SymmetricExceptWith(factory.Domain);
            List<T> resultValuesList = new List<T>(first);
            resultValuesList.Sort();
            return resultValuesList;
        }

        protected IEnumerable<T> SortedBinarySetOperation<T>(
            HashSet<T> first, 
            HashSet<T> second,
            Action<HashSet<T>, HashSet<T>> opAction)
        {
            opAction(first, second);
            List<T> resultValuesList = new List<T>(first);
            resultValuesList.Sort();
            return resultValuesList;
        }

        protected IEnumerable<T> SortedIntersect<T>(HashSet<T> first, HashSet<T> second)
        {
            return SortedBinarySetOperation(first, second, 
                (set1, set2) => set1.IntersectWith(set2));
        }

        protected IEnumerable<T> SortedUnion<T>(HashSet<T> first, HashSet<T> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.UnionWith(set2));
        }

        protected IEnumerable<T> SortedExcept<T>(HashSet<T> first, HashSet<T> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.ExceptWith(set2));
        }

        protected IEnumerable<T> SortedSymmetricExcept<T>(HashSet<T> first, HashSet<T> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.SymmetricExceptWith(set2));
        }
    }
}
