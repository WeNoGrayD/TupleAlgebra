using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraTests.ACTupleZipTests
{
    internal class ObjectSchema<TEntity>
    {
        #region Instance fields

        private IList<IEquivalenceClassBearer> _attributes;

        private bool _hashcodeCalculated = false;

        private int _hashcode;

        #endregion

        #region Static fields

        private static IAttributeInfo[] _attributeSchema;

        private static IDictionary<IAttributeInfo, int> _attributeIndeces;

        #endregion

        #region Static properties

        public static IAttributeInfo[] AttributeSchema { get => _attributeSchema; }

        public IEquivalenceClassBearer this [IAttributeInfo attribute]
        {
            get => _attributes[_attributeIndeces[attribute]];
        }

        #endregion

        public ObjectSchema(TEntity entity)
        {
            _attributes = new List<IEquivalenceClassBearer>();

            for (int i = 0; i < _attributeSchema.Length; i++)
            {
                _attributes.Add(_attributeSchema[i].ReturnECBearer());
                _attributeSchema[i].SetAttributeTo(entity, _attributes[i]);
            }

            return;
        }

        public ObjectSchema(
            TEntity entity, 
            IAttributeInfo exceptAttribute,
            out IEquivalenceClassBearer equClassBearer) 
            : this(entity)
        {
            equClassBearer = this[exceptAttribute];
            RemoveAttribute(exceptAttribute);

            return;
        }

        public static void InitAttributeSchema(IAttributeInfo[] attributeSchema)
        {
            _attributeSchema = attributeSchema;
            _attributeIndeces = new Dictionary<IAttributeInfo, int>();

            // Сохранение индексов, соответствующих атрибутам в списке.
            for (int i = 0; i < attributeSchema.Length; i++)
                _attributeIndeces.Add(attributeSchema[i], i);

            return;
        }

        public void RemoveAttribute(IAttributeInfo ai)
        {
            _attributes[_attributeIndeces[ai]] = null!;

            return;
        }

        public void AddAttribute(IAttributeInfo ai, IEquivalenceClassBearer equClassBearer)
        {
            _attributes[_attributeIndeces[ai]] = equClassBearer;

            return;
        }

        public override bool Equals(object? obj)
        {
            ObjectSchema<TEntity> second = (obj as ObjectSchema<TEntity>)!;
            int j = _attributes.Count >> 1, // Индекс с середины цикла, движущийся влево.
                n = _attributes.Count - 1;
            bool equals = true;

            if ((n & 0b1) == 0 &&
                !(_attributes[j]?.Equals(second._attributes[j]) ?? true))
            {
                CaseNotEquals();
                return equals;
            }

            j--;

            for (int i = 0; i <= j; i++, j--)
            {
                if (!(_attributes[i]?.Equals(second._attributes[i]) ?? true)) goto NOT_EQUALS;
                if (!(_attributes[j]?.Equals(second._attributes[j]) ?? true)) goto NOT_EQUALS;
                if (!(_attributes[n - j]?.Equals(second._attributes[n - j]) ?? true)) goto NOT_EQUALS;
                if (!(_attributes[n - i]?.Equals(second._attributes[n - i]) ?? true)) goto NOT_EQUALS;

                continue;

            NOT_EQUALS:

                CaseNotEquals();

                break;
            }

            return equals;

            void CaseNotEquals()
            {
                equals = false;

                return;
            }
        }

        public override int GetHashCode()
        {
            //if (_hashcodeCalculated) return _hashcode;
            //_hashcodeCalculated = true;

            int hc = 0;

            for (int i = 0; i < _attributes.Count; i++)
            {
                if (_attributes[i] is null) continue;

                hc += _attributes[i].GetHashCode();
            }

            return _hashcode = hc;// % 9013;
        }
    }

    internal class ObjectSchemaGroup<TEntity> : IEnumerable<ObjectSchema<TEntity>>
    {
        private ObjectSchema<TEntity> _schemaPattern;

        private IAttributeInfo _attributeInfo;

        private HashSet<IEquivalenceClassBearer> _attributeValueSet;

        public int AttributeValueSetCount { get => _attributeValueSet.Count; }

        public ObjectSchemaGroup(
            ObjectSchema<TEntity> schemaPattern,
            IAttributeInfo attributeInfo)
        {
            _schemaPattern = schemaPattern;
            _attributeInfo = attributeInfo;

            return;
        }

        public void InitAttributeValueSet()
        {
            _attributeValueSet = new HashSet<IEquivalenceClassBearer>();

            return;
        }

        public void AddAttributeValue(IEquivalenceClassBearer attributeValue)
        {
            _attributeValueSet.Add(attributeValue);

            return;
        }

        public IEnumerator<ObjectSchema<TEntity>> GetEnumerator()
        {
            foreach (IEquivalenceClassBearer attributeValue in _attributeValueSet)
            {
                _schemaPattern.AddAttribute(_attributeInfo, attributeValue);
                yield return _schemaPattern;
            }

            _schemaPattern.RemoveAttribute(_attributeInfo);

            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //public override bool Equals(object? obj) => _schemaPattern.Equals((obj as ObjectSchemaGroup<TEntity>)._schemaPattern);

        public override bool Equals(object? obj) => obj.Equals(_schemaPattern);

        public override int GetHashCode() => _schemaPattern.GetHashCode();
    }

    internal class ObjectRectangle<TEntity>
    {
        public IEquivalenceClassBearer[] FixedAttributes { get; private set; }

        public HashSet<IEquivalenceClassBearer>[] FreeAttributes { get; private set; }

        public ObjectRectangle(int attributeSchemaLength)
        {
            FixedAttributes = new IEquivalenceClassBearer[attributeSchemaLength];
            FreeAttributes = new HashSet<IEquivalenceClassBearer>[attributeSchemaLength];
            for (int i = 0; i < attributeSchemaLength; i++)
                FreeAttributes[i] = new HashSet<IEquivalenceClassBearer>();

            return;
        }
    }

    internal class ObjectArchiver<TEntity>
    {
        private IDictionary<IEquivalenceClassBearer, HashSet<ObjectSchema<TEntity>>>[]
            _schemasByAttributes;

        public void RectangleArchiving(IEnumerable<TEntity> entitySet)
        {
            // Общая для типа сущности схема атрибутов.
            IAttributeInfo[] attributeSchema = ObjectSchema<TEntity>.AttributeSchema;
            int attributeSchemaLength = attributeSchema.Length;
            _schemasByAttributes = new Dictionary<IEquivalenceClassBearer, HashSet<ObjectSchema<TEntity>>>[attributeSchemaLength];
            IDictionary<IEquivalenceClassBearer, HashSet<ObjectSchema<TEntity>>> schemasByAttributeX;
            ObjectSchema<TEntity> currentEntitySchema;
            IEquivalenceClassBearer attrValue;
            HashSet<ObjectSchema<TEntity>> schemaGroup;

            for (int i = 0; i < attributeSchemaLength; i++)
                _schemasByAttributes[i] = new Dictionary<IEquivalenceClassBearer, HashSet<ObjectSchema<TEntity>>>();

            foreach (TEntity entity in entitySet)
            {
                currentEntitySchema = new ObjectSchema<TEntity>(entity);

                for (int i = 0; i < attributeSchemaLength; i++)
                {
                    schemasByAttributeX = _schemasByAttributes[i];
                    attrValue = currentEntitySchema[attributeSchema[i]];
                    if (!schemasByAttributeX.ContainsKey(attrValue))
                        schemasByAttributeX.Add(attrValue, new HashSet<ObjectSchema<TEntity>>());
                    schemaGroup = schemasByAttributeX[attrValue];
                    schemaGroup.Add(currentEntitySchema);
                }
            }

            return;
        }

        private IDictionary<ObjectSchema<TEntity>, ObjectRectangle<TEntity>> _rectangles;

        public void RectanglePartition()
        {


        }

        public IList<(IAttributeInfo Attribute, float Metric)> CalculateMetric(
            IEnumerable<TEntity> entitySet)
        {
            // Общая для типа сущности схема атрибутов.
            IAttributeInfo[] attributeSchema = ObjectSchema<TEntity>.AttributeSchema;
            // Массив словарей групп атрибутов, где
            // i = номер атрибута
            // key = схема сущности без i-того атрибута
            // value = множество значений i-того атрибута для сущностей со схемой key
            HashSet<ObjectSchemaGroup<TEntity>>[] attributeSchemasPartitions = 
                new HashSet<ObjectSchemaGroup<TEntity>>[attributeSchema.Length];
            // Список пар "атрибут - метрика его набора групп".
            List<(IAttributeInfo Attribute, float Metric)> attributesByMetric =
                new List<(IAttributeInfo Attribute, float Metric)>();
            IAttributeInfo currentAttribute;
            HashSet<ObjectSchemaGroup<TEntity>> currentAttributeGroupSet;
            ObjectSchemaGroup<TEntity> currentGroup;
            IDictionary<IAttributeInfo, HashSet<IEquivalenceClassBearer>> valuesOfAttributes =
                new Dictionary<IAttributeInfo, HashSet<IEquivalenceClassBearer>>();
            HashSet<IEquivalenceClassBearer> currentAttributeValueSet;

            for (int i = 0; i < attributeSchema.Length; i++)
            {
                currentAttribute = attributeSchema[i];
                // Создание набора групп для i-того атрибута.
                CreateGroupSet(i);
                CalcMetricForCurrentAttribute();
            }

            // Сортировка по метрике
            attributesByMetric.Sort((a1, a2) => a1.Metric.CompareTo(a2.Metric));

            return attributesByMetric;

            void CreateGroupSet(int attributeIndex)
            {
                currentAttributeGroupSet = new HashSet<ObjectSchemaGroup<TEntity>>();
                attributeSchemasPartitions[attributeIndex] = currentAttributeGroupSet;
                ObjectSchemaGroup<TEntity> currentGroupBuf;
                ObjectSchema<TEntity> schema;
                IEquivalenceClassBearer currentAttributeValue;
                currentAttributeValueSet = new HashSet<IEquivalenceClassBearer>();
                valuesOfAttributes.Add(currentAttribute, currentAttributeValueSet);

                foreach (TEntity entity in entitySet)
                {
                    schema = new ObjectSchema<TEntity>(entity, currentAttribute, out currentAttributeValue);
                    currentGroup = new ObjectSchemaGroup<TEntity>(schema, currentAttribute);
                    if (!currentAttributeGroupSet.TryGetValue(currentGroup, out currentGroupBuf!))
                    {
                        currentAttributeGroupSet.Add(currentGroup);
                        currentGroup.InitAttributeValueSet();
                    }
                    else
                        currentGroup = currentGroupBuf;

                    currentGroup.AddAttributeValue(currentAttributeValue);
                    currentAttributeValueSet.Add(currentAttributeValue);
                }

                return;
            }

            void CalcMetricForCurrentAttribute()
            {
                int summaryAttributeValuesCount = currentAttributeValueSet.Count,
                    attributeGroupsCount = currentAttributeGroupSet.Count,
                    attributeValuesCountByGroup = 0;

                foreach (ObjectSchemaGroup<TEntity> group
                     in currentAttributeGroupSet)
                {
                    attributeValuesCountByGroup += group.AttributeValueSetCount;
                }

                float metric = ((float)attributeValuesCountByGroup) /
                    (attributeGroupsCount * summaryAttributeValuesCount);

                attributesByMetric.Add((currentAttribute, metric));

                return;
            }
        }
    }
}
