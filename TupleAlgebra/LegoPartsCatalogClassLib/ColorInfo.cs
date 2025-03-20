using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegoPartsCatalogClassLib
{
    [Flags]
    public enum ColorTrait : int
    {
        Standard = 0x0,
        Transparent = (0x1 << 6),
        Pearl = (0x2 << 6),
        Speckle = (0x4 << 6),
        Metallic = (0x8 << 6),
        Chrome = (0xA << 6)
    }

    /*
    public enum LegoPalette : int
    {
        //  Standard
        Black = 0x000000,
        ReddishBrown = 0x964b00,
        Red = 0xff0000,
        Green = 0x00ff00,
        Blue = 0x0000ff,
        Yellow = 0xffff00,
        DarkAzure = 0x043a7e,
        Purple = 0xcc00cc,
        White = 0xffffff,
        // Transparent
        TransBlack = ColorTrait.Transparent | Black,
        TransRed = ColorTrait.Transparent | Red,
        TransGreen = ColorTrait.Transparent | Red,
        TransLightBlue = ColorTrait.Transparent | Red,
        TransRed = ColorTrait.Transparent | Red,
    }
    */

    public record struct ColorInfo(
        string Name,
        string RGB,
        ColorTrait Trait)
    {
        //private const int RGB_MASK = 0x111111;

        public static ColorInfo[] Colors;

        static ColorInfo()
        {
            Colors = new ColorInfo[]
                {
                    new ("Black", "#000000", ColorTrait.Standard),
                    new ("Dark Gray", "#363636", ColorTrait.Standard),
                    new ("Reddish Brown", "#964b00", ColorTrait.Standard),
                    new ("Red", "#ff0000", ColorTrait.Standard),
                    new ("Green", "#00ff00", ColorTrait.Standard),
                    new ("Blue", "#0000ff", ColorTrait.Standard),
                    new ("Orange", "#ff9900", ColorTrait.Standard),
                    new ("Yellow", "#ffff00", ColorTrait.Standard),
                    new ("Dark Azure", "#00a1db", ColorTrait.Standard),
                    new ("Purple", "#cc00cc", ColorTrait.Standard),
                    new ("White", "#ffffff", ColorTrait.Standard),
                    new ("Trans-Black", "#000000", ColorTrait.Transparent),
                    new ("Trans-Red", "#ff0000", ColorTrait.Transparent),
                    new ("Trans-Green", "#00ff00", ColorTrait.Transparent),
                    new ("Trans-Light Blue", "#00ffff", ColorTrait.Transparent),
                    new ("Trans-Neon Orange", "#ff9900", ColorTrait.Transparent),
                    new ("Trans-Purple", "#cc00cc", ColorTrait.Transparent),
                    new ("Trans-Clear", "#ffffff", ColorTrait.Transparent),
                    new ("Pearl Copper", "#b87333", ColorTrait.Pearl),
                    new ("Pearl Gold", "#ffd700", ColorTrait.Pearl),
                    new ("Speckle Black", "#000000", ColorTrait.Speckle),
                    new ("Speckle Gold", "#ffd700", ColorTrait.Speckle),
                    new ("Metallic Copper", "#b87333", ColorTrait.Metallic),
                    new ("Metallic Silver", "#c0c0c0", ColorTrait.Metallic),
                    new ("Metallic Gold", "#ffd700", ColorTrait.Metallic),
                    new ("Chrome Green", "#00ff00", ColorTrait.Chrome),
                    new ("Chrome Pink", "#fc0fc0", ColorTrait.Chrome),
                    new ("Chrome Silver", "#c0c0c0", ColorTrait.Chrome),
                    new ("Chrome Gold", "#ffd700", ColorTrait.Chrome),
                };

            return;
        }

        /*
        static ColorInfo()
        {
            Colors = new ColorInfo[]
                {
                    new (LegoPalette.Black),
                    new (LegoPalette.ReddishBrown),
                    new (LegoPalette.Red),
                    new (LegoPalette.Green),
                    new (LegoPalette.Blue),
                    new (LegoPalette.Yellow),
                    new (LegoPalette.DarkAzure),
                    new (LegoPalette.Purple),
                    new (LegoPalette.White),
                    new ("Trans-Black", "#000000", ColorTrait.Transparent),
                    new ("Trans-Red", "#ff0000", ColorTrait.Transparent),
                    new ("Trans-Green", "#00ff00", ColorTrait.Transparent),
                    new ("Trans-Light Blue", "#00ffff", ColorTrait.Transparent),
                    new ("Trans-Purple", "#cc00cc", ColorTrait.Transparent),
                    new ("Trans-Clear", "#ffffff", ColorTrait.Transparent),
                    new ("Pearl Copper", "#b87333", ColorTrait.Pearl),
                    new ("Pearl Gold", "#ffd700", ColorTrait.Pearl),
                    new ("Speckle Black", "#000000", ColorTrait.Speckle),
                    new ("Speckle Gold", "#ffd700", ColorTrait.Speckle),
                    new ("Metallic Copper", "#b87333", ColorTrait.Metallic),
                    new ("Metallic Silver", "#c0c0c0", ColorTrait.Metallic),
                    new ("Metallic Gold", "#ffd700", ColorTrait.Metallic),
                    new ("Chrome Green", "#00ff00", ColorTrait.Chrome),
                    new ("Chrome Pink", "#fc0fc0", ColorTrait.Chrome),
                    new ("Chrome Silver", "#c0c0c0", ColorTrait.Chrome),
                    new ("Chrome Gold", "#ffd700", ColorTrait.Chrome),
                };

            return;
        }

        public ColorInfo(LegoPalette color)
            : this(
                  GetColorName(color),
                  GetColorRGB(color),
                  GetColorTrait(color))
        {
            return;
        }

        private static string GetColorName(LegoPalette color)
        {
            return Enum.GetName(color)!;
        }

        private static string GetColorRGB(LegoPalette color)
        {
            return $"#{(int)color & RGB_MASK:X6}";
        }

        private static ColorTrait GetColorTrait(LegoPalette color)
        {
            return (ColorTrait)((int)color & ~RGB_MASK);
        }

        */
    }
}
