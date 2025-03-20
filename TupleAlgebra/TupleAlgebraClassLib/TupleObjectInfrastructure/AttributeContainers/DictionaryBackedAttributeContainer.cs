using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.AttributeContainers
{
    using static TupleObjectHelper;

    public class DictionaryBackedAttributeContainer
        : Dictionary<AttributeName, ITupleObjectAttributeInfo>,
          IAttributeContainer
    {
        private IDictionary<AttributeName, int> _pluggedAttributes;

        private Lazy<AttributeName[]> _pluggedAttributeNames;

        public AttributeName[] PluggedAttributeNames
        {
            get => _pluggedAttributeNames.Value;
        }

        public IEnumerable<ITupleObjectAttributeInfo> PluggedAttributes
        {
            get => PluggedAttributeNames.Select(n => this[n]);
        }

        public int PluggedAttributesCount { get => _pluggedAttributes.Count; }

        public DictionaryBackedAttributeContainer()
            : base()
        {
            Init();

            return;
        }

        public DictionaryBackedAttributeContainer(
            IDictionary<AttributeName, ITupleObjectAttributeInfo> source,
            IDictionary<AttributeName, int> pluggedAttributes)
            : base(source)
        {
            Init();
            foreach (AttributeName attrName in pluggedAttributes.Keys)
            {
                _pluggedAttributes.Add(attrName, -1);
            }

            return;
        }

        private void Init()
        {
            _pluggedAttributes = new Dictionary<AttributeName, int>();

            return;
        }

        public bool ContainsAttribute(AttributeName attributeName)
        {
            return ContainsKey(attributeName);
        }

        private void PlugIn(AttributeName attrName)
        {
            _pluggedAttributes[attrName] = -1;

            return;
        }

        private void Unplug(AttributeName attrName)
        {
            _pluggedAttributes.Remove(attrName);

            return;
        }

        public bool IsPlugged(AttributeName attrName)
        {
            return _pluggedAttributes.ContainsKey(attrName);
        }

        public int GetAttributeLoc(AttributeName attrName)
        {
            return _pluggedAttributes[attrName];
        }

        public void AddAttribute(
            AttributeName attrName,
            ITupleObjectAttributeInfo attrInfo)
        {
            Add(attrName, attrInfo);

            return;
        }

        public void RemoveAttribute(AttributeName attrName)
        {
            this[attrName].RemoveQuery();

            return;
        }

        private void RemoveAttributeImpl(AttributeName attrName)
        {
            Remove(attrName);
            _pluggedAttributes.Remove(attrName);

            return;
        }

        /// <summary>
        /// Операция прикрепления атрибута будет выполняться чаще чем
        /// операция открепления. Из этого следует, что атрибуты 
        /// должны храниться так:
        /// [unplugged, ..., unplugged, plugged, ..., plugged]
        /// Таким образом будет выполняться меньше операций копирования
        /// элементов коллекции, т.к. вставка в конец массива производится
        /// быстрее всего.
        /// </summary>
        /// <param name="attrName"></param>
        public void AttachAttribute(AttributeName attrName)
        {
            this[attrName].AttachQuery();

            return;
        }
        public void AttachAttributeImpl(AttributeName attrName)
        {
            ITupleObjectAttributeInfo plugged = this[attrName];

            PlugIn(attrName);
            plugged.UndoQuery();

            return;
        }

        public void DetachAttribute(AttributeName attrName)
        {
            this[attrName].DetachQuery();

            return;
        }

        public void DetachAttributeImpl(AttributeName attrName)
        {
            ITupleObjectAttributeInfo unplugged = this[attrName];

            Unplug(attrName);
            unplugged.UndoQuery();

            return;
        }

        public void EndInitialize()
        {
            int i;
            ITupleObjectAttributeInfo attrInfo;
            Action removeHandler = null;

            foreach (AttributeName attrName in this.Keys)
            {
                attrInfo = this[attrName];
                if (attrInfo.Query == AttributeQuery.Removed)
                {
                    removeHandler += () => RemoveAttributeImpl(attrName);
                }
            }

            removeHandler?.Invoke();

            foreach (AttributeName attrName in this.Keys)
            {
                attrInfo = this[attrName];
                if (attrInfo.Query == AttributeQuery.Attached)
                {
                    AttachAttributeImpl(attrName);
                }
            }

            i = 0;
            foreach (AttributeName attrName in this.Keys)
            {
                attrInfo = this[attrName];
                if (attrInfo.Query == AttributeQuery.Detached)
                {
                    DetachAttributeImpl(attrName);
                }
                else if (_pluggedAttributes.ContainsKey(attrName))
                {
                    _pluggedAttributes[attrName] = i++;
                }
            }

            _pluggedAttributeNames = new Lazy<AttributeName[]>(
                InitPluggedAttributeNames);

            return;

            AttributeName[] InitPluggedAttributeNames()
            {
                AttributeName[] pluggedAttributeNames =
                    new AttributeName[PluggedAttributesCount];
                int j = 0;

                foreach (AttributeName attrName in _pluggedAttributes.Keys)
                {
                    pluggedAttributeNames[j++] = attrName;
                }

                return pluggedAttributeNames;
            }
        }

        public ITupleObjectAttributeInfo GetPluggedO1(AttributeName attrName)
        {
            if (!IsPlugged(attrName)) return null;

            return this[attrName];
        }

        public IAttributeContainer Clone()
        {
            return new DictionaryBackedAttributeContainer(
                this,
                _pluggedAttributes);
        }
    }
}
