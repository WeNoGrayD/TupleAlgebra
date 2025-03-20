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

        public int GetAttributeLoc(AttributeName attrName);

        public void AttachAttribute(AttributeName attributeName);

        public void DetachAttribute(AttributeName attributeName);

        public void RemoveAttribute(AttributeName attributeName);

        public ITupleObjectAttributeSetupWizard GetSetupWizard(AttributeName attrName);

        public ITupleObjectAttributeSetupWizard GetSetupWizard(int attrLoc);
    }

    /*
    public interface ITupleObjectSchemaProvider
    {
        AttributeInfo? this[string attributeName] { get; set; }
    }
    */
}
