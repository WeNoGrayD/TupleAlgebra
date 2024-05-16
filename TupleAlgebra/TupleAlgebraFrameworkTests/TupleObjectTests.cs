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

namespace TupleAlgebraFrameworkTests
{
    using static TupleObjectHelper;

    [TestClass]
    public class TupleObjectTests
    {
        protected List<ForumUser> _forumUsers = ForumDatabase.Domain;

        private static bool _forumUsersAreConfigured = false;

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
        public void Intersect()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);
            TupleObject<ForumUser>.Configure(CustomLikedUsers);

            ForumUser fu = new ForumUser(-1, "WeNoGrayD", 5, 10, 1, 0);
            TupleObject<ForumUser> weno1 = factory.CreateConjunctive(fu),
                weno2 = factory.CreateConjunctive<ForumUser>(
                factoryArgs: [
                    SetAC((ForumUser fu) => fu.Nickname,
                     new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<string>(["WeNoGrayD", "NewRevan"])
                    ),
                    SetAC((ForumUser fu) => fu.LikeCount,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([1, 5])
                    ),
                    SetAC((ForumUser fu) => fu.PostCount,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([10, 100])
                    ),
                    SetAC((ForumUser fu) => fu.Followers,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([1, 20])
                    ),
                    SetAC((ForumUser fu) => fu.Following,
                     new FiniteIterableAttributeComponentFactoryArgs<int>([0, 5])
                    )]);

            TupleObject<ForumUser> res = weno1 & weno2;

            return;
        }

        [TestMethod]
        public void CreateConjunctiveTupleFromEntity()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);

            ForumUser fu = new ForumUser(-1, "WeNoGrayD", 5, 10, 1, 0);

            TupleObject<ForumUser>.Configure(CustomLikedUsers);
            TupleObject<ForumUser> likedPersons = factory
                .CreateConjunctive<ForumUser>(fu);

            TupleObject<ForumUser> dLikedPersons = 
                (likedPersons as SingleTupleObject<ForumUser>)!
                .ToAlternateDiagonal(factory);

            TupleObject<ForumUser> dSysLikedPersons = factory
                .CreateDisjunctive(fu);

            return;
        }

        [TestMethod]
        public void CreateConjunctiveTuple()
        {
            TupleObjectFactory factory = new TupleObjectFactory(null);
            //TupleObjectBuilder<ForumUser> builder = factory.GetDefaultBuilder<ForumUser>();

            TupleObject<ForumUser>.Configure(CustomLikedUsers);
            TupleObject<ForumUser> likedPersons = factory.CreateConjunctive<ForumUser>(
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

            //TestTupleEnumeration(likedPersons, TestLikedPersons());

            IEnumerable<ForumUser> TestLikedPersons()
            {
                string weno = "WeNoGrayD", revan = "NewRevan";

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
            }
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
    }
}
