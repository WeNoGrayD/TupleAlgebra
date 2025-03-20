using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.AttributeContainers
{
    using static TupleObjectHelper;

    public class AttributeContainer
        : SortedList<AttributeName, ITupleObjectAttributeInfo>,
          IAttributeContainer
    {
        private AttributeComparer _comparer;

        private IDictionary<AttributeName, int> _pluggedAttributes;

        private Lazy<AttributeName[]> _pluggedAttributeNames;

        public AttributeName[] PluggedAttributeNames
        {
            get
            {
                return _pluggedAttributeNames.Value;
            }
        }

        public IEnumerable<ITupleObjectAttributeInfo> PluggedAttributes
        {
            get
            {
                for (int i = Count - PluggedAttributesCount; i < Count; i++)
                    yield return GetValueAtIndex(i);

                yield break;
            }
        }

        public int PluggedAttributesCount { get => _pluggedAttributes.Count; }

        public AttributeContainer()
            : base(new AttributeComparer())
        {
            Init();

            return;
        }

        public AttributeContainer(
            IDictionary<AttributeName, ITupleObjectAttributeInfo> source,
            IDictionary<AttributeName, int> pluggedAttributes)
            : base(new AttributeComparer())
        {
            Init(pluggedAttributes);

            foreach (var attr in source)
            {
                _comparer.targetIsPlugged = IsPlugged(attr.Key);
                AddAttribute(attr.Key, attr.Value);
            }

            return;
        }

        private void Init(
            IDictionary<AttributeName, int> pluggedAttributes = null)
        {
            _comparer = (Comparer as AttributeComparer)!;
            _comparer.container = this;
            if (pluggedAttributes is not null)
                _pluggedAttributes = new Dictionary<AttributeName, int>(pluggedAttributes);
            else
                _pluggedAttributes = new Dictionary<AttributeName, int>();
            _pluggedAttributeNames = new Lazy<AttributeName[]>(
                GetPluggedAttributeNames);

            return;

            IEnumerable<AttributeName> GetPluggedAttributeNamesIter()
            {
                for (int i = Count - PluggedAttributesCount; i < Count; i++)
                    yield return GetKeyAtIndex(i);

                yield break;
            }

            AttributeName[] GetPluggedAttributeNames()
            {
                return GetPluggedAttributeNamesIter().ToArray();
            }
        }

        public bool ContainsAttribute(AttributeName attributeName)
        {
            if (_comparer.targetIsPlugged = IsPlugged(attributeName))
                return true;

            return ContainsKey(attributeName);
        }

        private void PlugIn(AttributeName attrName)
        {
            _comparer.targetAttributeName = attrName;
            _comparer.targetIsPlugged = true;
            _pluggedAttributes.Add(attrName, -1);

            return;
        }

        private void Unplug(AttributeName attrName)
        {
            _comparer.targetAttributeName = attrName;
            _comparer.targetIsPlugged = false;
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
            _comparer.targetAttributeName = attrName;
            Add(attrName, attrInfo);

            return;
        }

        public void RemoveAttribute(AttributeName attrName)
        {
            _comparer.targetAttributeName = attrName;
            _comparer.targetIsPlugged = IsPlugged(attrName);
            this[attrName].RemoveQuery();

            return;
        }

        private void RemoveAttributeImpl(int attrLoc)
        {
            AttributeName attrName = GetKeyAtIndex(attrLoc);
            _comparer.targetAttributeName = attrName;
            _comparer.targetIsPlugged = IsPlugged(attrName);
            RemoveAt(attrLoc);
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
            _comparer.targetAttributeName = attrName;
            _comparer.targetIsPlugged = IsPlugged(attrName);
            this[attrName].AttachQuery();

            return;
        }
        public void AttachAttributeImpl(int attrLoc)
        {
            AttributeName attrName = GetKeyAtIndex(attrLoc);
            ITupleObjectAttributeInfo plugged = GetValueAtIndex(attrLoc);

            RemoveAt(attrLoc);
            PlugIn(attrName);
            Add(attrName, plugged);
            plugged.UndoQuery();

            return;
        }

        public void DetachAttribute(AttributeName attrName)
        {
            _comparer.targetAttributeName = attrName;
            _comparer.targetIsPlugged = IsPlugged(attrName);
            this[attrName].DetachQuery();

            return;
        }

        public void DetachAttributeImpl(int attrLoc)
        {
            AttributeName attrName = GetKeyAtIndex(attrLoc);
            ITupleObjectAttributeInfo unplugged = GetValueAtIndex(attrLoc);

            RemoveAt(attrLoc);
            Unplug(attrName);
            Add(attrName, unplugged);
            unplugged.UndoQuery();

            return;
        }

        public void EndInitialize()
        {
            int i, attrLen = this.Count - 1, treshold;
            ITupleObjectAttributeInfo attrInfo;

            for (i = attrLen; i >= 0; i--)
            {
                attrInfo = this.GetValueAtIndex(i);
                if (attrInfo.Query == AttributeQuery.Removed)
                {
                    RemoveAttributeImpl(i);
                    attrLen--;
                }
            }

            treshold = attrLen - PluggedAttributesCount;

            for (i = 0; i <= treshold; i++)
            {
                attrInfo = this.GetValueAtIndex(i);
                if (attrInfo.Query == AttributeQuery.Attached)
                {
                    AttachAttributeImpl(i--);
                }
            }

            treshold++;

            for (i = treshold; i <= attrLen; i++)
            {
                attrInfo = this.GetValueAtIndex(i);
                if (attrInfo.Query == AttributeQuery.Detached)
                {
                    DetachAttributeImpl(i);
                }
                else
                {
                    _pluggedAttributes[GetKeyAtIndex(i)] = i - treshold;
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

                for (int k = Count - PluggedAttributesCount; k < Count; k++)
                {
                    pluggedAttributeNames[j++] = GetKeyAtIndex(k);
                }

                return pluggedAttributeNames;
            }
        }

        public ITupleObjectAttributeInfo GetPluggedO1(AttributeName attrName)
        {
            return GetValueAtIndex(GetAttributeLoc(attrName));
        }

        private class AttributeComparer : IComparer<AttributeName>
        {
            public AttributeContainer container;

            public AttributeName targetAttributeName;

            public bool targetIsPlugged = false;

            public AttributeComparer()
            {
                return;
            }

            public int Compare(AttributeName an1, AttributeName an2)
            {
                bool a2IsPlugged = targetIsPlugged,
                     a1IsPlugged = container.IsPlugged(an1);
                int cmpRes = a1IsPlugged.CompareTo(a2IsPlugged);

                if (cmpRes == 0)
                {
                    /*
                     * Внутри разделов unplugged и plugged
                     * имена атрибутов должны быть раздельно
                     * отсортированы.
                     */
                    return an1.CompareTo(an2);
                }

                /*
                 * Таблица истинности
                 *       | false | true |
                 * false |   0   |  -1  |
                 * true  |   1   |   0  |
                 */

                return cmpRes;
            }
        }

        public IAttributeContainer Clone()
        {
            return new AttributeContainer(
                this,
                _pluggedAttributes);
        }
    }
}
