using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPartsCatalogClassLib
{
    public record class Clip(
        string Article,
        MoldTraitOrientation Orientation = default,
        byte Quantity = 1)
    {
        public static IEnumerable<Clip> Samples;

        static Clip()
        {
            Samples = new Clip[]
                {
                    new ("31577", MoldTraitOrientation.Horizontal, Quantity: 2), // crab
                    new ("90202", MoldTraitOrientation.Vertical, Quantity: 4), // clip hub
                };

            return;
        }
    }
}
