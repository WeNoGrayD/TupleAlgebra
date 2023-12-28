using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    /// <summary>
    /// Объект алгебры кортежей, представляющий собой один терм.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class SingleTupleObject<TEntity> : TupleObject<TEntity>
        where TEntity : new()
    {
        protected IAttributeComponent[] _components;

        public IAttributeComponent this[int attrLoc]
        {
            get => _components[attrLoc];
        }

        public IAttributeComponent this[AttributeInfo attrInfo]
        {
            get => _components[Schema.GetAttributeLoc(attrInfo)];
        }

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public SingleTupleObject(Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
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
            Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
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
            Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
            : this(new TupleObjectBuilder<TEntity>(schema), onTupleBuilding)
        {
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

        protected bool ContainsSpecificAttributeComponent(Func<IAttributeComponent, bool> gotcha)
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
            return ContainsSpecificAttributeComponent(component => component.Power.EqualsZero());
        }

        public bool ContainsFullAttributeComponent()
        {
            return ContainsSpecificAttributeComponent(component => component.Power.EqualsContinuum());
        }

        public void InitAttributes(IDictionary<AttributeName, IAlgebraicSetObject> components)
        {
            //_components = components;

            return;
        }

        protected IAlgebraicSetObject GetDefaultFictionalAttributeComponent(AttributeInfo attribute)
        {
            System.Reflection.MethodInfo getDefault = typeof(SingleTupleObject<TEntity>)
                .GetMethod(nameof(GetDefaultFictionalAttributeComponentImpl), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(attribute.DomainDataType);

            return getDefault.Invoke(this, new object[] { attribute }) as IAlgebraicSetObject;
        }

        protected abstract AttributeComponent<TData> GetDefaultFictionalAttributeComponentImpl<TData>(AttributeInfo attribute);


        #endregion

        #region IDisposable implementation

        protected override void DisposeImpl()
        {
            Schema.AttributeChanged -= SchemaAttributeChanged;

            return;
        }

        #endregion
    }
}
