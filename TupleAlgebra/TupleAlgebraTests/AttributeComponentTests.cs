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
        AttributeComponent<string> component1, component2;
        MockAttributeComponentFactory<int> intFactory;
        MockAttributeComponentFactory<string> stringFactory;

        class MockAttributeComponentFactory<TValue> : IAttributeComponentFactory<TValue>
            where TValue : IComparable<TValue>
        {
            public NonFictionalAttributeComponent<TValue> CreateNonFictional(IEnumerable<TValue> values)
            {
                return new NonFictionalAttributeComponent<TValue>(values);
            }

            public EmptyAttributeComponent<TValue> CreateEmpty()
            {
                return new EmptyAttributeComponent<TValue>();
            }

            public FullAttributeComponent<TValue> CreateFull()
            {
                return new FullAttributeComponent<TValue>();
            }
        }

        [TestInitialize]
        public void SetUp()
        {
            intFactory = new MockAttributeComponentFactory<int>();
            stringFactory = new MockAttributeComponentFactory<string>();
        }

        private IEnumerable<T> SortedBinarySetOperation<T>(
            HashSet<T> first, 
            HashSet<T> second,
            Action<HashSet<T>, HashSet<T>> opAction)
        {
            opAction(first, second);
            List<T> resultValuesList = new List<T>(first);
            resultValuesList.Sort();
            return resultValuesList;
        }

        private IEnumerable<T> SortedIntersect<T>(HashSet<T> first, HashSet<T> second)
        {
            return SortedBinarySetOperation(first, second, 
                (set1, set2) => set1.IntersectWith(set2));
        }

        private IEnumerable<T> SortedUnion<T>(HashSet<T> first, HashSet<T> second)
        {
            return SortedBinarySetOperation(first, second,
                (set1, set2) => set1.UnionWith(set2));
        }

        [TestMethod]
        public void NonFictionalComponentConjunctWithNonFictionalComponentTest()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            HashSet<string> component2Values = new HashSet<string>() { "d", "e", "f" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            AttributeComponent<string> result = component1 & component2;
            IEnumerable<string> resultValuesPredefined = 
                SortedIntersect(component1Values, component2Values),
                                resultValuesAsOperationResult = 
                result.Values.Cast<string>();
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            component1Values = new HashSet<string>() { "a", "c", "e", "f" };
            component2Values = new HashSet<string>() { "b", "c", "d", "e" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            resultValuesAsOperationResult = result.Values.Cast<string>();
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            component1Values = new HashSet<string>() { "a", "b", "c", "d", "e", "f" };
            component2Values = new HashSet<string>() { "a", "e", "f" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            resultValuesAsOperationResult = result.Values.Cast<string>();
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

        }

        [TestMethod]
        public void NonFictionalComponentConjunctWithEmptyComponentTest()
        {
        }

        [TestMethod]
        public void NonFictionalComponentConjunctWithFullComponentTest()
        {
        }

        [TestMethod]
        public void EmptyComponentConjunctWithNonFictionalComponentTest()
        {
        }

        [TestMethod]
        public void EmptyComponentConjunctWithEmptyComponentTest()
        {
        }

        [TestMethod]
        public void EmptyComponentConjunctWithFullComponentTest()
        {
        }

        [TestMethod]
        public void FullComponentConjunctWithNonFictionalComponentTest()
        {
        }

        [TestMethod]
        public void FullComponentConjunctWithEmptyComponentTest()
        {
        }

        [TestMethod]
        public void FullComponentConjunctWithFullComponentTest()
        {
        }


        [TestMethod]
        public void NonFictionalComponentDisjunctWithNonFictionalComponentTest()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            HashSet<string> component2Values = new HashSet<string>() { "d", "e", "f" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);
            
            AttributeComponent<string> result = component1 | component2;
            IEnumerable<string> resultValuesPredefined = SortedUnion(component1Values, component2Values),
                                resultValuesAsOperationResult = result.Values.Cast<string>();
            ((List<string>)resultValuesPredefined).Sort();
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            component1Values = new HashSet<string>() { "b", "e", "g", "h", "i" };
            component2Values = new HashSet<string>() { "a", "b", "c", "d" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            resultValuesAsOperationResult = result.Values.Cast<string>();
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            component1Values = new HashSet<string>() { "b", "e", "g", "h", "i" };
            component2Values = new HashSet<string>() { "a", "b", "c", "d", "i" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            resultValuesAsOperationResult = result.Values.Cast<string>();
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));
        }

        [TestMethod]
        public void NonFictionalComponentDisjunctWithEmptyComponentTest()
        {
        }

        [TestMethod]
        public void NonFictionalComponentDisjunctWithFullComponentTest()
        {
        }

        [TestMethod]
        public void EmptyComponentDisjunctWithNonFictionalComponentTest()
        {
        }

        [TestMethod]
        public void EmptyComponentDisjunctWithEmptyComponentTest()
        {
        }

        [TestMethod]
        public void EmptyComponentDisjunctWithFullComponentTest()
        {
        }

        [TestMethod]
        public void FullComponentDisjunctWithNonFictionalComponentTest()
        {
        }

        [TestMethod]
        public void FullComponentDisjunctWithEmptyComponentTest()
        {
        }

        [TestMethod]
        public void FullComponentDisjunctWithFullComponentTest()
        {
        }
    }
}
