using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.TupleObjects;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.TupleBased
{
    public interface ITupleBasedAttributeComponentFactory<TData>
        : INonFictionalAttributeComponentFactory<
            TData,
            TupleBasedAttributeComponentFactoryArgs<TData>,
            TupleBasedAttributeComponent<TData>,
            TupleBasedAttributeComponentFactoryArgs<TData>>
    {
        TupleBasedAttributeComponentFactoryArgs<TData>
            ISetOperationResultFactory<
                TupleBasedAttributeComponent<TData>,
                TupleBasedAttributeComponentFactoryArgs<TData>,
                TupleBasedAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
                TupleBasedAttributeComponentFactoryArgs<TData>
                opResultFactoryArgs)
        {
            return null;
        }

        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                TupleBasedAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictional(
                TupleBasedAttributeComponentFactoryArgs<TData> args)
        {
            IVariableContainer variables = null;
            switch (args.Mask)
            {
                case ITupleObject t when t.IsEmpty(): break;// return CreateEmpty();
                case ITupleObject t when t.IsFull(): break;// return CreateFull();
                case ISingleTupleObject tuple:
                    {
                        variables = GetVariablesFromSingleTupleObject(tuple);

                        break;
                    }
                case ITupleObjectSystem tupleSys:
                    {
                        variables = GetVariablesFromTupleObjectSystem(tupleSys);

                        break;
                    }
            }
            variables ??= VariableContainer.Empty();

            return new TupleBasedAttributeComponent<TData>(
                args.Power,
                args.Sample,
                args.Mask,
                args.AttributeNameWithinPredicate,
                variables,
                args.QueryProvider,
                args.QueryExpression);

            IVariableContainer GetVariablesFromSingleTupleObject(
                ISingleTupleObject tuple)
            {
                /*
                for (int attrLoc = 0; attrLoc < tuple.RowLength; attrLoc++)
                {
                    if (tuple[attrLoc] is IVariableAttributeComponent variable)
                    {
                        if (variables is null)
                            variables = new HashSet<IVariableAttributeComponent>();
                        variables.Add(variable);
                    }
                }

                return;
                */
                return tuple.VariableContainer;
            }

            IVariableContainer GetVariablesFromTupleObjectSystem(
                ITupleObjectSystem tupleSys)
            {
                VariableContainer container = new VariableContainer();
                for (int tuplePtr = 0; tuplePtr < tupleSys.ColumnLength; tuplePtr++)
                {
                    container.AddVariableRange(
                        GetVariablesFromSingleTupleObject(tupleSys[tuplePtr]));
                }

                return container;
            }
        }

        TupleBasedAttributeComponentFactoryArgs<TData>
            ISetOperationResultFactory<
                TupleBasedAttributeComponent<TData>,
                TupleBasedAttributeComponentFactoryArgs<TData>,
                TupleBasedAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
            TupleBasedAttributeComponent<TData> first,
            TupleBasedAttributeComponentFactoryArgs<TData> resultElements)
        {
            throw new NotImplementedException();
        }
    }

    public class TupleBasedAttributeComponentFactory<TData>
        : AttributeComponentFactory<TData>,
          ITupleBasedAttributeComponentFactory<TData>
    {
        public TupleBasedAttributeComponentFactory(
            AttributeDomain<TData> domain)
            : base(domain)
        { return; }
    }
}
