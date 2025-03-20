using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPartsCatalogClassLib
{
    public record class Connector(
        string OwnerArticle,
        LegoMeasureUnit Length)
    {
    }

    public enum BarFormFactor : byte
    {
        Straight,
        Curved
    }

    public record class Bar(
        string OwnerArticle,
        BarFormFactor FormFactor,
        LegoMeasureUnit Length,
        bool HasStopRing = false,
        byte Quantity = 1)
        : Connector(OwnerArticle, Length)
    {
        public static IEnumerable<Bar> Samples;

        static Bar()
        {
            Samples = new Bar[]
                {
                    new ("33078", BarFormFactor.Curved, new LegoBrickColumn(2)), // sausage
                    new ("30115", BarFormFactor.Curved, new LegoPlateRow(3)), // snake
                    new ("4042", BarFormFactor.Curved, new LegoPlateRow(5)), // curved mf stand
                    new ("4042", BarFormFactor.Straight, new LegoPlateRow(1)), // curved mf stand
                    new ("78258", BarFormFactor.Straight, new LegoPlateRow(2), true), // short bar w/ stop ring
                    new ("30374", BarFormFactor.Straight, new LegoPlateRow(4)), // magic wand (bar)
                    new ("63965", BarFormFactor.Straight, new LegoPlateRow(6), true), // long bar w/ stop ring
                    new ("61184", BarFormFactor.Straight, new LegoPlateRow(2)), // flick missile
                    new ("2566", BarFormFactor.Straight, new LegoPlateRow(1), Quantity: 4), // palm tree top
                    new ("2566", BarFormFactor.Straight, new LegoBrickColumn(1)), // palm tree top
                    new ("21699", BarFormFactor.Straight, new LegoPlateRow(2)), // kylo ren lightsaber
                    new ("21699", BarFormFactor.Straight, new LegoPlateRow(4)), // kylo ren lightsaber
                    new ("48723", BarFormFactor.Straight, new LegoPlateRow(1), Quantity: 4), // cross, hub
                    new ("64648", BarFormFactor.Straight, new LegoPlateColumn(1.5m)), // fish
                    new ("31577", BarFormFactor.Straight, new LegoPlateColumn(1.5m)), // crab
                    new ("20482", BarFormFactor.Straight, new LegoPlateColumn(1m)), // round tile w/ bar
                    new ("32606", BarFormFactor.Straight, new LegoPlateColumn(1m)), // flower w/ bar
                };

            return;
        }
    }

    public record class Axle(
        string OwnerArticle,
        LegoMeasureUnit Length,
        byte Quantity = 1)
        : Connector(OwnerArticle, Length)
    {
        public static IEnumerable<Axle> Samples;

        static Axle()
        {
            Samples = new Axle[]
                {
                    new ("4042", new LegoPlateRow(1)), // curved mf stand
                    new ("43093", new LegoPlateRow(1)), // pin w/ axle
                    new ("11214", new LegoPlateRow(1)), // double pin w/ axle
                    new ("18651", new LegoPlateRow(2)), // double axle w/ pin
                    new ("6587", new LegoPlateRow(3)), // triple axle w/ stud
                    new ("32062", new LegoPlateRow(2)), // double axle
                    new ("50450", new LegoPlateRow(32)), // 32x axle
                };

            return;
        }
    }

    public record class Pin(
        string OwnerArticle,
        LegoMeasureUnit Length,
        byte Quantity = 1)
        : Connector(OwnerArticle, Length)
    {
        public static IEnumerable<Pin> Samples;

        static Pin()
        {
            Samples = new Pin[]
                {
                    new ("61184", new LegoPlateRow(1)), // flick missile,
                    new ("89678", new LegoPlateRow(1)), // pin w/ stud
                    new ("32002", new LegoPlateRow(1)), // pin w/ half-pin
                    new ("32002", new LegoPlateRow(0.5m)), // pin w/ half-pin
                    new ("2780", new LegoPlateRow(1), Quantity: 2), // double pin
                    new ("6558", new LegoPlateRow(1)), // triple pin
                    new ("6558", new LegoPlateRow(2)), // triple pin
                    new ("43093", new LegoPlateRow(1)), // pin w/ axle
                    new ("11214", new LegoPlateRow(2)), // double pin w/ axle
                    new ("65304", new LegoPlateRow(2)), // double pin w/ axle hole
                    new ("18651", new LegoPlateRow(1)), // double axle w/ pin
                };

            return;
        }
    }

    public record class SmallPin(
        string OwnerArticle,
        LegoMeasureUnit Length,
        byte Quantity = 1)
        : Connector(OwnerArticle, Length)
    {
        public static IEnumerable<SmallPin> Samples;

        static SmallPin()
        {
            Samples = new SmallPin[]
                {

                };

            return;
        }
    }
}
