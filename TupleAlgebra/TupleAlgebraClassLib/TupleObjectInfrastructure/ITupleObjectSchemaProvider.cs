using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectHelper;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public interface ITupleObjectAttributeInfo
    {
        public AttributeName Name { get; }

        public AttributeQuery Query { get; }

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
            SetupWizardFactory { get; }

        AttributeSetupWizardFactoryHandler
            ITupleObjectAttributeInfo.SetupWizardFactory
        { get => (schema, attrName) => SetupWizardFactory(schema, attrName); }

        public ITupleObjectAttributeInfo<TAttribute> SetFactory(
            IAttributeComponentFactory<TAttribute> factory);
    }

    public interface ITupleObjectAttributeWithEqualityRelationInfo<T>
        : ITupleObjectAttributeInfo
    {
        public Comparer<T> EqualityComparer { get; }
    }

    public interface ITupleObjectSchemaProvider
        : IEnumerable<ITupleObjectAttributeInfo>
    {
        public ITupleObjectAttributeInfo this[AttributeName attributeName] 
        { get; set; }

        public IEnumerable<AttributeName> PluggedAttributeNames { get; }

        public void AttachAttribute(AttributeName attributeName);

        public void DetachAttribute(AttributeName attributeName);

        public void RemoveAttribute(AttributeName attributeName);
    }

    /*
    public interface ITupleObjectSchemaProvider
    {
        AttributeInfo? this[string attributeName] { get; set; }
    }
    */
}
