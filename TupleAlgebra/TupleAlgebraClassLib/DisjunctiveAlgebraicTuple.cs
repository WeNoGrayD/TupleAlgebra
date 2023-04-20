using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AlgebraicTupleInfrastructure;

namespace TupleAlgebraClassLib
{
    public class DisjunctiveAlgebraicTuple<TEntity> : AlgebraicTuple<TEntity>
    {
        public DisjunctiveAlgebraicTuple(Action<AlgebraicTupleBuilder<TEntity>> onTupleBuilding)
            : base(onTupleBuilding)
        { }

        public override AlgebraicTuple<TEntity> Convert(AlgebraicTuple<TEntity> diagonal)
        {
            throw new NotImplementedException();
        }

        public override AlgebraicTuple<TEntity> Diagonal()
        {
            throw new NotImplementedException();
        }
    }
}
