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
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectAcceptors;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectAcceptors;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;
    using static TupleObjectStaticDataStorage;

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

        public ITupleObjectOperationExecutorsContainer<TEntity> SetOperations
        { get => Storage.GetSetOperations<TEntity>(this); }

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
            TupleObjectBuilder<TEntity> builder =
                TupleObjectBuilder<TEntity>.StaticBuilder;
            onTupleBuilding(builder);
            builder.EndSchemaInitialization();

            return;
        }

        public void PassSchema(
            TupleObjectBuilder<TEntity> builder)
        {
            builder.Schema = Schema;

            return;
        }

        #endregion

        #region Instance methods

        public abstract bool IsEmpty();

        public abstract bool IsFull();

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

        public abstract TupleObject<TEntity> AlignWithSchema(
            TupleObjectSchema<TEntity> schema,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder);

        public TupleObject<TEntity> ConvertToAlternate()
        {
            return SetOperations.ConvertToAlternate(this);
        }

        public TupleObject<TEntity> ComplementThe()
        {
            return SetOperations.Complement(this);
        }

        public TupleObject<TEntity> IntersectWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.Intersect(this, second);
        }

        public TupleObject<TEntity> UnionWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.Union(this, second);
        }

        public TupleObject<TEntity> ExceptWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.Except(this, second);
        }

        public TupleObject<TEntity> SymmetricExceptWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.SymmetricExcept(this, second);
        }

        public bool Includes(
            TupleObject<TEntity> second)
        {
            return SetOperations.Include(this, second);
        }

        public bool EqualsTo(
            TupleObject<TEntity> second)
        {
            return SetOperations.Equal(this, second);
        }

        public bool IncludesOrEqualsTo(
            TupleObject<TEntity> second)
        {
            return SetOperations.IncludeOrEqual(this, second);
        }

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

        public static TupleObject<TEntity> operator ~(TupleObject<TEntity> first)
        {
            return first.ComplementThe();
        }

        public static TupleObject<TEntity> operator !(
            TupleObject<TEntity> first)
        {
            return first.ConvertToAlternate();
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

        public static bool operator >(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return first.Includes(second);
        }

        public static bool operator ==(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return first.EqualsTo(second);
        }

        public static bool operator >=(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return first.IncludesOrEqualsTo(second);
        }

        public static bool operator <(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return second.Includes(first);
        }

        public static bool operator !=(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return !first.EqualsTo(second);
        }

        public static bool operator <=(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return second.IncludesOrEqualsTo(first);
        }

        #endregion

        #region Nested types

        public abstract class FictionalTupleObjectOperationExecutorsContainer<CTOperand>
            : InstantSetOperationExecutorsContainer<
                TupleObject<TEntity>, 
                CTOperand, 
                TupleObjectFactory>,
              ITupleObjectOperationExecutorsContainer<TEntity>
        where CTOperand : TupleObject<TEntity>
        {
            #region Constructors

            public FictionalTupleObjectOperationExecutorsContainer(
                TupleObjectFactory factory,
                Func<TupleObjectFactoryUnarySetOperator<TEntity, CTOperand>>
                    complementationOperator,
                Func<TupleObjectInstantBinaryAcceptor<TEntity, CTOperand, TupleObject<TEntity>>>
                    intersectionOperator,
                Func<TupleObjectInstantBinaryAcceptor<TEntity, CTOperand, TupleObject<TEntity>>>
                    unionOperator,
                Func<TupleObjectInstantBinaryAcceptor<TEntity, CTOperand, TupleObject<TEntity>>>
                    differenceOperator,
                Func<TupleObjectInstantBinaryAcceptor<TEntity, CTOperand, TupleObject<TEntity>>>
                    symmetricExceptionOperator,
                Func<TupleObjectInstantBinaryAcceptor<TEntity, CTOperand, bool>>
                    inclusionComparer,
                Func<TupleObjectInstantBinaryAcceptor<TEntity, CTOperand, bool>>
                    equalityComparer,
                Func<TupleObjectInstantBinaryAcceptor<TEntity, CTOperand, bool>>
                    inclusionOrEquationComparer)
                : base(factory,
                       complementationOperator,
                       intersectionOperator,
                       unionOperator,
                       differenceOperator,
                       symmetricExceptionOperator,
                       inclusionComparer,
                       equalityComparer,
                       inclusionOrEquationComparer)
            {
                return;
            }

            #endregion

            public TupleObject<TEntity> ConvertToAlternate(TupleObject<TEntity> first)
            {
                throw new InvalidOperationException(
                    "Фиктивные кортежи не поддерживают операции конвертирования в альтернативный способ отображения.");
            }
        }

        public abstract class NonFictionalTupleObjectOperationExecutorsContainer<CTOperand>
            : FactorySetOperationExecutorsContainer<
                TupleObject<TEntity>, 
                CTOperand, 
                TupleObjectFactory>,
              ITupleObjectOperationExecutorsContainer<TEntity>
        where CTOperand : TupleObject<TEntity>
        {
            private Lazy<TupleObjectFactoryUnarySetOperator<TEntity, CTOperand>>
                _conversionToAlternateOperator;

            public TupleObjectFactoryUnarySetOperator<TEntity, CTOperand>
                ConversionToAlternateOperator => _conversionToAlternateOperator.Value;

            #region Constructors

            public NonFictionalTupleObjectOperationExecutorsContainer(
                TupleObjectFactory factory,
                Func<TupleObjectFactoryUnarySetOperator<TEntity, CTOperand>>
                    complementationOperator,
                Func<TupleObjectFactoryUnarySetOperator<TEntity, CTOperand>>
                    conversionToAlternateOperator,
                Func<TupleObjectFactoryBinaryAcceptor<TEntity, CTOperand, TupleObject<TEntity>>>
                    intersectionOperator,
                Func<TupleObjectFactoryBinaryAcceptor<TEntity, CTOperand, TupleObject<TEntity>>>
                    unionOperator,
                Func<TupleObjectFactoryBinaryAcceptor<TEntity, CTOperand, TupleObject<TEntity>>>
                    differenceOperator,
                Func<TupleObjectFactoryBinaryAcceptor<TEntity, CTOperand, TupleObject<TEntity>>>
                    symmetricExceptionOperator,
                Func<TupleObjectInstantBinaryAcceptor<TEntity, CTOperand, bool>>
                    inclusionComparer,
                Func<TupleObjectInstantBinaryAcceptor<TEntity, CTOperand, bool>>
                    equalityComparer,
                Func<TupleObjectInstantBinaryAcceptor<TEntity, CTOperand, bool>>
                    inclusionOrEquationComparer)
                : base(factory,
                       complementationOperator,
                       intersectionOperator,
                       unionOperator,
                       differenceOperator,
                       symmetricExceptionOperator,
                       inclusionComparer,
                       equalityComparer,
                       inclusionOrEquationComparer)
            {
                _conversionToAlternateOperator = new 
                    Lazy<TupleObjectFactoryUnarySetOperator<TEntity, CTOperand>>(conversionToAlternateOperator);

                return;
            }

            #endregion

            public TupleObject<TEntity> ConvertToAlternate(
                TupleObject<TEntity> first)
            {
                return ConversionToAlternateOperator.Accept(
                    (first as CTOperand)!, Factory);
            }
        }

        #endregion
    }
}
