using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TupleAlgebraTests.DataModels;
using System.Linq.Expressions;
using TupleAlgebraClassLib.LINQ2TAFramework;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;

namespace TupleAlgebraTests
{
    [TestClass]
    public class AttributeComponentQueryProviderTests
    {
        private OrderedFiniteEnumerableAttributeComponentFactory<ForumUser> _nonFictionalACFactory =
            new OrderedFiniteEnumerableAttributeComponentFactory<ForumUser>((IEnumerable<ForumUser>)null);

        protected IEnumerable<TData> CreateAttributeComponent<TData>(
            AttributeDomain<TData> domain,
            Dictionary<string, object> constructorParameters)
        {
            StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData> factoryArgs =
                new StreamingOrderedFiniteEnumerableAttributeComponentFactoryArgs<TData>(
                    values: constructorParameters["values"] as IEnumerable<TData>);

            return _nonFictionalACFactory.CreateNonFictional(factoryArgs) as IEnumerable<TData>;
            //return new OrderedFiniteEnumerableNonFictionalAttributeComponent<TData>(null,
            //    constructorParameters["values"] as IEnumerable<TData>);
        }

        [TestMethod]
        public void SelectQuery()
        {
            var fusersDomain = ForumDatabase.GetDomain();
            var fusers =
                from fuser in fusersDomain
                select fuser;

            string s1 = fusers.Expression.ToString(),
                s2 = Expression.Constant(fusers).ToString();

            //Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusersDomain.Universe).ToString());
            Assert.IsTrue(Enumerable.SequenceEqual(fusers, fusersDomain));

            fusers = fusersDomain.Select(fuser => fuser);

            //Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());
            Assert.IsTrue(Enumerable.SequenceEqual(fusers, fusersDomain));

            //var fuserAchievmentsDomain = ForumUser.GetGainedAchievmentsDomain();
            //var fusersAchievments = fusersDomain.Select(fuser => fuser.GainedAchievments, fuserAchievmentsDomain);
        }

        [TestMethod]
        public void SelectQuery2()
        {
            var fusersDomain = ForumDatabase.GetDomain();
            var fusers =
                from fuser in fusersDomain
                select fuser;

            string s1 = fusers.Expression.ToString(),
                s2 = Expression.Constant(fusers).ToString();

            //Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusersDomain.Universe).ToString());
            Assert.IsTrue(Enumerable.SequenceEqual(fusers, fusersDomain));

            fusers = fusersDomain.Select(fuser => fuser);

            //Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());
            Assert.IsTrue(Enumerable.SequenceEqual(fusers, fusersDomain));

            //var fuserAchievmentsDomain = ForumUser.GetGainedAchievmentsDomain();
            //var fusersAchievments = fusersDomain.Select(fuser => fuser.GainedAchievments, fuserAchievmentsDomain);
        }

        [TestMethod]
        public void WhereSelectQuery()
        {
            var fusersDomain = ForumDatabase.GetDomain();
            var filteredUsers =
                from fuser in fusersDomain
                where fuser.LikeCount > 11
                select fuser;
            var filteredUsersPredefined = 
                from fuser in ForumDatabase.Domain
                where fuser.LikeCount > 11
                select fuser;

            Assert.IsTrue(Enumerable.SequenceEqual(
                CreateAttributeComponent(
                    fusersDomain, 
                    new Dictionary<string, object>() 
                    { 
                        { "values", filteredUsersPredefined },
                        { "domainGetter", ForumDatabase.GetDomain }
                    }),
                filteredUsers));

            //var s = fusers.Expression.ToString();

            //Assert.AreEqual(fusers.Expression.ToString(), filteredFUsers.Expression.ToString());

            //List<ForumUser> filteredFUsersList = new List<ForumUser>();

            //foreach (var a in fusers) filteredFUsersList.Add(a);

            //fusers = filteredFUsers.Select(fuser => fuser);

            //Assert.AreEqual(fusers.Expression.ToString(), filteredFUsers.Expression.ToString());

            //var fuserAchievmentsDomain = ForumUser.GetGainedAchievmentsDomain();
            //var fusersAchievments = fusersDomain.Select(fuser => fuser.GainedAchievments, fuserAchievmentsDomain);
        }

        [TestMethod]
        public void WhereSelectWhereSelectQuery()
        {
            var fusersDomain = ForumDatabase.GetDomain();
            var fusers =
                from fuser in fusersDomain
                select fuser;

            //Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());

            fusers = fusersDomain.Select(fuser => fuser);

            //Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());

            //var fuserAchievmentsDomain = ForumUser.GetGainedAchievmentsDomain();
            //var fusersAchievments = fusersDomain.Select(fuser => fuser.GainedAchievments, fuserAchievmentsDomain);
        }

        [TestMethod]
        public void SelectWhereSelectQuery()
        {
            var fusersDomain = ForumDatabase.GetDomain();
            var fusers =
                from fuser in fusersDomain
                select fuser;

            //Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());

            fusers = fusersDomain.Select(fuser => fuser);

            //Assert.AreEqual(fusers.Expression.ToString(), Expression.Constant(fusers).ToString());

            //var fuserAchievmentsDomain = ForumUser.GetGainedAchievmentsDomain();
            //var fusersAchievments = fusersDomain.Select(fuser => fuser.GainedAchievments, fuserAchievmentsDomain);
        }
    }
}
