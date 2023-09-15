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
    public sealed class TupleObjectSchema<TEntity>
        : IEnumerable<AttributeInfo>,
          ITupleObjectSchemaProvider
    {
        #region Instance fields

        private Dictionary<AttributeName, AttributeInfo> _attributes;

        /*
        /// <summary>
        /// Построитель кортежа.
        /// </summary>
        public readonly TupleObjectBuilder<TEntity> Builder;
        */

        #endregion

        #region Instance properties

        public IDictionary<AttributeName, AttributeInfo> Attributes => _attributes;

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

        public TupleObjectSchema(
            //TupleObjectBuilder<TEntity> builder, 
            Dictionary<AttributeName, AttributeInfo> attributes = null)
        {
            _attributes = attributes ?? new Dictionary<AttributeName, AttributeInfo>();
            //Builder = builder;
        }

        #endregion

        #region Static methods

        public static TupleObjectSchema<TEntity> Create(TupleObjectBuilder<TEntity> builder)
        {
            return new TupleObjectSchema<TEntity>();//builder);
        }

        #endregion

        #region Instance methods

        public TupleObjectSchema<TEntity> Clone(TupleObjectBuilder<TEntity> builder)
        {
            Dictionary<AttributeName, AttributeInfo> attributes =
                _attributes.Select(kvp => kvp).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return new TupleObjectSchema<TEntity>(attributes); //builder, attributes);
        }

        private void OnAttributeChanged(AttributeChangedEventArgs eventArgs)
        {
            AttributeChanged?.Invoke(this, eventArgs);

            return;
        }

        public IDictionary<AttributeName, IAlgebraicSetObject> InitAttributes()
        {
            Dictionary<AttributeName, IAlgebraicSetObject> components = new Dictionary<AttributeName, IAlgebraicSetObject>();

            foreach ((AttributeName attributeName, AttributeInfo attribute) in this.Attributes)
            {
                if (!attribute.IsPlugged) continue;

                components.Add(attributeName, null);
            }

            return components;
        }

        public bool ContainsAttribute(string attributeName)
        {
            return _attributes.ContainsKey(attributeName);
        }

        public void AddAttribute<TDomainEntity>(
            string attributeName, AttributeDomain<TDomainEntity> attribute = null)
        {
            /*
            AttributeInfo attributeInfo = AttributeInfo.Construct(
                isPlugged: attribute is not null,
                domain: attribute);
            _attributes.Add(attributeName, attributeInfo);
            */

            return;
        }

        public void RemoveAttribute(string attributeName)
        {
            _attributes.Remove(attributeName);
        }

        public void GeneralizeWith(TupleObjectSchema<TEntity> second)
        {
            AttributeInfo attributeInfo1, attributeInfo2;
            foreach (string attributeName in _attributes.Keys)
            {
                (attributeInfo1, attributeInfo2) = (this[attributeName], second[attributeName]);
                switch ((attributeInfo1.IsPlugged, attributeInfo2.IsPlugged))
                {
                    case (true, true):
                        {
                            break;
                        }
                    case (false, false):
                        {
                            break;
                        }
                    case (false, true):
                        {
                            this[attributeName] = attributeInfo1.PlugIn();

                            break;
                        }
                    case (true, false):
                        {
                            second[attributeName] = attributeInfo2.PlugIn();

                            break;
                        }
                }
            }
        }

        private void AttachAttribute(string attributeName)
        {
            AttributeInfo attached = _attributes[attributeName].PlugIn();
            _attributes[attributeName] = attached;
            OnAttributeChanged(new AttributeChangedEventArgs(
                attributeName, 
                attached, 
                AttributeChangedEventArgs.Event.Attachment));

            return;
        }

        private void DetachAttribute(string attributeName)
        {
            AttributeInfo detached = _attributes[attributeName].UnPlug();
            _attributes[attributeName] = detached;
            OnAttributeChanged(new AttributeChangedEventArgs(
                attributeName, 
                detached, 
                AttributeChangedEventArgs.Event.Detachment));

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

        public static TupleObjectSchema<TEntity> operator +(TupleObjectSchema<TEntity> schema, string attributeName)
        {
            schema.AttachAttribute(attributeName);

            return schema;
        }

        public static TupleObjectSchema<TEntity> operator -(TupleObjectSchema<TEntity> schema, string attributeName)
        {
            schema.DetachAttribute(attributeName);

            return schema;
        }

        #endregion
    }
}
