using LegoPartsCatalogClassLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace LegoPartsCatalogApp.Models
{
    public class ColorGroupQueryModel
    {
        private TupleObject<ColorInfo> _groupRule;

        private ColorQueryModel _colors;

        public string GroupName { get; private set; }

        public TupleObject<ColorInfo> Query { get; private set; }

        public ColorGroupQueryModel(
            ColorQueryModel colors,
            string groupName,
            TupleObject<ColorInfo> groupRule)
        {
            _colors = colors;
            GroupName = groupName;
            _groupRule = groupRule;
            Query = _colors.Factory.CreateEmpty<ColorInfo>();

            return;
        }

        public IEnumerable<ColorInfo> GetColors()
        {
            return _colors.ColorsKb & _groupRule;
        }

        public void MakeQuery()
        {
            Query = _groupRule;

            return;
        }

        public void MakeQuery(IEnumerable<ColorInfo> selectedColors)
        {
            Query = _colors.Factory
                .CreateConjunctiveTupleSystem(selectedColors, null, null);

            return;
        }
    }
}
