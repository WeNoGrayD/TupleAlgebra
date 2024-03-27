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
    public class SymmetricExceptionOperator
        : NonFictionalAttributeComponentSymmetricExceptionOperator<bool, bool, BooleanNonFictionalAttributeComponent, IBooleanAttributeComponentFactory, BooleanAttributeComponentFactoryArgs>,
          IBooleanAttributeComponentBinaryOperator
    {
        public IAttributeComponent<bool> Accept(
            BooleanNonFictionalAttributeComponent first,
            BooleanNonFictionalAttributeComponent second,
            IBooleanAttributeComponentFactory factory)
        {
            if (first.Value == second.Value)
            {
                return factory.CreateEmpty();
            }
            else
            {
                return factory.CreateFull();
            }
        }
    }
}
