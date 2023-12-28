using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraTests.DataModels
{
    public enum ForumUserRank
    {
        Newbie = 1,
        Veteran = 2,
        Citizen = 3,
        HonoredCreator = 4,
        Administrator = 5
    }

    public class ForumUser : IComparable<ForumUser>
    {
        //[TAIgnoredAttribute()]
        public int Id { get; set; }
        //[TAHasEquivalenceRelation()]
        public string Nickname { get; set; }
        public int LikeCount { get; set; }
        public int PostCount { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        //[TAOneToOneRelation()]
        public List<ForumUser> LatestProfileVisitors { get; set; }
        //[TAHasOrderedFiniteEnumerableNumericDomain(typeof(int), new object[]{new Tuple<int, int, int>(1, 2, 1)})]
        public HashSet<int> GainedAchievments { get; set; }
        //[TAOneToOneRelation()]
        //[TAOneToOneRelation()]
        public Dictionary<DateTime, string> LatestComments { get; set; }
        public ForumUserRank Rank { get; set; }

        public ForumUser() { return; }

        public ForumUser(
            int id, string nickname, int likeCount, int postCount,
            int followers, int following,
            List<ForumUser> latestProfileVisitors = null,
            HashSet<int> gainedAchievments = null,
            Dictionary<DateTime, string> latestComments = null)
        {
            Id = id;
            Nickname = nickname;
            LikeCount = likeCount;
            PostCount = postCount;
            Followers = followers;
            Following = following;
            LatestProfileVisitors = latestProfileVisitors ?? new List<ForumUser>();
            GainedAchievments = gainedAchievments ?? new HashSet<int>();
            LatestComments = latestComments ?? new Dictionary<DateTime, string>();
        }

        public int CompareTo(ForumUser second)
        {
            return LikeCount.CompareTo(second.LikeCount);
        }

        static ForumUser()
        {
            ForumDatabase.Init();
        }
    }
}
