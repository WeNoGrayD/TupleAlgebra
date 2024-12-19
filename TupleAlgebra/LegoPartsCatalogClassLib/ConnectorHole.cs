using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPartsCatalogClassLib
{
    public record class ConnectorHole(
        string OwnerArticle,
        LegoMeasureUnit Length,
        byte Quantity,
        bool HasDiaphragm)
    {
    }

    public record class BarHole(
        string OwnerArticle,
        LegoMeasureUnit Length,
        byte Quantity = 1)
        : ConnectorHole(OwnerArticle, Length, Quantity, false)
    {
        public static IEnumerable<BarHole> Samples;

        static BarHole()
        {
            Samples = new BarHole[]
                {
                    new ("24122", new LegoPlateRow(1), Quantity: 2), // lightsaber hilt
                    new ("48723", new LegoBrickColumn(1)), // cross, hub
                    new ("64648", new LegoPlateColumn(0.5m)), // fish
                    new ("18651", new LegoPlateColumn(0.5m)), // double axle w/ pin
                };

            return;
        }
    }

    public record class AxleHole(
        string OwnerArticle,
        LegoMeasureUnit Length,
        byte Quantity = 1)
        : ConnectorHole(OwnerArticle, Length, Quantity, true)
    {
        public static IEnumerable<AxleHole> Samples;

        static AxleHole()
        {
            Samples = new AxleHole[]
                {
                    new ("65473", new LegoPlateRow(1), Quantity: 2), // big sausage
                    new ("24122", new LegoPlateRow(1)), // lightsaber hilt
                    new ("48723", new LegoBrickColumn(1)), // cross, hub
                    new ("65304", new LegoPlateRow(1)), // double pin w/ axle hole
                };

            return;
        }
    }

    public record class PinHole(
        string OwnerArticle,
        LegoMeasureUnit Length,
        byte Quantity = 1)
        : ConnectorHole(OwnerArticle, Length, Quantity, false)
    {
        public static IEnumerable<PinHole> Samples;

        static PinHole()
        {
            Samples = new PinHole[]
                {
                    new ("90202", new LegoPlateRow(1)), // clip hub
                    new ("62462", new LegoPlateRow(2)), // 2-pin tube
                };

            return;
        }
    }

    public record class SmallPinHole(
        string OwnerArticle,
        byte Quantity = 1)
        : ConnectorHole(OwnerArticle, new LegoPlateRow(0.5m), Quantity, false)
    {
        public static IEnumerable<SmallPinHole> Samples;

        static SmallPinHole()
        {
            Samples = new SmallPinHole[]
                {
                    new ("20482"), // round tile w/ bar
                    new ("2528pb01"), // pirate cap bicornbicorn
                    new ("3742"), // flower flat
                    new ("32606"), // flower w/ bar
                };

            return;
        }
    }
}
