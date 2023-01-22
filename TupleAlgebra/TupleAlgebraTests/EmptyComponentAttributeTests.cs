using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace TupleAlgebraTests
{
    [TestClass]
    public class EmptyComponentAttributeTests : AttributeComponentTests
    {
        [TestMethod]
        public void EmptyComponentCreating()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateEmpty();

            Assert.AreEqual(component1, component2);
        }

        [TestMethod]
        public void EmptyComponentComplemention()
        {
            component1 = intFactory.CreateEmpty();

            AttributeComponent<int> result = !component1;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentIntersectionWithEmptyComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 & component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentIntersectionWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 & component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentIntersectionWithFullComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 & component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentUnionWithEmptyComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 | component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentUnionWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 | component2;
            IEnumerable<int> resultValuesPredefined = component2;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void EmptyComponentUnionWithFullComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 | component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentExceptionWithEmptyComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentExceptionWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentExceptionWithFullComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 / component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentSymmetricExceptionWithEmptyComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateEmpty();

            AttributeComponent<int> result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentSymmetricExceptionWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateNonFictional(component2Values);

            AttributeComponent<int> result = component1 ^ component2;
            IEnumerable<int> resultValuesPredefined = component2;
            Assert.IsInstanceOfType(result, typeof(NonFictionalAttributeComponent<int>));
            Assert.IsTrue(Enumerable.SequenceEqual(resultValuesPredefined, result));
        }

        [TestMethod]
        public void EmptyComponentSymmetricExceptionWithFullComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateFull();

            AttributeComponent<int> result = component1 ^ component2;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<int>));
        }

        [TestMethod]
        public void EmptyComponentInclusionComparisonWithEmptyComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateEmpty();

            bool result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EmptyComponentInclusionComparisonWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateNonFictional(component2Values);

            bool result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EmptyComponenInclusionComparisonWithFullComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateFull();

            bool result = component1 > component2;
            Assert.IsFalse(result);
            result = component1 < component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EmptyComponentEqualityComparisonWithEmptyComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateEmpty();

            bool result = component1 == component2;
            Assert.IsTrue(result);
            result = component1 != component2;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EmptyComponentEqualityComparisonWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateNonFictional(component2Values);

            bool result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EmptyComponentEqualityComparisonWithFullComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateFull();

            bool result = component1 == component2;
            Assert.IsFalse(result);
            result = component1 != component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EmptyComponentInclusionOrEqualityComparisonWithEmptyComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateEmpty();

            bool result = component1 >= component2;
            Assert.IsTrue(result);
            result = component1 <= component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EmptyComponentInclusionOrEqualityComparisonWithNonFictionalComponent()
        {
            HashSet<int> component2Values = new HashSet<int>() { 0, 1, 2 };
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateNonFictional(component2Values);

            bool result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EmptyComponentInclusionOrEqualityComparisonWithFullComponent()
        {
            component1 = intFactory.CreateEmpty();
            component2 = intFactory.CreateFull();

            bool result = component1 >= component2;
            Assert.IsFalse(result);
            result = component1 <= component2;
            Assert.IsTrue(result);
        }
    }
}
