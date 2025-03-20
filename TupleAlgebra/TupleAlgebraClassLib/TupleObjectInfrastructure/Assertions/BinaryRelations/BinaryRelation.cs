using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure.Assertions.BinaryRelations
{
    public class BinaryRelation<TEntity>
        where TEntity : new()
    {
        public TupleObject<TEntity> Operand1 { get; set; }

        public TupleObject<TEntity> Operand2 { get; set; }

        public TABinaryRelation Relation { get; set; }

        protected BinaryRelation(
            TupleObject<TEntity> operand1,
            TupleObject<TEntity> operand2,
            TABinaryRelation relation)
        {
            Operand1 = operand1;
            Operand2 = operand2;
            Relation = relation;

            return;
        }
    }

    /// <summary>
    /// Утверждение о том, что два операнда равны.
    /// Даёт полную информацию об отношении двух операндов.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EqualityRelation<TEntity> : BinaryRelation<TEntity>
        where TEntity : new()
    {
        public EqualityRelation(
            TupleObject<TEntity> operand1,
            TupleObject<TEntity> operand2)
            : base(operand1, operand2, TABinaryRelation.EqualsTo)
        { return; }
    }

    /// <summary>
    /// Утверждение о том, что два операнда неравны.
    /// Не может сообщить больше информации.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class InequalityRelation<TEntity> : BinaryRelation<TEntity>
        where TEntity : new()
    {
        public InequalityRelation(
            TupleObject<TEntity> operand1,
            TupleObject<TEntity> operand2)
            : base(operand1, operand2, TABinaryRelation.NotEqualsTo)
        { return; }
    }

    /// <summary>
    /// Утверждение о том, что два операнда имеют пересечение, но один не включает другой и наоборот.
    /// Даёт частичную информацию об отношении двух операндов.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class LessThanRelation<TEntity> : BinaryRelation<TEntity>
        where TEntity : new()
    {
        public LessThanRelation(
            TupleObject<TEntity> operand1,
            TupleObject<TEntity> operand2)
            : base(operand1, operand2, TABinaryRelation.PartiallyOrthogonalTo)
        { return; }
    }

    /// <summary>
    /// Утверждение о том, что два операнда имеют пересечение, но один не включает другой и наоборот.
    /// Даёт частичную информацию об отношении двух операндов.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PartialOrthogonalityRelation<TEntity> : BinaryRelation<TEntity>
        where TEntity : new()
    {
        public PartialOrthogonalityRelation(
            TupleObject<TEntity> operand1,
            TupleObject<TEntity> operand2)
            : base(operand1, operand2, TABinaryRelation.PartiallyOrthogonalTo)
        { return; }
    }

    /// <summary>
    /// Утверждение о том, что два операнда не имеют пересечений.
    /// Даёт полную информацию об отношении двух операндов.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class FullOrthogonalityRelation<TEntity> : BinaryRelation<TEntity>
        where TEntity : new()
    {
        public FullOrthogonalityRelation(
            TupleObject<TEntity> operand1,
            TupleObject<TEntity> operand2)
            : base(operand1, operand2, TABinaryRelation.FullyOrthogonalTo)
        { return; }
    }
}
