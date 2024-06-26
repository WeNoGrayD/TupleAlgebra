﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;

    public interface ISingleTupleObject<TAttributeComponent>
        : ITupleObject, IDisposable
    {
        public TAttributeComponent this[int attrPtr] { get; set; }

        public TAttributeComponent this[AttributeName attrName]
        {
            get => this[Schema.GetAttributeLoc(attrName)];
            set => this[Schema.GetAttributeLoc(attrName)] = value;
        }

        public int RowLength { get; }

        public bool IsDefault(int attrLoc);
    }

    public interface ISingleTupleObject
        : ISingleTupleObject<IAttributeComponent>
    {
        public bool IsDefault(IAttributeComponent component);

        public IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory);
    }

    /// <summary>
    /// Объект алгебры кортежей, представляющий собой один терм.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class SingleTupleObject<TEntity> : 
        TupleObject<TEntity>, ISingleTupleObject
        where TEntity : new()
    {
        protected IAttributeComponent[] _components;

        public IAttributeComponent this[int attrLoc]
        {
            get => _components[attrLoc];
            set => _components[attrLoc] = value;
        }

        ITupleObjectSchemaProvider ITupleObject.Schema { get => Schema; }

        public int RowLength { get => _components.Length; }

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public SingleTupleObject(TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : this(new TupleObjectBuilder<TEntity>(), onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected SingleTupleObject(
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(builder, onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected SingleTupleObject(
            TupleObjectSchema<TEntity> schema,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : this(new TupleObjectBuilder<TEntity>(schema), onTupleBuilding)
        {
            return;
        }

        protected SingleTupleObject(TupleObjectSchema<TEntity> schema)
            : base(schema)
        {
            ITupleObjectSchemaProvider schemaProvider = schema;
            _components = new IAttributeComponent[schemaProvider.PluggedAttributeNames.Count()];

            return;
        }

        #endregion

        #region Instance methods

        public bool IsDefault(int attrLoc) => IsDefault(this[attrLoc]);

        public abstract bool IsDefault(IAttributeComponent component);

        public override TupleObject<TEntity> AlignWithSchema(
            TupleObjectSchema<TEntity> newSchema,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
        {
            TupleObjectSchema<TEntity> oldSchema = Schema;
            if (object.ReferenceEquals(oldSchema, newSchema)) return this;

            builder = builder ?? factory.GetBuilder<TEntity>();

            return Reproduce(
                GetFactoryArgs(), 
                factory, 
                newSchema.PassToBuilder,
                builder);

            IEnumerable< IndexedComponentFactoryArgs<IAttributeComponent>> 
                GetFactoryArgs()
            {
                int oldAttrLoc, newAttrLoc;
                foreach (AttributeName attrName in newSchema.PluggedAttributeNames)
                {
                    if (oldSchema.IsPlugged(attrName))
                    {
                        oldAttrLoc = oldSchema.GetAttributeLoc(attrName);
                        newAttrLoc = newSchema.GetAttributeLoc(attrName);
                        yield return new(newAttrLoc, builder, this[oldAttrLoc]);
                    }
                }

                yield break;
            }
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override bool IsFull()
        {
            return false;
        }

        /// <summary>
        /// Кортеж непустой, если имеется хотя бы одна неленивая компонента
        /// либо ленивая непустая компонента.
        /// </summary>
        /// <returns></returns>
        public override bool IsFalse()
        {
            for (int attrLoc = 0; attrLoc < _components.Length; attrLoc++)
            {
                if (!this[attrLoc].IsFalse()) return false;
            }

            return true;
        }

        /// <summary>
        /// Сортировка массива компонент атрибутов кортежа в зависимости от задачи.
        /// </summary>
        private void RearrangeAttributeComponents()
        {


            return;
        }

        protected bool ContainsSpecificAttributeComponent
            (Func<IAttributeComponent, bool> gotcha)
        {
            bool containsSpecific = false;
            IAttributeComponent component;

            for (int i = 0; i < _components.Length; i++)
            {
                if ((component = _components[i]) is null || !gotcha(component)) continue;

                containsSpecific = true;
                break;
            }

            return containsSpecific;
        }

        public bool Contains<TAttributeComponent>()
            where TAttributeComponent : IAttributeComponent
        {
            return ContainsSpecificAttributeComponent(component => component is TAttributeComponent);
        }

        public bool ContainsEmptyAttributeComponent()
        {
            return ContainsSpecificAttributeComponent(component => component.IsEmpty);
        }

        public bool ContainsFullAttributeComponent()
        {
            return ContainsSpecificAttributeComponent(component => component.IsFull);
        }

        public void InitAttributes(IDictionary<AttributeName, IAlgebraicSetObject> components)
        {
            //_components = components;

            return;
        }

        public abstract IAttributeComponent<TAttribute> 
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory);

        public abstract TupleObject<TEntity> Reproduce(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> components,
            TupleObjectFactory factory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder);

        #endregion

        #region IDisposable implementation

        protected override void DisposeImpl()
        {
            //Schema.AttributeChanged -= SchemaAttributeChanged;

            return;
        }

        #endregion
    }
}
