using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraFrameworkTests.DataModels
{
    public enum MinifigurePart
    {
        HipsAndLegs,
        UnderTorso,
        Torso,
        Head,
        Headgear,
        Hair,
        Neckgear,
        Accessories
    }

    public class MinifigureInfo
    {
        public string Name { get; set; }

        public virtual (DateTime Start, DateTime End) YearReleased { get; set; }

        public IDictionary<MinifigurePart, BrickInfo> Inventory { get; private set; }

        private MinifigureInfo(string name, IDictionary<MinifigurePart, BrickInfo> inventory = null)
        {
            Name = name;
            Inventory = inventory ?? new Dictionary<MinifigurePart, BrickInfo>();
        }

        public MinifigureInfo(
            string name,
            (DateTime Start, DateTime End) yearReleased,
            IDictionary<MinifigurePart, BrickInfo> inventory = null)
            : this(name, inventory)
        {
            YearReleased = yearReleased;
        }

        public MinifigureInfo(
            string name,
            DateTime yearReleasedStart,
            IDictionary<MinifigurePart, BrickInfo> inventory = null)
            : this(name, inventory)
        {
            YearReleased = (yearReleasedStart, new DateTime(DateTime.Now.Year, 0, 0));
        }
    }
}
