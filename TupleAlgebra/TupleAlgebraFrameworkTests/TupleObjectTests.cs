using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using TupleAlgebraClassLib.TAFrameworkCustomAttributes;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraFrameworkTests.DataModels;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using System.Reflection;
using System.Collections;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Iterable.Finite;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.TupleObjectInfrastructure.AttributeContainers;
using TupleAlgebraClassLib.TupleObjectInfrastructure.DisjunctiveTupleSystemInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure.ConjunctiveTupleSystemInfrastructure;
using System.Runtime.InteropServices;

namespace TupleAlgebraFrameworkTests
{
    using static TupleObjectHelper;
    using static TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure.LINQ2TupleAlgebra;

    [TestClass]
    public class TupleObjectTests
    {
        protected List<ForumUser> _forumUsers = ForumDatabase.Domain;

        private static bool _forumUsersAreConfigured = false;

        private static bool _alphabetWasConfigured = false;

        protected void PrintSchema(ITupleObject tupleObject)
        {
            Console.WriteLine();
            Console.Write("[");
            foreach (var attrName in tupleObject.Schema.PluggedAttributeNames)
            {
                Console.Write($"{attrName,14}|");
            }
            Console.WriteLine("]");

            return;
        }

        protected void PrintSingleTupleObject(
            ISingleTupleObject tuple,
            string defaultAcSymbol,
            string openingBracket,
            string closingBracket,
            bool printSchema = true)
        {
            if (printSchema)
            {
                PrintSchema(tuple);
            }

            IAttributeComponent ac;

            Console.Write(openingBracket);
            for (int attrLoc = 0; attrLoc < tuple.RowLength; attrLoc++)
            {
                if (attrLoc > 0) Console.Write(", ");
                ac = tuple[attrLoc];
                if (ac.IsDefault)
                {
                    Console.Write($"{defaultAcSymbol + "      ",15}");
                    continue;
                }
                Console.Write($"[{string.Join(',', ExplodeAC()),13}]");
            }
            Console.WriteLine(closingBracket);

            return;

            IEnumerable<string> ExplodeAC()
            {
                foreach (object d in (IEnumerable)ac)
                {
                    yield return d.ToString();
                }

                yield break;
            }
        }

        protected void PrintTupleObjectSystem(
            ITupleObjectSystem tupleSys,
            string defaultAcSymbol,
            string openingBracket,
            string closingBracket)
        {
            PrintSchema(tupleSys);

            for (int i = 0; i < tupleSys.ColumnLength; i++)
            {
                Console.Write($"{i,-3} ");
                PrintSingleTupleObject(
                    tupleSys[i], 
                    defaultAcSymbol,
                    openingBracket,
                    closingBracket,
                    false);
            }

            return;
        }

        protected void PrintConjunctiveTuple(
            ISingleTupleObject tuple)
        {
            PrintSingleTupleObject(tuple, "*", "[", "]");

            return;
        }

        protected void PrintDisjunctiveTuple(
            ISingleTupleObject tuple)
        {
            PrintSingleTupleObject(tuple, "Ø", "]", "[");

            return;
        }

        protected void PrintConjunctiveTupleSystem(
            ITupleObjectSystem tupleSys)
        {
            PrintTupleObjectSystem(tupleSys, "*", "[", "]");

            return;
        }

        protected void PrintDisjunctiveTupleSystem(
            ITupleObjectSystem tupleSys)
        {
            PrintTupleObjectSystem(tupleSys, "Ø", "]", "[");

            return;
        }

        public void Params(object? obj = null, params (AttributeName an, object? o)[] they)
        { }

        public void CallParams()
        {
            LambdaExpression v = (ForumUser fu) => fu.Nickname;
            Params(they: [(v, null), ("1", 1)]);
            SingleTupleObjectFactoryArgs<ForumUser, string> args =
                ((ForumUser fu) => fu.Nickname,
                     new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<string>(["WeNoGrayD", "revanie"])
                    );
        }

