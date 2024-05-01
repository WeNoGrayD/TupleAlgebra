using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public struct AttributeName
    {
        private static AttributeMemberExtractor MemberExtractor =
            new AttributeMemberExtractor();

        private string _value;

        public AttributeName(string value)
        {
            _value = value;
        }

        public int CompareTo(
            AttributeName an2)
        {
            return _value.CompareTo(an2._value);
        }

        public static int Compare(
            AttributeName an1,
            AttributeName an2)
        {
            return an1.CompareTo(an2);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value;
        }

        public static implicit operator AttributeName(string name)
        {
            return new AttributeName(name);
        }

        public static implicit operator AttributeName(
            PropertyInfo attributeProperty)
        {
            return attributeProperty.Name;
        }

        public static implicit operator AttributeName(
            LambdaExpression attributeGetter)
        {
            return MemberExtractor.ExtractFrom(attributeGetter).Name;
        }

        public static implicit operator string(AttributeName name)
        {
            return name._value;
        }
    }
}
