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
using Newtonsoft.Json.Linq;
using UniversalClassLib.ExpressionVisitors;
using System.Collections;
using System.Xml.Linq;

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

            public int? ForeignKey { get; set; }

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

            public DependentEntity(int id, int foreignKey, MainEntity? np)
            {
                Id = id;
                ForeignKey = foreignKey;
                NavigationalProperty = np;
            }

            public override string ToString()
            {
                return "{DependentEntity: " + $"{Id}, {ForeignKey}, {NavigationalProperty}" + "}";
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

            public override string ToString()
            {
                return "{MainEntity: " + $"{Id}, {Name}" + "}";
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

        /*
        [TestMethod]
        public void TestKeySelection()
        {
            Expression<Func<DependentEntity, int?>> simpleKey =
                (DependentEntity d) => d.ForeignKey;
            LambdaExpression complexKey1 =
                (DependentEntity d) => new { d.ForeignKey, d.NavigationalProperty },
                complexKey2 = (DependentEntity d) => new ForeignKey() 
                    { Id = d.ForeignKey, NavigationalProperty = d.NavigationalProperty },
                complexKey3 = (DependentEntity d) => new ForeignKey(d.ForeignKey, d.NavigationalProperty);

            return;
        }
        */

        [TestMethod]
        public void TestNavigation()
        {
            /*
            Type entityType = typeof(DependentEntity);
            MemberInfo[] attributes = [
                entityType.GetProperty(nameof(DependentEntity.ForeignKey)),
                entityType.GetProperty(nameof(DependentEntity.NavigationalProperty))];
            ParameterExpression propertySourceEnumeratorsExpr =
                Expression.Parameter(typeof(IEnumerator[]), "properties");
            Expression constructorExpr = Expression.MemberInit(
                Expression.New(entityType),
                ConstructMembersAssignment());
            Expression block = Expression.Block();

            Expression MakeKvpParamExpr()
            {
                ParameterExpression kvpExpr =
                    Expression.Parameter(typeof(KeyValuePair<int, MainEntity>), "kvp");
            }

            MemberAssignment[] ConstructMembersAssignment()
            {
                MemberAssignment[] members = new MemberAssignment[attributes.Length];

                for (int i = 0; i < attributes.Length; i++)
                {
                    members[i] = Expression.Bind(attributes[i], GetPropertyValueOf(i));
                }

                return members;
            }

            Expression GetPropertyValueOf(int attrId)
            {
                Type propertyType = attributes[attrId] switch
                {
                    FieldInfo fi => fi.FieldType,
                    PropertyInfo pi => pi.PropertyType,
                    _ => null
                },
                     genericEnumerator = typeof(IEnumerator<>).MakeGenericType(propertyType);
                PropertyInfo getCurrentInfo = genericEnumerator.GetProperty(ENUM_CURRENT_ITEM)!;
                Expression ithEnumerator =
                    Expression.ArrayAccess(propertySourceEnumeratorsExpr, Expression.Constant(attrId)),
                           downcastedIthEnumerator =
                    Expression.TypeAs(ithEnumerator, genericEnumerator),
                           getCurrent =
                    Expression.MakeMemberAccess(downcastedIthEnumerator, getCurrentInfo);

                return getCurrent;
            }
            */
            TupleObjectFactory factory = new TupleObjectFactory(null);
            IAttributeComponentFactory<int> intFactory =
                new OrderedFiniteEnumerableAttributeComponentFactory<int>(
                    Enumerable.Range(0, 10));

            MainEntity m1 = new MainEntity(1, "Einz"),
                       m2 = new MainEntity(2, "Zwei"),
                       m3 = new MainEntity(3, "Drei"),
                       m4 = new MainEntity(4, "Vier"),
                       m5 = new MainEntity(5, "Funf"),
                       m6 = new MainEntity(6, "Sechs"),
                       m7 = new MainEntity(7, "Sieben"),
                       m8 = new MainEntity(8, "Acht"),
                       m9 = new MainEntity(9, "Neun");
            List<MainEntity> mainUniverse = [m1, m2, m3, m4, m5, m6, m7, m8, m9];

            TupleObject<MainEntity>.Configure(ConfigureMain);
            TupleObject<MainEntity> mains = factory
                .CreateConjunctiveTupleSystem<MainEntity>(mainUniverse,
                    null,
                    null);

            /*
            LambdaExpression e = (DependentEntity d) =>
            new KeyValuePair<int, MainEntity>(
                new Func<DependentEntity, int>((DependentEntity de) => de.Id)(d),
                new Func<DependentEntity, MainEntity>((DependentEntity de) => de.NavigationalProperty)(d));
            LambdaExpression ide = (DependentEntity de) => de.ForeignKey,
                mede = (DependentEntity dr) => dr.NavigationalProperty;
            //var eee = e.Compile() as Func<DependentEntity, KeyValuePair<int, MainEntity>>;
            //var kvp = eee(new(1, new MainEntity(2, "zwei")));
            /*
            ParameterExpression entityParameter = ide.Parameters[0];
            Expression ctorExpr = Expression.New(
                typeof(KeyValuePair<int, MainEntity>)
                .GetConstructor([typeof(int), typeof(MainEntity)]),
                new List<LambdaExpression>() { ide, mede }.Select(f => f.Body));
            var resExpr = Expression.Lambda<Func<DependentEntity, KeyValuePair<int, MainEntity>>>(
                    ctorExpr, entityParameter);
            *//*
            var resExpr = LambdaExpressionHelper.ProduceKeyValuePairGetter<
                      DependentEntity, int, MainEntity>(
                      ide,
                      mede);
            var eee = resExpr.Compile();// as Func<DependentEntity, KeyValuePair<int, MainEntity>>;
            var kvp = eee(new(1, 2, m2));
            */

            TupleObject<DependentEntity>.Configure(ConfigureDependent);

            TupleObjectBuilder<DependentEntity> staticBuilder =
                TupleObjectBuilder<DependentEntity>.StaticBuilder;

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
                    new DependentEntity(5, 5), new(6, m6), new(7, 7), new(8, m8), new(9, m9)],
                    null,
                    null);

            Console.WriteLine("d1:");
            foreach (DependentEntity e in d1)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("d2:");
            foreach (DependentEntity e in d2)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("d3:");
            foreach (DependentEntity e in d3)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("d4:");
            foreach (DependentEntity e in d4)
            {
                Console.WriteLine(e);
            }

            return;

            void ConfigureMain(TupleObjectBuilder<MainEntity> builder)
            {
                IEnumerable<string> numbers = mainUniverse.Select(m => m.Name);

                builder.Attribute(d => d.Id)
                    .SetFactory(intFactory).Attach();
                builder.Attribute(m => m.Name)
                    .SetFactory(
                        new OrderedFiniteEnumerableAttributeComponentFactory<string>(numbers))
                    .Attach();
            }

            void ConfigureDependent(TupleObjectBuilder<DependentEntity> builder)
            {
                builder.Attribute(d => d.Id).Ignore();
                builder.HasOne(d => d.NavigationalProperty)
                    .HasForeignKey(d => d.ForeignKey)
                    .HasPrincipalKey(
                        mains, 
                        m => m.Id,
                        (domain) => new UnorderedFiniteEnumerableAttributeComponentFactory<MainEntity>(domain),
                        (domain) => new UnorderedFiniteEnumerableAttributeComponentFactory<int?>(domain.Cast<int?>()))
                    .Attach();
                /*
                builder.Attribute(d => d.ForeignKey).SetFactory(intFactory);
                builder.Attribute(d => d.NavigationalProperty).Attach();
                */
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
