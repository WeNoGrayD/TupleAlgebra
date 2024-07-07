using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public interface ITupleObjectAttributeManager
    {
        public bool IsEmpty(ISingleTupleObject tuple);

        public bool IsFull(ISingleTupleObject tuple);

        public IVariableAttributeComponent CreateVariable(string name);

        public IAttributeComponent GetComponent(
            System.Linq.Expressions.Expression factoryArgs);

        public ITupleObjectAttributeManager SetComponent(
            ISingleTupleObject tuple,
            IAttributeComponent ac);

        public ITupleObjectAttributeManager SetComponent(
            ISingleTupleObject tuple,
            IAttributeComponentFactoryArgs factoryArgs);

        public ITupleObjectAttributeManager SetComponent(
            IQueriedSingleTupleObject tuple,
            System.Linq.Expressions.Expression factoryArgs);

        public ITupleObjectAttributeManager SetComponentWithComplementionAccumulation(
            ISingleTupleObject tuple,
            IAttributeComponent component);

        public ITupleObjectAttributeManager SetComponentWithComplementionAccumulation(
            ISingleTupleObject tuple,
            IAttributeComponentFactoryArgs factoryArgs);

        public ITupleObjectAttributeManager SetDefaultFictionalAttributeComponent(
            ISingleTupleObject tuple);

        public ITupleObjectAttributeManager SetDefaultFictionalAttributeComponent(
            IQueriedSingleTupleObject tuple);

        public ITupleObjectAttributeManager
                SetComponentToProjectionOfOntoMember<TEntity>(
                    ISingleTupleObject tuple,
                    TEntity entity,
                    bool withTrailingComplement = false);

        public ITupleObjectAttributeManager
                SetComponentToProjectionOfOntoMember<TEntity>(
                    ISingleTupleObject tuple,
                    IEnumerable<TEntity> entitySet);
    }
}
