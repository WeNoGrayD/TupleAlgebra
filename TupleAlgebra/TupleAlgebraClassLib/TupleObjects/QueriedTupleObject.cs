using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.LINQ2TAFramework.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using System.Runtime.CompilerServices;

namespace TupleAlgebraClassLib.TupleObjects
{
    public enum QueriedTupleType : sbyte
    {
        U = -1,
        D = 0,
        C = 1
    }

    internal class QueriedTupleObject<TEntity>
        : TupleObject<TEntity>
        where TEntity : new()
    {
        #region Instance fields

        private Expression _queryExpression;

        #endregion

        #region Instance properties

        public override Expression Expression
        {
            get => _queryExpression;
        }

        public virtual QueriedTupleType StructureType 
        { get => QueriedTupleType.U; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public QueriedTupleObject(
            Expression queryExpression,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(new TupleObjectBuilder<TEntity>(), onTupleBuilding)
        {
            _queryExpression = queryExpression ?? Expression.Constant(this);

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedTupleObject(
            Expression queryExpression,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base (builder, onTupleBuilding)
        {
            _queryExpression = queryExpression ?? Expression.Constant(this);

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedTupleObject(
            Expression queryExpression,
            TupleObjectSchema<TEntity> schema,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(new TupleObjectBuilder<TEntity>(schema), onTupleBuilding)
        {
            _queryExpression = queryExpression ?? Expression.Constant(this);

            return;
        }

        protected QueriedTupleObject(
            Expression queryExpression, 
            TupleObjectSchema<TEntity> schema)
            : base(schema)
        {
            _queryExpression = queryExpression ?? Expression.Constant(this);

            return;
        }

        #endregion

        #region Instance methods

        public override bool IsEmpty()
        {
            throw new NotImplementedException();
        }

        public override bool IsFull()
        {
            throw new NotImplementedException();
        }

        public override TupleObject<TEntity> AlignWithSchema(
            TupleObjectSchema<TEntity> schema,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
        {
            return factory.CreateQueried<TEntity>(Expression, schema.PassToBuilder);
        }

        public TupleObject<TEntity> ExecuteQuery()
        {
            return (Provider.Execute<TupleObject<TEntity>>(Expression));
        }

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            return ExecuteQuery().GetEnumerator();
        }

        protected override void DisposeImpl()
        {
            return;
        }

        public override TupleObject<TEntity> ConvertToAlternate()
        {
            return SetOperations.ConvertToAlternate(ExecuteQuery());
        }

        public override TupleObject<TEntity> ComplementThe()
        {
            return SetOperations.Complement(ExecuteQuery());
        }

        public override TupleObject<TEntity> IntersectWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.Intersect(
                ExecuteQuery(),
                DefineOperand(second));
        }

        public override TupleObject<TEntity> UnionWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.Union(
                ExecuteQuery(),
                DefineOperand(second));
        }

        public override TupleObject<TEntity> ExceptWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.Except(
                ExecuteQuery(),
                DefineOperand(second));
        }

        public override TupleObject<TEntity> SymmetricExceptWith(
            TupleObject<TEntity> second)
        {
            return SetOperations.SymmetricExcept(
                ExecuteQuery(),
                DefineOperand(second));
        }

        public override bool Includes(
            TupleObject<TEntity> second)
        {
            return SetOperations.Include(
                ExecuteQuery(),
                DefineOperand(second));
        }

        public override bool EqualsTo(
            TupleObject<TEntity> second)
        {
            return SetOperations.Equal(
                ExecuteQuery(),
                DefineOperand(second));
        }

        public override bool IncludesOrEqualsTo(
            TupleObject<TEntity> second)
        {
            return SetOperations.IncludeOrEqual(
                ExecuteQuery(), 
                DefineOperand(second));
        }

        #endregion
    }

    public interface IQueriedSingleTupleObject
        : ISingleTupleObject<Expression>
    {
        public Expression GetDefaultFictionalAttributeComponent<TAttribute>(
            IAttributeComponentFactory<TAttribute> factory);
    }

    internal abstract class QueriedSingleTupleObject<TEntity>
        : QueriedTupleObject<TEntity>,
          IQueriedSingleTupleObject
        where TEntity : new()
    {
        private Expression[] _components;

        ITupleObjectSchemaProvider ITupleObject.Schema { get => Schema; }

        public Expression this[int attrLoc]
        {
            get => _components[attrLoc];
            set => _components[attrLoc] = value;
        }

        public int RowLength { get => _components.Length; }

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public QueriedSingleTupleObject(
            Expression[] componentsQueryExpressions,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  null, 
                  new TupleObjectBuilder<TEntity>(), 
                  onTupleBuilding)
        {
            _components = componentsQueryExpressions;

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedSingleTupleObject(
            Expression[] componentsQueryExpressions,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(null, builder, onTupleBuilding)
        {
            _components = componentsQueryExpressions;

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedSingleTupleObject(
            Expression[] componentsQueryExpressions,
            TupleObjectSchema<TEntity> schema,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  null,
                  new TupleObjectBuilder<TEntity>(schema), 
                  onTupleBuilding)
        {
            _components = componentsQueryExpressions;

            return;
        }

        protected QueriedSingleTupleObject(
            Expression[] componentsQueryExpressions,
            TupleObjectSchema<TEntity> schema)
            : base(null, schema)
        {
            _components = componentsQueryExpressions;

            return;
        }

        protected QueriedSingleTupleObject(
            TupleObjectSchema<TEntity> schema)
            : this(new Expression[schema.PluggedAttributesCount], schema)
        {
            return;
        }

        #endregion

        #region Instance methods

        public bool IsDefault(int attrLoc) => false;

        public abstract Expression GetDefaultFictionalAttributeComponent<TAttribute>(
            IAttributeComponentFactory<TAttribute> factory);

        #endregion
    }

    internal abstract class QueriedTupleObjectSystem<
        TEntity, 
        TQueriedSingleTupleObject>
        : QueriedTupleObject<TEntity>,
          ITupleObjectSystem<Expression, TQueriedSingleTupleObject>
        where TEntity : new()
        where TQueriedSingleTupleObject : QueriedSingleTupleObject<TEntity>
    {
        public TQueriedSingleTupleObject this[int tupleLoc]
        {
            get => Tuples[tupleLoc];
            set => Tuples[tupleLoc] = value;
        }

        public TQueriedSingleTupleObject[] Tuples { get; private set; }

        public int RowLength { get => Schema.PluggedAttributesCount; }

        public int ColumnLength { get => Tuples.Length; }

        public bool IsDiagonal { get; set; } = false;

        public bool IsOrthogonal { get; set; } = false;

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public QueriedTupleObjectSystem(
            TQueriedSingleTupleObject[] tupleQueries,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  null,
                  new TupleObjectBuilder<TEntity>(),
                  onTupleBuilding)
        {
            Tuples = tupleQueries;

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedTupleObjectSystem(
            TQueriedSingleTupleObject[] tupleQueries,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(null, builder, onTupleBuilding)
        {
            Tuples = tupleQueries;

            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedTupleObjectSystem(
            TQueriedSingleTupleObject[] tupleQueries,
            TupleObjectSchema<TEntity> schema,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  null,
                  new TupleObjectBuilder<TEntity>(schema),
                  onTupleBuilding)
        {
            Tuples = tupleQueries;

            return;
        }

        protected QueriedTupleObjectSystem(
            TQueriedSingleTupleObject[] tupleQueries,
            TupleObjectSchema<TEntity> schema)
            : base(null, schema)
        {
            Tuples = tupleQueries;

            return;
        }

        protected QueriedTupleObjectSystem(
            TupleObjectSchema<TEntity> schema,
            int len)
            : base(null, schema)
        {
            Tuples = new TQueriedSingleTupleObject[len];

            return;
        }

        protected QueriedTupleObjectSystem(
            TupleObjectSchema<TEntity> schema,
            IList<IQueriedSingleTupleObject> tuples)
            : base(null, schema)
        {
            Tuples = tuples.OfType<TQueriedSingleTupleObject>().ToArray();

            return;
        }

        #endregion

        public void TrimRedundantRows(int len)
        {
            if (len < Tuples.Length)
                Tuples = Tuples[..len];

            return;
        }
    }

    internal class QueriedConjunctiveTuple<TEntity>
        : QueriedSingleTupleObject<TEntity>
        where TEntity : new()
    {
        #region Instance properties

        public override QueriedTupleType StructureType
        { get => QueriedTupleType.C; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public QueriedConjunctiveTuple(
            Expression[] componentsQueryExpressions,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  componentsQueryExpressions,
                  new TupleObjectBuilder<TEntity>(),
                  onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedConjunctiveTuple(
            Expression[] componentsQueryExpressions,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  componentsQueryExpressions,
                  builder,
                  onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedConjunctiveTuple(
            Expression[] componentsQueryExpressions,
            TupleObjectSchema<TEntity> schema,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  componentsQueryExpressions,
                  new TupleObjectBuilder<TEntity>(schema),
                  onTupleBuilding)
        {
            return;
        }

        protected QueriedConjunctiveTuple(
            Expression[] componentsQueryExpressions,
            TupleObjectSchema<TEntity> schema)
            : base(
                  componentsQueryExpressions,
                  schema)
        {
            return;
        }

        public QueriedConjunctiveTuple(
            TupleObjectSchema<TEntity> schema)
            : base(schema)
        {
            return;
        }

        #endregion

        public override Expression GetDefaultFictionalAttributeComponent<TAttribute>(
            IAttributeComponentFactory<TAttribute> factory)
        {
            return Expression.Call(
                Expression.Constant(factory),
                typeof(IAttributeComponentFactory<TAttribute>)
                .GetMethod(
                    nameof(IAttributeComponentFactory<TAttribute>.CreateFull),
                    System.Reflection.BindingFlags.Public | 
                    System.Reflection.BindingFlags.Instance,
                    new Type[0]));
        }
    }

    internal class QueriedDisjunctiveTuple<TEntity>
        : QueriedSingleTupleObject<TEntity>
        where TEntity : new()
    {
        #region Instance properties

        public override QueriedTupleType StructureType
        { get => QueriedTupleType.D; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public QueriedDisjunctiveTuple(
            Expression[] componentsQueryExpressions,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  componentsQueryExpressions,
                  new TupleObjectBuilder<TEntity>(),
                  onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedDisjunctiveTuple(
            Expression[] componentsQueryExpressions,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  componentsQueryExpressions,
                  builder, 
                  onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedDisjunctiveTuple(
            Expression[] componentsQueryExpressions,
            TupleObjectSchema<TEntity> schema,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  componentsQueryExpressions,
                  new TupleObjectBuilder<TEntity>(schema),
                  onTupleBuilding)
        {
            return;
        }

        protected QueriedDisjunctiveTuple(
            Expression[] componentsQueryExpressions,
            TupleObjectSchema<TEntity> schema)
            : base(componentsQueryExpressions, schema)
        {
            return;
        }

        public QueriedDisjunctiveTuple(
            TupleObjectSchema<TEntity> schema)
            : base(schema)
        {
            return;
        }

        #endregion

        public override Expression GetDefaultFictionalAttributeComponent<TAttribute>(
            IAttributeComponentFactory<TAttribute> factory)
        {
            return Expression.Call(
                Expression.Constant(factory),
                typeof(IAttributeComponentFactory<TAttribute>)
                .GetMethod(
                    nameof(IAttributeComponentFactory<TAttribute>.CreateEmpty),
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance,
                    new Type[0]));
        }
    }

    internal class QueriedConjunctiveTupleSystem<TEntity>
        : QueriedTupleObjectSystem<TEntity, QueriedConjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        #region Instance properties

        public override QueriedTupleType StructureType
        { get => QueriedTupleType.C; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public QueriedConjunctiveTupleSystem(
            QueriedConjunctiveTuple<TEntity>[] tupleQueries,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  tupleQueries,
                  new TupleObjectBuilder<TEntity>(),
                  onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedConjunctiveTupleSystem(
            QueriedConjunctiveTuple<TEntity>[] tupleQueries,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(tupleQueries, builder, onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedConjunctiveTupleSystem(
            QueriedConjunctiveTuple<TEntity>[] tupleQueries,
            TupleObjectSchema<TEntity> schema,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  tupleQueries,
                  new TupleObjectBuilder<TEntity>(schema),
                  onTupleBuilding)
        {
            return;
        }

        protected QueriedConjunctiveTupleSystem(
            QueriedConjunctiveTuple<TEntity>[] tupleQueries,
            TupleObjectSchema<TEntity> schema)
            : base(tupleQueries, schema)
        {
            return;
        }

        public QueriedConjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            int len)
            : base(schema, len)
        {
            return;
        }

        public QueriedConjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            IList<IQueriedSingleTupleObject> tuples)
            : base(schema, tuples)
        {
            return;
        }

        #endregion
    }

    internal class QueriedDisjunctiveTupleSystem<TEntity>
        : QueriedTupleObjectSystem<TEntity, QueriedDisjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        #region Instance properties

        public override QueriedTupleType StructureType
        { get => QueriedTupleType.D; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        public QueriedDisjunctiveTupleSystem(
            QueriedDisjunctiveTuple<TEntity>[] tupleQueries,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  tupleQueries,
                  new TupleObjectBuilder<TEntity>(),
                  onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedDisjunctiveTupleSystem(
            QueriedDisjunctiveTuple<TEntity>[] tupleQueries,
            TupleObjectBuilder<TEntity> builder,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(tupleQueries, builder, onTupleBuilding)
        {
            return;
        }

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="onTupleBuilding"></param>
        protected QueriedDisjunctiveTupleSystem(
            QueriedDisjunctiveTuple<TEntity>[] tupleQueries,
            TupleObjectSchema<TEntity> schema,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(
                  tupleQueries,
                  new TupleObjectBuilder<TEntity>(schema),
                  onTupleBuilding)
        {
            return;
        }

        protected QueriedDisjunctiveTupleSystem(
            QueriedDisjunctiveTuple<TEntity>[] tupleQueries,
            TupleObjectSchema<TEntity> schema)
            : base(tupleQueries, schema)
        {
            return;
        }

        public QueriedDisjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            int len)
            : base(schema, len)
        {
            return;
        }

        public QueriedDisjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            IList<IQueriedSingleTupleObject> tuples)
            : base(schema, tuples)
        {
            return;
        }

        #endregion
    }
}
