using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraFrameworkTests.DataModels
{
    public class SetInfo
    {
        public int Article { get; set; }

        public string Name { get; set; }

        public string Series { get; set; }

        public virtual (DateTime Start, DateTime End) YearReleased { get; set; }

        public IDictionary<string, (BrickInfo Part, int Count)> Inventory { get; set; }

        public IDictionary<string, (MinifigureInfo Minifigure, int Count)> Minifigures { get; set; }

        private SetInfo(
            int article,
            string name,
            string series,
            IEnumerable<(BrickInfo Part, int Count)> inventory = null,
            IEnumerable<(MinifigureInfo Minifigure, int Count)> minifigures = null)
        {
            Article = article;
            Name = name;
            Series = series;
            Inventory = inventory?.ToDictionary(partInfo => partInfo.Part.PartNumber, partInfo => partInfo)
                ?? new Dictionary<string, (BrickInfo, int)>();
            Minifigures = minifigures?.ToDictionary(
                    minifigureInfo => minifigureInfo.Minifigure.Name,
                    minifigureInfo => minifigureInfo)
                ?? new Dictionary<string, (MinifigureInfo, int)>();
        }

        public SetInfo(
            int article,
            string name,
            string series,
            (DateTime Start, DateTime End) yearReleased,
            IEnumerable<(BrickInfo Part, int Count)> inventory = null,
            IEnumerable<(MinifigureInfo Minifigure, int Count)> minifigures = null)
            : this(article, name, series, inventory, minifigures)
        {
            YearReleased = yearReleased;
        }

        public SetInfo(
            int article,
            string name,
            string series,
            DateTime yearReleasedStart,
            IEnumerable<(BrickInfo Part, int Count)> inventory = null,
            IEnumerable<(MinifigureInfo Minifigure, int Count)> minifigures = null)
            : this(article, name, series, inventory, minifigures)
        {
            YearReleased = (yearReleasedStart, new DateTime(DateTime.Now.Year, 0, 0));
        }

        static SetInfo()
        {
            LegoDatabase.Init();
        }

        public void AddPart(BrickInfo part, int count) => Inventory.Add(part.PartNumber, (part, count));

        public void AddMinifigure(MinifigureInfo minifigure, int count) =>
            Minifigures.Add(minifigure.Name, (minifigure, count));
    }
}
