using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean;

namespace TupleAlgebraTests
{
    [TestClass]
    public class BooleanAttributeComponentTests
    {
        [TestMethod]
        public void ComplementationTest()
        {
            AttributeComponent<bool> first, result;

            first = BooleanNonFictionalAttributeComponent.False;

            result = ~first;
            Assert.AreEqual(result, BooleanNonFictionalAttributeComponent.True);

            first = BooleanNonFictionalAttributeComponent.True;

            result = ~first;
            Assert.AreEqual(result, BooleanNonFictionalAttributeComponent.False);
        }

        [TestMethod]
        public void IntersectionTest()
        {
            AttributeComponent<bool> first, second, result;

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateEmpty();

            result = first & second;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateEmpty();

            result = first & second;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first & second; 
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first & second;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first & second;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first & second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateFull();

            result = first & second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateFull();

            result = first & second;
            Assert.AreEqual(result, first);
        }

        [TestMethod]
        public void UnionTest()
        {
            AttributeComponent<bool> first, second, result;

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateEmpty();

            result = first | second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateEmpty();

            result = first | second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first | second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first | second;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first | second;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first | second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateFull();

            result = first | second;
            Assert.AreEqual(result, second);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateFull();

            result = first | second;
            Assert.AreEqual(result, second);
        }

        [TestMethod]
        public void SymmetricExceptionTest()
        {
            AttributeComponent<bool> first, second, result;

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateEmpty();

            result = first ^ second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateEmpty();

            result = first ^ second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first ^ second;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first ^ second;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first ^ second;
            Assert.IsInstanceOfType(result, typeof(FullAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first ^ second;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateFull();

            result = first ^ second;
            Assert.AreEqual(result, BooleanNonFictionalAttributeComponent.True);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateFull();

            result = first ^ second;
            Assert.AreEqual(result, BooleanNonFictionalAttributeComponent.False);
        }

        [TestMethod]
        public void ExceptionTest()
        {
            AttributeComponent<bool> first, second, result;

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateEmpty();

            result = first / second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateEmpty();

            result = first / second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first / second;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first / second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first / second;
            Assert.AreEqual(result, first);

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first / second;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateFull();

            result = first / second;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<bool>));

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateFull();

            result = first / second;
            Assert.IsInstanceOfType(result, typeof(EmptyAttributeComponent<bool>));
        }

        [TestMethod]
        public void EqualityTest()
        {
            AttributeComponent<bool> first, second;
            bool result;

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateEmpty();

            result = first == second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateEmpty();

            result = first == second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first == second;
            Assert.IsTrue(result);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first == second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first == second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first == second;
            Assert.IsTrue(result);

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateFull();

            result = first == second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateFull();

            result = first == second;
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void InclusionTest()
        {
            AttributeComponent<bool> first, second;
            bool result;

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateEmpty();

            result = first < second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateEmpty();

            result = first < second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first < second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first < second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first < second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first < second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateFull();

            result = first < second;
            Assert.IsTrue(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateFull();

            result = first < second;
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void InclusionOrEqualityTest()
        {
            AttributeComponent<bool> first, second;
            bool result;

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateEmpty();

            result = first <= second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateEmpty();

            result = first <= second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first <= second;
            Assert.IsTrue(result);

            first = BooleanNonFictionalAttributeComponent.False;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first <= second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.False;

            result = first <= second;
            Assert.IsFalse(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = BooleanNonFictionalAttributeComponent.True;

            result = first <= second;
            Assert.IsTrue(result);

            first = BooleanNonFictionalAttributeComponent.False;
            second = first.Factory.CreateFull();

            result = first <= second;
            Assert.IsTrue(result);

            first = BooleanNonFictionalAttributeComponent.True;
            second = first.Factory.CreateFull();

            result = first <= second;
            Assert.IsTrue(result);
        }
    }
}
