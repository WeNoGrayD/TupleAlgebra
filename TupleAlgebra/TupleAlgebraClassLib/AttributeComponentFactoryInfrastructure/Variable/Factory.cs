using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;

namespace TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable
{
    public interface IVariableAttributeComponentFactory<TData>
        : INonFictionalAttributeComponentFactory<
            TData,
            VariableAttributeComponentFactoryArgs<TData>,
            VariableAttributeComponent<TData>,
            VariableAttributeComponentFactoryArgs<TData>>
    {
        VariableAttributeComponentFactoryArgs<TData>
            ISetOperationResultFactory<
                VariableAttributeComponent<TData>,
                VariableAttributeComponentFactoryArgs<TData>,
                VariableAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
                VariableAttributeComponentFactoryArgs<TData>
                opResultFactoryArgs)
        {
            return null;
        }

        NonFictionalAttributeComponent<TData>
            INonFictionalAttributeComponentFactory2<
                TData,
                VariableAttributeComponentFactoryArgs<TData>>
            .CreateSpecificNonFictional(
                VariableAttributeComponentFactoryArgs<TData> args)
        {
            return new VariableAttributeComponent<TData>(
                args.Power,
                args.Name,
                args.QueryProvider,
                args.QueryExpression);
        }

        VariableAttributeComponentFactoryArgs<TData>
            ISetOperationResultFactory<
                VariableAttributeComponent<TData>,
                VariableAttributeComponentFactoryArgs<TData>,
                VariableAttributeComponentFactoryArgs<TData>,
                AttributeComponent<TData>>
            .CreateFactoryArgs(
            VariableAttributeComponent<TData> first,
            VariableAttributeComponentFactoryArgs<TData> resultElements)
        {
            return null;
        }
    }

    public class VariableAttributeComponentFactory<TData>
        : AttributeComponentFactory<TData>,
          IVariableAttributeComponentFactory<TData>
    {
        public VariableAttributeComponentFactory(
            AttributeDomain<TData> domain)
            : base(domain)
        { return; }
    }
}
