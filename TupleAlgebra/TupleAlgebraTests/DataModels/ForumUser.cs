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

        public static readonly List<ForumUser> Domain;

        public static Dictionary<DateTime, string> Comments = new Dictionary<DateTime, string>()
        {
            { new DateTime(2022, 4, 3), "blah blah" },
            { new DateTime(2021, 7, 16), "blah blah blah blah" }
        };

        static ForumUser()
        {
            Comments = new Dictionary<DateTime, string>()
            {
                { new DateTime(2022, 4, 3), "blah blah" },
                { new DateTime(2021, 7, 16), "blah blah blah blah" }
            };

            Domain = new List<ForumUser>()
            {
                new ForumUser(1, "WeNoGrayD", 10, 5, 1, 1,
                    gainedAchievments : new HashSet<int>(Enumerable.Range(1, 5))),
                new ForumUser(2, "OwlFromAnimatedFilm", 2500, 1000, 20, 4,
                    gainedAchievments : new HashSet<int>(Enumerable.Range(1, 20))),
                new ForumUser(3, "Timemechanic", 200, 45, 2, 3),
                new ForumUser(4, "PolkovnikNaBelomKone", 50, 65, 3, 0),
                new ForumUser(5, "NewRevan", 100, 55, 5, 2)
            };

            Domain.Find(fu => fu.Nickname == "WeNoGrayD").LatestProfileVisitors.AddRange(
                new[] { Domain.Find(fu => fu.Nickname == "NewRevan") });
            Domain.Find(fu => fu.Nickname == "OwlFromAnimatedFilm").LatestProfileVisitors.AddRange(
                new[] {
                    Domain.Find(fu => fu.Nickname == "Timemechanic"), 
                    Domain.Find(fu => fu.Nickname == "NewRevan"),
                    Domain.Find(fu => fu.Nickname == "PolkovnikNaBelomKone")});
            Domain.Find(fu => fu.Nickname == "Timemechanic").LatestProfileVisitors.AddRange(
                new[] {
                    Domain.Find(fu => fu.Nickname == "NewRevan"),
                    Domain.Find(fu => fu.Nickname == "OwlFromAnimatedFilm") });
            Domain.Find(fu => fu.Nickname == "PolkovnikNaBelomKone").LatestProfileVisitors.AddRange(
                new[] {
                    Domain.Find(fu => fu.Nickname == "OwlFromAnimatedFilm"),
                    Domain.Find(fu => fu.Nickname == "NewRevan"),
                    Domain.Find(fu => fu.Nickname == "WeNoGrayD") });
            Domain.Find(fu => fu.Nickname == "NewRevan").LatestProfileVisitors.AddRange(
                new[] {
                    Domain.Find(fu => fu.Nickname == "WeNoGrayD"),
                    Domain.Find(fu => fu.Nickname == "OwlFromAnimatedFilm") });

            Domain.Find(fu => fu.Nickname == "OwlFromAnimatedFilm").LatestComments.Add(
                new DateTime(2022, 4, 3), 
                Comments[new DateTime(2022, 4, 3)]);
            Domain.Find(fu => fu.Nickname == "NewRevan").LatestComments.Add(
                new DateTime(2021, 7, 16), 
                Comments[new DateTime(2021, 7, 16)]);
        }

        public static AttributeDomain<ForumUser> GetDomain()
        {
            return new OrderedFiniteEnumerableAttributeDomain<ForumUser>(Domain);
        }

        public static IEnumerable<ForumUser> GetLatestVisitorsDomain()
        {
            return Domain;
        }

        public static IEnumerable<int> GetGainedAchievmentsDomain()
        {
            return Enumerable.Range(1, 100);
        }

        public static ILookup<DateTime, string> GetLatestCommentsByDateTimeDomain()
        {
            return (from fuser in Domain
                    from comment in fuser.LatestComments
                    select comment).ToLookup(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static IDictionary<string, string> GetLatestCommentByNicknameDomain()
        {
            return (from fuser in Domain
                    let comment = fuser.LatestComments.LastOrDefault()
                    select new { Name = fuser.Nickname, Comment = comment.Value })
                    .ToDictionary(fuserData => fuserData.Name, fuserData => fuserData.Comment);
        }

        public static IEnumerable<ForumUserRank> GetForumUserRankDomain()
        {
            return Enum.GetValues(typeof(ForumUserRank)).Cast<ForumUserRank>();
        }
    }
}
