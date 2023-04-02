using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraTests.DataModels
{
    public static class BrickerDatabase
    {
        private static bool _isInitialized = false;

        public static IEnumerable<BrickerUser> Users { get; private set; }

        static BrickerDatabase()
        {
            Init();
        }

        public static void Init()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            List<BrickerUser> users = new List<BrickerUser>();

            BrickerUser wenograyd = new BrickerUser(1, "WeNoGrayD",
                new Dictionary<string, ForumUser>() 
                { 
                    { "PhantomsBrick.ru", ForumDatabase.Domain.First(fu => fu.Nickname == "WeNoGrayD") },
                    { "DoubleBrick.ru", ForumDatabase.Domain.First(fu => fu.Nickname == "WeNoGrayD") }
                });

            Users = users;
        }
    }
}
