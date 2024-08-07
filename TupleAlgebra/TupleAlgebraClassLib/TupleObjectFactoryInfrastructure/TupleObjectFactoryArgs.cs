﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Xml.Linq;

namespace TupleAlgebraClassLib.TupleObjectFactoryInfrastructure
{
    using static TupleObjectHelper;

    public readonly struct IndexedComponentFactoryArgs<TComponentSource>
    {
        public int Index { get; init; } = -1;

        public ITupleObjectAttributeManager TupleManager { get; init; }

        public TComponentSource ComponentSource { get; init; } = default;

        public bool IsDefault { get; init; }

        public IndexedComponentFactoryArgs()
        {
            IsDefault = true;

            return;
        }

        public IndexedComponentFactoryArgs(
            int index,
            ITupleObjectAttributeManager tupleManager)
        {
            Index = index;
            TupleManager = tupleManager;
            IsDefault = true;

            return;
        }

        public IndexedComponentFactoryArgs(
            int index,
            TComponentSource componentSource)
        {
            Index = index;
            ComponentSource = componentSource;
            IsDefault = false;

            return;
        }

        public IndexedComponentFactoryArgs(
            int index,
            ITupleObjectBuilder builder,
            TComponentSource componentSource)
            : this(index, componentSource)
        {
            TupleManager = builder.AttributeAt(index).CreateManager();

            return;
        }

        public IndexedComponentFactoryArgs(
            int index,
            ITupleObjectBuilder builder)
        {
            Index = index;
            TupleManager = builder.AttributeAt(index).CreateManager();
            IsDefault = true;

            return;
        }

        public IndexedComponentFactoryArgs(
            int index,
            ITupleObjectSchemaProvider schema)
        {
            Index = index;
            TupleManager = schema.GetSetupWizard(index).CreateManager();
            IsDefault = true;

            return;
        }

        public IndexedComponentFactoryArgs(
            int index,
            ITupleObjectSchemaProvider schema,
            TComponentSource componentSource)
            : this(index, componentSource)
        {
            TupleManager = schema.GetSetupWizard(index).CreateManager();

            return;
        }

        public IndexedComponentFactoryArgs(
            int index,
            ITupleObjectAttributeManager tupleManager,
            TComponentSource componentSource)
            : this(index, componentSource)
        {
            TupleManager = tupleManager;

            return;
        }

        public IndexedComponentFactoryArgs(
            int index,
            ITupleObjectSchemaProvider schema,
            Func<ITupleObjectAttributeManager, TComponentSource> getComponent)
        {
            Index = index;
            TupleManager = schema.GetSetupWizard(index).CreateManager();
            ComponentSource = getComponent(TupleManager);

            return;
        }
    }

    public readonly struct NamedComponentFactoryArgs<TComponentSource>
    {
        public AttributeName Name { get; init; }

        public ITupleObjectAttributeManager TupleManager { get; init; }

        public TComponentSource ComponentSource { get; init; } = default;

        public NamedComponentFactoryArgs() { }

        private NamedComponentFactoryArgs(
            AttributeName name,
            TComponentSource componentSource)
        {
            Name = name;
            ComponentSource = componentSource;

            return;
        }

        public NamedComponentFactoryArgs(
            AttributeName name,
            ITupleObjectBuilder builder,
            TComponentSource componentSource)
            : this(name, componentSource)
        {
            TupleManager = builder.Attribute(name).CreateManager();

            return;
        }

        public NamedComponentFactoryArgs(
            AttributeName name,
            ITupleObjectSchemaProvider schema,
            TComponentSource componentSource)
            : this(name, componentSource)
        {
            TupleManager = schema.GetSetupWizard(name).CreateManager();

            return;
        }

        public NamedComponentFactoryArgs(
            AttributeName name,
            ITupleObjectAttributeManager tupleManager,
            TComponentSource componentSource)
            : this(name, componentSource)
        {
            TupleManager = tupleManager;

            return;
        }

        /*
        public static implicit operator (AttributeName Name, TComponentSource ComponentSource)(
             NamedComponentFactoryArgs<TComponentSource> factoryArgs)
        {
            return (factoryArgs.Name, factoryArgs.ComponentSource);
        }
        */
    }

    public interface ISingleTupleObjectFactoryArgs
    {
        LambdaExpression Getter { get; }

