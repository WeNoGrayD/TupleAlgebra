using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraTests.ACTupleZipTests
{
    internal class Data
    {
        private static int[] _ints;
        private static uint[] _uints;
        private static float[] _floats;
        private static double[] _doubles;
        private static decimal[] _decimals;

        private static Random[] _randoms;

        public static IEnumerable<Data> Enumeration { get; }

        static Data()
        {
            _ints = new[] { 30, -810, -6260, 9 };
            _uints = new[] { 256256u, 12u, 0u, 444u };
            _floats = new[] { 0.34f, -5040.12f, 16535.33333f, -111.209f };
            _doubles = new[] { 778.1249573021, -0.3, Math.Pow(4, -12000), -8888888888888888.8 };
            _decimals = new[] { -1/5m, -23/89m, 300/78, 118/5 };

            _randoms = new[] {
                new Random(112),
                new Random(409),
                new Random(3008),
                new Random(21),
                new Random(15679)
            };

            Enumeration = InitEnumeration(5000);

            return;
        }

        private Data() 
        {
            i = _ints[_randoms[0].NextInt64(0, _ints.Length)];
            ui = _uints[_randoms[1].NextInt64(0, _uints.Length)];
            f = _floats[_randoms[2].NextInt64(0, _floats.Length)];
            d = _doubles[_randoms[3].NextInt64(0, _doubles.Length)];
            dec = _decimals[_randoms[4].NextInt64(0, _decimals.Length)];

            return;
        }

        public Data(int i, uint ui, float f, double d, decimal dec)
        {
            this.i = i;
            this.ui = ui;
            this.f = f;
            this.d = d;
            this.dec = dec;

            return;
        }

        private static IEnumerable<Data> InitEnumeration(int count)
        {
            Data[] enumeration = new Data[count];

            for (int i = 0; i < count; i++)
            {
                enumeration[i] = new Data();
            }

            return enumeration;
        }

        public int i { get; private set; }
        public uint ui { get; private set; }
        public float f { get; private set; }
        public double d { get; private set; }
        public decimal dec { get; private set; }
    }
}
