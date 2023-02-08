using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.AlgebraicTupleInfrastructure
{
    public sealed class AlgebraicTupleSchema<TEntity>
        : IAlgebraicTupleSchemaProvider
    {
        private Dictionary<string, AttributeInfo> _attributes;

        AttributeInfo? IAlgebraicTupleSchemaProvider.this[string attributeName]
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

        public AlgebraicTupleSchema()
        {
            _attributes = new Dictionary<string, AttributeInfo>();
        }

        private AlgebraicTupleSchema(Dictionary<string, AttributeInfo> attributes)
        {
            _attributes = attributes;
        }

        public bool ContainsAttribute(string attributeName)
        {
            return _attributes.ContainsKey(attributeName);
        }

        public void AddAttribute<TDomainEntity>(
            string attributeName, AttributeDomain<TDomainEntity> attribute = null)
        {
            AttributeInfo attributeInfo = AttributeInfo.Construct(!(attribute is null), attribute);
            _attributes.Add(attributeName, attributeInfo);
        }

        public void RemoveAttribute(string attributeName)
        {
            _attributes.Remove(attributeName);
        }

        public AlgebraicTupleSchema<TEntity> Clone()
        {
            Dictionary<string, AttributeInfo> attributes =
                _attributes.Select(kvp => kvp).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return new AlgebraicTupleSchema<TEntity>(attributes);
        }

        public void GeneralizeWith(AlgebraicTupleSchema<TEntity> second)
        {
            AttributeInfo attributeInfo1, attributeInfo2;
            foreach (string attributeName in _attributes.Keys)
            {
                (attributeInfo1, attributeInfo2) = (this[attributeName], second[attributeName]);
                if (attributeInfo1 != attributeInfo2)
                    (this[attributeName], second[attributeName]) = (attributeInfo1.PlugIn(), attributeInfo2.PlugIn());
            }
        }

        private void ConnectAttribute(string attributeName)
        {
            _attributes[attributeName] = _attributes[attributeName].PlugIn();
        }

        private void DisconnectAttribute(string attributeName)
        {
            _attributes[attributeName] = _attributes[attributeName].UnPlug();
        }

        public static bool operator +(AlgebraicTupleSchema<TEntity> schema, string attributeName)
        {
            schema.ConnectAttribute(attributeName);
            return true;
        }

        public static bool operator -(AlgebraicTupleSchema<TEntity> schema, string attributeName)
        {
            schema.DisconnectAttribute(attributeName);
            return true;
        }
    }
}
