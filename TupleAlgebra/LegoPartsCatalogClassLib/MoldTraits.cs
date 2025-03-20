using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPartsCatalogClassLib
{
    public enum MoldTraits : byte
    {
        AntiStud,
        Stud,
        SmallPin,
        SmallPinHole,
        Pin,
        PinHole,
        Axle,
        AxleHole,
        Bar,
        BarHole,
        Clip,
        Decorated,
        Marbled
    }

    /// <summary>
    /// Orientation of mold trait. 
    /// Context-related.
    /// </summary>
    public enum MoldTraitOrientation : byte
    {
        NotApplicable = 0,
        Horizontal = 0b000,
        HorizontalUp = 0b100,
        HorizontalDown = 0b1000,
        Vertical = 0b000010,
        VerticalLeft = 0b010010,
        VerticalRight = 0b100010,
        Diagonal = 0b1000000
    }
}
