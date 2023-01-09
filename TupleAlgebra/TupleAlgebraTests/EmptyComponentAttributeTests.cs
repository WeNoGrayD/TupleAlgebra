using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TupleAlgebraTests
{
    [TestClass]
    public class EmptyComponentAttributeTests : AttributeComponentTests
    {
        [TestMethod]
        public void EmptyComponentConjunctWithNonFictionalComponent()
        {
        }

        [TestMethod]
        public void EmptyComponentConjunctWithEmptyComponent()
        {
        }

        [TestMethod]
        public void EmptyComponentConjunctWithFullComponent()
        {
        }

        [TestMethod]
        public void EmptyComponentDisjunctWithNonFictionalComponent()
        {
        }

        [TestMethod]
        public void EmptyComponentDisjunctWithEmptyComponent()
        {
        }

        [TestMethod]
        public void EmptyComponentDisjunctWithFullComponent()
        {
        }
    }
}
