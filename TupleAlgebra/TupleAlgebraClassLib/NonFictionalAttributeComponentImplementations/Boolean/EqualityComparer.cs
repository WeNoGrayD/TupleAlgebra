﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    public class EqualityComparer
        : NonFictionalAttributeComponentEqualityComparer<bool, BooleanNonFictionalAttributeComponent>,
          IBooleanAttributeComponentBooleanOperator
    {
        public bool Accept(
            BooleanNonFictionalAttributeComponent first,
            BooleanNonFictionalAttributeComponent second)
        {
            return first.Value == second.Value;
        }
    }
}
