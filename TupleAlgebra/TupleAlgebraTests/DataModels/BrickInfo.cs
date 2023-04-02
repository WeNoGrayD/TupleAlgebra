using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraTests.DataModels
{
    public enum RelationBetweenBricks
    {
        OldVersion,
        NewVersion,
        Coworker,
        SimilarPattern
    }

    public class BrickInfo
    {
        public virtual string PartNumber { get; set; }

        public virtual string Description { get; set; }

        public virtual (DateTime Start, DateTime End) YearReleased { get; set; }

        public virtual IDictionary<RelationBetweenBricks, BrickInfo> RelatedParts { get; set; }

        protected BrickInfo() { }

        private BrickInfo(
            string partNumber,
            string description,
            IDictionary<RelationBetweenBricks, BrickInfo> relatedParts = null)
        {
            PartNumber = partNumber;
            Description = description;
            RelatedParts = relatedParts ?? new Dictionary<RelationBetweenBricks, BrickInfo>();
        }

        public BrickInfo(
            string partNumber,
            string description,
            (DateTime Start, DateTime End) yearReleased,
            IDictionary<RelationBetweenBricks, BrickInfo> relatedParts = null)
            : this(partNumber, description, relatedParts)
        {
            YearReleased = yearReleased;
        }

        public BrickInfo(
            string partNumber,
            string description,
            DateTime yearReleasedStart,
            IDictionary<RelationBetweenBricks, BrickInfo> relatedParts = null)
            : this(partNumber, description, relatedParts)
        {
            YearReleased = (yearReleasedStart, new DateTime(DateTime.Now.Year, 0, 0));
        }

        public virtual TDecorator As<TDecorator>() where TDecorator : BrickInfo => null;
    }

    public class BrickInfoDecorator : BrickInfo
    {
        protected BrickInfo _content;

        public override string PartNumber => _content.PartNumber;

        public override string Description => _content.Description;

        public override IDictionary<RelationBetweenBricks, BrickInfo> RelatedParts => _content.RelatedParts;

        public BrickInfoDecorator(BrickInfo content)
        {
            _content = content;
        }

        public override TDecorator As<TDecorator>()
        {
            return this as TDecorator ?? this._content.As<TDecorator>();
        }
    }

    public class ColouredBrickInfo : BrickInfoDecorator
    {
        protected string _colourId;

        protected string _colourDescription;

        public virtual byte[] Colour { get; set; }

        public override string PartNumber => _content.PartNumber + _colourId;

        public override string Description => _content.Description + ", " + _colourDescription;

        public ColouredBrickInfo(
            BrickInfo content,
            string colourId,
            string colourDescription,
            byte[] colour)
            : base(content)
        {
            if (colour.Length < 3 || colour.Length > 4)
                throw new ArgumentException($"Color должен содержать 3 или 4 байта.");

            _colourId = colourId;
            _colourDescription = colourDescription;
            Colour = colour;
        }
    }

    public class DecoratedBrickInfo : BrickInfoDecorator
    {
        protected string _decorationId;

        protected string _decorationDescription;

        public override string PartNumber => _content.PartNumber + _decorationId;

        public override string Description => _content.Description + ", " + _decorationDescription;

        public DecoratedBrickInfo(
            BrickInfo content,
            string decorationId,
            string decorationDescription)
            : base(content)
        {
            _decorationId = decorationId;
            _decorationDescription = decorationDescription;
        }
    }
}