        [TestMethod]
        public void TestTypeInfo()
        {
            TypeInfo entityType = typeof(ForumUser).GetTypeInfo();

            foreach (PropertyInfo propertyInfo in entityType.DeclaredProperties
                .Where(pi => (pi.SetMethod?.IsPublic ?? false) && (pi.GetMethod?.IsPublic ?? false)))
            {
                ;
            }

            return;
        }

        [TestMethod]
        public void TestAttributeContainer()
        {
            (AttributeName Name, ITupleObjectAttributeInfo Info)
                nickname = ("Nickname", new AttributeInfo<ForumUser, string>((fu => fu.Nickname))),
                likes = ("Likes", new AttributeInfo<ForumUser, int>((fu => fu.LikeCount))),
                posts = ("Posts", new AttributeInfo<ForumUser, int>((fu => fu.PostCount))),
                followers = ("Followers", new AttributeInfo<ForumUser, int>((fu => fu.Followers))),
                following = ("Following", new AttributeInfo<ForumUser, int>((fu => fu.Following)));

            IAttributeContainer ac = new DictionaryBackedAttributeContainer();
            ac.AddAttribute(nickname.Name, nickname.Info);
            ac.AddAttribute(likes.Name, likes.Info);
            ac.AddAttribute(posts.Name, posts.Info);
            ac.AddAttribute(followers.Name, followers.Info);
            ac.AddAttribute(following.Name, following.Info);

            ac.AttachAttribute(likes.Name);
            ac.AttachAttribute(posts.Name);
            ac.AttachAttribute(following.Name);

            ac.EndInitialize();

            PrintPlugged();
            ac = ac.Clone();

            ac.DetachAttribute(following.Name);
            ac.AttachAttribute(followers.Name);

            ac.EndInitialize();

            PrintPlugged();
            ac = ac.Clone();

            ac.DetachAttribute(posts.Name);
            ac.AttachAttribute(nickname.Name);

            ac.EndInitialize();

            PrintPlugged();

            return;

            void PrintPlugged()
            {
                (AttributeName Name, ITupleObjectAttributeInfo Info)[] attrs =
                    [nickname, likes, posts, followers, following];
                string isPlugged = "IsPlugged", isntPlugged = "IsNotPlugged";
                foreach ((AttributeName Name, ITupleObjectAttributeInfo Info) attr in attrs)
                {
                    Console.WriteLine($"{attr.Name}: {(ac.IsPlugged(attr.Name) ? isPlugged : isntPlugged)}");
                }

                return;
            }
        }

        public void TestTupleEnumeration<TEntity>(
            TupleObject<TEntity> tuple,
            IEnumerable<TEntity> testData)
            where TEntity : new()
        {
            IEnumerator<TEntity> tupleEnum = tuple.GetEnumerator();
            foreach (TEntity e1 in testData)
            {
                Assert.IsTrue(tupleEnum.MoveNext());
                Assert.IsTrue(e1!.Equals(tupleEnum.Current));
            }
            Assert.IsFalse(tupleEnum.MoveNext());

            return;
        }

        [TestMethod]
        public void CreateConjunctiveTupleTimeTest()
        {
            Stopwatch sw = new Stopwatch();
            int testCount = 10000;
            double time = 0;

            CreateConjunctiveTuple();

            for (int i = 0; i < testCount; i++)
            {
                sw.Start();

                CreateConjunctiveTuple();

                sw.Stop();

                time += sw.ElapsedMilliseconds;

                sw.Reset();
            }
            time = time / testCount;
            Console.WriteLine($"Create ctuple costs {time} ms.");

            return;
        }

        [TestMethod]
        public void Complement()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);
            TupleObject<ForumUser>.Configure(CustomLikedUsers);

