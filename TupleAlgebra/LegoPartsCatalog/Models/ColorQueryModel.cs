using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjects;
using LegoPartsCatalogClassLib;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Specialized.EnumBased;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Streaming;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable.Buffered;

namespace LegoPartsCatalogApp.Models
{
    using static TupleObjectHelper;

    public class ColorQueryModel
    {
        public TupleObject<ColorInfo> Query { get; private set; }

        public TupleObject<ColorInfo> ColorsKb { get; private set; }

        public TupleObjectFactory Factory;

        public ColorGroupQueryModel[] ColorGroups { get; private set; }

        public ColorQueryModel()
        {
            Factory = new TupleObjectFactory(null);
            TupleObject<ColorInfo>.Configure(ConfigureColors);
            ColorsKb = Factory.CreateConjunctiveTupleSystem(ColorInfo.Colors);
            Query = null;
            ColorGroups = [
                new ColorGroupQueryModel(this, "Стандарт", StandardColorsRule()),
                new ColorGroupQueryModel(this, "Прозрачный", TransparentColorsRule()),
                new ColorGroupQueryModel(this, "Жемчуг", PearlColorsRule()),
                new ColorGroupQueryModel(this, "Матовый", SpeckleColorsRule()),
                new ColorGroupQueryModel(this, "Металлик", MetallicColorsRule()),
                new ColorGroupQueryModel(this, "Хром", ChromeColorsRule())
                ];

            return;
        }

        public void MakeQuery()
        {
            Query = Factory.CreateEmpty<ColorInfo>();

            foreach (var colorGroup in ColorGroups)
                Query |= colorGroup.Query;

            if (Query.IsFalse()) Query = Factory.CreateFull<ColorInfo>();

            return;
        }

        private static void ConfigureColors(TupleObjectBuilder<ColorInfo> builder)
        {
            IEnumerable<ColorInfo> colors = ColorInfo.Colors;

            builder.Attribute(ci => ci.Name)
                .SetFactory(
                    new OrderedFiniteEnumerableAttributeComponentFactory<string>(
                        colors.Select(ci => ci.Name)))
                .Attach();
            builder.Attribute(ci => ci.RGB)
                .SetFactory(
                    new UnorderedFiniteEnumerableAttributeComponentFactory<string>(
                        colors.Select(ci => ci.RGB).ToHashSet()))
                .Attach();
            builder.Attribute(ci => ci.Trait)
                .SetFactory(EnumBasedAttributeComponentFactory<ColorTrait>.Instance)
                .Attach();

            return;
        }

        public TupleObject<ColorInfo> StandardColorsRule()
        {
            return Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [
                        SetAC<ColorInfo, ColorTrait>(ci => ci.Trait, new BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<ColorTrait>([ColorTrait.Standard]))
                    ],
                    null, null);
        }

        public TupleObject<ColorInfo> TransparentColorsRule()
        {
            return Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [
                        SetAC<ColorInfo, ColorTrait>(ci => ci.Trait, new BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<ColorTrait>([ColorTrait.Transparent]))
                    ],
                    null, null);
        }

        public TupleObject<ColorInfo> PearlColorsRule()
        {
            return Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [
                        SetAC<ColorInfo, ColorTrait>(ci => ci.Trait, new BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<ColorTrait>([ColorTrait.Pearl]))
                    ],
                    null, null);
        }

        public TupleObject<ColorInfo> SpeckleColorsRule()
        {
            return Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [
                        SetAC<ColorInfo, ColorTrait>(ci => ci.Trait, new BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<ColorTrait>([ColorTrait.Speckle]))
                    ],
                    null, null);
        }

        public TupleObject<ColorInfo> MetallicColorsRule()
        {
            return Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [
                        SetAC<ColorInfo, ColorTrait>(ci => ci.Trait, new BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<ColorTrait>([ColorTrait.Metallic]))
                    ],
                    null, null);
        }

        public TupleObject<ColorInfo> ChromeColorsRule()
        {
            return Factory.
                CreateConjunctiveTuple<ColorInfo>(
                    [
                        SetAC<ColorInfo, ColorTrait>(ci => ci.Trait, new BufferedOrderedFiniteEnumerableAttributeComponentFactoryArgs<ColorTrait>([ColorTrait.Chrome]))
                    ],
                    null, null);
        }
    }
}
