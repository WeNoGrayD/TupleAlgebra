using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TupleAlgebraTests
{
    [TestClass]
    public class NonFictionalAttributeComponentTests : AttributeComponentTests
    {
        [TestMethod]
        public void NonFictionalComponentConjunctWithEmptyComponent()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateEmpty();

            AttributeComponent<string> result = component1 & component2;
            IEnumerable<string> resultValuesPredefined = new HashSet<string>();
            Assert.IsTrue(result is EmptyAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<string>();
            component1 = stringFactory.CreateNonFictional(component1Values);

            result = component1 & component2;
            Assert.IsTrue(result is EmptyAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void NonFictionalComponentConjunctWithNonFictionalComponent()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            sw.Stop();
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
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateFull();

            AttributeComponent<string> result = component1 & component2;
            IEnumerable<string> resultValuesPredefined = component1Values,
                                resultValuesAsOperationResult =
                result.Cast<string>();
            Assert.IsTrue(result is NonFictionalAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            Stopwatch sw = new Stopwatch();
            sw.Start();

            component1Values = new HashSet<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i" };
            component1 = stringFactory.CreateNonFictional(component1Values);

            result = component1 & component2;
            resultValuesPredefined = component1Values;
            resultValuesAsOperationResult = result.Cast<string>();
            Assert.IsTrue(result is NonFictionalAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            ;
        }

        [TestMethod]
        public void NonFictionalComponentDisjunctWithEmptyComponent()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateEmpty();

            AttributeComponent<string> result = component1 | component2;
            IEnumerable<string> resultValuesPredefined = component1Values,
                                resultValuesAsOperationResult =
                result.Cast<string>();
            Assert.IsTrue(result is NonFictionalAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            Stopwatch sw = new Stopwatch();
            sw.Start();

            component1Values = new HashSet<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i" };
            component1 = stringFactory.CreateNonFictional(component1Values);

            result = component1 | component2;
            resultValuesPredefined = component1Values;
            resultValuesAsOperationResult = result.Cast<string>();
            Assert.IsTrue(result is NonFictionalAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));
            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            ;
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
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateFull();

            AttributeComponent<string> result = component1 | component2;
            IEnumerable<string> resultValuesPredefined = stringFactory.FactoryDomain,
                                resultValuesAsOperationResult =
                result.Cast<string>();
            Assert.IsTrue(result is FullAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            Stopwatch sw = new Stopwatch();
            sw.Start();

            component1Values = new HashSet<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i" };
            component1 = stringFactory.CreateNonFictional(component1Values);

            result = component1 | component2;
            resultValuesPredefined = stringFactory.FactoryDomain;
            resultValuesAsOperationResult = result.Cast<string>();
            Assert.IsTrue(result is FullAttributeComponent<string>);
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, resultValuesAsOperationResult));

            sw.Stop();
            var (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            ;
        }

        [TestMethod]
        public void NonFictionalComponentEqualityComparisonWithEmptyComponent()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateEmpty();

            bool result = component1 == component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<string>() { "b", "e", "g", "h", "i", "a" };
            component1 = stringFactory.CreateNonFictional(component1Values);

            result = component1 == component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<string>();
            component1 = stringFactory.CreateNonFictional(component1Values);

            result = component1 == component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NonFictionalComponentEqualityComparisonWithNonFictionalComponent()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            HashSet<string> component2Values = new HashSet<string>() { "d", "e", "f" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            bool result = component1 == component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<string>() { "b", "e", "g", "h", "i", "a" };
            component2Values = new HashSet<string>() { "i", "e", "b", "h", "g" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<string>() { "b", "e", "g", "h", "i" };
            component2Values = new HashSet<string>() { "i", "e", "b", "h", "g" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NonFictionalComponentEqualityComparisonWithFullComponent()
        {
            HashSet<string> component1Values = new HashSet<string>() { "a", "b", "c" };
            component1 = stringFactory.CreateNonFictional(component1Values);
            component2 = stringFactory.CreateFull();

            bool result = component1 == component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<string>() { "b", "e", "g", "h", "i", "a" };
            component1 = stringFactory.CreateNonFictional(component1Values);

            result = component1 == component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<string>(stringFactory.FactoryDomain);
            component1 = stringFactory.CreateNonFictional(component1Values);

            result = component1 == component2;
            Assert.IsTrue(result);
        }
    }
}
