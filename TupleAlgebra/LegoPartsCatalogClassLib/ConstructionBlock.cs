using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace LegoPartsCatalogClassLib
{
    public enum ConstructionBlockBase
    {
        Circle,
        Semicircle,
        QuarterCircle,
        Triangle,
        Square,
        Rounded,
        Octagonal,
        HeartShaped,
        Other
    }

    public record class ConstructionBlock(
        string Article,
        ConstructionBlockBase Base,
        decimal Length,
        decimal Width,
        decimal Height)
    {
    }

    public class PartProvider
    {
        string Article { get; set; }
    }

    public class PartProvider2 : PartProvider
    { }

    public record class PartTraitTables(
        PartTrait Trait,
        TupleObject<PartProvider> Table)
    {
    }
}
