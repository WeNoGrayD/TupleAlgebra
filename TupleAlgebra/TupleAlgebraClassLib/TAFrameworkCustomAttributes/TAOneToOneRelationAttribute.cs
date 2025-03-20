using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TAFrameworkCustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class TAOneToOneRelationAttribute : Attribute
    {
        public TAOneToOneRelationAttribute()
        {
        }
    }
}
