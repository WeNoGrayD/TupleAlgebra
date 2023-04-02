using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Drawing;

namespace TupleAlgebraTests.DataModels
{
    public class BrickerUser
    {
        public int Id { get; set; }

        public string Nickname { get; set; }

        public IDictionary<string, ForumUser> ImOnOtherSites { get; private set; }

        public IDictionary<int, (SetInfo Set, int Count)> MySets { get; private set; }

        public int PartsCount 
        { 
            get => (from setInfo in MySets
                    from partInfo in setInfo.Value.Set.Inventory
                    select partInfo.Value.Count * setInfo.Value.Count)
                    .Sum();
        }

        public BrickerUser(
            int id, 
            string nickname,
            IDictionary<string, ForumUser> imOnOtherSites = null,
            IEnumerable<(SetInfo Set, int Count)> mySets = null)
        {
            Id = id;
            Nickname = nickname;
            ImOnOtherSites = imOnOtherSites ?? new Dictionary<string, ForumUser>();
            MySets = mySets.ToDictionary(setInfo => setInfo.Set.Article, setInfo => setInfo)
                ?? new Dictionary<int, (SetInfo, int)>();
        }
    }
}