        AttributeName AttributeName { get => Getter; }

        IAttributeComponentFactoryArgs ComponentFactoryArgs { get; }

        /*
        public (AttributeName Name, IAttributeComponentFactoryArgs ComponentFactoryArgs)
            ToTuple() => (Getter, ComponentFactoryArgs);
        
        */

        public NamedComponentFactoryArgs<IAttributeComponentFactoryArgs>
            ToNamedComponentFactoryArgs<TEntity>(
                TupleObjectBuilder<TEntity> builder)
        {
            return new(AttributeName, builder, ComponentFactoryArgs);
        }
    }

    public record SingleTupleObjectFactoryArgs<TEntity, TAttribute>
        : ISingleTupleObjectFactoryArgs
    {
        private Expression<Func<TEntity, TAttribute>> _getter;

        private NonFictionalAttributeComponentFactoryArgs<TAttribute> _componentFactoryArgs;

        LambdaExpression ISingleTupleObjectFactoryArgs
            .Getter
        { get => _getter; }

        IAttributeComponentFactoryArgs ISingleTupleObjectFactoryArgs.
            ComponentFactoryArgs
        { get => _componentFactoryArgs; }

        public SingleTupleObjectFactoryArgs(
            Expression<Func<TEntity, TAttribute>> getter,
            NonFictionalAttributeComponentFactoryArgs<TAttribute> componentFactoryArgs)
        {
            _getter = getter;
            _componentFactoryArgs = componentFactoryArgs;

            return;
        }

        public static implicit operator SingleTupleObjectFactoryArgs<TEntity, TAttribute>(
            (Expression<Func<TEntity, TAttribute>> Getter,
             NonFictionalAttributeComponentFactoryArgs<TAttribute> ComponentFactoryArgs)
            factoryArgs)
        {
            return new SingleTupleObjectFactoryArgs<TEntity, TAttribute>(
                factoryArgs.Getter, factoryArgs.ComponentFactoryArgs);
        }
    }


    /*
public class TupleObjectFactoryArgs<TEntity>
    where TEntity : new()
{
    #region Instance fields

    private Delegate _onTupleBuilding;

    #endregion
    */

    /*
    #region Instance properties

    public TupleObjectSchema<TEntity> Schema { get; set; }

        public IQueryProvider QueryProvider { get; set; }

        public Expression QueryExpression { get; set; }

        #endregion

        #region Constructors

        protected TupleObjectFactoryArgs(
            IQueryProvider queryProvider, 
            Expression queryExpression)
        {
            QueryProvider = queryProvider;
            QueryExpression = queryExpression;

            return;
        }

    #endregion

    #region Instance methods

    protected void SetOnTupleBuildingHandler<TEntity>(TupleObjectBuildingHandler<TEntity> onTupleBuilding)
    {
        _onTupleBuilding = onTupleBuilding;

        return;
    }

    public TupleObjectBuildingHandler<TEntity> GetOnTupleBuildingHandler<TEntity>()
    {
        return _onTupleBuilding as TupleObjectBuildingHandler<TEntity>;
    }

    #endregion
}
*/

    /*
    public class TupleObjectCopyFactoryArgs<TEntity> : TupleObjectFactoryArgs<TEntity>
        where TEntity : new()
    {
        public TupleObject<TEntity> Source { get; private set; }

        public TupleObjectCopyFactoryArgs(
            TupleObject<TEntity> source,
            object actionAfterCopy = null,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(source.Schema, queryProvider, queryExpression)
        {
            Source = source;

            return;
        }
    }
    */

    /*
    public class SingleTupleObjectFactoryArgs<TEntity> : TupleObjectFactoryArgs<TEntity>
        where TEntity : new()
    {
        public SingleTupleObjectFactoryArgs(
            IQueryProvider queryProvider = null,
            Expression queryExpression = null,
            params (LambdaExpression AttributeGetterExpr, IAttributeComponent AttributeComponent)[] attributes)
            : base(queryProvider, queryExpression)
        { }
    }

    public class TupleSystemObjectFactoryArgs<TEntity> : TupleObjectFactoryArgs<TEntity>
        where TEntity : new()
    {
        public TupleSystemObjectFactoryArgs(
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(queryProvider, queryExpression)
        { }
    }
    */
}
