using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.Annotations
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CalculatedPropertyAttribute : Attribute
    {
        public AttributeName[] RelatedMembers { get; private set; }

        public CalculatedPropertyAttribute(string[] relatedMembers)
        {
            RelatedMembers = relatedMembers
                .Select<string, AttributeName>(rm => rm)
                .ToArray();

            return;
        }
    }

    public class MockType
    {
        public int a, b;

        [CalculatedProperty([
            nameof(a),
            nameof(b)])]
        public int C { get => a + b; }
    }
}
