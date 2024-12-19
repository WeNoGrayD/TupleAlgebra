using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraFrameworkTests.DataModels
{
    public class Alphabet<TA>
    {
        public TA A { get; set; }

        public Alphabet() { }

        public Alphabet(TA a) 
        {
            A = a;
            return;
        }
    }

    public class Alphabet<TA, TB>
        : Alphabet<TA>
    {
        public TB B { get; set; }

        public Alphabet() { }

        public Alphabet(TA a, TB b) : base(a)
        {
            B = b;
            return;
        }
    }

    public class Alphabet<TA, TB, TC>
        : Alphabet<TA, TB>
    {
        public TC C { get; set; }

        public Alphabet() { }

        public Alphabet(TA a, TB b, TC c) : base(a, b)
        {
            C = c;
            return;
        }
    }

    public class Alphabet<TA, TB, TC, TD>
        : Alphabet<TA, TB, TC>
    {
        public TD D { get; set; }

        public Alphabet() { }

        public Alphabet(TA a, TB b, TC c, TD d) : base(a, b, c)
        {
            D = d;
            return;
        }
    }

    public class CharAlphabet : Alphabet<char, char, char>
    {
        public CharAlphabet() { }

        public CharAlphabet(char a, char b, char c) : base(a, b, c)
        {
            return;
        }
    }
}
