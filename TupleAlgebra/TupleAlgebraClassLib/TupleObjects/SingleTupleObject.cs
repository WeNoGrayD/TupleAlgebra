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

    public interface ISingleTupleObject
    {
        public IAttributeComponent this[int attrPtr] { get; set; }

        public IAttributeComponent this[AttributeName attrName] { get; set; }

        public int RowLength { get; }

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

        public IAttributeComponent this[AttributeName attrName]
        {
            get => _components[Schema.GetAttributeLoc(attrName)];
            set => _components[Schema.GetAttributeLoc(attrName)] = value;
        }

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

        public override TupleObject<TEntity> AlignWithSchema(
            TupleObjectSchema<TEntity> schema,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
        {
            if (object.ReferenceEquals(Schema, schema)) return this;

            builder = builder ?? factory.GetBuilder<TEntity>();

            int len = schema.PluggedAttributesCount;
            IndexedComponentFactoryArgs<IAttributeComponent>[] components =
                new IndexedComponentFactoryArgs<IAttributeComponent>[len];

            int attrLoc = 0;
            for (attrLoc = 0; attrLoc < len; attrLoc++)
            {
                components[attrLoc] = new();
            }
            for (int i = 0; i < Schema.PluggedAttributesCount; i++)
            {
                attrLoc = schema.GetAttributeLoc(Schema.AttributeAt(i));
                components[attrLoc] = new(attrLoc, builder, this[i]);
            }

            return Reproduce(components, factory, schema.PassToBuilder, builder);
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
            return ContainsSpecificAttributeComponent(component => component.IsEmpty());
        }

        public bool ContainsFullAttributeComponent()
        {
            return ContainsSpecificAttributeComponent(component => component.IsFull());
        }

        public void InitAttributes(IDictionary<AttributeName, IAlgebraicSetObject> components)
        {
            //_components = components;

            return;
        }

        protected IAlgebraicSetObject GetDefaultFictionalAttributeComponent<TAttributeInfo>(
            ITupleObjectAttributeInfo attribute)
        {
            /*
            System.Reflection.MethodInfo getDefault = typeof(SingleTupleObject<TEntity>)
                .GetMethod(nameof(GetDefaultFictionalAttributeComponentImpl), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(attribute.DomainDataType);
            */
            return null;
            //return getDefault.Invoke(this, new object[] { attribute }) as IAlgebraicSetObject;
        }

        public abstract IAttributeComponent<TAttribute> 
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory);

        public abstract TupleObject<TEntity> Reproduce(
            IEnumerable<IndexedComponentFactoryArgs<IAttributeComponent>> components,
            TupleObjectFactory factory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder);

        public abstract TupleObject<TEntity> ToAlternateDiagonal(
            TupleObjectFactory factory);


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
