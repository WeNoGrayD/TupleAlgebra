using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraTests
{
    [TestClass]
    public class IEnumerableTests
    {
        private class A
        {
            public int InvokeCount { get; private set; } = 0;

            private int _obj;
            public int Obj
            {
                get
                {
                    InvokeCount++;
                    return _obj;
                }
            }

            public A(int obj) => _obj = obj;
        }

        [TestMethod]
        public void Test()
        {
            List<A> _as = new List<A>() { new A(1), new A(20), new A(300)};
            IEnumerable<int> objs = _as.Select(a => a.Obj);
            foreach (int obj in objs)
                Console.WriteLine(obj);
            foreach (int obj in objs.Skip(1))
                Console.WriteLine(obj);
            Console.WriteLine("Invokes:");
            foreach (A a in _as)
                Console.WriteLine(a.InvokeCount);
        }
    }
}
