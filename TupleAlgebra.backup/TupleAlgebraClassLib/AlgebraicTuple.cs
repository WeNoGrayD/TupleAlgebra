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
    public abstract class AlgebraicTuple<TEntity>
    {
        public AlgebraicTupleSchema<TEntity> Schema { get; private set; }

        public readonly AlgebraicTupleBuilder<TEntity> _builder;

        static AlgebraicTuple()
        {
            AlgebraicTupleBuilder<TEntity>.BuildSchemaPattern();
        }

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

        private static void Generalize(AlgebraicTuple<TEntity> first, AlgebraicTuple<TEntity> second)
        {
            first.Schema.GeneralizeWith(second.Schema);
        }

        public static bool operator +(AlgebraicTuple<TEntity> tuple, string attributeName)
        {
            return tuple.Schema + attributeName;
        }

        public static bool operator -(AlgebraicTuple<TEntity> tuple, string attributeName)
        {
            return tuple.Schema - attributeName;
        }
    }
}
