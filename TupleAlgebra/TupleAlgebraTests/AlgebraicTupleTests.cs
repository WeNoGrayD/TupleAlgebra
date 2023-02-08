using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;
using TupleAlgebraClassLib.TAFrameworkCustomAttributes;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraClassLib.AlgebraicTupleInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerableNonFictionalAttributeComponentInfrastructure;
using TupleAlgebraTests.DataModels;

namespace TupleAlgebraTests
{
    [TestClass]
    public class AlgebraicTupleTests
    {
        protected List<ForumUser> _forumUsers = ForumUser.Domain;

        [TestMethod]
        public void TestMethod1()
        {
            AlgebraicTuple<ForumUser> likedPersons = new ConjunctiveAlgebraicTuple<ForumUser>(CustomLikedUsers);

            Assert.IsFalse(likedPersons.Schema.ContainsAttribute(nameof(ForumUser.Id)));
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.Nickname)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LikeCount)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.PostCount)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.Followers)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.Following)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LatestProfileVisitors)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.GainedAchievments)].IsPlugged);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LatestComments)].IsPlugged);

            bool attributeCheck = likedPersons - nameof(ForumUser.LikeCount);
            Assert.IsFalse(likedPersons.Schema[nameof(ForumUser.LikeCount)].IsPlugged);
            attributeCheck = likedPersons + nameof(ForumUser.LikeCount);
            Assert.IsTrue(likedPersons.Schema[nameof(ForumUser.LikeCount)].IsPlugged);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Expression<Func<ForumUser, List<ForumUser>>> expr = (user => user.LatestProfileVisitors);
            sw.Stop();
            (long ms, long ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Restart();
            var m = expr.Compile();
            sw.Stop();
            (ms, ticks) = (sw.ElapsedMilliseconds, sw.ElapsedTicks);
            ;
        }

        void CustomLikedUsers(AlgebraicTupleBuilder<ForumUser> builder)
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
            AttributeDomain<ForumUser> users =
                new OrderedFiniteEnumerableAttributeDomain<ForumUser>(_forumUsers);
            builder.Attribute(user => user.LatestProfileVisitors).OneToOneRelation().SetDomain(users);
            builder.Attribute(user => user.GainedAchievments).OneToOneRelation().SetDomain(intRange);
            //AttributeDomain<IGrouping<DateTime, string>> comments =
            //    new LookupBasedOrderedFiniteEnumerableAttributeDomain<DateTime, string>(ForumUser.GetLatestCommentsDomain());
            //builder.Attribute(user => user.LatestComments).OneToOneRelation().SetDomain(comments, 
            //    (Func<IGrouping<DateTime, string>, IEnumerable<KeyValuePair<DateTime, string>>>)EnumerateComments);
        }

        private IEnumerable<KeyValuePair<DateTime, string>> EnumerateComments(IGrouping<DateTime, string> comments)
        {
            IEnumerator<string> commentsEnumerator = comments.GetEnumerator();
            while (commentsEnumerator.MoveNext())
                yield return new KeyValuePair<DateTime, string>(comments.Key, commentsEnumerator.Current);
        }
    }
}
