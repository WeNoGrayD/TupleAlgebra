using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TupleAlgebraTests
{
    [TestClass]
    public class FiniteEnumerableNonFictionalAttributeComponentTests : AttributeComponentTests
    {
        [TestMethod]
        public void NonFictionalComponentCreating()
        {
            HashSet<int> component1Values = new HashSet<int>();
            component1 = intFactory.CreateNonFictional(component1Values);

            Assert.IsInstanceOfType(component1, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 1, 2 };
            List<int> component1ValuesSorted = new List<int>(component1Values);
            component1ValuesSorted.Sort();
            component1 = intFactory.CreateNonFictional(component1Values);

            Assert.IsInstanceOfType(component1, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(component1, component1ValuesSorted));

            component1Values = new HashSet<int>(intFactory.FactoryDomain);
            component1 = intFactory.CreateNonFictional(component1Values);

            Assert.IsInstanceOfType(component1, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void NonFictionalComponentComplemention()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);

            AttributeComponent<int> result = !component1;
            IEnumerable<int> resultValuesPredefined = SortedComplement(component1Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = !component1;
            resultValuesPredefined = SortedComplement(component1Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = !component1;
            resultValuesPredefined = SortedComplement(component1Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 3, 4, 6, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = !component1;
            resultValuesPredefined = SortedComplement(component1Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = !component1;
            resultValuesPredefined = SortedComplement(component1Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = !component1;
            resultValuesPredefined = SortedComplement(component1Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = !component1;
            resultValuesPredefined = SortedComplement(component1Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void NonFictionalComponentIntersectionWithEmptyComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 & component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void NonFictionalComponentIntersectionWithNonFictionalComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            HashSet<int> component2Values = new HashSet<int>() { 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 & component2;
            IEnumerable<int> resultValuesPredefined;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 5, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 3, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 6, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 7, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component2Values = new HashSet<int>() { 3, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = component2;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 3, 5 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0 };
            component2Values = new HashSet<int>() { 0 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 4, 8 };
            component2Values = new HashSet<int>() { 0, 4, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 4, 6, 8  };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 & component2;
            resultValuesPredefined = SortedIntersect(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void NonFictionalComponentIntersectionWithFullComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 & component2;
            IEnumerable<int> resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void NonFictionalComponentUnionWithEmptyComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 | component2;
            IEnumerable<int> resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void NonFictionalComponentUnionWithNonFictionalComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            HashSet<int> component2Values = new HashSet<int>() { 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 | component2;
            IEnumerable<int> resultValuesPredefined = SortedUnion(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 5, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 3, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 6, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 7, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component2Values = new HashSet<int>() { 3, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 3, 5 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = component2;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0 };
            component2Values = new HashSet<int>() { 0 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 4, 8 };
            component2Values = new HashSet<int>() { 0, 4, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = SortedUnion(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 | component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void NonFictionalComponentUnionWithFullComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 | component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void NonFictionalComponentExceptionWithEmptyComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 / component2;
            IEnumerable<int> resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void NonFictionalComponentExceptionWithNonFictionalComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            HashSet<int> component2Values = new HashSet<int>() { 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 / component2;
            IEnumerable<int> resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            resultValuesPredefined = SortedExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            resultValuesPredefined = SortedExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            resultValuesPredefined = SortedExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 5, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            resultValuesPredefined = SortedExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            resultValuesPredefined = SortedExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            resultValuesPredefined = SortedExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 3, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 6, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            resultValuesPredefined = SortedExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 7, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            resultValuesPredefined = SortedExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component2Values = new HashSet<int>() { 3, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            resultValuesPredefined = SortedExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 3, 5 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0 };
            component2Values = new HashSet<int>() { 0 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 4, 8 };
            component2Values = new HashSet<int>() { 0, 4, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 / component2;
            resultValuesPredefined = SortedExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void NonFictionalComponentExceptionWithFullComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            HashSet<int> domainValues = new HashSet<int>(intFactory.FactoryDomain);
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void NonFictionalComponentSymmetricExceptionWithEmptyComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 ^ component2;
            IEnumerable<int> resultValuesPredefined = component1;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void NonFictionalComponentSymmetricExceptionWithNonFictionalComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            HashSet<int> component2Values = new HashSet<int>() { 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 ^ component2;
            IEnumerable<int> resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 5, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 3, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 6, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 2, 4, 7, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component2Values = new HashSet<int>() { 3, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 3, 5 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0 };
            component2Values = new HashSet<int>() { 0 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 4, 8 };
            component2Values = new HashSet<int>() { 0, 4, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));

            component1Values = new HashSet<int>() { 9 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component2Values = new HashSet<int>() { 0 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));

            component1Values = new HashSet<int>() { 0 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void NonFictionalComponentSymmetricExceptionWithFullComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            HashSet<int> domainValues = new HashSet<int>(intFactory.FactoryDomain);
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 ^ component2;
            IEnumerable<int> resultValuesPredefined = SortedSymmetricExcept(component1Values, domainValues);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, domainValues);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, domainValues);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, domainValues);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, domainValues);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);

            result = component1 ^ component2;
            resultValuesPredefined = SortedSymmetricExcept(component1Values, domainValues);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void NonFictionalComponentInclusionComparisonWithEmptyComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateEmpty();

            bool result = component1 > component2;
            Assert.IsTrue(result);
            result = component1 < component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NonFictionalComponentInclusionComparisonWithNonFictionalComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            HashSet<int> component2Values = new HashSet<int>() { 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            bool result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 5, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 3, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 6, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 7, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsTrue(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component2Values = new HashSet<int>() { 1, 3, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsTrue(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 3, 4, 5 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0 };
            component2Values = new HashSet<int>() { 0 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 4, 8 };
            component2Values = new HashSet<int>() { 0, 4, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NonFictionalComponentInclusionComparisonWithFullComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateFull();

            bool result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NonFictionalComponentEqualityComparisonWithEmptyComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateEmpty();

            bool result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NonFictionalComponentEqualityComparisonWithNonFictionalComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            HashSet<int> component2Values = new HashSet<int>() { 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            bool result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 2, 5, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 3, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 6, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 7, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component2Values = new HashSet<int>() { 1, 3, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 3, 4, 5 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0 };
            component2Values = new HashSet<int>() { 0 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsTrue(result);
            result = component1 != component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 4, 8 };
            component2Values = new HashSet<int>() { 0, 4, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsTrue(result);
            result = component1 != component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsTrue(result);
            result = component1 != component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsTrue(result);
            result = component1 != component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsTrue(result);
            result = component1 != component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsTrue(result);
            result = component1 != component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsTrue(result);
            result = component1 != component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NonFictionalComponentEqualityComparisonWithFullComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateFull();

            bool result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NonFictionalComponentInclusionOrEqualityComparisonWithEmptyComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateEmpty();

            bool result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NonFictionalComponentInclusionOrEqualityComparisonWithNonFictionalComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            HashSet<int> component2Values = new HashSet<int>() { 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            bool result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 5, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 3, 4, 6, 8 };
            component2Values = new HashSet<int>() { 1, 3, 4, 6, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 7, 9 };
            component2Values = new HashSet<int>() { 1, 3, 5, 7, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component2Values = new HashSet<int>() { 1, 3, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsFalse(result);

            component1Values = new HashSet<int>() { 3, 4, 5 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0 };
            component2Values = new HashSet<int>() { 0 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 4, 8 };
            component2Values = new HashSet<int>() { 0, 4, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component2Values = new HashSet<int>() { 0, 2, 4, 6, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 6, 7, 8 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsTrue(result);

            component1Values = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            component2Values = new HashSet<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateNonFictional(component2Values);

            result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NonFictionalComponentInclusionOrEqualityComparisonWithFullComponent()
        {
            HashSet<int> component1Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateNonFictional(component1Values);
            component2 = intFactory.CreateFull();

            bool result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsTrue(result);
        }
    }
}
