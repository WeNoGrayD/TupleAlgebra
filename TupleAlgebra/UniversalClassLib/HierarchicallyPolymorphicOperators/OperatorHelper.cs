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

        public static readonly MethodInfo InstantBinaryOperatorVisitImplMIPattern;

        public static readonly MethodInfo FactoryBinaryOperatorVisitImplMIPattern;

        #endregion

        #region Constructors

        static OperatorHelper()
        {
            InstantBinaryOperatorVisitImplMIPattern = typeof(OperatorHelper)
                .GetMethod(nameof(InstantBinaryOperatorVisitImpl), BindingFlags.NonPublic | BindingFlags.Static);
            FactoryBinaryOperatorVisitImplMIPattern = typeof(OperatorHelper)
                .GetMethod(nameof(FactoryBinaryOperatorVisitImpl), BindingFlags.NonPublic | BindingFlags.Static);

            return;
        }

        #endregion

        #region Static methods

        private static TOperationResult InstantBinaryOperatorVisitImpl<TOperand1, DTOperand2, TOperationResult>(
            IInstantBinaryOperator<TOperand1, DTOperand2, TOperationResult> ibOperator,
            TOperand1 first,
            DTOperand2 second)
        {
            return ibOperator.Visit(first, second);
        }

        private static TOperationResult FactoryBinaryOperatorVisitImpl<TOperand1, DTOperand2, TOperationResultFactory, TOperationResult>(
            IFactoryBinaryOperator<TOperand1, DTOperand2, TOperationResultFactory, TOperationResult> fbOperator,
            TOperand1 first,
            DTOperand2 second,
            TOperationResultFactory factory)
        {
            return fbOperator.Visit(first, second, factory);
        }

        #endregion
    }
}
