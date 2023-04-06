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
using TupleAlgebraClassLib.AlgebraicTupleInfrastructure;

namespace TupleAlgebraClassLib
{
    /// <summary>
    /// Кортеж данных о сущностях определённого типа.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class AlgebraicTuple<TEntity>
    {
        /// <summary>
        /// Схема кортежа. Содержит данные 
        /// </summary>
        public AlgebraicTupleSchema<TEntity> Schema { get; private set; }

        /// <summary>
        /// Построитель кортежа.
        /// </summary>
        public readonly AlgebraicTupleBuilder<TEntity> _builder;

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static AlgebraicTuple()
        {
            AlgebraicTupleBuilder<TEntity>.BuildSchemaPattern();
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public AlgebraicTuple(Action<AlgebraicTupleBuilder<TEntity>> onTupleBuilding)
        {
            _builder = new AlgebraicTupleBuilder<TEntity>();
            onTupleBuilding(_builder);

            Schema = _builder.Schema;
        }

        void ProcessDomain<TDomainEntity>(string attributeName, AttributeDomain<TDomainEntity> attribute)
            where TDomainEntity : IComparable<TDomainEntity>
        {
            Type entityType = typeof(TEntity);
            TEntity entity = default(TEntity);
            entityType.InvokeMember(
                attributeName, BindingFlags.GetField | BindingFlags.GetProperty, null, entity, new object[] { });
        }

        /// <summary>
        /// Приведение схем двух кортежей к общему виду.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        private static void Generalize(AlgebraicTuple<TEntity> first, AlgebraicTuple<TEntity> second)
        {
            first.Schema.GeneralizeWith(second.Schema);
        }

        /// <summary>
        /// Оператор присоединения атрибута с заданным именем к схеме кортежа.
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool operator +(AlgebraicTuple<TEntity> tuple, string attributeName)
        {
            return tuple.Schema + attributeName;
        }

        /// <summary>
        /// Оператор отсоединения атрибута с заданным именем от схемы кортежа.
        /// </summary>
        /// <param name="tuple"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static bool operator -(AlgebraicTuple<TEntity> tuple, string attributeName)
        {
            return tuple.Schema - attributeName;
        }
    }
}
