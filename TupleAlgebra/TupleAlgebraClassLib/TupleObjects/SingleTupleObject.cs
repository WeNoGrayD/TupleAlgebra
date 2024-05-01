using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;

    public interface ISingleTupleObject
    {
        public IAttributeComponent this[AttributeName attrName] { get; set; }

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
        }

        public IAttributeComponent this[AttributeName attrName]
        {
            get => _components[Schema.GetAttributeLoc(attrName)];
            set => _components[Schema.GetAttributeLoc(attrName)] = value;
        }

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
