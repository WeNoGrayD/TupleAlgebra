using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.Assertions
{
    public enum TAUnaryOperator
    {
        ComplementOf
    }
    public enum TABinaryOperator
    {
        IntersectWith,
        UnionWith,
        SymmetricExceptWith,
        ExceptWith
    }

    [Flags]
    public enum TABinaryRelation
    {
        EqualsTo,
        NotEqualsTo,
        GreaterThan,
        LesserThan,
        PartiallyOrthogonalTo,
        FullyOrthogonalTo
    }

    /// <summary>
    /// База знаний в определённом АК-контексте.
    /// Содержит знания об отношениях АК-объектов и операциях над АК-объектами
    /// для ускоренной обработки некоторых других операций.
    /// Дочерние классы можно ориентировать на другие сферы знаний.
    /// </summary>
    public class Assertions
    {
        public void AssertUnaryOperation<TEntity>(
            TupleObject<TEntity> operand1,
            TAUnaryOperator unaryOp)
            where TEntity : new()
        {
            (object Operand1, TAUnaryOperator Operator) assertion = (operand1, unaryOp);
        }

        public void AssertBinaryOperation<TEntity>(
            TupleObject<TEntity> operand1,
            TupleObject<TEntity> operand2,
            TABinaryOperator binaryOp)
            where TEntity : new()
        {
            (object Operand1, object Operand2, TABinaryOperator Operator) assertion =
                (operand1, operand2, binaryOp);
        }

        public void AssertBinaryRelation<TEntity>(
            TupleObject<TEntity> operand1,
            TupleObject<TEntity> operand2,
            TABinaryRelation binaryRel)
            where TEntity : new()
        {
            (object Operand1, object Operand2, TABinaryRelation Operator) assertion =
                (operand1, operand2, binaryRel);
        }
    }
}