            ForumUser fu = new ForumUser(-1, "WeNoGrayD", 10, 5, 1, 1);
            TupleObject<ForumUser> weno1 = factory.CreateConjunctiveTuple(fu),
                weno2 = factory.CreateConjunctiveTuple<ForumUser>(
                factoryArgs: [
                    SetAC((ForumUser fu) => fu.Nickname,
                     new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<string>(["WeNoGrayD", "NewRevan"])
                    ),
                    SetAC((ForumUser fu) => fu.LikeCount,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([10, 100])
                    ),
                    SetAC((ForumUser fu) => fu.PostCount,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([5, 55])
                    ),
                    SetAC((ForumUser fu) => fu.Followers,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([1, 5])
                    ),
                    SetAC((ForumUser fu) => fu.Following,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([1, 2])
                    )]),
               weno3 = factory.CreateConjunctiveTupleSystem<ForumUser>(ForumDatabase.Domain);

            TupleObject<ForumUser> res;

            res = ~weno1;
            res = ~weno2;
            res = ~weno3;
            res = ~factory.CreateEmpty<ForumUser>(new TupleObjectBuilder<ForumUser>(res.Schema));

            int i = 0;
            foreach (var fu2 in res)
            {
                if (i++ == 5) break;
                Console.WriteLine($"nick: {fu2.Nickname}, lc: {fu2.LikeCount}, pc: {fu2.PostCount}, followers: {fu2.Followers}, following: {fu2.Following}");
            }

