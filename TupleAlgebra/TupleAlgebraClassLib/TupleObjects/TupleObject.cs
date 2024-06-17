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
using TupleAlgebraClassLib.AttributeComponentVisitors;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectVisitors;
using UniversalClassLib.HierarchicallyPolymorphicOperators;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectVisitors;
using TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;
    using static TupleObjectStaticDataStorage;

    public interface ITupleObject 
        : IQueryable, IFactoryProvider<TupleObjectFactory>
    {
        public ITupleObjectSchemaProvider Schema { get; }

        public void PassSchema<TEntity>(ITupleObjectBuilder builder)
        {
            builder.SetSchema<TEntity>(Schema as TupleObjectSchema<TEntity>);

            return;
        }
    }
    
    /// <summary>
    /// Кортеж данных о сущностях определённого типа.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class TupleObject<TEntity>
        : IQueryable<TEntity>,
          IDisposable,
          ITupleObject
        where TEntity : new()
    {
        #region Instance fields

        private bool _isDisposed = false;

        #endregion

        #region Instance properties

        public ITupleObjectOperationExecutorsContainer<TEntity> SetOperations
        { get => Storage.GetSetOperations<TEntity>(this); }

        /// <summary>
        /// Схема кортежа. Содержит данные 
        /// </summary>
        public TupleObjectSchema<TEntity> Schema { get; private set; }

        ITupleObjectSchemaProvider ITupleObject.Schema { get => Schema; }

        public TupleObjectFactory Factory => Storage.GetFactory();

        public virtual Expression Expression 
        { 
            get => Expression.Constant(this); 
        }

        public virtual Type ElementType { get => typeof(TEntity); }

        public virtual IQueryProvider Provider 
        { get => new TupleObjectQueryProvider(); }

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

        public virtual bool IsFalse()
        {
            return IsEmpty() || !GetEnumerator().MoveNext();
        }

        public virtual bool IsTrue()
        {
            return IsFull();
        }

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

        public virtual TupleObject<TEntity> ExecuteQuery()
        {
            return (Provider.Execute<TupleObject<TEntity>>(Expression));
        }

        protected TupleObject<TEntity> DefineOperand(
            TupleObject<TEntity> operand)
        {
            if (operand is QueriedTupleObject<TEntity> queried)
                return queried.ExecuteQuery();

            return operand;
        }

        public abstract TupleObject<TEntity> AlignWithSchema(
            TupleObjectSchema<TEntity> schema,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder);

        public virtual TupleObject<TEntity> ConvertToAlternate()
        {
            return SetOperations.ConvertToAlternate(this);
        }

        public virtual TupleObject<TEntity> ComplementThe()
        {
            return SetOperations.Complement(this);
        }

        public virtual TupleObject<TEntity> IntersectWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.Intersect(this, second.ExecuteQuery());
        }

        public virtual TupleObject<TEntity> UnionWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.Union(this, second.ExecuteQuery());
        }

        public virtual TupleObject<TEntity> ExceptWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.Except(this, second.ExecuteQuery());
        }

        public virtual TupleObject<TEntity> SymmetricExceptWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.SymmetricExcept(this, second.ExecuteQuery());
        }

        public virtual bool Includes(
            TupleObject<TEntity> second)
        {
            return SetOperations.Include(this, second.ExecuteQuery());
        }

        public virtual bool EqualsTo(
            TupleObject<TEntity> second)
        {
            return SetOperations.Equal(this, second.ExecuteQuery());
        }

        public virtual bool IncludesOrEqualsTo(
            TupleObject<TEntity> second)
        {
            return SetOperations.IncludeOrEqual(this, second.ExecuteQuery());
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

        public static bool operator false(TupleObject<TEntity> tuple)
        {
            return tuple.IsFalse();
        }

        public static bool operator true(TupleObject<TEntity> tuple)
        {
            return tuple.IsTrue();
        }

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
            return second.IncludesOrEqualsTo(first);
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
            return second.Includes(first);
        }

        public static bool operator <(
            TupleObject<TEntity> first,
            TupleObject<TEntity> second)
        {
            return first.IncludesOrEqualsTo(second);
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
            return first.Includes(second);
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
                Func<TupleObjectInstantBinaryVisitor<TEntity, CTOperand, TupleObject<TEntity>>>
                    intersectionOperator,
                Func<TupleObjectInstantBinaryVisitor<TEntity, CTOperand, TupleObject<TEntity>>>
                    unionOperator,
                Func<TupleObjectInstantBinaryVisitor<TEntity, CTOperand, TupleObject<TEntity>>>
                    differenceOperator,
                Func<TupleObjectInstantBinaryVisitor<TEntity, CTOperand, TupleObject<TEntity>>>
                    symmetricExceptionOperator,
                Func<TupleObjectInstantBinaryVisitor<TEntity, CTOperand, bool>>
                    inclusionComparer,
                Func<TupleObjectInstantBinaryVisitor<TEntity, CTOperand, bool>>
                    equalityComparer,
                Func<TupleObjectInstantBinaryVisitor<TEntity, CTOperand, bool>>
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
                Func<TupleObjectFactoryBinaryVisitor<TEntity, CTOperand, TupleObject<TEntity>>>
                    intersectionOperator,
                Func<TupleObjectFactoryBinaryVisitor<TEntity, CTOperand, TupleObject<TEntity>>>
                    unionOperator,
                Func<TupleObjectFactoryBinaryVisitor<TEntity, CTOperand, TupleObject<TEntity>>>
                    differenceOperator,
                Func<TupleObjectFactoryBinaryVisitor<TEntity, CTOperand, TupleObject<TEntity>>>
                    symmetricExceptionOperator,
                Func<TupleObjectInstantBinaryVisitor<TEntity, CTOperand, bool>>
                    inclusionComparer,
                Func<TupleObjectInstantBinaryVisitor<TEntity, CTOperand, bool>>
                    equalityComparer,
                Func<TupleObjectInstantBinaryVisitor<TEntity, CTOperand, bool>>
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
                return ConversionToAlternateOperator.Visit(
                    (first as CTOperand)!, Factory);
            }
        }

        #endregion
    }
}
