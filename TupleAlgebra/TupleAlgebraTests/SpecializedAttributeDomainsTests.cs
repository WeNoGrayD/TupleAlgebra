using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TupleAlgebraClassLib.SpecializedAttributeComponents.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.SpecializedAttributeDomains;
using TupleAlgebraTests.DataModels;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using System.Runtime.CompilerServices;
using TupleAlgebraClassLib.AttributeComponents;

[assembly: InternalsVisibleTo("TupleAlgebraClassLib")]

namespace TupleAlgebraTests
{
    [TestClass]
    public class SpecializedAttributeDomainsTests
    {
        internal class DataWithKnownHashCode
        {
            private int _hashcode;

            public DataWithKnownHashCode(int hashcode)
            {
                _hashcode = hashcode;
            }

            public override int GetHashCode()
            {
                return _hashcode;
            }
        }

        internal class DomainOfDataWithKnownHashCode 
            : OrderedFiniteEnumerableAttributeDomain<OrderedAttributeComponentOfDataWithKnownHashCode, DataWithKnownHashCode>
        {
            private static IEnumerable<DataWithKnownHashCode> _universum = new DataWithKnownHashCode[]
            {
                new DataWithKnownHashCode(12),
                new DataWithKnownHashCode(-9),
                new DataWithKnownHashCode(0),
                new DataWithKnownHashCode(-100),
                new DataWithKnownHashCode(45)
            };

            public DomainOfDataWithKnownHashCode()
                : base(_universum)
            {
                return;
            }
        }

        internal class OrderedAttributeComponentOfDataWithKnownHashCode 
            : OrderedFiniteEnumerableNonFictionalAttributeComponent<DataWithKnownHashCode>
        {
            protected override IComparer<DataWithKnownHashCode> InitOrderingComparerImpl()
            {
                Comparison<DataWithKnownHashCode> comparison = (x1, x2) =>
                {
                    int hc1 = x1.GetHashCode(), hc2 = x2.GetHashCode();
                    return hc2.CompareTo(hc1);
                };

                return Comparer<DataWithKnownHashCode>.Create(comparison);
            }
        }

        [TestMethod]
        public void OrderedAttributeComponentOfDataWithKnownHashCodeTest()
        {
            DomainOfDataWithKnownHashCode domain = new DomainOfDataWithKnownHashCode();

            var queryTemp = domain.Select(x => new DataWithKnownHashCode(x.GetHashCode() * x.GetHashCode()));
            AttributeComponent<DataWithKnownHashCode> query = queryTemp
                .Provider.Execute<IEnumerable<DataWithKnownHashCode>>(queryTemp.Expression)     
                as AttributeComponent<DataWithKnownHashCode>;

            int hc1 = query.First().GetHashCode(), hc2;
            foreach (DataWithKnownHashCode data in query.Skip(1))
            {
                hc2 = data.GetHashCode();
                Assert.IsTrue(hc2 <= hc1);
                hc1 = hc2;
            }
        }

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
