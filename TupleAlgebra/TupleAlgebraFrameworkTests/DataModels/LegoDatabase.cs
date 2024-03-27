using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraFrameworkTests.DataModels
{
    public static class LegoDatabase
    {
        private static bool _isInitialized = false;

        public static IEnumerable<BrickInfo> Parts { get; private set; }

        public static IEnumerable<MinifigureInfo> Minifigures { get; private set; }

        public static IEnumerable<SetInfo> Sets { get; private set; }

        static LegoDatabase()
        {
            Init();
        }

        public static void Init()
        {
            if (_isInitialized) return;
            _isInitialized = true;

            List<BrickInfo> parts = new List<BrickInfo>();
            List<MinifigureInfo> minifigures = new List<MinifigureInfo>();
            List<SetInfo> sets = new List<SetInfo>();

            BrickInfo brick2x4Mold = new BrickInfo("a001", "Brick 2x4", new DateTime(1971, 0, 0)),
                      plate1x2Mold = new BrickInfo("a109", "Plate 1x2", new DateTime(1974, 0, 0)),
                      cheese1x1 = new BrickInfo("a658", "Slope 1x1 cheese", new DateTime(2007, 0, 0)),
                      bowslope1x2 = new BrickInfo("a777", "Bow slope 1x2", new DateTime(2012, 0, 0)),
                      grayBrick2x4 = new ColouredBrickInfo(brick2x4Mold, "05", "gray", new byte[3] { 100, 100, 100 }),
                      sandgreenBrick2x4 = new ColouredBrickInfo(brick2x4Mold, "05", "sand green", new byte[3] { 0, 237, 50 }),
                      sandtanBrick2x4 = new ColouredBrickInfo(brick2x4Mold, "08", "sand tan", new byte[3] { 36, 237, 229 }),
                      grayPlate1x2 = new ColouredBrickInfo(plate1x2Mold, "05", "gray", new byte[3] { 100, 100, 100 });

            InitDataSource(parts,
                           brick2x4Mold,
                           plate1x2Mold,
                           cheese1x1,
                           bowslope1x2,
                           grayBrick2x4,
                           sandgreenBrick2x4,
                           sandtanBrick2x4,
                           grayPlate1x2);

            BrickInfo mfHeadMold = new BrickInfo("c1133", "Minifigure Head", new DateTime(1978, 0, 0)),
                      yellowMfHeadMold = new ColouredBrickInfo(mfHeadMold, "01", "yellow", new byte[3] { 0, 255, 255 }),
                      lightnougatMfHeadMold = new ColouredBrickInfo(mfHeadMold, "02", "light nougat", new byte[3] { 40, 255, 255 }),
                      oldHanSoloMfHead = new DecoratedBrickInfo(yellowMfHeadMold, "pb987", "Han Solo"),
                      newHanSoloMfHead = new DecoratedBrickInfo(lightnougatMfHeadMold, "pb456", "Han Solo"),
                      oldHarryPotterMfHead = new DecoratedBrickInfo(yellowMfHeadMold, "pb123", "Harry Potter"),
                      oldRonWeasleyMfHead = new DecoratedBrickInfo(yellowMfHeadMold, "pb294", "Ron Weasley"),
                      oldHermioneGrangerMfHead = new DecoratedBrickInfo(yellowMfHeadMold, "pb045", "HermioneGranger");

            InitDataSource(parts,
                           mfHeadMold,
                           yellowMfHeadMold,
                           lightnougatMfHeadMold,
                           oldHanSoloMfHead,
                           newHanSoloMfHead,
                           oldHarryPotterMfHead,
                           oldRonWeasleyMfHead,
                           oldHermioneGrangerMfHead);

            BrickInfo mfTorsoMold = new BrickInfo("c1138", "Minifigure Torso", new DateTime(1978, 0, 0)),
                      redMfTorsoMold = new ColouredBrickInfo(mfTorsoMold, "03", "red", new byte[3] { 255, 0, 0 }),
                      blackAndWhiteMfTorsoMold = new ColouredBrickInfo(mfTorsoMold, "04", "black and white", new byte[3] { 0, 0, 0 }),
                      grayMfTorsoMold = new ColouredBrickInfo(mfTorsoMold, "05", "gray", new byte[3] { 100, 100, 100 }),
                      hansoloJacketMfTorso = new DecoratedBrickInfo(blackAndWhiteMfTorsoMold, "pb610", "Han Solo"),
                      gryffindorStudentMfTorso = new DecoratedBrickInfo(grayMfTorsoMold, "pb079", "Gryffindor student");

            InitDataSource(parts,
                           mfTorsoMold,
                           redMfTorsoMold,
                           blackAndWhiteMfTorsoMold,
                           grayMfTorsoMold,
                           hansoloJacketMfTorso,
                           gryffindorStudentMfTorso);

            BrickInfo mfHipsAndLegsMold = new BrickInfo("c2259", "Minifigure Hips And legs", new DateTime(1978, 0, 0)),
                      blueMfHipsAndLegsMold = new ColouredBrickInfo(mfHipsAndLegsMold, "06", "blue", new byte[3] { 0, 0, 255 }),
                      darkblueMfHipsAndLegsMold = new ColouredBrickInfo(mfHipsAndLegsMold, "07", "dark blue", new byte[3] { 0, 40, 255 }),
                      grayMfHipsAndLegsMold = new ColouredBrickInfo(mfHipsAndLegsMold, "05", "gray", new byte[3] { 100, 100, 100 }),
                      hansoloPantsMfHipsAndLegs = new DecoratedBrickInfo(darkblueMfHipsAndLegsMold, "pb884", "Han Solo");

            InitDataSource(parts,
                           mfHipsAndLegsMold,
                           blueMfHipsAndLegsMold,
                           darkblueMfHipsAndLegsMold,
                           grayMfHipsAndLegsMold,
                           hansoloPantsMfHipsAndLegs);

            MinifigureInfo newHanSoloMf = new MinifigureInfo("Han Solo (New)", new DateTime(2008, 0, 0),
                new Dictionary<MinifigurePart, BrickInfo>()
                {
                    { MinifigurePart.Head, newHanSoloMfHead },
                    { MinifigurePart.Torso, hansoloJacketMfTorso },
                    { MinifigurePart.HipsAndLegs, hansoloPantsMfHipsAndLegs }
                }),
                           oldHanSoloMf = new MinifigureInfo("Han Solo (Old)", new DateTime(1999, 0, 0),
                new Dictionary<MinifigurePart, BrickInfo>()
                {
                    { MinifigurePart.Head, oldHanSoloMfHead },
                    { MinifigurePart.Torso, hansoloJacketMfTorso },
                    { MinifigurePart.HipsAndLegs, hansoloPantsMfHipsAndLegs }
                }),
                           oldHarryPotterMf = new MinifigureInfo("Harry Potter (Old)", new DateTime(2001, 0, 0),
                new Dictionary<MinifigurePart, BrickInfo>()
                {
                    { MinifigurePart.Head, oldHarryPotterMfHead },
                    { MinifigurePart.Torso, gryffindorStudentMfTorso },
                    { MinifigurePart.HipsAndLegs, grayMfHipsAndLegsMold }
                }),
                           oldRonWeasleyMf = new MinifigureInfo("Ron Weasley (Old)", new DateTime(2001, 0, 0),
                new Dictionary<MinifigurePart, BrickInfo>()
                {
                    { MinifigurePart.Head, oldRonWeasleyMfHead },
                    { MinifigurePart.Torso, gryffindorStudentMfTorso },
                    { MinifigurePart.HipsAndLegs, grayMfHipsAndLegsMold }
                }),
                           oldHermioneGrangerMf = new MinifigureInfo("Hermione Granger (Old)", new DateTime(2001, 0, 0),
                new Dictionary<MinifigurePart, BrickInfo>()
                {
                    { MinifigurePart.Head, oldHermioneGrangerMfHead },
                    { MinifigurePart.Torso, gryffindorStudentMfTorso },
                    { MinifigurePart.HipsAndLegs, grayMfHipsAndLegsMold }
                }),
                           classinMf = new MinifigureInfo("Classic minifigure", new DateTime(1980, 0, 0),
                new Dictionary<MinifigurePart, BrickInfo>()
                {
                    { MinifigurePart.Head, yellowMfHeadMold },
                    { MinifigurePart.Torso, redMfTorsoMold },
                    { MinifigurePart.HipsAndLegs, blueMfHipsAndLegsMold }
                });

            InitDataSource(minifigures,
                           newHanSoloMf,
                           oldHanSoloMf,
                           oldHarryPotterMf,
                           oldRonWeasleyMf,
                           oldHermioneGrangerMf,
                           classinMf);

            SetInfo oldMilenniumFalcon = new SetInfo(3011, "Milennium Falcon", "Star Wars", new DateTime(1999, 0, 0),
                new[] { (grayBrick2x4, 100), (grayPlate1x2, 25) },
                new[] { (oldHanSoloMf, 1) }),
                    newMilenniumFalcon = new SetInfo(8088, "Milennium Falcon", "Star Wars", new DateTime(2008, 0, 0),
                new[] { (grayBrick2x4, 5000), (grayPlate1x2, 1000) },
                new[] { (newHanSoloMf, 1) }),
                    oldHogwarts = new SetInfo(5097, "Hogwarts", "HarryPotter", new DateTime(2001, 0, 0),
                new[] { (sandtanBrick2x4, 150), (sandgreenBrick2x4, 60), (grayPlate1x2, 35) },
                new[] { (oldHarryPotterMf, 1), (oldRonWeasleyMf, 1), (oldHermioneGrangerMf, 1) }),
                    oldChamberOfSecrets = new SetInfo(5099, "Hogwarts", "HarryPotter", new DateTime(2001, 0, 0),
                new[] { (grayBrick2x4, 110), (sandgreenBrick2x4, 20), (grayPlate1x2, 49) },
                new[] { (oldHarryPotterMf, 1), (oldRonWeasleyMf, 1) });

            InitDataSource(sets,
                           oldMilenniumFalcon,
                           newMilenniumFalcon,
                           oldHogwarts,
                           oldChamberOfSecrets);

            Parts = parts;
            Minifigures = minifigures;
            Sets = sets;
        }

        private static void InitDataSource<T>(IList<T> expanded, params T[] values)
        {
            foreach (T value in values) expanded.Add(value);
        }
    }
}
