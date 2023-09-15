using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TupleAlgebraTests.ACTupleZipTests
{
    [TestClass]
    public class ACTupleZipTests
    {
        private static bool _equClassInfrastructureInitialized = false;

        [TestInitialize]
        public void Startup()
        {
            InitEquClassInfrastructure();
        }

        [TestMethod]
        public void DataZipTest()
        {
            EquivalenceClassTable<Data> equClassTable = new EquivalenceClassTable<Data>();

            equClassTable.MakePartition(Data.Enumeration);

            int minPartitionModule = equClassTable.EquivalenceClassPartitions.Min(
                (kvp) => kvp.Value.Count),
                maxPartitionModule = equClassTable.EquivalenceClassPartitions.Max(
                (kvp) => kvp.Value.Count);
            double avgPartitionModule = equClassTable.EquivalenceClassPartitions.Average(
                (kvp) => kvp.Value.Count);

            return;
        }

        [TestMethod]
        public void NonGenericDataZipTest()
        {
            NonGenericEquivalenceClassTable<Data> equClassTable =
                new NonGenericEquivalenceClassTable<Data>();

            equClassTable.MakePartition(Data.Enumeration);

            int minPartitionModule = equClassTable.EquivalenceClassPartitions.Min(
                (kvp) => kvp.Value.Count),
                maxPartitionModule = equClassTable.EquivalenceClassPartitions.Max(
                (kvp) => kvp.Value.Count);
            double avgPartitionModule = equClassTable.EquivalenceClassPartitions.Average(
                (kvp) => kvp.Value.Count);

            return;
        }

        [TestMethod]
        public void DataZipStopwatchTest()
        {
            Stopwatch sw = new Stopwatch();
            EquivalenceClassTable<Data> equClassTable = new EquivalenceClassTable<Data>();
            int testCount = 1000;
            long msSum = 0;

            for (int i = 0; i < testCount; i++)
            {
                equClassTable.EquivalenceClassPartitions.Clear();

                sw.Start();

                equClassTable.MakePartition(Data.Enumeration);

                sw.Stop();

                msSum += sw.ElapsedMilliseconds;

                sw.Reset();
            }

            double avgMsTime = msSum / testCount; 

            return;
        }

        [TestMethod]
        public void NonGenericDataZipStopwatchTest()
        {
            Stopwatch sw = new Stopwatch();
            NonGenericEquivalenceClassTable<Data> equClassTable =
                new NonGenericEquivalenceClassTable<Data>();
            int testCount = 1000;
            long msSum = 0;

            for (int i = 0; i < testCount; i++)
            {
                equClassTable.EquivalenceClassPartitions.Clear();

                sw.Start();

                equClassTable.MakePartition(Data.Enumeration);

                sw.Stop();

                msSum += sw.ElapsedMilliseconds;

                sw.Reset();
            }

            double avgMsTime = msSum / testCount;

            return;
        }

        private static void InitEquClassInfrastructure()
        {
            if (_equClassInfrastructureInitialized) return;

            _equClassInfrastructureInitialized = true;

            /*
            EquivalenceClassBearer<int>.SetPropertyGetter((Data data) => data.i);
            EquivalenceClassBearer<uint>.SetPropertyGetter((Data data) => data.ui);
            EquivalenceClassBearer<float>.SetPropertyGetter((Data data) => data.f);
            EquivalenceClassBearer<double>.SetPropertyGetter((Data data) => data.d);
            EquivalenceClassBearer<decimal>.SetPropertyGetter((Data data) => data.dec);
            */

            AttributeInfo<int> iAttr = new AttributeInfo<int>();
            AttributeInfo<uint> uiAttr = new AttributeInfo<uint>();
            AttributeInfo<float> fAttr = new AttributeInfo<float>();
            AttributeInfo<double> dAttr = new AttributeInfo<double>();
            AttributeInfo<decimal> decAttr = new AttributeInfo<decimal>();
            iAttr.SetAttributeGetter((Data data) => data.i);
            uiAttr.SetAttributeGetter((Data data) => data.ui);
            fAttr.SetAttributeGetter((Data data) => data.f);
            dAttr.SetAttributeGetter((Data data) => data.d);
            decAttr.SetAttributeGetter((Data data) => data.dec);
            IAttributeInfo[] attributeSchema =
                new IAttributeInfo[] {
                    iAttr,
                    uiAttr,
                    fAttr,
                    dAttr,
                    decAttr
                };

            EquivalenceClassSchema<Data>.InitAttributeSchema(attributeSchema);

            NonGenericEquivalenceClassSchema<Data>.SetAttributeGetters(
                attributeSchema,
                (Data data) => data.i,
                (Data data) => data.ui,
                (Data data) => data.f,
                (Data data) => data.d,
                (Data data) => data.dec
                );
        }
    }
}
