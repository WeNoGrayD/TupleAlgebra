using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraTests.DataModels
{
    public static class ForumDatabase
    {
        private static bool _isInitialized = false;

        public static List<ForumUser> Domain { get; private set; }

        public static Dictionary<DateTime, string> Comments { get; private set; } = new Dictionary<DateTime, string>()
        {
            { new DateTime(2022, 4, 3), "blah blah" },
            { new DateTime(2021, 7, 16), "blah blah blah blah" }
        };

        static ForumDatabase()
        {
            Init();
        }

        public static void Init()
        {
            if (_isInitialized) return;
            _isInitialized = true;

                Comments = new Dictionary<DateTime, string>()
            {
                { new DateTime(2022, 4, 3), "blah blah" },
                { new DateTime(2021, 7, 16), "blah blah blah blah" }
            };

            List<ForumUser>  domain = new List<ForumUser>()
            {
                new ForumUser(1, "WeNoGrayD", 10, 5, 1, 1,
                    gainedAchievments : new HashSet<int>(Enumerable.Range(1, 5))),
                new ForumUser(2, "OwlFromAnimatedFilm", 2500, 1000, 20, 4,
                    gainedAchievments : new HashSet<int>(Enumerable.Range(1, 20))),
                new ForumUser(3, "Timemechanic", 200, 45, 2, 3),
                new ForumUser(4, "PolkovnikNaBelomKone", 50, 65, 3, 0),
                new ForumUser(5, "NewRevan", 100, 55, 5, 2)
            };

            domain.Find(fu => fu.Nickname == "WeNoGrayD").LatestProfileVisitors.AddRange(
                new[] { domain.Find(fu => fu.Nickname == "NewRevan") });
            domain.Find(fu => fu.Nickname == "OwlFromAnimatedFilm").LatestProfileVisitors.AddRange(
                new[] {
                    domain.Find(fu => fu.Nickname == "Timemechanic"),
                    domain.Find(fu => fu.Nickname == "NewRevan"),
                    domain.Find(fu => fu.Nickname == "PolkovnikNaBelomKone")});
            domain.Find(fu => fu.Nickname == "Timemechanic").LatestProfileVisitors.AddRange(
                new[] {
                    domain.Find(fu => fu.Nickname == "NewRevan"),
                    domain.Find(fu => fu.Nickname == "OwlFromAnimatedFilm") });
            domain.Find(fu => fu.Nickname == "PolkovnikNaBelomKone").LatestProfileVisitors.AddRange(
                new[] {
                    domain.Find(fu => fu.Nickname == "OwlFromAnimatedFilm"),
                    domain.Find(fu => fu.Nickname == "NewRevan"),
                    domain.Find(fu => fu.Nickname == "WeNoGrayD") });
            domain.Find(fu => fu.Nickname == "NewRevan").LatestProfileVisitors.AddRange(
                new[] {
                    domain.Find(fu => fu.Nickname == "WeNoGrayD"),
                    domain.Find(fu => fu.Nickname == "OwlFromAnimatedFilm") });

            domain.Find(fu => fu.Nickname == "OwlFromAnimatedFilm").LatestComments.Add(
                new DateTime(2022, 4, 3),
                Comments[new DateTime(2022, 4, 3)]);
            domain.Find(fu => fu.Nickname == "NewRevan").LatestComments.Add(
                new DateTime(2021, 7, 16),
                Comments[new DateTime(2021, 7, 16)]);

            Domain = domain;
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
