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
using TupleAlgebraTests.DataModels;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;

namespace TupleAlgebraTests
{
    [TestClass]
    public class TupleObjectTests
    {
        protected List<ForumUser> _forumUsers = ForumDatabase.Domain;

        private bool _forumUsersAreConfigured = false;

        [TestMethod]
        public void TestMethod1()
        {
            TupleObject<ForumUser>.Configure(CustomLikedUsers);
            TupleObject<ForumUser> likedPersons = new ConjunctiveTuple<ForumUser>();

            Assert.IsFalse(likedPersons.Schema.ContainsAttribute(nameof(ForumUser.Id)));
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.Nickname)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LikeCount)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.PostCount)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.Followers)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.Following)].IsPlugged);
            //Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LatestProfileVisitors)].IsPlugged);
            //Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.GainedAchievments)].IsPlugged);
            //Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LatestComments)].IsPlugged);

            bool attributeCheck = likedPersons - nameof(ForumUser.LikeCount);
            Assert.IsFalse(likedPersons.Schema[nameof(ForumUser.LikeCount)].IsPlugged);
            attributeCheck = likedPersons + nameof(ForumUser.LikeCount);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LikeCount)].IsPlugged);
        }

        public void CustomLikedUsers(TupleObjectBuilder<ForumUser> builder)
        {
            builder.Attribute(user => user.Id).Ignore();
            AttributeDomain<string> nicknames = 
                new OrderedFiniteEnumerableAttributeDomain<string>(_forumUsers.Select(user => user.Nickname));
            builder.Attribute(user => user.Nickname).SetDomain(nicknames);
            AttributeDomain<int> intRange =
                new Integer32OrderedFiniteEnumerableAttributeDomain((0, 1000, 1));
            builder.Attribute(user => user.LikeCount).SetDomain(intRange);
            builder.Attribute(user => user.PostCount).SetDomain(intRange);
            builder.Attribute(user => user.Followers).SetDomain(intRange);
            builder.Attribute(user => user.Following).SetDomain(intRange);
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
        public void CreateConjunctiveTuple()
        {
            TupleObjectFactory factory;
            using (TAContext context = new TAContext())
            {
                factory = context.Factory;
                TupleObject<ForumUser>.Configure(CustomLikedUsers);
                TupleObject<ForumUser> ct1 = factory.CreateConjunctive(_forumUsers[0]);
            }

            return;
        }

        public void TestDispose()
        {
            TupleObjectFactory factory;
            using (TAContext context = new TAContext())
            {
                factory = context.Factory;
                TupleObject<int> cts1 = factory.CreateConjunctive<int>(),
                                 dt1 = factory.CreateDisjunctive<int>(),
                                 ct1 = cts1 / dt1;
            }
            // factory.Dispose();
            //      cts1.Dispose();
            //      dt1.Dispose();
            //      ct1.Dispose();
        }
    }
}
