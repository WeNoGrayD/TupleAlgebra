using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
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

    public sealed class TupleObjectSchema<TEntity>
        : ITupleObjectSchemaProvider
    {
        #region Instance fields

        private IAttributeContainer _attributes;

        private Lazy<EntityFactoryHandler<TEntity>> _entityFactory;

        #endregion

        #region Instance properties

        public IAttributeContainer Attributes => _attributes;

        public IEnumerable<AttributeName> PluggedAttributeNames
        {
            get => _attributes.PluggedAttributeNames;
        }

        public int Count { get => _attributes.Count; }

        public int PluggedAttributesCount 
        { get => _attributes.PluggedAttributesCount; }

        public IEnumerable<ITupleObjectAttributeInfo> PluggedAttributes
        {
            get => _attributes.PluggedAttributes;
        }

        public static bool IsEntityPrimitive { get; private set; }

        public EntityFactoryHandler<TEntity> EntityFactory 
        {
            get
            {
                return _entityFactory.Value;
            }
        }

        #endregion

        #region Indexers

        public ITupleObjectAttributeInfo this[AttributeName attributeName]
        {
            get
            {
                return ContainsAttribute(attributeName) ?
                    _attributes[attributeName] : 
                    null;
            }
            set
            {
                if (ContainsAttribute(attributeName))
                    _attributes[attributeName] = value;
                else
                    _attributes.AddAttribute(attributeName, value);
                return;
            }
        }

        #endregion

        /*
        #region Instance events

        public event EventHandler<AttributeChangedEventArgs> AttributeChanged;

        #endregion
        */

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static TupleObjectSchema()
        {
            IsEntityPrimitive = typeof(TEntity).IsPrimitive;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="attributes"></param>
        public TupleObjectSchema(IAttributeContainer attributes = null)
        {
            if (attributes is not null)
            {
                _attributes = attributes.Clone();
            }
            else
            {
                _attributes = new AttributeContainer();
            }
            _entityFactory = new Lazy<EntityFactoryHandler<TEntity>>(MakeEntityFactoryBuilder); 

            return;
        }

        #endregion

        #region Static methods

        #endregion

        #region Instance methods

        private EntityFactoryHandler<TEntity> MakeEntityFactoryBuilder()
        {
            System.Reflection.MemberInfo[] attributeProperties = 
                _attributes.PluggedAttributes.Select(a => a.AttributeMember).ToArray();
            return (new EntityFactoryBuilder()).Build<TEntity>(attributeProperties);
        }

        public TupleObjectSchema<TEntity> Clone()
        {
            return new TupleObjectSchema<TEntity>(_attributes);
        }

        /*
        private void OnAttributeChanged(AttributeChangedEventArgs eventArgs)
        {
            AttributeChanged?.Invoke(this, eventArgs);

            return;
        }
        */

        public AttributeName AttributeAt(
            int attrPtr)
        {
            return _attributes.PluggedAttributeNames[attrPtr];
        }

        public bool IsPlugged(AttributeName attrName)
        {
            return _attributes.IsPlugged(attrName);
        }

        public void EndInit()
        {
            _attributes.EndInitialize();

            return;
        }

        /// <summary>
        /// Получения индекса атрибута в последовательности компонент кортежа
        /// с данной схемой.
        /// </summary>
        /// <param name="attrInfo"></param>
        /// <returns></returns>
        public int GetAttributeLoc(AttributeName attrName)
        {
            return _attributes.GetAttributeLoc(attrName);
        }

        public bool ContainsAttribute(AttributeName attributeName)
        {
            return _attributes.ContainsAttribute(attributeName);
        }

        public void AddAttribute<TAttribute>(
            Expression<AttributeGetterHandler<TEntity, TAttribute>>
                attributeGetterExpr,
            AttributeName attributeName)
        {
            ITupleObjectAttributeInfo attributeInfo = 
                new AttributeInfo<TEntity, TAttribute>(
                    attributeGetterExpr);
            _attributes.AddAttribute(attributeName, attributeInfo);

            return;
        }

        public void RemoveAttribute(AttributeName attributeName)
        {
            _attributes.RemoveAttribute(attributeName);

            return;
        }

        /// <summary>
        /// Применяется только во время инициализации схемы.
        /// </summary>
        /// <param name="attributeName"></param>
        public void AttachAttribute(AttributeName attributeName)
        {
            _attributes.AttachAttribute(attributeName);

            return;
        }

        /// <summary>
        /// Применяется только во время инициализации схемы.
        /// </summary>
        /// <param name="attributeName"></param>
        public void DetachAttribute(AttributeName attributeName)
        {
            _attributes.DetachAttribute(attributeName);

            return;
        }

        private ITupleObjectAttributeInfo GeneralizeAttributes(
            AttributeName attrName,
            IAttributeContainer secondAttributes,
            out bool gotFirst,
            out bool gotSecond)
        {
            bool secondIsPlugged = secondAttributes.IsPlugged(attrName);

            (ITupleObjectAttributeInfo res, gotFirst, gotSecond) =
                (_attributes.IsPlugged(attrName), secondIsPlugged) 
                switch
                {
                    (true, _) => (_attributes.GetPluggedO1(attrName), true, secondIsPlugged),
                    (false, false) => (_attributes[attrName], true, true),
                    _ => (secondAttributes.GetPluggedO1(attrName), false, true)
                };

            return res;
        }

        public TupleObjectBuildingHandler<TEntity> GeneralizeWith(
            TupleObjectSchema<TEntity> second)
        {
            return (this.Equals(second)) ? 
                (builder) => builder.Schema = this : 
                GeneralizeWithImpl;

            void GeneralizeWithImpl(TupleObjectBuilder<TEntity> builder)
            {
                /*
                 * Совершается попытка выдать обобщённую схему без копирования
                 * и создания новой.
                 */

                TupleObjectSchema<TEntity> resultSchema = null;
                bool thisIsGeneral = true, secondIsGeneral = true,
                     thisIsGeneralBuf = true, secondIsGeneralBuf = true,
                     hasGeneral = true;
                ITupleObjectAttributeInfo currentAttribute = null;
                IAttributeContainer secondAttributes = second._attributes;

                foreach (AttributeName attributeName in _attributes.Keys)
                {
                    currentAttribute = GeneralizeAttributes(
                        attributeName,
                        secondAttributes,
                        out thisIsGeneralBuf,
                        out secondIsGeneralBuf);

                    if (hasGeneral)
                    {
                        thisIsGeneralBuf &= thisIsGeneral;
                        secondIsGeneralBuf &= secondIsGeneral;
                        hasGeneral &= HasGeneralOnCurrentStep();
                    }

                    if (!hasGeneral)
                    {
                        if (resultSchema is null)
                        {
                            InitGeneralSchema();
                        }
                        resultSchema[attributeName] = currentAttribute;
                    }
                    else
                    {
                        thisIsGeneral = thisIsGeneralBuf;
                        secondIsGeneral = secondIsGeneralBuf;
                    }
                }

                if (hasGeneral)
                {
                    if (thisIsGeneral) resultSchema = this;
                    else resultSchema = second;
                }

                builder.Schema = resultSchema;

                return;

                void InitGeneralSchema()
                {
                    if (thisIsGeneral) resultSchema = this.Clone();
                    else resultSchema = second.Clone();
                }

                bool HasGeneralOnCurrentStep() =>
                    (thisIsGeneralBuf || secondIsGeneralBuf);
            }
        }

        #region IEnumerable<AttributeInfo> implementation

        public IEnumerator<ITupleObjectAttributeInfo> GetEnumerator()
        {
            return _attributes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /*
        #region Operators

        /// <summary>
        /// Оператор прикрепления атрибута.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static TupleObjectSchema<TEntity> operator +(
            TupleObjectSchema<TEntity> schema, 
            string attributeName)
        {
            schema.AttachAttribute(attributeName);

            return schema;
        }

        /// <summary>
        /// Оператор открепления атрибута.
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static TupleObjectSchema<TEntity> operator -(
            TupleObjectSchema<TEntity> schema, 
            string attributeName)
        {
            schema.DetachAttribute(attributeName);

            return schema;
        }

        #endregion
        */

        #endregion
    }
}
