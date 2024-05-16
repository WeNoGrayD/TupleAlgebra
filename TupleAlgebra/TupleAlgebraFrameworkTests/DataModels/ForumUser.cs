using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;

namespace TupleAlgebraFrameworkTests.DataModels
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

        /// <summary>
        /// Здесь какая-то ерунда.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            return (obj is ForumUser fu2) && this == fu2;
        }

        public static bool operator ==(ForumUser fu1, ForumUser fu2)
        {
            return fu1.Nickname == fu2.Nickname &&
                fu1.LikeCount == fu2.LikeCount &&
                fu1.PostCount == fu2.PostCount &&
                fu1.Followers == fu2.Followers &&
                fu1.Following == fu2.Following &&
                ((fu1.LatestProfileVisitors is null && fu2.LatestProfileVisitors is null) ||
                 Enumerable.SequenceEqual(fu1.LatestProfileVisitors!, fu2.LatestProfileVisitors ?? [])) &&
                ((fu1.GainedAchievments is null && fu2.GainedAchievments is null) ||
                 fu1.GainedAchievments!.SetEquals(fu2.GainedAchievments ?? [])) &&
                LatestCommentsAreEqual();

            bool LatestCommentsAreEqual()
            {
                if (fu1.LatestComments is null && fu2.LatestComments is null)
                    return true;
                if (fu1.LatestComments is null)
                    return false;
                fu2.LatestComments = fu2.LatestComments ?? [];

                bool res;
                var fu1LatestComments = fu1.LatestComments;
                var fu2LatestComments = fu2.LatestComments;
                HashSet<DateTime> fu1Keys = fu1LatestComments.Keys.ToHashSet();

                foreach (DateTime dt in fu1LatestComments.Keys)
                {
                    res = fu2LatestComments.ContainsKey(dt) &&
                        fu1LatestComments[dt] == fu2LatestComments[dt];

                    if (!res) return false;

                    fu1Keys.Add(dt);
                }

                if (fu2LatestComments.Count != fu1Keys.Count) return false;

                return true;
            }
        }

        public static bool operator !=(ForumUser fu1, ForumUser fu2)
        {
            return !(fu1 == fu2);
        }
    }
}
