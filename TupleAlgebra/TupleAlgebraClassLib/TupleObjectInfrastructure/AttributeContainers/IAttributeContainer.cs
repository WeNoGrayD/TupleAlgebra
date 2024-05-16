using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.AttributeContainers
{
    using static TupleObjectHelper;

    public interface IAttributeContainer
        : IDictionary<AttributeName, ITupleObjectAttributeInfo>
    {
        public AttributeName[] PluggedAttributeNames { get; }

        public IEnumerable<ITupleObjectAttributeInfo> PluggedAttributes { get; }

        public int PluggedAttributesCount { get; }

        public bool ContainsAttribute(AttributeName attributeName);

        public bool IsPlugged(AttributeName attrName);

        public int GetAttributeLoc(AttributeName attrName);

        public void AddAttribute(
            AttributeName attrName,
            ITupleObjectAttributeInfo attrInfo);

        public void RemoveAttribute(AttributeName attrName);

        public void AttachAttribute(AttributeName attrName);

        public void DetachAttribute(AttributeName attrName);

        public void EndInitialize();

        public ITupleObjectAttributeInfo GetPluggedO1(AttributeName attrName);

        public IAttributeContainer Clone();
    }
}
