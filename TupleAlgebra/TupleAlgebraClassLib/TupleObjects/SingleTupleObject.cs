using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    public abstract class SingleTupleObject<TEntity> : TupleObject<TEntity>
    {
        private IDictionary<AttributeName, IAlgebraicSetObject> _components =
            new Dictionary<AttributeName, IAlgebraicSetObject>();

        public IAlgebraicSetObject this[AttributeName name]
        {
            get => _components[name];
            set => _components[name] = value;
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
            Schema.AttributeChanged += SchemaAttributeChanged;

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

        public bool Contains<TAttributeComponent>()
            where TAttributeComponent : IAlgebraicSetObject
        {
            return _components.Values.Any(component => component is TAttributeComponent);
        }

        public bool ContainsEmptyAttributeComponent()
        {
            return false;// return _components.Values.Any(component => component.IsEmpty());
        }

        public bool ContainsFullAttributeComponent()
        {
            return false;// return _components.Values.Any(component => component.IsFull());
        }

        public void InitAttributes(IDictionary<AttributeName, IAlgebraicSetObject> components)
        {
            _components = components;

            return;
        }

        protected void SchemaAttributeChanged(object sender, AttributeChangedEventArgs eventArgs)
        {
            switch (eventArgs.ChangingEvent)
            {
                case AttributeChangedEventArgs.Event.Attachment:
                    {
                        _components.Add(
                            eventArgs.AttributeName,
                            GetDefaultFictionalAttributeComponent(eventArgs.Attribute));

                        break;
                    }
                case AttributeChangedEventArgs.Event.Detachment:
                case AttributeChangedEventArgs.Event.Deletion:
                    {
                        _components.Remove(eventArgs.AttributeName);

                        break;
                    }
            }

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
