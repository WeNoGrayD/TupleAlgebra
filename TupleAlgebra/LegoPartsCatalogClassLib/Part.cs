using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPartsCatalogClassLib
{
    public record class Part(
        string Article, 
        string PageUrl, 
        string ImageSource)
    {
        public static IEnumerable<Part> Parts { get; private set; }

        static Part()
        {
            Parts = [
                new ("47847", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=47847&name=Rock%20Panel%202%20x%204%20x%206&category=%5BRock%5D#T=C", "https://img.bricklink.com/ItemImage/EXTN/47409.png"), // rock panel
                new ("47847pb002", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=47847pb002&name=Rock%20Panel%202%20x%204%20x%206%20with%20Marbled%20Dark%20Bluish%20Gray%20Pattern&category=%5BRock%5D#T=C", "https://img.bricklink.com/ItemImage/PN/18/47847pb002.png"), // rock panel (marbled)
                //new ("47759pb05", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=47759pb05&name=Wedge%202%20x%204%20Triple%20with%20Red%20Eyes%20Pattern%20(Sticker)%20-%20Set%208056&category=%5BWedge,%20Decorated%5D#T=C&C=11", "https://img.bricklink.com/ItemImage/PN/11/47759pb05.png"), // wedge with crab eyes pattern
                //new ("3001pb018", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=3001pb018&name=Brick%202%20x%204%20with%20%27KNIGHT%20BUS%27%20Pattern&category=%5BBrick,%20Decorated%5D#T=C&C=89", "https://img.bricklink.com/ItemImage/PN/89/3001pb018.png"), // brick 2x4 "KNIGHT BUS"
                new ("24122", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=24122&name=Technic,%20Axle%20Connector%20Hub%20with%20Two%20Bar%20Holders%20Perpendicular%20(Lightsaber%20Hilt)&category=%5BTechnic,%20Connector%5D#T=C", "https://img.bricklink.com/ItemImage/PN/77/24122.png"), // lightsaber hilt
                new ("48723", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=48723&name=Technic,%20Axle%20Connector%20Hub%20with%204%20Bars%20and%20Pin%20Hole&category=%5BTechnic,%20Connector%5D#T=C", "https://img.bricklink.com/ItemImage/PN/88/48723.png"), // cross, hub
                new ("33078", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=33078&name=Hot%20Dog%20/%20Sausage&category=%5BFood%20&%20Drink%5D#T=C", "https://img.bricklink.com/ItemImage/PN/5/33078.png"), // sausage
                new ("78258", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=78258&name=Bar%202L%20with%20Stop%20Ring&category=%5BBar%5D#T=C", "https://img.bricklink.com/ItemImage/PN/85/78258.png"), // short bar w/ stop ring
                new ("30374", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=30374&name=Bar%20%20%204L%20(Lightsaber%20Blade%20/%20Wand)&category=%5BBar%5D#T=C", "https://img.bricklink.com/ItemImage/PN/34/30374.png"), // magic wand (bar)
                new ("63965", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=63965&name=Bar%20%20%206L%20with%20Stop%20Ring&category=%5BBar%5D#T=C", "https://img.bricklink.com/ItemImage/PN/3/63965.png"), // long bar w/ stop ring
                new ("61184", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=61184&name=Technic,%20Pin%201/2%20with%202L%20Bar%20Extension%20(Flick%20Missile)&category=%5BTechnic,%20Pin%5D#T=C", "https://img.bricklink.com/ItemImage/PN/86/61184.png"), // // flick missile
                new ("2566", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=2566&name=Bar%201.2L%20with%20Top%20Stud%20and%204%20Bar%20Arms%20Up%20(Palm%20Tree%20Top)&category=%5BBar%5D#T=C", "https://img.bricklink.com/ItemImage/PN/8/2566.png"), // palm tree top
                new ("21699", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=21699&name=Bar%20%20%204L%20/%202L%20Crossed%20(Lightsaber%20Blade)&category=%5BBar%5D#T=C", "https://img.bricklink.com/ItemImage/PN/17/21699.png"), // kylo ren lightsaber
                new ("64648", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=64648&name=Fish&category=%5BAnimal,%20Water%5D#T=C", "https://img.bricklink.com/ItemImage/PN/4/64648.png"), // fish
                new ("31577", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=33121&name=Crab&category=%5BAnimal,%20Water%5D#T=C", "https://img.bricklink.com/ItemImage/PN/59/33121.png"), // crab
                new ("30115", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=30115&name=Snake&category=%5BAnimal,%20Land%5D#T=C", "https://img.bricklink.com/ItemImage/PN/5/30115.png"), // snake
                new ("20482", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=20482&name=Tile,%20Round%201%20x%201%20with%20Bar%20and%20Pin%20Holder&category=%5BTile,%20Round%5D#T=C&C=115", "https://img.bricklink.com/ItemImage/PN/115/20482.png"), // round tile w/ bar
                new ("4042", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=4042&name=Bar%20Curved%20with%20Axle%201L%20and%201%20x%201%20Round%20Plate%20End&category=%5BBar%5D#T=C&C=12", "https://img.bricklink.com/ItemImage/PN/12/4042.png"), // curved mf stand
                new ("89678", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=89678#T=C", "https://img.bricklink.com/ItemImage/PN/5/89678.png"), // pin w/ stud
                new ("32002", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=32002#T=C", "https://img.bricklink.com/ItemImage/PN/2/32002.png"), // pin w/ half-pin
                new ("2780", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=2780#T=C", "https://img.bricklink.com/ItemImage/PN/11/2780.png"), // double pin
                new ("6558", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=6558#T=C", "https://img.bricklink.com/ItemImage/PN/11/6558.png"), // triple pin
                new ("43093", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=43093#T=C", "https://img.bricklink.com/ItemImage/PN/7/43093.png"), // pin w/ axle
                new ("11214", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=11214#T=C", "https://img.bricklink.com/ItemImage/PN/85/11214.png"), // double pin w/ axle
                new ("65304", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=32054#T=C", "https://img.bricklink.com/ItemImage/PN/5/32054.png"), // double pin w/ axle hole
                new ("18651", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=18651#T=C", "https://img.bricklink.com/ItemImage/PN/11/18651.png"), // double axle w/ pin
                new ("6587", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=6587#T=C", "https://img.bricklink.com/ItemImage/PN/69/6587.png"), // triple axle w/ stud
                new ("32062", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=32062#T=C", "https://img.bricklink.com/ItemImage/PN/11/32062.png"), // double axle
                new ("50450", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=50450#T=C", "https://img.bricklink.com/ItemImage/PN/1/50450.png"), // 32x axle
                new ("90202", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=90202#T=C", "https://img.bricklink.com/ItemImage/PN/11/90202.png"), // clip hub
                new ("62462", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=62462#T=C", "https://img.bricklink.com/ItemImage/PN/3/62462.png"), // 2-pin tube
                new ("2528", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=2528#T=C", "https://img.bricklink.com/ItemImage/EXTN/30155.png"), // pirate cap bicorn
                new ("2528pb01", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=2528pb01&name=Minifigure,%20Headgear%20Hat,%20Pirate%20Bicorne%20with%20White%20Skull%20and%20Crossbones%20Small%20Pattern&category=%5BMinifigure,%20Headgear%5D#T=C", "https://img.bricklink.com/ItemImage/PN/11/2528pb01.png"), // pirate cap bicorn (printed)
                new ("3742", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=3742#T=C", "https://img.bricklink.com/ItemImage/PN/3/3742.png"), // flower flat
                new ("32606", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=32606#T=C", "https://img.bricklink.com/ItemImage/EXTN/67430.png"), // flower w/ bar
                new ("51342", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=51342#T=C", "https://img.bricklink.com/ItemImage/PN/11/51342.png"), // dragon wing, 1
                new ("51342pb07", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=51342pb07&name=Dragon%20Wing%2019%20x%2011%20with%20Marbled%20Red%20Trailing%20Edge%20Pattern&category=%5BAnimal,%20Body%20Part,%20Decorated%5D#T=C", "https://img.bricklink.com/ItemImage/PN/11/51342pb07.png"), // dragon wing, 1 (marbled)
                new ("2546", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=2546&name=Bird,%20Parrot%20with%20Small%20Beak&category=%5BAnimal,%20Air%5D#T=C", "https://img.bricklink.com/ItemImage/PN/23/2546.png"), // parrot, old
                new ("2546p01", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=2546p01&name=Bird,%20Parrot%20with%20Small%20Beak%20with%20Green%20and%20Yellow%20Feathers,%20White%20Face%20and%20Yellow%20Beak%20Pattern&category=%5BAnimal,%20Air%5D#T=C", "https://img.bricklink.com/ItemImage/PN/5/2546p01.png"), // parrot, old (decorated)
                new ("2546p02", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=2546p02&name=Bird,%20Parrot%20with%20Small%20Beak%20with%20Marbled%20Red%20Pattern&category=%5BAnimal,%20Air%5D#T=C", "https://img.bricklink.com/ItemImage/PN/6/2546p02.png"), // parrot, old (marbled)
                new ("16606pb001", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=16606pb001&name=Dog,%20Husky%20with%20Marbled%20Dark%20Bluish%20Gray%20Ears%20and%20Back%20and%20Printed%20Black%20Eyes%20and%20Nose,%20White%20Face%20and%20Ears%20Pattern&category=%5BAnimal,%20Land%5D#T=C", "https://img.bricklink.com/ItemImage/PN/1/16606pb001.png"), // dog, husky, white (marbled)
                new ("16606pb002", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=16606pb002&name=Dog,%20Husky%20with%20Marbled%20Black%20Ears%20and%20Back%20and%20Printed%20Black%20Eyes%20and%20Nose,%20Dark%20Orange%20Face%20and%20Ears%20Pattern%20(Red%20/%20Copper)&category=%5BAnimal,%20Land%5D#T=C&C=68", "https://img.bricklink.com/ItemImage/PN/68/16606pb002.png"), // dog, husky, dark orange (marbled)
                new ("55706", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=51342#T=C", "https://img.bricklink.com/ItemImage/PN/11/51342.png"), // dragon wing, 2
                new ("55706pb01", "https://www.bricklink.com/v2/catalog/catalogitem.page?P=55706pb01&name=Dragon%20Wing%208%20x%2010,%20Green%20and%20Yellow%20Trailing%20Edge%20Pattern&category=%5BAnimal,%20Body%20Part,%20Decorated%5D#T=C", "https://img.bricklink.com/ItemImage/PN/63/55706pb01.png"), // dragon wing, 2 (marbled)
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
