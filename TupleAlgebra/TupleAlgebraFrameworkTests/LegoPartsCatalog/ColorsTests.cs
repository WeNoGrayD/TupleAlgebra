using LegoPartsCatalogClassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using System.Reflection;
using System.Linq.Expressions;

namespace TupleAlgebraFrameworkTests.LegoPartsCatalog
{
    using static TupleObjectHelper;
    using static Logger;

    [TestClass]
    public class ColorsTests
    {
        private class Program
        {
            private byte[] _cs = new byte[256];

            private int _esp = 0;

            public byte[] CS { get => _cs; set => _cs = value; }

            public int ESP { get => _esp++; set => _esp = value; }

            public byte Cmd { get => _cs[ESP]; }
        }

        [TestMethod]
        public void TestIL()
        {
            TypeInfo programType = typeof(Program).GetTypeInfo();
            foreach (PropertyInfo prop in programType.DeclaredProperties)
            {
                Console.WriteLine($"{prop.Name}");
                var getm = prop.GetMethod;
                var del = getm.CreateDelegate<Func<byte>>();
                var msilStream = prop.GetMethod.GetMethodBody().GetILAsByteArray();
                foreach (var c in msilStream)
                    Console.WriteLine($"{c:B8}");
            }

            return;
        }


        internal class DependentEntity
        {
            public int Id { get; set; }

            public int ForeignKey { get; set; }

            public MainEntity? NavigationalProperty { get; set; }

            public DependentEntity() { }

            public DependentEntity(int id, int foreignKey)
            {
                Id = id;
                ForeignKey = foreignKey;
            }

            public DependentEntity(int id, MainEntity? np)
            {
                Id = id;
                NavigationalProperty = np;
            }
        }

        internal class MainEntity
        {
            public int Id { get; set; }

            public string? Name { get; set; }

            public MainEntity()
            { }

            public MainEntity(int id, string name)
            {
                Id = id;
                Name = name;
                return;
            }
        }

        internal class ForeignKey
        {
            public int Id { get; set; }

            public MainEntity? NavigationalProperty { get; set; }

            public ForeignKey() { }

            public ForeignKey(int id, MainEntity navProp)
            {
                Id = id;
                NavigationalProperty = navProp;
            }
        }

        [TestMethod]
        public void TestKeySelection()
        {
            Expression<Func<DependentEntity, int>> simpleKey =
                (DependentEntity d) => d.ForeignKey;
            LambdaExpression complexKey1 =
                (DependentEntity d) => new { d.ForeignKey, d.NavigationalProperty },
                complexKey2 = (DependentEntity d) => new ForeignKey() 
                    { Id = d.ForeignKey, NavigationalProperty = d.NavigationalProperty },
                complexKey3 = (DependentEntity d) => new ForeignKey(d.ForeignKey, d.NavigationalProperty);

            return;
        }

        [TestMethod]
        public void TestNavigation()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);
            IAttributeComponentFactory<int> intFactory =
                new OrderedFiniteEnumerableAttributeComponentFactory<int>(
                    Enumerable.Range(0, 10));
            TupleObject<MainEntity>.Configure(ConfigureMain);
            MainEntity m1 = new MainEntity(1, "Einz"),
                       m2 = new MainEntity(2, "Zwei"),
                       m3 = new MainEntity(3, "Drei"),
                       m4 = new MainEntity(4, "Vier"),
                       m5 = new MainEntity(5, "Funf"),
                       m6 = new MainEntity(6, "Sechs"),
                       m7 = new MainEntity(7, "Sieben"),
                       m8 = new MainEntity(8, "Acht"),
                       m9 = new MainEntity(9, "Neun");
            TupleObject<MainEntity> mains = factory
                .CreateConjunctiveTupleSystem<MainEntity>([m1, m2, m3],
                    null,
                    null);
            TupleObject<DependentEntity>.Configure(ConfigureDependent);

            TupleObject<DependentEntity> d1 = factory
                .CreateConjunctiveTupleSystem<DependentEntity>([
                    new DependentEntity(1, 1), new(2, 2), new(3, 3)],
                    null,
                    null),
                    d2 = factory
                .CreateConjunctiveTupleSystem<DependentEntity>([
                    new DependentEntity(3, m3), new(4, m4), new(5, m5)],
                    null,
                    null),
                    d3 = factory
                .CreateConjunctiveTupleSystem<DependentEntity>([
                    new DependentEntity(2, 2), new(4, 4), new(6, 6)],
                    null,
                    null),
                    d4 = factory
                .CreateConjunctiveTupleSystem<DependentEntity>([
                    new DependentEntity(5, 5), new(8, m8), new(9, m9)],
                    null,
                    null);

            return;

            void ConfigureMain(TupleObjectBuilder<MainEntity> builder)
            {
                builder.Attribute(d => d.Id).SetFactory(intFactory).Attach();
            }

