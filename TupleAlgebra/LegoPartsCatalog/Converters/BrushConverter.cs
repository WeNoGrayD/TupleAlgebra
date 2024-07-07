using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LegoPartsCatalogApp.Converters
{
    public class SolidColorBrushConverter : IValueConverter
    {
        private static Regex _rgbCodeRule;

        private IDictionary<string, SolidColorBrush> _brushes;

        static SolidColorBrushConverter()
        {
            _rgbCodeRule = new Regex("#(?<R>[0-9A-Fa-f]{2})(?<G>[0-9A-Fa-f]{2})(?<B>[0-9A-Fa-f]{2})", 
                RegexOptions.Compiled);

            return;
        }

        public SolidColorBrushConverter()
        {
            _brushes = new Dictionary<string, SolidColorBrush>();

            return;
        }

        public object Convert(
            object value, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            string rgbCode = value as string;
            if (rgbCode is null) throw new Exception("RGB code is not a string.");

            if (_brushes.ContainsKey(rgbCode)) return _brushes[rgbCode];

            SolidColorBrush newBrush = CreateBrush(rgbCode);
            _brushes.Add(rgbCode, newBrush);

            return newBrush;
        }

        public object ConvertBack(
            object value, 
            Type targetType, 
            object parameter, 
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private SolidColorBrush CreateBrush(string rgbCode)
        {
            Match rgbMatch = _rgbCodeRule.Match(rgbCode);
            if (!rgbMatch.Success)
            {
                throw new Exception($"Invalid RGB code: {rgbCode}");
            }

            return new SolidColorBrush(
                Color.FromArgb(
                    255,
                    ParseByte("R"),
                    ParseByte("G"),
                    ParseByte("B")
                    )
                );

            byte ParseByte(string colorComponent)
            {
                return byte.Parse(
                    rgbMatch.Groups[colorComponent].Captures[0].Value, 
                    System.Globalization.NumberStyles.AllowHexSpecifier);
            }
        }
    }
}
