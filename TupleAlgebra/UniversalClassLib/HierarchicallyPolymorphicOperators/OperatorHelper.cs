using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace UniversalClassLib.HierarchicallyPolymorphicOperators
{
    internal static class OperatorHelper
    {
        #region Static fields

        public static readonly MethodInfo InstantBinaryOperatorAcceptImplMIPattern;

        public static readonly MethodInfo FactoryBinaryOperatorAcceptImplMIPattern;

        #endregion

        #region Constructors

        static OperatorHelper()
        {
            InstantBinaryOperatorAcceptImplMIPattern = typeof(OperatorHelper)
                .GetMethod(nameof(InstantBinaryOperatorAcceptImpl), BindingFlags.NonPublic | BindingFlags.Static);
            FactoryBinaryOperatorAcceptImplMIPattern = typeof(OperatorHelper)
                .GetMethod(nameof(FactoryBinaryOperatorAcceptImpl), BindingFlags.NonPublic | BindingFlags.Static);

            return;
        }

        #endregion

        #region Static methods

        private static TOperationResult InstantBinaryOperatorAcceptImpl<TOperand1, DTOperand2, TOperationResult>(
            IInstantBinaryOperator<TOperand1, DTOperand2, TOperationResult> ibOperator,
            TOperand1 first,
            DTOperand2 second)
        {
            return ibOperator.Accept(first, second);
        }

        private static TOperationResult FactoryBinaryOperatorAcceptImpl<TOperand1, DTOperand2, TOperationResultFactory, TOperationResult>(
            IFactoryBinaryOperator<TOperand1, DTOperand2, TOperationResultFactory, TOperationResult> fbOperator,
            TOperand1 first,
            DTOperand2 second,
            TOperationResultFactory factory)
        {
            return fbOperator.Accept(first, second, factory);
        }

        #endregion
    }
}
