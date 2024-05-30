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
using TupleAlgebraClassLib.TupleObjectInfrastructure.AttributeContainers;
using UniversalClassLib;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public sealed class TupleObjectSchema<TEntity>
        : ITupleObjectSchemaProvider
    {
        #region Instance fields

        private IAttributeContainer _attributes;

        private Lazy<EntityFactoryHandler<TEntity>> _entityFactory;

        private ITupleObjectAttributeSetupWizard[] _setupWizards;

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
                _attributes = new DictionaryBackedAttributeContainer();
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

        public ITupleObjectAttributeSetupWizard GetSetupWizard(AttributeName attrName)
        {
            return _setupWizards[GetAttributeLoc(attrName)];
        }

        public ITupleObjectAttributeSetupWizard GetSetupWizard(int attrLoc)
        {
            return _setupWizards[attrLoc];
        }

        public bool IsPlugged(AttributeName attrName)
        {
            return _attributes.IsPlugged(attrName);
        }

        public void EndInit()
        {
            _attributes.EndInitialize();

            int len = PluggedAttributesCount;
            AttributeName attrName;
            _setupWizards = new ITupleObjectAttributeSetupWizard[len];
            for (int attrPtr = 0; attrPtr < len; attrPtr++)
            {
                attrName = AttributeAt(attrPtr);
                _setupWizards[attrPtr] =
                    _attributes[attrName].SetupWizardFactory(this, attrName);
            }

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

        public void PassToBuilder(TupleObjectBuilder<TEntity> builder)
        {
            builder.Schema = this;
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

        public TupleObjectSchema<TEntity> GeneralizeWith(
            TupleObjectSchema<TEntity> second)
        {
            if (this.Equals(second)) return this;

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
            else
                resultSchema.EndInit();

            return resultSchema;

            void InitGeneralSchema()
            {
                if (thisIsGeneral) resultSchema = this.Clone();
                else resultSchema = second.Clone();
            }

            bool HasGeneralOnCurrentStep() =>
                (thisIsGeneralBuf || secondIsGeneralBuf);
        }

        public TupleObjectBuildingHandler<TEntity> GeneralizationHandler(
            TupleObjectSchema<TEntity> second)
        {
            return (this.Equals(second)) ? 
                PassToBuilder : 
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
