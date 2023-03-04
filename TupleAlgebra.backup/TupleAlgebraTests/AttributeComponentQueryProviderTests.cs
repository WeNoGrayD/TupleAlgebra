using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TupleAlgebraTests.DataModels;
using System.Linq.Expressions;
using TupleAlgebraClassLib.LINQ2TAFramework;

namespace TupleAlgebraTests
{
    [TestClass]
    public class AttributeComponentQueryProviderTests
    {
        [TestMethod]
        public void SelectQuery()
        {
            var fusersDomain = ForumUser.GetDomain();
            var fusers =
                from fuser in fusersDomain
                select fuser;

            string s1 = fusers.Expression.ToString(),
                s2 = Expression.Constant(fusers).ToString();

            Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());
            Assert.IsTrue(Enumerable.SequenceEqual(fusers, fusersDomain));

            fusers = fusersDomain.Select(fuser => fuser);

            Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());
            Assert.IsTrue(Enumerable.SequenceEqual(fusers, fusersDomain));

            //var fuserAchievmentsDomain = ForumUser.GetGainedAchievmentsDomain();
            //var fusersAchievments = fusersDomain.Select(fuser => fuser.GainedAchievments, fuserAchievmentsDomain);
        }

        [TestMethod]
        public void WhereSelectQuery()
        {
            var fusersDomain = ForumUser.GetDomain();
            var filteredFUsers = fusersDomain.Where(fuser => fuser.LikeCount > 11);
            var fusers =
                from fuser in fusersDomain
                where fuser.LikeCount > 11
                select fuser;

            var s = fusers.Expression.ToString();

            Assert.AreEqual(fusers.Expression.ToString(), filteredFUsers.Expression.ToString());

            List<ForumUser> filteredFUsersList = new List<ForumUser>();

            foreach (var a in fusers) filteredFUsersList.Add(a);

            fusers = filteredFUsers.Select(fuser => fuser);

            Assert.AreEqual(fusers.Expression.ToString(), filteredFUsers.Expression.ToString());

            //var fuserAchievmentsDomain = ForumUser.GetGainedAchievmentsDomain();
            //var fusersAchievments = fusersDomain.Select(fuser => fuser.GainedAchievments, fuserAchievmentsDomain);
        }

        [TestMethod]
        public void WhereSelectWhereSelectQuery()
        {
            var fusersDomain = ForumUser.GetDomain();
            var fusers =
                from fuser in fusersDomain
                select fuser;

            Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());

            fusers = fusersDomain.Select(fuser => fuser);

            Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());

            //var fuserAchievmentsDomain = ForumUser.GetGainedAchievmentsDomain();
            //var fusersAchievments = fusersDomain.Select(fuser => fuser.GainedAchievments, fuserAchievmentsDomain);
        }

        [TestMethod]
        public void SelectWhereSelectQuery()
        {
            var fusersDomain = ForumUser.GetDomain();
            var fusers =
                from fuser in fusersDomain
                select fuser;

            Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());

            fusers = fusersDomain.Select(fuser => fuser);

            Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());

            //var fuserAchievmentsDomain = ForumUser.GetGainedAchievmentsDomain();
            //var fusersAchievments = fusersDomain.Select(fuser => fuser.GainedAchievments, fuserAchievmentsDomain);
        }
    }
}
