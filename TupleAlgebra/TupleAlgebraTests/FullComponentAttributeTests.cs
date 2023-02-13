using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TupleAlgebraTests
{
    [TestClass]
    public class FullComponentAttributeTests : AttributeComponentTests
    {
        [TestMethod]
        public void FullComponentCreating()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateFull();

            Assert.AreNotEqual(component1, component2);
        }

        [TestMethod]
        public void FullComponentComplemention()
        {
            component1 = intFactory.CreateFull();

            AttributeComponent<int> result = !component1;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void FullComponentIntersectionWithEmptyComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 & component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void FullComponentIntersectionWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 & component2;
            IEnumerable<int> resultValuesPredefined = component2;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void FullComponentIntersectionWithFullComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 & component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void FullComponentUnionWithEmptyComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 | component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void FullComponentUnionWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 | component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void FullComponentUnionWithFullComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 | component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void FullComponentExceptionWithEmptyComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void FullComponentExceptionWithNonFictionalComponent()
        {
            HashSet<int> domainValues = new HashSet<int>(intFactory.FactoryDomain);
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 / component2;
            IEnumerable<int> resultValuesPredefined = SortedExcept(domainValues, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void FullComponentExceptionWithFullComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void FullComponentSymmetricExceptionWithEmptyComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void FullComponentSymmetricExceptionWithNonFictionalComponent()
        {
            HashSet<int> domainValues = new HashSet<int>(intFactory.FactoryDomain);
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 ^ component2;
            IEnumerable<int> resultValuesPredefined = SortedSymmetricExcept(domainValues, component2Values);
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void FullComponentSymmetricExceptionWithFullComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void FullComponentInclusionComparisonWithEmptyComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateEmpty();

            bool result = component1 > component2;
            Assert.IsTrue(result);
            result = component1 < component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FullComponentInclusionComparisonWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateNonFictional(component2Values);

            bool result = component1 > component2;
            Assert.IsTrue(result);
            result = component1 < component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FullComponenInclusionComparisonWithFullComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateFull();

            bool result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FullComponentEqualityComparisonWithEmptyComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateEmpty();

            bool result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FullComponentEqualityComparisonWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateNonFictional(component2Values);

            bool result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void FullComponentEqualityComparisonWithFullComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateFull();

            bool result = component1 == component2;
            Assert.IsTrue(result);
            result = component1 != component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FullComponentInclusionOrEqualityComparisonWithEmptyComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateEmpty();

            bool result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FullComponentInclusionOrEqualityComparisonWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateNonFictional(component2Values);

            bool result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FullComponentInclusionOrEqualityComparisonWithFullComponent()
        {
            component1 = intFactory.CreateFull();
            component2 = intFactory.CreateFull();

            bool result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsTrue(result);
        }
    }
}
