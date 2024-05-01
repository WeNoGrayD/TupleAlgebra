using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Reflection;
using System.Globalization;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;

    /// <summary>
    /// Кортеж данных о сущностях определённого типа.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class TupleObject<TEntity>
        : IQueryable<TEntity>,
          IDisposable
        where TEntity : new()
    {
        #region Instance fields

        private bool _isDisposed = false;

        #endregion

        #region Instance properties

        public virtual Expression Expression { get; protected set; }

        public virtual Type ElementType { get => typeof(TEntity); }

        public virtual IQueryProvider Provider { get; protected set; }

        /// <summary>
        /// Схема кортежа. Содержит данные 
        /// </summary>
        public TupleObjectSchema<TEntity> Schema { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static TupleObject()
        {
            //TupleObjectBuilder<TEntity>.Init();
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public TupleObject(TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : this(new TupleObjectBuilder<TEntity>(), onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected TupleObject(
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
        {
            if (onTupleBuilding is not null) onTupleBuilding(builder);
            Schema = builder.Schema;
            //Schema.AttributeChanged += SchemaAttributeChanged;

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected TupleObject(
            TupleObjectSchema<TEntity> schema,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : this(new TupleObjectBuilder<TEntity>(schema), onTupleBuilding)
        {
            return;
        }

        protected TupleObject(TupleObjectSchema<TEntity> schema)
        {
            Schema = schema;

            return;
        }

        #endregion

        #region Static methods

        public static void Configure(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
        {
            onTupleBuilding += EndSchemaInitialization;
            onTupleBuilding(TupleObjectBuilder<TEntity>.StaticBuilder);

            return;
        }

        public static void EndSchemaInitialization(
            TupleObjectBuilder<TEntity> builder)
        {
            builder.Schema.EndInit();

            return;
        }

        #endregion

        #region Instance methods

        /*
        protected void SchemaAttributeChanged(object sender, AttributeChangedEventArgs eventArgs)
        {
            switch (eventArgs.ChangingEvent)
            {
                case AttributeChangedEventArgs.Event.Attachment:
                    {
                        //_components.Add(
                        //    eventArgs.AttributeName,
                        //    GetDefaultFictionalAttributeComponent(eventArgs.Attribute));
                        OnAttributeAttached(sender, eventArgs);

                        break;
                    }
                case AttributeChangedEventArgs.Event.Detachment:
                    {
                        OnAttributeDetached(sender, eventArgs);

                        break;
                    }
                case AttributeChangedEventArgs.Event.Deletion:
                    {
                        //_components.Remove(eventArgs.AttributeName);
                        OnAttributeDeleted(sender, eventArgs);

                        break;
                    }
            }

            return;
        }

        protected virtual void OnAttributeAttached(object sender, AttributeChangedEventArgs eventArgs)
        {
            return;
        }

        protected virtual void OnAttributeDetached(object sender, AttributeChangedEventArgs eventArgs)
        {
            return;
        }

        protected virtual void OnAttributeDeleted(object sender, AttributeChangedEventArgs eventArgs)
        {
            return;
        }
        */

        void ProcessDomain<TDomainEntity>(string attributeName, AttributeDomain<TDomainEntity> attribute)
            where TDomainEntity : IComparable<TDomainEntity>
        {
            Type entityType = typeof(TEntity);
            TEntity entity = default;
            entityType.InvokeMember(
                attributeName, BindingFlags.GetField | BindingFlags.GetProperty, null, entity, new object[] { });
        }

        /// <summary>
        /// Приведение схем двух кортежей к общему виду.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private static TupleObjectSchema<TEntity> Generalize(
            TupleObject<TEntity> first, TupleObject<TEntity> second)
        {
            return null;
            //return first.Schema.GeneralizeWith(second.Schema);
        }

        public abstract TupleObject<TEntity> Diagonal();

        protected abstract TupleObject<TEntity> ComplementThe();

        protected abstract TupleObject<TEntity> IntersectWith(TupleObject<TEntity> second);

        protected abstract TupleObject<TEntity> UnionWith(TupleObject<TEntity> second);

        protected abstract TupleObject<TEntity> ExceptWith(TupleObject<TEntity> second);

        protected abstract TupleObject<TEntity> SymmetricExceptWith(TupleObject<TEntity> second);

        public abstract TupleObject<TEntity> Convert(TupleObject<TEntity> diagonal);

        #endregion

        #region IEnumerable<TEntity> implementation

        /// <summary>
        /// Получение перечислителя хранимых сущностей.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TEntity> GetEnumerator()
        {
            return GetEnumeratorImpl();//(Provider.Execute<IEnumerable<TEntity>>(Expression) as TupleObject<TEntity>).GetEnumeratorImpl();
        }

        /// <summary>
        /// Получение перечислителя хранимых сущностей, специфичное для конкретных объектов АК.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator<TEntity> GetEnumeratorImpl();

        /// <summary>
        /// Получение перечислителя хранимых сущностей.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            if (_isDisposed)
                return;

            DisposeImpl();

            _isDisposed = true;
        }

        protected abstract void DisposeImpl();

        #endregion

        #region Operators

        /// <summary>
        /// Оператор присоединения атрибута с заданным именем к схеме кортежа.
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static TupleObject<TEntity> operator +(TupleObject<TEntity> tuple, string attributeName)
        {
            //tuple.Schema += attributeName;

            return tuple;
        }

        /// <summary>
        /// Оператор отсоединения атрибута с заданным именем от схемы кортежа.
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static TupleObject<TEntity> operator -(TupleObject<TEntity> tuple, string attributeName)
        {
            //tuple.Schema -= attributeName;

            return tuple;
        }

        public static TupleObject<TEntity> operator !(TupleObject<TEntity> first)
        {
            return first.ComplementThe();
        }

        public static TupleObject<TEntity> operator &(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return first.IntersectWith(second);
        }

        public static TupleObject<TEntity> operator |(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return first.UnionWith(second);
        }

        public static TupleObject<TEntity> operator /(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return first.ExceptWith(second);
        }

        public static TupleObject<TEntity> operator ^(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return first.SymmetricExceptWith(second);
        }

        #endregion
    }
}
