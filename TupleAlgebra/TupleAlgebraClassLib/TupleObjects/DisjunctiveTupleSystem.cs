using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;

    public class DisjunctiveTupleSystem<TEntity> 
        : TupleObjectSystem<TEntity, DisjunctiveTuple<TEntity>>
        where TEntity : new()
    {
        public DisjunctiveTupleSystem(TupleObjectBuildingHandler<TEntity> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        public DisjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            int len)
            : base(schema, len)
        { }

        public DisjunctiveTupleSystem(
            TupleObjectSchema<TEntity> schema,
            IList<SingleTupleObject<TEntity>> tuples)
            : base(schema, tuples)
        { }

        public override IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory)
        {
            return factory.CreateEmpty();
        }

        public override TupleObject<TEntity> Reproduce(
            IEnumerable<TupleObject<TEntity>> tuples,
            TupleObjectFactory factory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder)
        {
            return factory.CreateDisjunctive(tuples, onTupleBuilding, builder);
        }

        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            return null;
        }

        public override TupleObject<TEntity> Convert(TupleObject<TEntity> diagonal)
        {
            throw new NotImplementedException();
        }

        public override TupleObject<TEntity> Diagonal()
        {
            throw new NotImplementedException();
        }

        public TupleObject<TEntity> ToCTuple(
            TupleObjectFactory factory)
        {
            return factory.CreateConjunctive<TEntity>(Factory().ToArray());

            IEnumerable<ISingleTupleObjectFactoryArgs> Factory()
            {
                Func<IEnumerator[], ISingleTupleObjectFactoryArgs> entityFactory 
                    = null;
                IEnumerator[] componentsEnumerators = null;
                int startComponentLoc = 0;
                Stack<IEnumerator> componentsStack = null;
                int stackPtr = startComponentLoc,
                    componentsCount = componentsEnumerators.Length - stackPtr;
                IEnumerator componentEnumerator;

                if (componentsStack is null)
                    componentsStack = new Stack<IEnumerator>(componentsCount);
                componentsStack.Push(componentEnumerator = componentsEnumerators[stackPtr]);

                int[] js = new int[_tuples.Length];
                int j;

                for (int k = 0; k < _tuples.Length * Schema.Count; k++)
                {
                    for (int i = 0; i < _tuples.Length; i++)
                    {
                        if ((j = k - i) < 0)
                        { }
                    }
                }

                do
                {
                    if (componentEnumerator.MoveNext())
                    {
                        if (stackPtr == componentsCount)
                        {
                            yield return entityFactory(componentsEnumerators);
                        }
                        else
                        {
                            componentsStack.Push(componentsEnumerators[++stackPtr]);
                        }
                    }
                    else
                    {
                        stackPtr--;
                        /*
                         * Указатель стека переходит на перечислитель предыдущего атрибута,
                         * а перечислитель текущего атрибута перезапускается, чтобы
                         * в следующий раз перебор начался сначала.
                         */
                        componentsStack.Pop().Reset();
                    }
                }
                while (componentsStack.TryPeek(out componentEnumerator));

                yield break;
            }
        }
    }
}