            void ConfigureDependent(TupleObjectBuilder<DependentEntity> builder)
            {
                builder.Attribute(d => d.Id).Ignore();
                builder.HasOne(d => d.NavigationalProperty)
                    .HasForeignKey(d => d.ForeignKey)
                    .HasPrincipalKey(mains, m => m.Id);
                builder.Attribute(d => d.ForeignKey).SetFactory(intFactory);
                builder.Attribute(d => d.NavigationalProperty).Attach();
            }
        }

        /*
        bool _wereConfigured = false;

        public TupleObject<ColorInfo> ColorsKb { get; private set; }
        TupleObjectFactory Factory;

        [TestInitialize]
        public void Init()
        {
            Factory = new TupleObjectFactory(null);
            TupleObject<ColorInfo>.Configure(ConfigureColors);
            ColorsKb = Factory.CreateConjunctiveTupleSystem(ColorInfo.Colors);

            return;
        }

        [TestMethod]
        public void TestStandard()
        {
            TupleObject<ColorInfo> chromeRule = Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [
                        SetAC<ColorInfo, bool>(ci => ci.IsTransparent, new BooleanAttributeComponentFactoryArgs(false)),
                        SetAC<ColorInfo, bool>(ci => ci.IsPearl, new BooleanAttributeComponentFactoryArgs(false)),
                        SetAC<ColorInfo, bool>(ci => ci.IsSpeckle, new BooleanAttributeComponentFactoryArgs(false)),
                        SetAC<ColorInfo, bool>(ci => ci.IsMetallic, new BooleanAttributeComponentFactoryArgs(false)),
                        SetAC<ColorInfo, bool>(ci => ci.IsChrome, new BooleanAttributeComponentFactoryArgs(false)),
                    ],
                    null, null);

            PrintTupleObject(chromeRule & ColorsKb);
        }

        [TestMethod]
        public void TestTransparent()
        {
            TupleObject<ColorInfo> transparentRule = Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [SetAC<ColorInfo, bool>(ci => ci.IsTransparent, new BooleanAttributeComponentFactoryArgs(true))],
                    null, null);

            PrintTupleObject(transparentRule & ColorsKb);
        }

        [TestMethod]
        public void TestPearl()
        {
            TupleObject<ColorInfo> pearlRule = Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [SetAC<ColorInfo, bool>(ci => ci.IsPearl, new BooleanAttributeComponentFactoryArgs(true))],
                    null, null);

            PrintTupleObject(pearlRule & ColorsKb);
        }

        [TestMethod]
        public void TestSpeckle()
        {
            TupleObject<ColorInfo> speckleRule = Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [SetAC<ColorInfo, bool>(ci => ci.IsSpeckle, new BooleanAttributeComponentFactoryArgs(true))],
                    null, null);

            PrintTupleObject(speckleRule & ColorsKb);
        }

        [TestMethod]
        public void TestMetallic()
        {
            TupleObject<ColorInfo> metallicRule = Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [SetAC<ColorInfo, bool>(ci => ci.IsMetallic, new BooleanAttributeComponentFactoryArgs(true))],
                    null, null);

            PrintTupleObject(metallicRule & ColorsKb);
        }

        [TestMethod]
        public void TestChrome()
        {
            TupleObject<ColorInfo> chromeRule = Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [SetAC<ColorInfo, bool>(ci => ci.IsChrome, new BooleanAttributeComponentFactoryArgs(true))],
                    null, null);

            PrintTupleObject(chromeRule & ColorsKb);
        }

        private void ConfigureColors(TupleObjectBuilder<ColorInfo> builder)
        {
            if (_wereConfigured) return;
            _wereConfigured = true;

            IEnumerable<ColorInfo> colors = ColorInfo.Colors;

            builder.Attribute(ci => ci.Name)
                .SetFactory(
                    new OrderedFiniteEnumerableAttributeComponentFactory<string>(
                        colors.Select(ci => ci.Name)))
                .Attach();
            builder.Attribute(ci => ci.RGB)
                .SetFactory(
                    new UnorderedFiniteEnumerableAttributeComponentFactory<string>(
                        colors.Select(ci => ci.RGB).ToHashSet()))
                .Attach();
            builder.Attribute(ci => ci.IsTransparent)
                .SetFactory(BooleanAttributeComponentFactory.Instance)
                .Attach();
            builder.Attribute(ci => ci.IsPearl)
                .SetFactory(BooleanAttributeComponentFactory.Instance)
                .Attach();
            builder.Attribute(ci => ci.IsSpeckle)
                .SetFactory(BooleanAttributeComponentFactory.Instance)
                .Attach();
            builder.Attribute(ci => ci.IsMetallic)
                .SetFactory(BooleanAttributeComponentFactory.Instance)
                .Attach();
            builder.Attribute(ci => ci.IsChrome)
                .SetFactory(BooleanAttributeComponentFactory.Instance)
                .Attach();

            return;
        }
        */
    }
}
