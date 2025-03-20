using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPartsCatalogClassLib
{
    public record struct LegoMeasureUnit(decimal Value) // half of plate height
    {
        public static implicit operator LegoMeasureUnit(LegoPlateRow row)
        {
            return new(row.Value * 5m);
        }

        public static implicit operator LegoMeasureUnit(LegoPlateColumn col)
        {
            return new(col.Value * 2m);
        }

        public static implicit operator LegoMeasureUnit(LegoBrickColumn col)
        {
            return new(col.Value * 6m);
        }

        public static implicit operator LegoPlateRow(LegoMeasureUnit unit)
        {
            return new(unit);
        }

        public static implicit operator LegoPlateColumn(LegoMeasureUnit unit)
        {
            return new(unit);
        }

        public static implicit operator LegoBrickColumn(LegoMeasureUnit unit)
        {
            return new(unit);
        }

        /*
        public static LegoMeasureUnit operator *(LegoMeasureUnit u, decimal d)
        {
            return new(u.Value * d);
        }
        */
    }

    public record struct LegoPlateRow(decimal Value)
    {
        public LegoPlateRow(LegoMeasureUnit unit)
            : this(unit.Value / 5m)
        {
            return;
        }

        public LegoPlateRow(LegoPlateColumn col)
            : this((LegoMeasureUnit)col)
        {
            return;
        }

        public LegoPlateRow(LegoBrickColumn col)
            : this((LegoMeasureUnit)col)
        {
            return;
        }
    }

    public record struct LegoPlateColumn(decimal Value)
    {
        public LegoPlateColumn(LegoMeasureUnit unit)
            : this(unit.Value / 2m)
        {
            return;
        }

        public LegoPlateColumn(LegoPlateRow row)
            : this((LegoMeasureUnit)row)
        {
            return;
        }

        public LegoPlateColumn(LegoBrickColumn col)
            : this((LegoMeasureUnit)col)
        {
            return;
        }
    }

    public record struct LegoBrickColumn(decimal Value)
    {
        public LegoBrickColumn(LegoMeasureUnit unit)
            : this(unit.Value / 6m)
        {
            return;
        }

        public LegoBrickColumn(LegoPlateRow row)
            : this((LegoMeasureUnit)row)
        {
            return;
        }

        public LegoBrickColumn(LegoPlateColumn col)
            : this((LegoMeasureUnit)col)
        {
            return;
        }
    }
}
