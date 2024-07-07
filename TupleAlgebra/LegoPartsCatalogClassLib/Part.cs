using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPartsCatalogClassLib
{
    public enum PartTrait : byte
    {
        Flat,
        Studs,
        SNOT,
        SmallPin,
        SmallPinHole,
        Pin,
        PinHole,
        Axle,
        AxleHole,
        Bar,
        Clips,
        Decorated,
        Marbled
    }

    public record class Part(
        string Article, 
        string PageUrl, 
        string ImageSource)
    {
        public static IEnumerable<Part> Parts { get; private set; }

        static Part()
        {
            Parts = [
                new ("47847pb002", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=47847pb002&name=Rock%20Panel%202%20x%204%20x%206%20with%20Marbled%20Dark%20Bluish%20Gray%20Pattern&category=%5BRock%5D#T=C", "https://img.bricklink.com/ItemImage/PN/18/47847pb002.png"),
                new ("47759pb05", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=47759pb05&name=Wedge%202%20x%204%20Triple%20with%20Red%20Eyes%20Pattern%20(Sticker)%20-%20Set%208056&category=%5BWedge,%20Decorated%5D#T=C&C=11", "https://img.bricklink.com/ItemImage/PN/11/47759pb05.png"),
                new ("3001pb018", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=3001pb018&name=Brick%202%20x%204%20with%20%27KNIGHT%20BUS%27%20Pattern&category=%5BBrick,%20Decorated%5D#T=C&C=89", "https://img.bricklink.com/ItemImage/PN/89/3001pb018.png"),
            ];

            return;
        }

        /*
        public string Article { get; set; }

        public string ImageSource { get; set; }

        public Part(string article
        */
    }
}
