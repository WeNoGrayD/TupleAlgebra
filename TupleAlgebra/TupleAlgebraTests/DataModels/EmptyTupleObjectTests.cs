using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraTests.DataModels
{
    [TestClass]
    public class EmptyTupleObjectTests
    {
        [TestMethod]
        public void Test()
        {
            EmptyTupleObject<int> eto = new EmptyTupleObject<int>(null);
            ConjunctiveTuple<int> ct = new ConjunctiveTuple<int>(null);
            TupleObject<int> tores = eto & ct;
        }
    }
}
