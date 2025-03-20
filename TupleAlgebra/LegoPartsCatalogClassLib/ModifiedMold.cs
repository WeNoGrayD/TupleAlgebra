using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPartsCatalogClassLib
{
    public record class ModifiedMold(
        string Article,
        string? ParentArticle)
    {
    }
    public record class Decorated(
        string Article,
        string? ParentArticle = null)
        : ModifiedMold(Article, ParentArticle)
    {
        public static IEnumerable<Decorated> Samples;

        static Decorated()
        {
            Samples = new Decorated[]
                {
                    new ("2528pb01", "2528"), // pirate cap bicorn
                };

            return;
        }
    }

    public record class Marbled(
        string Article,
        string? ParentArticle = null)
        : ModifiedMold(Article, ParentArticle)
    {
        public static IEnumerable<Marbled> Samples;

        static Marbled()
        {
            Samples = new Marbled[]
                {
                new ("47847pb002", "47847"), // rock panel (marbled)
                };

            return;
        }
    }
}
