using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib;
using System.Collections.Generic;
using System.Linq;

namespace TupleAlgebraTests
{
    [TestClass]
    public class NonFictionalAttributeComponentTests : AttributeComponentTests
    {
        [TestMethod]
        public void NonFictionalComponentConjunctWithEmptyComponent()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            HashSet<string> component2Values = new HashSet<string>() { };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateEmpty();

            AttributeComponent<string> result = component1 & component2;
            IEnumerable<string> resultValuesPredefined =
                SortedIntersect(component1Values, component2Values),
                                resultValuesAsOperationResult =
                result.Cast<string>();
            Assert.IsTrue(result is EmptyAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            result = component1 & component2;
            resultValuesPredefined =
                SortedIntersect(component1Values, component2Values);
            resultValuesAsOperationResult =
                result.Cast<string>();
            Assert.IsTrue(result is EmptyAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));
        }

        [TestMethod]
        public void NonFictionalComponentConjunctWithNonFictionalComponent()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            HashSet<string> component2Values = new HashSet<string>() { "d", "e", "f" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            AttributeComponent<string> result = component1 & component2;
            IEnumerable<string> resultValuesPredefined =
                SortedIntersect(component1Values, component2Values),
                                resultValuesAsOperationResult =
                result.Cast<string>();
            Assert.IsTrue(result is EmptyAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            component1Values = new HashSet<string>() { "a", "c", "e", "f" };
            component2Values = new HashSet<string>() { "b", "c", "d", "e" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            resultValuesAsOperationResult = result.Cast<string>();
            Assert.IsTrue(result is NonFictionalAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            component1Values = new HashSet<string>() { "a", "b", "c", "d", "e", "f" };
            component2Values = new HashSet<string>() { "a", "e", "f" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            resultValuesAsOperationResult = result.Cast<string>();
            Assert.IsTrue(result is NonFictionalAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));
        }

        [TestMethod]
        public void NonFictionalComponentConjunctWithFullComponent()
        {
        }

        [TestMethod]
        public void NonFictionalComponentDisjunctWithEmptyComponent()
        {
        }

        [TestMethod]
        public void NonFictionalComponentDisjunctWithNonFictionalComponent()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            HashSet<string> component2Values = new HashSet<string>() { "d", "e", "f" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            AttributeComponent<string> result = component1 | component2;
            IEnumerable<string> resultValuesPredefined = SortedUnion(component1Values, component2Values),
                                resultValuesAsOperationResult = result.Cast<string>();
            ((List<string>)resultValuesPredefined).Sort();
            Assert.IsTrue(result is NonFictionalAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            component1Values = new HashSet<string>() { "b", "e", "g", "h", "i" };
            component2Values = new HashSet<string>() { "a", "b", "c", "d" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            resultValuesAsOperationResult = result.Cast<string>();
            Assert.IsTrue(result is NonFictionalAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            component1Values = new HashSet<string>() { "b", "e", "g", "h", "i" };
            component2Values = new HashSet<string>() { "a", "b", "c", "d", "i" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            resultValuesAsOperationResult = result.Cast<string>();
            Assert.IsTrue(result is NonFictionalAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));
        }

        [TestMethod]
        public void NonFictionalComponentDisjunctWithFullComponent()
        {
        }

        [TestMethod]
        public void NonFictionalComponentEqualityComparisonWithNonFictionalComponent()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            HashSet<string> component2Values = new HashSet<string>() { "d", "e", "f" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            bool result = component1 == component2;
            bool resultValuesPredefined = false;
            Assert.AreEqual(resultValuesPredefined, result);

            component1Values = new HashSet<string>() { "b", "e", "g", "h", "i", "a" };
            component2Values = new HashSet<string>() { "i", "e", "b", "h", "g" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            resultValuesPredefined = false;
            Assert.AreEqual(resultValuesPredefined, result);

            component1Values = new HashSet<string>() { "b", "e", "g", "h", "i" };
            component2Values = new HashSet<string>() { "i", "e", "b", "h", "g" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            resultValuesPredefined = true;
            Assert.AreEqual(resultValuesPredefined, result);
        }
    }
}
