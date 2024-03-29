﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace UniversalClassLib.HierarchicallyPolymorphicOperators
{
    using static OperatorHelper;

    public abstract class InstantBinaryOperator<TOperand1, TOperand2, TOperationResult>
    {
        #region Methods

        public TOperationResult Accept(TOperand1 first, TOperand2 second)
        {
            var v = (this.GetType()).GetInterfaces();

            return (TOperationResult)InstantBinaryOperatorAcceptImplMIPattern
                .MakeGenericMethod(typeof(TOperand1), second.GetType(), typeof(TOperationResult))
                .Invoke(null, new object[] { this, first, second });
        }

        #endregion
    }
}
