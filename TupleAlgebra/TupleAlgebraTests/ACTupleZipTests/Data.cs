using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraTests.ACTupleZipTests
{
    internal class Data
    {
        private static int[] _uniformInts, _nonUniformInts;
        private static uint[] _uniformUints, _nonUniformUints;
        private static float[] _uniformFloats, _nonUniformFloats;
        private static double[] _uniformDoubles, _nonUniformDoubles;
        private static decimal[] _uniformDecimals, _nonUniformDecimals;

        private static Random[] _randoms;

        public static IEnumerable<Data> UniformEnumeration { get; }
        public static IEnumerable<Data> NonUniformEnumeration { get; }

        static Data()
        {
            _uniformInts = new[] { 30, -810, -6260, 9 };
            _nonUniformInts = new[] { 30, -810, -6260, 9, 425 * -203, (int)Math.Pow(3, 9) };
            _uniformUints = new[] { 256256u, 12u, 0u, 444u };
            _nonUniformUints = new[] { 256256u, 12u, 0u, 444u, (uint)Math.Pow(12, 5) };
            _uniformFloats = new[] { 0.34f, -5040.12f, 16535.33333f, -111.209f };
            _nonUniformFloats = new[] { 0.34f, -5040.12f, 16535.33333f, -111.209f, (float)Math.Pow(3, 9) };
            _uniformDoubles = new[] { 778.1249573021, -0.3, Math.Pow(4, -12000), -8888888888888888.8 };
            _nonUniformDoubles = new[] { 778.1249573021, -0.3, Math.Pow(4, -12000), -8888888888888888.8 };
            _uniformDecimals = new[] { -1/5m, -23/89m, 300/78m, 118/5m };
            _nonUniformDecimals = new[] { -1 / 5m, -23 / 89m };

            _randoms = new[] {
                new Random(112),
                new Random(409),
                new Random(3008),
                new Random(21),
                new Random(15679)
            };

            UniformEnumeration = CreateUniformEnumeration(5000);
            NonUniformEnumeration = CreateNonUniformEnumeration(5000);

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

        public static IEnumerable<Data> CreateUniformEnumeration(
            int count,
            Random[] randoms = null)
        {
            if (randoms is not null)
                (_randoms, randoms) = (randoms, _randoms);

            for (int i = 0; i < count; i++)
            {
                yield return new UniformData();
            }

            if (randoms is not null)
                (_randoms, randoms) = (randoms, _randoms);

            yield break;
        }

        public static IEnumerable<Data> CreateNonUniformEnumeration(
            int count,
            Random[] randoms = null)
        {

            if (randoms is not null)
                (_randoms, randoms) = (randoms, _randoms);

            for (int i = 0; i < count; i++)
            {
                yield return new NonUniformData();
            }

            if (randoms is not null)
                (_randoms, randoms) = (randoms, _randoms);

            yield break;
        }

        public int i { get; private set; }
        public uint ui { get; private set; }
        public float f { get; private set; }
        public double d { get; private set; }
        public decimal dec { get; private set; }

        private class UniformData : Data
        {
            public UniformData()
                : base(
                    _uniformInts[_randoms[0].NextInt64(0, _uniformInts.Length)],
                    _uniformUints[_randoms[1].NextInt64(0, _uniformUints.Length)],
                    _uniformFloats[_randoms[2].NextInt64(0, _uniformFloats.Length)],
                    _uniformDoubles[_randoms[3].NextInt64(0, _uniformDoubles.Length)],
                    _uniformDecimals[_randoms[4].NextInt64(0, _uniformDecimals.Length)]
                    )
            {
                return;
            }
        }

        private class NonUniformData : Data
        {
            public NonUniformData()
                : base(
                    _nonUniformInts[_randoms[0].NextInt64(0, _nonUniformInts.Length)],
                    _nonUniformUints[_randoms[1].NextInt64(0, _nonUniformUints.Length)],
                    _nonUniformFloats[_randoms[2].NextInt64(0, _nonUniformFloats.Length)],
                    _nonUniformDoubles[_randoms[3].NextInt64(0, _nonUniformDoubles.Length)],
                    _nonUniformDecimals[_randoms[4].NextInt64(0, _nonUniformDecimals.Length)]
                    )
            {
                return;
            }
        }
    }
}
