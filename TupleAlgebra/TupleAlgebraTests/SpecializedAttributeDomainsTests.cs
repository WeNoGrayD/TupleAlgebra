using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib;
using TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraTests.DataModels;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TupleAlgebraTests
{
    [TestClass]
    public class SpecializedAttributeDomainsTests
    {
        [TestMethod]
        public void EnumBasedOrderedFiniteEnumerableAttributeComponentTest()
        {
            EnumBasedOrderedFiniteEnumerableAttributeDomain<ForumUserRank> fuserRanksDomain
                = new EnumBasedOrderedFiniteEnumerableAttributeDomain<ForumUserRank>();
            HashSet<ForumUserRank> fuserRanksDomainValues = new HashSet<ForumUserRank>(fuserRanksDomain);

            foreach (ForumUserRank fuserRank in Enum.GetValues(typeof(ForumUserRank)))
                Assert.IsTrue(fuserRanksDomain.Contains(fuserRank));

            /*
            HashSet<ForumUserRank> component1Values = new HashSet<ForumUserRank>()
                { ForumUserRank.Newbie, ForumUserRank.Administrator };
            EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<ForumUserRank> fuserRanksSubset
                = new EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<ForumUserRank>(
                    fuserRanksDomain, component1Values);
                    */
        }

        [TestMethod]
        public void DictionaryBasedOrderedFiniteEnumerableAttributeComponentTest()
        {
            DictionaryBasedOrderedFiniteEnumerableAttributeDomain<string, string> fuserCommentsByNicknameDomain = 
                new DictionaryBasedOrderedFiniteEnumerableAttributeDomain<string, string>(ForumDatabase.GetLatestCommentByNicknameDomain());
            HashSet<KeyValuePair<string, string>> fuserCommentsByNicknameDomainValues = 
                new HashSet<KeyValuePair<string, string>>(fuserCommentsByNicknameDomain);

            foreach (KeyValuePair<string, string> fuserData in ForumDatabase.GetLatestCommentByNicknameDomain())
                Assert.IsTrue(fuserCommentsByNicknameDomain.Contains(fuserData));

            /*
            HashSet<ForumUserRank> component1Values = new HashSet<ForumUserRank>()
                { ForumUserRank.Newbie, ForumUserRank.Administrator };
            EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<ForumUserRank> fuserRanksSubset
                = new EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<ForumUserRank>(
                    fuserRanksDomain, component1Values);
                    */
        }

        [TestMethod]
        public void MethodCallChainTest()
        {
            /*
            var users = (from fuser in ForumUser.GetDomain()
                          where fuser.LikeCount > 0
                          where fuser.LatestProfileVisitors.Count > 0
                          where fuser.Nickname[0] == 'W'
                          select fuser).Any(fuser => fuser.GainedAchievments.Contains(4));
                        
            ForumUser.GetDomain().Where(p => p.LikeCount > 0).Where(p => p.LatestProfileVisitors.Count > 0)
                .Where(p => p.Nickname[0] == 'W').Any(p => p.GainedAchievments.Contains(4));
                */
            Expression <Func<bool>> expr = 
                () => ForumDatabase.Domain.Where(p => p.LikeCount > 0).Where(p => p.LatestProfileVisitors.Count > 0)
                .Where(p => p.Nickname[0] == 'W').Any(p => p.GainedAchievments.Contains(4));
                
            //NonFictionalAttributeComponent<int>.NonFictionalAttributeComponentQueryProvider.Test(expr);

            /*
            HashSet<ForumUserRank> component1Values = new HashSet<ForumUserRank>()
                { ForumUserRank.Newbie, ForumUserRank.Administrator };
            EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<ForumUserRank> fuserRanksSubset
                = new EnumBasedOrderedFiniteEnumerableNonFictionalAttributeComponent<ForumUserRank>(
                    fuserRanksDomain, component1Values);
                    */
        }
    }
}
