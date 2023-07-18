using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public class AttributeChangedEventArgs
    {
        public readonly AttributeName AttributeName;

        public readonly AttributeInfo Attribute;

        public readonly Event ChangingEvent;

        public AttributeChangedEventArgs(AttributeName attributeName, AttributeInfo attribute, Event changingEvent)
        {
            AttributeName = attributeName;
            Attribute = attribute;
            ChangingEvent = changingEvent;
        }

        #region Enums

        public enum Event
        {
            Attachment,
            Detachment,
            Deletion
        }

        #endregion
    }
}
