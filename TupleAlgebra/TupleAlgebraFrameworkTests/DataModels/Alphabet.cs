using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraFrameworkTests.DataModels
{
    public class Alphabet<T1>
    {
        public T1 A { get; set; }
    }

    public class Alphabet<T1, T2>
        : Alphabet<T1>
    {
        public T2 B { get; set; }
    }

    public class Alphabet<T1, T2, T3>
        : Alphabet<T1, T2>
    {
        public T3 C { get; set; }
    }

    public class Alphabet<T1, T2, T3, T4>
        : Alphabet<T1, T2, T3>
    {
        public T4 D { get; set; }
    }
}
