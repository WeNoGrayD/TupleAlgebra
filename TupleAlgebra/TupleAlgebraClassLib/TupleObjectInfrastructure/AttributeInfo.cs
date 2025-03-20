using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using System.Reflection;
using System.Linq.Expressions;
using UniversalClassLib;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Navigational;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational.WithSimpleKey;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.UnorderedFiniteEnumerable;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Navigational;
using UniversalClassLib.ExpressionVisitors;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;
    using static AttributeComponentHelper;
    
    public enum AttributeQuery : sbyte
    {
        Removed = -2,
        Detached = -1,
        None = 0,
        Attached = 1
    }
    public interface ITupleObjectAttributeInfo
    {
        public AttributeName Name { get; }

        public AttributeQuery Query { get; protected set; }

        /// <summary>
        /// Отношение эквивалентности.
        /// </summary>
        public bool HasEquivalenceRelation { get; }

        /// <summary>
        /// Информация о члене типа, соответствующем атрибуту.
        /// </summary>
        public MemberInfo AttributeMember { get; }

        //public ITupleObjectAttributeSetupWizard SetupWizard { get; }

        public AttributeSetupWizardFactoryHandler SetupWizardFactory { get; }

        /*
        public ITupleObjectAttributeInfo GeneralizeWith(
            ITupleObjectAttributeInfo second,
            out bool gotFirst,
            out bool gotSecond);
        */

        public ITupleObjectAttributeInfo PlugIn();

        public ITupleObjectAttributeInfo Unplug();

        public ITupleObjectAttributeInfo SetEquivalenceRelation();

        public ITupleObjectAttributeInfo UnsetEquivalenceRelation();

        public void UndoQuery();

        public void AttachQuery();

        public void DetachQuery();

        public void RemoveQuery();
    }

    public interface ITupleObjectAttributeInfo<TAttribute>
        : ITupleObjectAttributeInfo
    {
        public IAttributeComponentFactory<TAttribute> ComponentFactory { get; }

        /*
        public new ITupleObjectAttributeSetupWizard<TAttribute> SetupWizard { get; }

        ITupleObjectAttributeSetupWizard 
            ITupleObjectAttributeInfo.SetupWizard { get => SetupWizard; }
        */

        public new AttributeSetupWizardFactoryHandler<TAttribute>
            SetupWizardFactory
        { get; }

        AttributeSetupWizardFactoryHandler
            ITupleObjectAttributeInfo.SetupWizardFactory
        { get => (schema, attrName) => SetupWizardFactory(schema, attrName); }

        public ITupleObjectAttributeInfo<TAttribute> SetFactory(
            IAttributeComponentFactory<TAttribute> factory);
    }

    public interface ITupleObjectAttributeInfo<TEntity, TAttribute>
        : ITupleObjectAttributeInfo<TAttribute>
    {
        public Func<TEntity, TAttribute> AttributeGetter { get; }
    }

    public record AttributeInfo<TEntity, TAttribute, TComponentFactory>
        : ITupleObjectAttributeInfo<TEntity, TAttribute>
        where TComponentFactory : class, IAttributeComponentFactory<TAttribute>
    {
        public virtual AttributeName Name { get => AttributeMember.Name; }

        /// <summary>
        /// Запрос к атрибуту: 
        /// </summary>
        public AttributeQuery Query { get; set; }

        protected Expression<Func<TEntity, TAttribute>> _attributeGetterExpr;

        public Func<TEntity, TAttribute> AttributeGetter 
        { get; init; }

        public MemberInfo AttributeMember { get; set; }

        IAttributeComponentFactory<TAttribute>
            ITupleObjectAttributeInfo<TAttribute>.ComponentFactory 
            { get => ComponentFactory; }

        public TComponentFactory ComponentFactory { get; init; }

        //public ITupleObjectAttributeSetupWizard<TAttribute> SetupWizard { get; init; }

        public AttributeSetupWizardFactoryHandler<TAttribute>
            SetupWizardFactory { get; init; }

        /// <summary>
        /// Отношение эквивалентности.
        /// </summary>
        public bool HasEquivalenceRelation { get; private set; }

        public AttributeDomain<TAttribute> Domain { get => ComponentFactory.Domain; }

        /*
        public AttributeInfo(
            Expression<Func<TEntity, TAttribute>> 
                attributeGetterExpr,
            IAttributeComponentFactory<TAttribute> componentFactory = null,
            ITupleObjectAttributeSetupWizard<TAttribute> setupWizard = null,
            string attributeName = null,
            bool hasEquivalenceRelation = false)
        {
            Query = AttributeQuery.None;
            AttributeGetter = attributeGetterExpr.Compile();
            AttributeMember = MemberExtractor.ExtractFrom(attributeGetterExpr);
            ComponentFactory = componentFactory;
            SetupWizard = setupWizard;
            HasEquivalenceRelation = hasEquivalenceRelation;

            return;
        }
        */

        public AttributeInfo(
            Expression<Func<TEntity, TAttribute>>
                attributeGetterExpr,
            TComponentFactory componentFactory = null,
            AttributeSetupWizardFactoryHandler<TAttribute> setupWizardFactory = null,
            bool hasEquivalenceRelation = false)
        {
            _attributeGetterExpr = attributeGetterExpr;
            Query = AttributeQuery.None;
            AttributeGetter = attributeGetterExpr.Compile();
            //(AttributeGetter, AttributeMember) = Deconstruct(attributeGetterExpr);
            ComponentFactory = componentFactory;
            SetupWizardFactory = setupWizardFactory ??
                TupleObjectOneToOneAttributeSetupWizard<TAttribute>.Construct;
            HasEquivalenceRelation = hasEquivalenceRelation;

            return;
        }

        protected MemberInfo GetMember<TE, TA>(Expression<Func<TE, TA>> attrGetterExpr)
        {
            return MemberExtractor.ExtractFrom(attrGetterExpr);
        }

        protected (Func<TE, TA> AttrGetter, MemberInfo AttrMember)
            Deconstruct<TE, TA>(Expression<Func<TE, TA>> attrGetterExpr)
        {
            return (
                attrGetterExpr.Compile(),
                GetMember(attrGetterExpr)
                );
        }

        public virtual void UndoQuery()
        {
            Query = AttributeQuery.None;

            return;
        }

        public virtual void AttachQuery()
        {
            Query = AttributeQuery.Attached;

            return;
        }

        public virtual void DetachQuery()
        {
            Query = AttributeQuery.Detached;

            return;
        }

        public virtual void RemoveQuery()
        {
            Query = AttributeQuery.Removed;

            return;
        }

        /*
        public ITupleObjectAttributeInfo GeneralizeWith(
            ITupleObjectAttributeInfo second,
            out bool gotFirst,
            out bool gotSecond)
        {
            if (this.AttributeMember != second.AttributeMember)
                throw new Exception($"Попытка генерализации двух разных атрибутов: {this.AttributeMember.Name} и {second.AttributeMember.Name}!");

            (ITupleObjectAttributeInfo res, gotFirst, gotSecond) = 
                (this.IsPlugged, second.IsPlugged) switch
            {
                (false, false) or (true, true) => (this, true, true),
                (true, false) => (this, true, false),
                _ => (second, false, true)
            };

            return res;
        }
        */

        public ITupleObjectAttributeInfo<TAttribute> SetFactory(
            IAttributeComponentFactory<TAttribute> factory)
        {
            return this with { ComponentFactory = factory as TComponentFactory };
        }

        public ITupleObjectAttributeInfo PlugIn()
        {
            return null;
            //return this with
            //{
            //    IsPlugged = true
            //};
        }

        public ITupleObjectAttributeInfo Unplug()
        {
            return null;
            //return this with
            //{
            //    IsPlugged = false
            //};
        }

        public ITupleObjectAttributeInfo SetEquivalenceRelation()
        {
            return this with
            {
                HasEquivalenceRelation = true
            };
        }

        public ITupleObjectAttributeInfo UnsetEquivalenceRelation()
        {
            return this with
            {
                HasEquivalenceRelation = false
            };
        }

        public override int GetHashCode() => Name.GetHashCode();
    }

    /*
    public record CalculatedAttributeInfo<TEntity, TAttribute> 
        : AttributeInfo<TEntity, TAttribute>
    {
        private ITupleObjectAttributeInfo[] _relatedAttributes;

        public CalculatedAttributeInfo(
            Expression<Func<TEntity, TAttribute>>
                attributeGetterExpr,
            ITupleObjectAttributeInfo[] relatedAttributes,
            IAttributeComponentFactory<TAttribute> componentFactory = null,
            AttributeSetupWizardFactoryHandler<TAttribute> setupWizardFactory = null,
            bool hasEquivalenceRelation = false)
            : base(
                  attributeGetterExpr,
                  componentFactory,
                  setupWizardFactory,
                  hasEquivalenceRelation)
        {
            _relatedAttributes = relatedAttributes;

            return;
        }

        public void ReattachRelatedAttributes(TupleObjectBuilder<TEntity> builder)
        {
            foreach (var attr in _relatedAttributes)
                builder.Attribute(attr.Name).Attach();

            return;
        }
    }
    */

    /*
    public record NavigationalAttributeInfo<TEntity, TKey, TAttribute, TKeyContainer>
        : AttributeInfo<TEntity, TAttribute>
    {
        public NavigationalAttributeInfo(
            Expression<Func<TEntity, TAttribute>>
                navAttrGetterExpr,
            IAttributeComponentFactory<TAttribute> componentFactory = null,
            AttributeSetupWizardFactoryHandler<TAttribute> setupWizardFactory = null,
            bool hasEquivalenceRelation = false)
            : base(
                  navAttrGetterExpr,
                  componentFactory,
                  setupWizardFactory,
                  hasEquivalenceRelation)
        {

            return;
        }
    }
    */

    public record AttributeInfo<TEntity, TAttribute>
        : AttributeInfo<TEntity, TAttribute, IAttributeComponentFactory<TAttribute>>
    {
        public AttributeInfo(
            Expression<Func<TEntity, TAttribute>>
                attributeGetterExpr,
            IAttributeComponentFactory<TAttribute> componentFactory = null,
            AttributeSetupWizardFactoryHandler<TAttribute> setupWizardFactory = null,
            bool hasEquivalenceRelation = false)
            : base(
                  attributeGetterExpr,
                  componentFactory,
                  setupWizardFactory,
                  hasEquivalenceRelation)
        {
            AttributeMember = GetMember(attributeGetterExpr);

            return;
        }
    }

    public interface INavigationalAttributeInfo<TKey, TNavigationalAttribute>
        : ITupleObjectAttributeInfo<TNavigationalAttribute>
        where TNavigationalAttribute : new()
    {
        public IAttributeComponentFactory<TKey> 
            KeyComponentFactory { get; }

        public IAttributeComponentFactory<TNavigationalAttribute> 
            NavigationalComponentFactory { get => ComponentFactory; }

        public AttributeSetupWizardFactoryHandler<TKey>
            KeyAttributeSetupWizardFactory
        { get; }


        public AttributeSetupWizardFactoryHandler<TNavigationalAttribute>
            NavigationalAttributeSetupWizardFactory
        { get => SetupWizardFactory; }

        public ITupleObjectAttributeInfo<TKey> SetKeyAttributeFactory(
            IAttributeComponentFactory<TKey> factory);

        public ITupleObjectAttributeInfo<TNavigationalAttribute>
            SetNavigationalAttributeFactory(
            IAttributeComponentFactory<TNavigationalAttribute> factory)
        {
            return SetFactory(factory);
        }
    }

    /*
    internal record NavigationalAttributeInfo<
        TEntity, TKey, TNavigationalAttribute>
        : AttributeInfo<
            TEntity,
            TNavigationalAttribute,
            NavigationalAttributeComponentFactory<TKey, TNavigationalAttribute>>
            //INavigationalAttributeInfo<TKey, TNavigationalAttribute>
        where TKey : new()
        where TNavigationalAttribute : new()
    {
        public AttributeName KeyAttributeName { get => KeyAttributeMember.Name; }

        public MemberInfo KeyAttributeMember { get; init; }

        public Func<TEntity, TKey> KeyAttributeGetter
        { get; init; }

        public Expression<Func<TEntity, TNavigationalAttribute>>
            NavigationalAttributeGetterExpr
        { get; private set; }

        public TupleObject<TNavigationalAttribute> ReferencedKb
        { get; private set; }

        public AttributeSetupWizardFactoryHandler<TKey>
            KeyAttributeSetupWizardFactory
        { get; }

        private static TupleObjectFactory _toFactory = new(null);

        public NavigationalAttributeInfo(
            Expression<Func<TEntity, TKey>>
                keyAttrGetterExpr,
            Expression<Func<TEntity, TNavigationalAttribute>>
                navAttrGetterExpr,
            TupleObject<TNavigationalAttribute> source,
            Func<AttributeDomain<TNavigationalAttribute>, IAttributeComponentFactory<TNavigationalAttribute>>
            navAttrFactory,
            Expression<Func<TNavigationalAttribute, TKey>> principalKeySelector,
            AttributeSetupWizardFactoryHandler<TNavigationalAttribute> setupWizardFactory = null,
            bool hasEquivalenceRelation = false)
            : base(
                  navAttrGetterExpr,
                  CreateFactory(source, principalKeySelector),
                  setupWizardFactory,
                  hasEquivalenceRelation)
        {
            KeyAttributeGetter = keyAttrGetterExpr.Compile();
            KeyAttributeMember = MemberExtractor.ExtractFrom(keyAttrGetterExpr);
            NavigationalAttributeGetterExpr = navAttrGetterExpr;
            ReferencedKb = source;

            return;
        }

        private static NavigationalAttributeComponentFactory<TKey, TNavigationalAttribute>
            CreateFactory(
            TupleObject<TNavigationalAttribute> source,
            Expression<Func<TNavigationalAttribute, TKey>> principalKeySelector)
        {
            return null;
        }

        /*
        private AttributeComponent<TNavigationalAttribute>
            NavigateByKey(AttributeComponent<TKey> ac)
        {
            var builder = _toFactory.GetDefaultBuilder<TNavigationalAttribute>();
            var tupleRes = ReferencedKb & _toFactory.CreateConjunctiveTuple(
                [new NamedComponentFactoryArgs<IAttributeComponent>(
                    KeyAttributeName, builder, ac)],
                null,
                builder);

            return ComponentFactory.SelectValue(tupleRes);
        }
    }
        */

    /*
    public record struct NavigationalAttributeMemberInfo<TEntity, TAttribute>
        (Expression<Func<TEntity, TAttribute>> AttributeGetterExpr)
    { }
    */

    internal class NavigationalPropertyMemberInfo
        : MemberInfo
    {
        private Type _propertyType;

        public override string Name
        {
            get => $"{ForeignKeyMemberInfo.Name}:{NavigationalAttributeMemberInfo.Name}";
        }

        public override MemberTypes MemberType
        {
            get => MemberTypes.Custom;
        }

        public override Type ReflectedType
        {
            get => NavigationalAttributeMemberInfo.ReflectedType;
        }

        public override Type DeclaringType
        {
            get => NavigationalAttributeMemberInfo.DeclaringType;
        }

        public Type PropertyType
        {
            get => _propertyType = typeof(KeyValuePair<,>)
                .MakeGenericType(
                GetMemberType(ForeignKeyMemberInfo),
                GetMemberType(NavigationalAttributeMemberInfo));
        }

        public MemberInfo ForeignKeyMemberInfo { get; private set; }

        public MemberInfo NavigationalAttributeMemberInfo { get; private set; }

        public NavigationalPropertyMemberInfo(
            MemberInfo foreignKeyMemberInfo,
            MemberInfo navigationalAttributeMemberInfo)
        {
            ForeignKeyMemberInfo = foreignKeyMemberInfo;
            NavigationalAttributeMemberInfo = navigationalAttributeMemberInfo;

            return;
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            return ForeignKeyMemberInfo.IsDefined(attributeType, inherit) ||
                NavigationalAttributeMemberInfo.IsDefined(attributeType, inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public Type GetMemberType(MemberInfo mi)
        {
            return mi switch
            {
                FieldInfo fi => fi.FieldType,
                PropertyInfo pi => pi.PropertyType,
                _ => throw new Exception()
            };
        }
    }

    internal record NavigationalAttributeWithSimpleForeignKeyInfo<
        TEntity, TKey, TPrincipalKey, TNavigationalAttribute>
        : AttributeInfo<
            TEntity,
            KeyValuePair<TKey, TNavigationalAttribute>,
            INavigationalAttributeComponentFactory<TKey, TNavigationalAttribute>>
        //NavigationalAttributeComponentWithSimpleForeignKeyFactory<TKey, TNavigationalAttribute>>//,
        //INavigationalAttributeInfo<TKey, TNavigationalAttribute>
        where TKey : new()
        where TNavigationalAttribute : new()
    {
        /*
        public override AttributeName Name 
        { 
            get => $"{ForeignKeyAttributeName}:{ValueAttributeName}"; 
        }
        */

        public AttributeName ForeignKeyAttributeName { get => ForeignKeyAttributeMember.Name; }

        public MemberInfo ForeignKeyAttributeMember { get; init; }

        public Func<TEntity, TKey> ForeignKeyAttributeGetter
        { get; init; }

        public AttributeName ValueAttributeName { get => ValueAttributeMember.Name; }

        public MemberInfo ValueAttributeMember { get; init; }

        public Func<TEntity, TNavigationalAttribute> ValueAttributeGetter
        { get; init; }

        public AttributeName PrincipalKeyAttributeName { get; init; }

        public TupleObject<TNavigationalAttribute> ReferencedKb
        { get; private set; }

        public AttributeSetupWizardFactoryHandler<TKey>
            ForeignKeyAttributeSetupWizardFactory
        { get; }

        //private NavigationalAttributeWithSimpleForeignKeyInfo<
        //    TEntity, TKey, TNavigationalAttribute>[] _siblings;

        private int _siblingId;

        private static TupleObjectFactory _toFactory = new(null);

        private IEnumerableNonFictionalAttributeComponentFactory<TPrincipalKey> 
            _principalKeyFactory;

        public NavigationalAttributeWithSimpleForeignKeyInfo(
            Expression<Func<TEntity, TKey>>
                foreignKeyAttrGetterExpr,
            Expression<Func<TEntity, TNavigationalAttribute>>
                navAttrGetterExpr,
            TupleObject<TNavigationalAttribute> referencedKb,
            Expression<Func<TNavigationalAttribute, TKey>> principalKeySelectorExpr,
            Func<IEnumerable<TPrincipalKey>, IAttributeComponentFactory<TKey>>
            keyFactory,
            Func<IEnumerable<TNavigationalAttribute>, IAttributeComponentFactory<TNavigationalAttribute>>
            navAttrFactory = null,
            AttributeSetupWizardFactoryHandler<KeyValuePair<TKey, TNavigationalAttribute>>
            setupWizardFactory = null,
            bool hasEquivalenceRelation = false)
            : base(
                  LambdaExpressionHelper.ProduceKeyValuePairGetter<
                      TEntity, TKey, TNavigationalAttribute>(
                      foreignKeyAttrGetterExpr,
                      navAttrGetterExpr),
                  CreateFactory(
                      referencedKb, 
                      principalKeySelectorExpr,
                      keyFactory,
                      navAttrFactory),
                  setupWizardFactory,
                  hasEquivalenceRelation)
        {
            (
                ForeignKeyAttributeMember, 
                ValueAttributeMember,
                ForeignKeyAttributeGetter,
                ValueAttributeGetter) =
                DeconstructFromKeyValuePairGetter();
            ReferencedKb = referencedKb;
            PrincipalKeyAttributeName = GetMember(principalKeySelectorExpr).Name;
            AttributeMember = new NavigationalPropertyMemberInfo(
                ForeignKeyAttributeMember,
                ValueAttributeMember);

            var principalKeySelector = principalKeySelectorExpr.Compile();
            var navigateByKey = NavigationalAttributeComponentHelper
                .CreateNavigationBySimpleForeignKey<TKey, TNavigationalAttribute>(
                    referencedKb,
                    ToEntityTuple,
                    ToEntityComponent);
            Helper.RegisterNavigational<
                TKey,
                TNavigationalAttribute,
                NavigationalAttributeComponent<TKey, TNavigationalAttribute>>(
                ComponentFactory,
                navigateByKey,
                principalKeySelector,
                NavigationalAttributeComponentHelper.CreateSimplePrincipalKeySelection(
                    ComponentFactory.KeyAttributeComponentFactory as IEnumerableNonFictionalAttributeComponentFactory<TKey>,
                    principalKeySelector));
            ComponentFactory.PrincipalKeySelector = principalKeySelector;
            ComponentFactory.NavigateByKey = navigateByKey;

            var builder = _toFactory.GetBuilder<TNavigationalAttribute>(
                referencedKb.Schema.PassToBuilder);
            _principalKeyFactory =
                builder.Attribute((AttributeName)principalKeySelectorExpr)
                .GetFactory<TPrincipalKey>()
                as IEnumerableNonFictionalAttributeComponentFactory<TPrincipalKey>;

            return;
        }

        private (
            MemberInfo KeyMember, 
            MemberInfo ValueMember,
            Func<TEntity, TKey> KeyGetter,
            Func<TEntity, TNavigationalAttribute> ValueGetter) 
            DeconstructFromKeyValuePairGetter()
        {
            ParameterExpression entityParamExpr = _attributeGetterExpr.Parameters[0];
            NewExpression kvpNew = _attributeGetterExpr.Body as NewExpression;
            MemberExpression keyMemberExpr = 
                kvpNew.Arguments[0] as MemberExpression,
                             valueMemberExpr =
                kvpNew.Arguments[1] as MemberExpression;
            Expression<Func<TEntity, TKey>> keyGetterExpr =
                Expression.Lambda<Func<TEntity, TKey>>(
                    keyMemberExpr, entityParamExpr);
            Expression<Func<TEntity, TNavigationalAttribute>> valueGetterExpr =
                Expression.Lambda<Func<TEntity, TNavigationalAttribute>>(
                    valueMemberExpr, entityParamExpr);

            return (
                keyMemberExpr.Member, 
                valueMemberExpr.Member, 
                keyGetterExpr.Compile(),
                valueGetterExpr.Compile());
        }

        private TupleObject<TNavigationalAttribute> ToEntityTuple(
            TupleObjectFactory toFactory,
            AttributeComponent<TKey> foreignKey)
        {
            var builder = toFactory.GetBuilder<TNavigationalAttribute>(
                ReferencedKb.Schema.PassToBuilder);
            var farg = new NamedComponentFactoryArgs<IAttributeComponent>(
                PrincipalKeyAttributeName,
                builder,
                _principalKeyFactory.CreateNonFictional(
                    Enumerable.Cast<TPrincipalKey>(foreignKey)));

            return toFactory.CreateConjunctiveTuple(
                [farg], 
                null, 
                builder);
        }

        private AttributeComponent<TNavigationalAttribute> ToEntityComponent(
            TupleObject<TNavigationalAttribute> navComponent)
        {
            var farg = 
                new NavigationalAttributeComponentFactoryArgs<TKey, TNavigationalAttribute>(
                navComponent);

            return ComponentFactory.CreateValueAttributeComponent(farg);
        }

        private static NavigationalAttributeComponentFactory<TKey, TNavigationalAttribute>
            CreateFactory(
            TupleObject<TNavigationalAttribute> referencedKb,
            Expression<Func<TNavigationalAttribute, TKey>> principalKeySelector,
            Func<IEnumerable<TPrincipalKey>, IAttributeComponentFactory<TKey>>
            keyFactory = null,
            Func<IEnumerable<TNavigationalAttribute>, IAttributeComponentFactory<TNavigationalAttribute>>
            navAttrFactory = null)
        {
            var builder = _toFactory.GetBuilder<TNavigationalAttribute>(
                referencedKb.Schema.PassToBuilder);
            IAttributeComponentFactory<TPrincipalKey> foreignKeyFactoryValue =
                builder.Attribute((AttributeName)principalKeySelector)
                .GetFactory<TPrincipalKey>();
            if (foreignKeyFactoryValue is null)
            {
                throw new Exception("principalKeySelector is not an instance of Expression<Func<TData, TForeignKey>>.");
            }

            keyFactory ??= (_) =>
            {
                return foreignKeyFactoryValue
                    as IAttributeComponentFactory<TKey>;
            };

            Func<IAttributeComponentFactory<TKey>> foreignKeyFactory = () =>
            {
                return keyFactory(foreignKeyFactoryValue.Domain);
            };

            NavigationalAttributeComponentFactory<TKey, TNavigationalAttribute> factory;
            if (navAttrFactory is not null)
            {
                factory = new NavigationalAttributeComponentFactory<TKey, TNavigationalAttribute>(
                    foreignKeyFactory(),
                    navAttrFactory(referencedKb));
            }
            else
            {
                factory = new NavigationalAttributeComponentFactory<TKey, TNavigationalAttribute>(
                    foreignKeyFactory(),
                    referencedKb);
            }

            return factory;


        }

        /*
        public void SetSiblings(
            NavigationalAttributeWithSimpleForeignKeyInfo<
            TEntity, TKey, TNavigationalAttribute>[] siblings,
            int siblingId)
        {
            _siblings = siblings;
            _siblingId = siblingId;

            return;
        }

        public override void AttachQuery()
        {
            base.AttachQuery();
            PropagateQueryToSiblings();

            return;
        }

        public override void DetachQuery()
        {
            base.DetachQuery();
            PropagateQueryToSiblings();

            return;
        }

        public override void RemoveQuery()
        {
            base.RemoveQuery();
            PropagateQueryToSiblings();

            return;
        }

        private void PropagateQueryToSiblings()
        {
            foreach (var sibling in _siblings)
            {
                if (sibling._siblingId == _siblingId) continue;

                sibling.TakeQuery(Query);
            }
        }

        private void TakeQuery(AttributeQuery query)
        {
            Query = query;

            return;
        }
        */
    }

    /*
    public record NavigationalAttributeWithComplexKeyInfo<
        TEntity, 
        TKey, 
        TNavigationalAttribute>
        : AttributeInfo<TEntity, TNavigationalAttribute>
        where TKey : new()
        where TNavigationalAttribute : new()
    {
        public TupleObject<TNavigationalAttribute> ReferencedKb
        { get; private set; }

        public IEnumerable<AttributeName> ForeignKeyCompoundNames { get; private set; }

        public MemberInfo[] ForeignKeyCompoundMembers { get; private set; }

        public Func<TEntity, dynamic>[] ForeignKeyCompoundAttributeGetters 
        { get; private set; } 

        public NavigationalAttributeWithComplexKeyInfo(
            TupleObjectSchema<TKey> keySchema,
            Expression<Func<TEntity, TKey>>
                foreignKeyAttrGetterExpr,
            Expression<Func<TEntity, TNavigationalAttribute>>
                navAttrGetterExpr,
            TupleObject<TNavigationalAttribute> referencedKb,
            Func<AttributeDomain<TNavigationalAttribute>, IAttributeComponentFactory<TNavigationalAttribute>>
            navAttrFactory,
            AttributeSetupWizardFactoryHandler<TNavigationalAttribute> setupWizardFactory = null,
            bool hasEquivalenceRelation = false)
            : base(
                  navAttrGetterExpr,
                  componentFactory,
                  setupWizardFactory,
                  hasEquivalenceRelation)
        {
            ReferencedKb = referencedKb;
            Deconstruct(foreignKeyAttrGetterExpr);

            return;
        }

        private void Deconstruct(
            Expression<Func<TEntity, TKey>> foreignKeyGetterExpr)
        {
            var foreignKeyCompoundExprs = 
                (foreignKeyGetterExpr.Body as NewExpression).Arguments;
            ForeignKeyCompoundMembers = 
                new MemberInfo[foreignKeyCompoundExprs.Count];
            ForeignKeyCompoundAttributeGetters =
                new Func<TEntity, dynamic>[foreignKeyCompoundExprs.Count];
            int i = 0;
            LambdaExpression foreignKeyCompoundAttrGetterExpr;
            foreach (MemberAssignment maExpr 
                 in foreignKeyCompoundExprs.OfType<MemberAssignment>())
            {
                ForeignKeyCompoundMembers[i] = maExpr.Member;
                foreignKeyCompoundAttrGetterExpr = 
                    maExpr.Expression as LambdaExpression;
                ForeignKeyCompoundAttributeGetters[i] =
                    foreignKeyCompoundAttrGetterExpr.Compile() as Func<TEntity, dynamic>;
                i++;
            }

            return;
        }
    }
    */

    /*

    public enum AttributeQuery : byte
    {
        None = 0,
        Attached = 1,
        Detached = 2
    }

    /// <summary>
    /// Информация об атрибуте в составе схемы кортежа.
    /// </summary>
    public struct AttributeInfo
    {
        #region Instance fields

        /// <summary>
        /// Делегат получения значения атрибута из сущности.
        /// </summary>
        private Delegate _attributeGetter;

        /// <summary>
        /// Запрос к атрибуту: 
        /// </summary>
        private AttributeQuery _query;

        #endregion

        #region Instance properties

        /// <summary>
        /// Домен атрибута.
        /// </summary>
        public IAttributeComponentProvider Domain { get; private set; }

        /// <summary>
        /// Флаг подключения атрибута к схеме.
        /// </summary>
        public bool IsPlugged { get; private set; }

        /// <summary>
        /// Отношение эквивалентности.
        /// </summary>
        public bool HasEquivalenceRelation { get; private set; }

        /// <summary>
        /// Тип данных домена.
        /// </summary>
        public Type DomainDataType { get; private set; }

        /// <summary>
        /// Информация о свойстве сущности.
        /// </summary>
        public PropertyInfo AttributeProperty { get; init; }

        /// <summary>
        /// Настройщик.
        /// </summary>
        public object SetupWizard { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструктор экземпляра.
        /// </summary>
        /// <param name="isPlugged"></param>
        /// <param name="domain"></param>
        private AttributeInfo(
            Type domainDataType,
            bool hasEquivalenceRelation,
            bool isPlugged,
            IAttributeComponentProvider domain = null,
            Delegate attributeGetter = null)
        {
            IsPlugged = isPlugged;
            Domain = domain;
            DomainDataType = domainDataType;
            _attributeGetter = attributeGetter;
            SetupWizard = null;
        }

        #endregion

        #region Construction methods

        public static AttributeInfo Construct<TEntity, TAttribute>(
            bool hasEquivalenceRelation = false,
            bool isPlugged = false,
            AttributeDomain<TAttribute> domain = null,
            Func<TEntity, TAttribute> attributeGetter = null)
        {
            return new AttributeInfo(
                domainDataType: typeof(TAttribute),
                hasEquivalenceRelation: hasEquivalenceRelation,
                isPlugged: isPlugged,
                domain: domain,
                attributeGetter: attributeGetter);
        }

        #endregion

        #region Cloning methods

        public AttributeInfo CloneWith<TEntity, TAttribute>(
            IAttributeComponentProvider domain = null,
            Func<TEntity, TAttribute> attributeGetter = null,
            bool hasEquivalenceRelation = false)
        {
            bool isPlugged = (_query == AttributeQuery.Attached) || 
                (_query != AttributeQuery.Detached && IsPlugged);

            return this with
            {
                IsPlugged = isPlugged,
                _query = AttributeQuery.None,
                Domain = domain ?? this.Domain,
                _attributeGetter = attributeGetter ?? this._attributeGetter
            };
        }

        #endregion

        public AttributeDomain<TAttribute> GetDomain<TAttribute>()
        {
            return Domain as AttributeDomain<TAttribute>;
        }

        public IAttributeComponentProvider GetDomain()
        {
            return Domain;
        }

        public void SetDomain(IAttributeComponentProvider domain)
        {
            Domain = domain;

            return;
        }

        public ITupleObjectAttributeSetupWizard<TAttribute> GetSetupWizard<TAttribute>()
        {
            return SetupWizard as ITupleObjectAttributeSetupWizard<TAttribute>;
        }

        public void SetSetupWizard<TAttribute>(
            ITupleObjectAttributeSetupWizard<TAttribute> setupWizard)
        {
            SetupWizard = setupWizard;

            return;
        }

        public void SetAttributeGetter(Delegate attributeGetter)
        {
            _attributeGetter = attributeGetter;

            return;
        }

        public Func<TEntity, TAttribute> AttributeGetter<TEntity, TAttribute>()
        {
            return _attributeGetter as Func<TEntity, TAttribute>;
        }

        private AttributeInfo UndoQuery()
        {
            return this with { _query = AttributeQuery.None };
        }

        private AttributeInfo AttachQuery()
        {
            return this with { _query = AttributeQuery.Attached };
        }

        private AttributeInfo DetachQuery()
        {
            return this with { _query = AttributeQuery.Detached };
        }

        /// <summary>
        /// Создание запроса к атрибуту на его прикрепление.
        /// </summary>
        /// <returns></returns>
        public AttributeInfo Attach()
        {
            return (IsPlugged, _query) switch
            {
                (_, AttributeQuery.Attached) => this,
                (true, AttributeQuery.None) => this,
                (true, _) => UndoQuery(),
                (_, _) => AttachQuery(),
            };
        }

        /// <summary>
        /// Создание запроса к атрибуту на его открепление.
        /// </summary>
        /// <returns></returns>
        public AttributeInfo Detach()
        {
            return (IsPlugged, _query) switch
            {
                (_, AttributeQuery.Detached) => this,
                (false, AttributeQuery.None) => this,
                (false, _) => UndoQuery(),
                (_, _) => DetachQuery()
            };
        }

        public AttributeInfo ExecuteQuery()
        {
            if (_query == AttributeQuery.None)
                return this;


            bool isPlugged = (_query == AttributeQuery.Attached) ||
                (_query != AttributeQuery.Detached && IsPlugged);

            return this with { IsPlugged = IsPlugged, _query = AttributeQuery.None };
        }

        #region Equivalence relation set/unset methods

        private static IEqualityComparer<TAttribute> CreateEquivalenceRelationComparer<TAttribute>(
            Func<TAttribute, TAttribute, bool> equivalenceRelation) =>
            new EquivalenceRelationComparer<TAttribute>(equivalenceRelation);

        public AttributeInfo SetEquivalenceRelation<T>()
        {
            return this with
            {
                HasEquivalenceRelation = true
            };
        }

        public AttributeInfo UnsetEquivalenceRelation()
        {
            return this with
            {
                HasEquivalenceRelation = false
            };
        }

        #endregion

        #region Operators

        public static bool operator ==(AttributeInfo first, AttributeInfo second)
        {
            return first.IsPlugged == second.IsPlugged;
        }

        public static bool operator !=(AttributeInfo first, AttributeInfo second)
        {
            return !(first == second);
        }

        #endregion

        #region Nested types

        private class EquivalenceRelationComparer<T>
            : IEqualityComparer<T>
        {
            private Func<T, T, bool> _equalityComparison;

            public EquivalenceRelationComparer(Func<T, T, bool> equalityComparison)
            {
                _equalityComparison = equalityComparison;
                //_equalityComparison = Comparer<T>.Create(((Comparison<T>)null));

                return;
            }

            public bool Equals(T first, T second)
            {
                return _equalityComparison(first, second);
            }

            public int GetHashCode(T obj) => obj.GetHashCode();
        }

        #endregion
    }
    */
}
