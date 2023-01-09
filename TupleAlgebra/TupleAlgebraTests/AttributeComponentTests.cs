using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib;
using System.Collections.Generic;
using System.Linq;

namespace TupleAlgebraTests
{
    [TestClass]
    public class AttributeComponentTests
    {
        protected AttributeComponent<string> component1, component2;
        protected MockAttributeComponentFactory<int> intFactory;
        protected MockAttributeComponentFactory<string> stringFactory;

        protected class MockAttributeComponentFactory<TValue> : IAttributeComponentFactory<TValue>
            where TValue : IComparable<TValue>
        {
            private readonly AttributeDomain<TValue> _factoryDomain;

            public MockAttributeComponentFactory(AttributeDomain<TValue> factoryDomain)
            {
                _factoryDomain = factoryDomain;
            }

            public EmptyAttributeComponent<TValue> CreateEmpty()
            {
                return EmptyAttributeComponent<TValue>.Instance;
            }

            public AttributeComponent<TValue> CreateNonFictional(IEnumerable<TValue> values)
            {
                NonFictionalAttributeComponent<TValue> nonFictional = 
                    new FiniteEnumerableNonFictionalAttributeComponent<TValue>(_factoryDomain, values);
                if (nonFictional.IsEmpty())
                    return CreateEmpty();
                else if (nonFictional.IsFull())
                    return CreateFull();
                else
                    return nonFictional;
            }

            public FullAttributeComponent<TValue> CreateFull()
            {
                return new FullAttributeComponent<TValue>(_factoryDomain);
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            intFactory = new MockAttributeComponentFactory<int>(
                new FiniteEnumerableAttributeDomain<int>(Enumerable.Range(0, 10)));
            stringFactory = new MockAttributeComponentFactory<string>(
                new FiniteEnumerableAttributeDomain<string>(
                    new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" }));
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
    }
}
