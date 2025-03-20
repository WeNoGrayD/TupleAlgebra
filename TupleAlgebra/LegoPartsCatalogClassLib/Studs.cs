using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPartsCatalogClassLib
{
    public record class Stud(
        string Article,
        byte Quantity = 1)
    {
        public static IEnumerable<Stud> Samples;

        static Stud()
        {
            Samples = new Stud[]
                {
                    new ("31577"), // crab
                    new ("64648"), // fish
                    new ("2566"), // palm tree top
                    new ("4042", Quantity: 2), // curved mf stand
                    new ("18651"), // double axle w/ pin
                    new ("2528pb01"), // pirate cap bicorn
                    new ("3742"), // flower flat
                    new ("47847", Quantity: 4), // rock panel
                };

            return;
        }
    }

    public record class AntiStud(
        string Article,
        byte Quantity = 1)
    {
        public static IEnumerable<AntiStud> Samples;

        static AntiStud()
        {
            Samples = new AntiStud[]
                {
                    new ("31577"), // crab
                    new ("20482"), // round tile w/ bar
                    new ("47847", Quantity: 6), // rock panel
                };

            return;
        }
    }
}
