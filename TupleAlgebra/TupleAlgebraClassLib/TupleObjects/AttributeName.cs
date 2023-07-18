using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjects
{
    public struct AttributeName
    {
        private string _value;

        public AttributeName(string value)
        {
            _value = value;
        }

        public static implicit operator AttributeName(string name)
        {
            return new AttributeName(name);
        }

        public static implicit operator string(AttributeName name)
        {
            return name._value;
        }
    }
}
