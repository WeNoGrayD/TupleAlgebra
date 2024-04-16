using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public sealed class TupleObjectSchema<TEntity>
        : IEnumerable<AttributeInfo>,
          ITupleObjectSchemaProvider
    {
        #region Instance fields

        private IDictionary<AttributeName, AttributeInfo> _attributes;

        private IDictionary<AttributeInfo, int> _attributeLocations;

        /*
        /// <summary>
        /// Построитель кортежа.
        /// </summary>
        public readonly TupleObjectBuilder<TEntity> Builder;
        */

        private Lazy<EntityFactoryHandler<TEntity>> _entityFactory;

        #endregion

        #region Instance properties

        public IDictionary<AttributeName, AttributeInfo> Attributes => _attributes;

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

        AttributeInfo? ITupleObjectSchemaProvider.this[string attributeName]
        {
            get
            {
                return ContainsAttribute(attributeName) ? _attributes[attributeName] : (AttributeInfo?)null;
            }
            set
            {
                if (value is null)
                    RemoveAttribute(attributeName);
                else
                    _attributes[attributeName] = (AttributeInfo)value;
            }
        }

        public AttributeInfo this[string attributeName]
        {
            get => _attributes[attributeName];
            private set => _attributes[attributeName] = value;
        }

        #endregion

        #region Instance events

        public event EventHandler<AttributeChangedEventArgs> AttributeChanged;

        #endregion

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
        public TupleObjectSchema(
            IDictionary<AttributeName, AttributeInfo> attributes = null)
        {
            if (attributes is not null)
            {
                _attributes = attributes;
                InitAttributeLocations();
            }
            else
            {
                _attributes = new Dictionary<AttributeName, AttributeInfo>();
            }

            return;
        }

        #endregion

        #region Static methods


        #endregion

        #region Instance methods

        private EntityFactoryHandler<TEntity> MakeEntityFactoryBuilder()
        {
            System.Reflection.PropertyInfo[] attributeProperties = 
                _attributes.Values.Select(a => a.AttributeProperty).ToArray();
            return (new EntityFactoryBuilder()).Build<TEntity>(attributeProperties);
        }

        /// <summary>
        /// Инициализация массива индексов атрибутов.
        /// </summary>
        private void InitAttributeLocations()
        {
            int attrLoc = 0;
            _attributeLocations = new Dictionary<AttributeInfo, int>();
            
            foreach (AttributeInfo attrInfo in _attributes.Values)
            {
                _attributeLocations.Add(attrInfo, attrLoc++);
            }

            return;
        }

        public EntityFactoryHandler<TEntity> GetEntityFactory()
        {
            return null;
        }

        public TupleObjectSchema<TEntity> Clone()
        {
            IDictionary<AttributeName, AttributeInfo> attributes =
                _attributes.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return new TupleObjectSchema<TEntity>(attributes); 
        }

        private void OnAttributeChanged(AttributeChangedEventArgs eventArgs)
        {
            AttributeChanged?.Invoke(this, eventArgs);

            return;
        }

        public IDictionary<AttributeName, IAlgebraicSetObject> InitAttributes()
        {
            Dictionary<AttributeName, IAlgebraicSetObject> components = 
                new Dictionary<AttributeName, IAlgebraicSetObject>();

            foreach ((AttributeName attributeName, AttributeInfo attribute) in this.Attributes)
            {
                if (!attribute.IsPlugged) continue;

                components.Add(attributeName, null);
            }

            return components;
        }

        /// <summary>
        /// Получения индекса атрибута в последовательности компонент кортежа
        /// с данной схемой.
        /// </summary>
        /// <param name="attrInfo"></param>
        /// <returns></returns>
        public int GetAttributeLoc(AttributeInfo attrInfo) => _attributeLocations[attrInfo];

        public bool ContainsAttribute(string attributeName)
        {
            return _attributes.ContainsKey(attributeName);
        }

        public void AddAttribute<TDomainEntity>(
            string attributeName, AttributeDomain<TDomainEntity> attribute = null)
        {
            AttributeInfo attributeInfo = AttributeInfo.Construct<TEntity, TDomainEntity>(
                isPlugged: attribute is not null,
                domain: attribute);
            _attributes.Add(attributeName, attributeInfo);

            return;
        }

        public void RemoveAttribute(string attributeName)
        {
            _attributes.Remove(attributeName);
        }

        public TupleObjectSchema<TEntity> GeneralizeWith(TupleObjectSchema<TEntity> second)
        {
            TupleObjectSchema<TEntity> resultSchema = 
                this.Clone();
            AttributeInfo attributeInfo1, attributeInfo2;

            foreach (string attributeName in _attributes.Keys)
            {
                (attributeInfo1, attributeInfo2) = (this[attributeName], second[attributeName]);
                switch ((attributeInfo1.IsPlugged, attributeInfo2.IsPlugged))
                {
                    case (true, true):
                    case (true, false):
                        {
                            break;
                        }
                    case (_, _):
                        {
                            resultSchema.AttachAttribute(attributeName);

                            break;
                        }
                }
            }

            resultSchema.EndInitializingAttributes();

            return resultSchema;
        }

        /// <summary>
        /// Выполнение запросов над атрибутами, то есть полноценное прикрепление и открепление
        /// атрибутов для обязательно новой схемы.
        /// </summary>
        public void EndInitializingAttributes()
        {
            foreach (string attributeName in _attributes.Keys)
            {
                _attributes[attributeName] = _attributes[attributeName].ExecuteQuery();
            }

            return;
        }

        public void AttachAttribute(string attributeName)
        {
            _attributes[attributeName] = _attributes[attributeName].Attach();
            /*
            AttributeInfo attached = _attributes[attributeName].Attach();
            _attributes[attributeName] = attached;
            OnAttributeChanged(new AttributeChangedEventArgs(
                attributeName, 
                attached, 
                AttributeChangedEventArgs.Event.Attachment));
            */

            return;
        }

        public void DetachAttribute(string attributeName)
        {
            _attributes[attributeName] = _attributes[attributeName].Detach();
            /*
            AttributeInfo detached = _attributes[attributeName].Detach();
            _attributes[attributeName] = detached;
            OnAttributeChanged(new AttributeChangedEventArgs(
                attributeName, 
                detached, 
                AttributeChangedEventArgs.Event.Detachment));
            */

            return;
        }

        #endregion

        #region IEnumerable<AttributeInfo> implementation

        public IEnumerator<AttributeInfo> GetEnumerator()
        {
            return _attributes.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

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
    }
}
