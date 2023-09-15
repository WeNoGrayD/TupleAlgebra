﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.AttributeComponentAcceptors;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Boolean;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Boolean
{
    internal class IntersectionOperator
        : NonFictionalAttributeComponentIntersectionOperator<bool, BooleanNonFictionalAttributeComponent>,
          IFactoryBinaryAttributeComponentAcceptor<bool, BooleanNonFictionalAttributeComponent, BooleanNonFictionalAttributeComponent, AttributeComponent<bool>>
    {
        public AttributeComponent<bool> Accept(
            BooleanNonFictionalAttributeComponent first,
            BooleanNonFictionalAttributeComponent second,
            AttributeComponentFactory factory)
        {
            if (first.Value == second.Value)
            {
                return first;
            }
            else
            {
                return factory.CreateEmpty<bool>(first.GetDomain);
            }
        }
    }
}