            return;
        }

        [TestMethod]
        public void Intersect()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);
            TupleObject<ForumUser>.Configure(CustomLikedUsers);

            ForumUser fu = new ForumUser(-1, "WeNoGrayD", 10, 5, 1, 1);
            TupleObject<ForumUser> weno1 = factory.CreateConjunctiveTuple(fu),
                weno2 = factory.CreateConjunctiveTuple<ForumUser>(
                factoryArgs: [
                    SetAC((ForumUser fu) => fu.Nickname,
                     new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<string>(["WeNoGrayD", "NewRevan"])
                    ),
                    SetAC((ForumUser fu) => fu.LikeCount,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([10, 100])
                    ),
                    SetAC((ForumUser fu) => fu.PostCount,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([5, 55])
                    ),
                    SetAC((ForumUser fu) => fu.Followers,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([1, 5])
                    ),
                    SetAC((ForumUser fu) => fu.Following,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([1, 2])
                    )]),
               weno3 = factory.CreateConjunctiveTupleSystem<ForumUser>(ForumDatabase.Domain);

            TupleObject<ForumUser> res;

            Stopwatch sw = new Stopwatch();
            int testCount = 10000;
            double time = 0;

            for (int i = 0; i < testCount; i++)
            {
                sw.Start();

                res = weno3 & weno1;
                res = weno3 & weno2;
                res = weno1 & weno2;

                sw.Stop();

                time += sw.ElapsedMilliseconds;

                sw.Reset();
            }
            time = time / testCount;
            //Console.WriteLine($"Intersecting ctuple 3 times (sync) costs {time} ms.");
            Console.WriteLine($"Intersecting ctuple 3 times (async) costs {time} ms.");

            return;
        }

        [TestMethod]
        public void CreateConjunctiveTupleFromEntity()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);

            ForumUser fu = new ForumUser(-1, "WeNoGrayD", 5, 10, 1, 0);

            TupleObject<ForumUser>.Configure(CustomLikedUsers);
            TupleObject<ForumUser> likedPersons = factory
                .CreateConjunctiveTuple<ForumUser>(fu);

            TupleObject<ForumUser> dLikedPersons = 
                likedPersons.ConvertToAlternate();

            TupleObject<ForumUser> dSysLikedPersons = factory
                .CreateDisjunctiveTupleSystem(fu);

            return;
        }

        [TestMethod]
        public void IntersectDisjunctiveTupleSystem()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);
            TupleObject<Alphabet<int, int, int>>.Configure(CustomAlphabet);

            ISingleTupleObjectFactoryArgs[][] factoryArgs =
                [
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 2])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 4])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 6])),
                    ],
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 7])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 8])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 9])),
                    ],
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 10])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 11])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 12])),
                    ],
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 13])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 14])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 15])),
                    ]
                ];

            TupleObject<Alphabet<int, int, int>> alphabet = factory
                .CreateDisjunctiveTupleSystem<Alphabet<int, int, int>>(
                tupleSysFactoryArgs: factoryArgs);

            DisjunctiveTupleSystemTrueIntersectionOperator<Alphabet<int, int, int>>
                intersector = new DisjunctiveTupleSystemTrueIntersectionOperator<Alphabet<int, int, int>>();

            var res = intersector.Intersect(
                alphabet as DisjunctiveTupleSystem<Alphabet<int, int, int>>,
                factory);
            var resList = new List<ConjunctiveTuple<Alphabet<int, int, int>>>(100);

            var cSys = res as ITupleObjectSystem;
            Console.WriteLine($"Number of tuples is {cSys.ColumnLength}");

            PrintConjunctiveTupleSystem(cSys);

            return;
        }

        [TestMethod]
        public void UnionConjunctiveTupleSystem()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);
            TupleObject<Alphabet<int, int, int>>.Configure(CustomAlphabet);

            /*
            ISingleTupleObjectFactoryArgs[][] factoryArgs =
                [
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 2])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 4])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 6])),
                    ],
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 7])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 8])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 9])),
                    ],
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 10])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 11])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 12])),
                    ],
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 13])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 14])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 15])),
                    ]
                ];
            */
            ISingleTupleObjectFactoryArgs[][] factoryArgs =
                [
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 2, 7, 10, 13])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 4, 8, 11, 14])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 6, 9, 12, 15])),
                    ],
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 7])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 8])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 9])),
                    ],
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 10])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 11])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 12])),
                    ],
                    [
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.A,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([1, 13])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.B,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([3, 14])),
                        SetAC<Alphabet<int, int, int>, int>(abc => abc.C,
                            new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<int>([5, 15])),
                    ]
                ];

            TupleObject<Alphabet<int, int, int>> alphabet = factory
                .CreateConjunctiveTupleSystem<Alphabet<int, int, int>>(
                tupleSysFactoryArgs: factoryArgs);
            PrintConjunctiveTupleSystem(alphabet as ITupleObjectSystem);

            ConjunctiveTupleSystemTrueUnionOperator<Alphabet<int, int, int>>
                uniter = new ConjunctiveTupleSystemTrueUnionOperator<Alphabet<int, int, int>>();

            var res = uniter.Union(
                alphabet as ConjunctiveTupleSystem<Alphabet<int, int, int>>,
                factory);
            var resList = new List<ConjunctiveTuple<Alphabet<int, int, int>>>(100);

            var dSys = res as ITupleObjectSystem;
            //dSys.TrimRedundantRows(4);
            Console.WriteLine($"Number of tuples is {dSys.ColumnLength}");

            Console.WriteLine("Disjunctive form:");
            PrintDisjunctiveTupleSystem(dSys);
            /*
            Console.WriteLine("Conjunctive form:");
            var cSys = res.ConvertToAlternate() as ITupleObjectSystem;
            Console.WriteLine($"Number of tuples is {cSys.ColumnLength}");
            PrintConjunctiveTupleSystem(cSys);
            */

            return;
        }

        [TestMethod]
        public void CreateConjunctiveTuple()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);
            //TupleObjectBuilder<ForumUser> builder = factory.GetDefaultBuilder<ForumUser>();

            TupleObject<ForumUser>.Configure(CustomLikedUsers);
            TupleObject<ForumUser> likedPersons = factory.CreateConjunctiveTuple<ForumUser>(
                factoryArgs: [
                    SetAC((ForumUser fu) => fu.Nickname,
                     new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<string>(["WeNoGrayD", "NewRevan"])
                    ),
                    SetAC((ForumUser fu) => fu.LikeCount,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([1, 5])
                    ),
                    SetAC((ForumUser fu) => fu.PostCount,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([5, 100])
                    ),
                    SetAC((ForumUser fu) => fu.Followers,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([1, 20])
                    ),
                    SetAC((ForumUser fu) => fu.Following,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([0, 5])
                    )]);

            /*
            Assert.IsFalse(likedPersons.Schema.ContainsAttribute(nameof(ForumUser.Id)));
            Assert.IsTrue(likedPersons.Schema.IsPlugged(nameof(ForumUser.Nickname)));
            Assert.IsTrue(likedPersons.Schema.IsPlugged(nameof(ForumUser.LikeCount)));
            Assert.IsTrue(likedPersons.Schema.IsPlugged(nameof(ForumUser.PostCount)));
            Assert.IsTrue(likedPersons.Schema.IsPlugged(nameof(ForumUser.Followers)));
            Assert.IsTrue(likedPersons.Schema.IsPlugged(nameof(ForumUser.Following)));
            */
            //Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LatestProfileVisitors)].IsPlugged);
            //Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.GainedAchievments)].IsPlugged);
            //Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LatestComments)].IsPlugged);

            //bool attributeCheck = likedPersons - nameof(ForumUser.LikeCount);
            //likedPersons -= nameof(ForumUser.LikeCount);
            //Assert.IsFalse(likedPersons.Schema[nameof(ForumUser.LikeCount)].IsPlugged);
            //attributeCheck = likedPersons + nameof(ForumUser.LikeCount);
            //likedPersons += nameof(ForumUser.LikeCount);
            //Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LikeCount)].IsPlugged);

            TestTupleEnumeration(likedPersons, TestLikedPersons());

            IEnumerable<ForumUser> TestLikedPersons()
            {
                string weno = "WeNoGrayD", revan = "NewRevan";

                yield return new ForumUser(0, revan, 1, 5, 1, 0);
                yield return new ForumUser(0, revan, 1, 5, 1, 5);

                yield return new ForumUser(0, revan, 1, 5, 20, 0);
                yield return new ForumUser(0, revan, 1, 5, 20, 5);

                yield return new ForumUser(0, revan, 1, 100, 1, 0);
                yield return new ForumUser(0, revan, 1, 100, 1, 5);
                yield return new ForumUser(0, revan, 1, 100, 20, 0);
                yield return new ForumUser(0, revan, 1, 100, 20, 5);

                yield return new ForumUser(0, revan, 5, 5, 1, 0);
                yield return new ForumUser(0, revan, 5, 5, 1, 5);
                yield return new ForumUser(0, revan, 5, 5, 20, 0);
                yield return new ForumUser(0, revan, 5, 5, 20, 5);
                yield return new ForumUser(0, revan, 5, 100, 1, 0);
                yield return new ForumUser(0, revan, 5, 100, 1, 5);
                yield return new ForumUser(0, revan, 5, 100, 20, 0);
                yield return new ForumUser(0, revan, 5, 100, 20, 5);

                yield return new ForumUser(0, weno, 1, 5, 1, 0);
                yield return new ForumUser(0, weno, 1, 5, 1, 5);

                yield return new ForumUser(0, weno, 1, 5, 20, 0);
                yield return new ForumUser(0, weno, 1, 5, 20, 5);

                yield return new ForumUser(0, weno, 1, 100, 1, 0);
                yield return new ForumUser(0, weno, 1, 100, 1, 5);
                yield return new ForumUser(0, weno, 1, 100, 20, 0);
                yield return new ForumUser(0, weno, 1, 100, 20, 5);

                yield return new ForumUser(0, weno, 5, 5, 1, 0);
                yield return new ForumUser(0, weno, 5, 5, 1, 5);
                yield return new ForumUser(0, weno, 5, 5, 20, 0);
                yield return new ForumUser(0, weno, 5, 5, 20, 5);
                yield return new ForumUser(0, weno, 5, 100, 1, 0);
                yield return new ForumUser(0, weno, 5, 100, 1, 5);
                yield return new ForumUser(0, weno, 5, 100, 20, 0);
                yield return new ForumUser(0, weno, 5, 100, 20, 5);

                /*
                yield return new ForumUser(0, revan, 1, 5, 1, 0);
                yield return new ForumUser(0, revan, 1, 100, 1, 0);

                yield return new ForumUser(0, weno, 1, 5, 1, 0);
                yield return new ForumUser(0, weno, 1, 100, 1, 0);

                yield return new ForumUser(0, revan, 5, 5, 1, 0);
                yield return new ForumUser(0, revan, 5, 100, 1, 0);

                yield return new ForumUser(0, weno, 5, 5, 1, 0);
                yield return new ForumUser(0, weno, 5, 100, 1, 0);

                yield return new ForumUser(0, revan, 1, 5, 1, 5);
                yield return new ForumUser(0, revan, 1, 100, 1, 5);

                yield return new ForumUser(0, weno, 1, 5, 1, 5);
                yield return new ForumUser(0, weno, 1, 100, 1, 5);

                yield return new ForumUser(0, revan, 5, 5, 1, 5);
                yield return new ForumUser(0, revan, 5, 100, 1, 5);

                yield return new ForumUser(0, weno, 5, 5, 1, 5);
                yield return new ForumUser(0, weno, 5, 100, 1, 5);

                yield return new ForumUser(0, revan, 1, 5, 20, 0);
                yield return new ForumUser(0, revan, 1, 100, 20, 0);

                yield return new ForumUser(0, weno, 1, 5, 20, 0);
                yield return new ForumUser(0, weno, 1, 100, 20, 0);

                yield return new ForumUser(0, revan, 5, 5, 20, 0);
                yield return new ForumUser(0, revan, 5, 100, 20, 0);

                yield return new ForumUser(0, weno, 5, 5, 20, 0);
                yield return new ForumUser(0, weno, 5, 100, 20, 0);

                yield return new ForumUser(0, revan, 1, 5, 20, 5);
                yield return new ForumUser(0, revan, 1, 100, 20, 5);

                yield return new ForumUser(0, weno, 1, 5, 20, 5);
                yield return new ForumUser(0, weno, 1, 100, 20, 5);

                yield return new ForumUser(0, revan, 5, 5, 20, 5);
                yield return new ForumUser(0, revan, 5, 100, 20, 5);

                yield return new ForumUser(0, weno, 5, 5, 20, 5);
                yield return new ForumUser(0, weno, 5, 100, 20, 5);
                */

                yield break;
            }
        }

        public void CustomAlphabet(
            TupleObjectBuilder<Alphabet<int, int, int>> builder)
        {
            if (_alphabetWasConfigured) return;
            _alphabetWasConfigured = true;

            IAttributeComponentFactory<int> intFactory =
                new OrderedFiniteEnumerableAttributeComponentFactory<int>(
                    Enumerable.Range(1, 21));

            builder.Attribute(abc => abc.A).SetFactory(intFactory).Attach();
            builder.Attribute(abc => abc.B).SetFactory(intFactory).Attach();
            builder.Attribute(abc => abc.C).SetFactory(intFactory).Attach();
        }

        public void CustomLikedUsers(TupleObjectBuilder<ForumUser> builder)
        {
            if (_forumUsersAreConfigured) return;
            _forumUsersAreConfigured = true;

            IAttributeComponentFactory<string> nicknameFactory =
                new OrderedFiniteEnumerableAttributeComponentFactory<string>(
                    _forumUsers.Select(user => user.Nickname));
            IAttributeComponentFactory<int> intFactory =
                new FiniteIterableAttributeComponentFactory<int>(
                    Enumerable.Range(0, 1000));

            builder.Attribute(user => user.Id).Ignore();
            builder.Attribute(user => user.Nickname).SetFactory(nicknameFactory).Attach();
            builder.Attribute(user => user.LikeCount).SetFactory(intFactory).Attach();
            builder.Attribute(user => user.PostCount).SetFactory(intFactory).Attach();
            builder.Attribute(user => user.Followers).SetFactory(intFactory).Attach();
            builder.Attribute(user => user.Following).SetFactory(intFactory).Attach();

            //AttributeDomain<ForumUser> users =
            //    new OrderedFiniteEnumerableAttributeDomain<ForumUser>(_forumUsers);
            //builder.Attribute(user => user.LatestProfileVisitors).OneToOneRelation().SetDomain(users);
            //builder.Attribute(user => user.GainedAchievments).OneToOneRelation().SetDomain(intRange);
            //AttributeDomain<IGrouping<DateTime, string>> comments =
            //    new LookupBasedOrderedFiniteEnumerableAttributeDomain<DateTime, string>(ForumDatabase.GetLatestCommentsByDateTimeDomain());
            //builder.Attribute(user => user.LatestComments).OneToOneRelation().SetDomain(comments.ShiftMany(EnumerateComments));
        }

        private IEnumerable<KeyValuePair<DateTime, string>> EnumerateComments(IGrouping<DateTime, string> comments)
        {
            IEnumerator<string> commentsEnumerator = comments.GetEnumerator();
            while (commentsEnumerator.MoveNext())
                yield return new KeyValuePair<DateTime, string>(comments.Key, commentsEnumerator.Current);
        }

        [TestMethod]
        public void CreateConjunctiveTuple2()
        {
            TupleObjectFactory factory;
            using (TAContext context = new TAContext())
            {
                factory = context.Factory;
                TupleObject<ForumUser>.Configure(CustomLikedUsers);
                //TupleObject<ForumUser> ct1 = factory.CreateConjunctive(_forumUsers[0]);
            }

            return;
        }

        public void TestDispose()
        {
            TupleObjectFactory factory;
            using (TAContext context = new TAContext())
            {
                factory = context.Factory;
                //TupleObject<int> cts1 = factory.CreateConjunctive<int>(),
                //                 dt1 = factory.CreateDisjunctive<int>(),
                //                 ct1 = cts1 / dt1;
            }
            // factory.Dispose();
            //      cts1.Dispose();
            //      dt1.Dispose();
            //      ct1.Dispose();
        }

        [TestMethod]
        public void CreateGetterAndSetter()
        {
            //Expression eFU = Expression.Lambda((string nickname, int postCount) => new ForumUser() { Nickname = nickname, PostCount = postCount });
            Expression eFU = Expression.Lambda((Dictionary<PropertyInfo, int> dict, PropertyInfo prop) => dict[prop]);
            Expression eNicknameGetterLambda = (ForumUser fu) => fu.Nickname;
            MemberExpression eNicknameGetter = ((eNicknameGetterLambda as LambdaExpression).Body as MemberExpression);
            PropertyInfo nicknameGetterInfo = (eNicknameGetter.Member as PropertyInfo);
            ParameterExpression nicknamePropertyValue = Expression.Parameter(typeof(string), "nickname");
            MemberAssignment eNicknameSetter = Expression.Bind(nicknameGetterInfo, nicknamePropertyValue);
            Expression ePostCountGetterLambda = (ForumUser fu) => fu.PostCount;
            MemberExpression ePostCountGetter = ((ePostCountGetterLambda as LambdaExpression).Body as MemberExpression);
            PropertyInfo postCountGetterInfo = (ePostCountGetter.Member as PropertyInfo);
            ParameterExpression postCountPropertyValue = Expression.Parameter(typeof(int), "postCount");
            MemberAssignment ePostCountSetter = Expression.Bind(postCountGetterInfo, postCountPropertyValue);
            Expression eConstructor = Expression.MemberInit(
                Expression.New(typeof(ForumUser)), 
                eNicknameSetter,
                ePostCountSetter);
            LambdaExpression eConstructorLambda = Expression.Lambda(eConstructor, nicknamePropertyValue, postCountPropertyValue);
            var c = (eConstructorLambda as LambdaExpression)!.Compile() as Func<string, int, ForumUser>;
            ForumUser fu = c("WeNoGranD", 120);
            //LambdaExpression eSetterLambda = Expression.Lambda(eSetter, propertyValue);

            IEnumerator[] enumerators = new IEnumerator[]
                { 
                    (new List<string> { "WeNoGranD", "Реван" }).GetEnumerator(), 
                    ((IEnumerable<int>)new int[] { 120, 65 }).GetEnumerator()
                };

            EntityFactoryBuilder entityFactoryBuilder = new EntityFactoryBuilder();
            Type fuType = typeof(ForumUser);
            PropertyInfo[] properties = new PropertyInfo[] {
                fuType.GetProperty(nameof(ForumUser.Nickname)),
                fuType.GetProperty(nameof(ForumUser.PostCount))
            };
            Expression entityFactoryExpr = null;//entityFactoryBuilder.Build(fuType, properties);
            Func<IEnumerator[], ForumUser> entityFactory = (entityFactoryExpr as LambdaExpression)
                .Compile() as Func<IEnumerator[], ForumUser>;
            enumerators[0].MoveNext();
            enumerators[1].MoveNext();
            fu = entityFactory(enumerators);
            enumerators[1].MoveNext();
            fu = entityFactory(enumerators);
            enumerators[1].Reset();
            enumerators[0].MoveNext();
            enumerators[1].MoveNext();
            fu = entityFactory(enumerators);
            enumerators[1].MoveNext();
            fu = entityFactory(enumerators);


            return;
        }

        private bool _cartesianDataAreConfigured = false;

        public void CustomCartesianData(TupleObjectBuilder<CartesianData> builder)
        {
            if (_cartesianDataAreConfigured) return;
            _cartesianDataAreConfigured = true;

            /*
            AttributeDomain<int> intRange =
                new Integer32OrderedFiniteEnumerableAttributeDomain((1, 10));
            builder.Attribute(cd => cd.D1).SetDomain(intRange);
            builder.Attribute(cd => cd.D2).SetDomain(intRange);
            builder.Attribute(cd => cd.D3).SetDomain(intRange);
            */
        }

        [TestMethod]
        public void TupleObjectQueriesTest()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);

            TupleObject<ForumUser>.Configure(CustomLikedUsers);
            TupleObject<ForumUser> likedPersons = factory.CreateConjunctiveTuple<ForumUser>(
                factoryArgs: [
                    SetAC((ForumUser fu) => fu.Nickname,
                     new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<string>(["WeNoGrayD", "NewRevan"])
                    ),
                    SetAC((ForumUser fu) => fu.LikeCount,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([1, 5])
                    ),
                    SetAC((ForumUser fu) => fu.PostCount,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([5, 100])
                    ),
                    SetAC((ForumUser fu) => fu.Followers,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([1, 20])
                    ),
                    SetAC((ForumUser fu) => fu.Following,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([0, 5])
                    )]);

            Console.WriteLine("likedPersons & ][");
            var where = likedPersons
                .Where((fu) => fu.Nickname.StartsWith('W') || fu.LikeCount > 1 || fu.PostCount > 5 || fu.Followers > 5 || fu.Following > 2);
            where = where.ExecuteQuery();
            where = !where;

            PrintConjunctiveTupleSystem((dynamic)where);

            Console.WriteLine("likedPersons & ]--[");
            where = likedPersons
                .Where((fu) => (fu.Nickname.StartsWith('W') || fu.LikeCount > 1) && (fu.PostCount > 5 || fu.Followers > 5) && fu.Following > 2);
            where = where.ExecuteQuery();
            where = !where;

            PrintConjunctiveTupleSystem((dynamic)where);

            Console.WriteLine("likedPersons & ([--] & [--])");
            where = likedPersons
                .Where((fu) => ((fu.Nickname.StartsWith('W') && fu.LikeCount > 1) || (fu.PostCount > 5 && fu.Followers > 5)) && ((fu.Nickname.StartsWith('N') && fu.LikeCount < 5) || (fu.PostCount < 100 && fu.Followers < 20)));
            where = where.ExecuteQuery();
            PrintConjunctiveTupleSystem((dynamic)where);
            where = !where;
            PrintDisjunctiveTupleSystem((dynamic)where);
            /*
            where = !where;
            PrintDisjunctiveTupleSystem((dynamic)where);
            */

            return;
        }
    }
}
