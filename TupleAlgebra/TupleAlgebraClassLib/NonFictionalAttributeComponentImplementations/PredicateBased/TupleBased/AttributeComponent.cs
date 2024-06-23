using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.LINQ2TAFramework.AttributeComponentInfrastructure.Default;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Iterable.Finite;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.Filtering;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.PredicateBased.TupleBased;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;
using System.Runtime.CompilerServices;
using TupleAlgebraClassLib.NonFictionalAttributeComponentInfrastructure;

namespace TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased
{
    using static AttributeComponentHelper;

    public interface IAttributeComponentWithVariables 
        : IAttributeComponent
    {
        public IVariableContainer Variables
        { get; }

        public IEnumerable<IAttributeComponent> UnfoldAsEnum();
    }

    public class TupleBasedAttributeComponent<TData>
        : NonFictionalAttributeComponent<TData>, 
          IAttributeComponentWithVariables,
          IFiniteEnumerableAttributeComponent<TData>
    {
        #region Instance fields

        #endregion

        #region Instance properties

        public override bool IsLazy { get => true; }

        public ITupleObject Sample { get; private set; }

        public ITupleObject Mask { get; private set; }

        public AttributeName AttributeNameWithinPredicate { get; }

        public IVariableContainer Variables 
        { get; private set; }

        public IEnumerable<TData> Values => Unfold();

        #endregion

        #region Constructors

        static TupleBasedAttributeComponent()
        {
            Helper.RegisterType<TData, TupleBasedAttributeComponent<TData>>(
                acFactory: (domain) => new TupleBasedAttributeComponentFactory<TData>(domain),
                setOperations: (factory) => new TupleBasedAttributeComponentOperationExecutorsContainer((ITupleBasedAttributeComponentFactory<TData>)factory));

            return;
        }

        public TupleBasedAttributeComponent(
            AttributeComponentPower power,
            ITupleObject sample,
            ITupleObject mask,
            AttributeName attrNameWithinPredicate,
            IVariableContainer variables,
            IQueryProvider queryProvider = null,
            Expression queryExpression = null)
            : base(
                  power,
                  queryProvider ?? new DefaultAttributeComponentQueryProvider(),
                  queryExpression)
        {
            Sample = sample;
            Mask = mask;
            AttributeNameWithinPredicate = attrNameWithinPredicate;
            Variables = variables;

            return;
        }

        #endregion

        #region Instance methods

        public override AttributeComponentFactoryArgs ZipInfoImpl<TReproducedData>(
            IEnumerable<TReproducedData> populatingData)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<TData> GetEnumeratorImpl()
        {
            return Unfold().GetEnumerator();
        }

        public AttributeComponent<TData> Unfold()
        {
            return (((dynamic)Mask).IntersectWith((dynamic)Sample)) switch
            {
                ITupleObject t when t.IsEmpty() => Factory.CreateEmpty(),
                ITupleObject t when t.IsFull() => Factory.CreateFull(),
                ISingleTupleObject tuple =>
                    GetCorrespondFromSingleTupleObject(tuple) as AttributeComponent<TData>,
                //ITupleObjectSystem tupleSys =>
                //    GetCorrespondFromTupleObjectSystem(tupleSys) as AttributeComponent<TData>
            };
        }

        private IAttributeComponent<TData> GetCorrespondFromSingleTupleObject(
            ISingleTupleObject tuple)
        {
            return (tuple[AttributeNameWithinPredicate] as IAttributeComponent<TData>)!;
        }

        private IAttributeComponent<TData> GetCorrespondFromTupleObjectSystem(
            ITupleObjectSystem tupleSys)
        {
            IAttributeComponent<TData> res = Factory.CreateEmpty();

            for (int tuplePtr = 0; tuplePtr < tupleSys.ColumnLength; tuplePtr++)
            {
                res = res.UnionWith(
                    GetCorrespondFromSingleTupleObject(tupleSys[tuplePtr]));
            }

            return res;
        }

        IEnumerable<IAttributeComponent<TData>> UnfoldAsEnumImpl()
        {
            IVariableContainerSnapshot variablesSnapshot;

            // предположим, что маска - всегда c-кортеж
            // сэмпл - c-кортеж или c-система

            switch (Sample)
            {
                case ISingleTupleObject tuple:
                    {
                        variablesSnapshot = Variables.SaveSnapshot();

                        switch (((dynamic)Mask).IntersectWith((dynamic)tuple))
                        {
                            case ISingleTupleObject res:
                                {
                                    yield return GetCorrespondFromSingleTupleObject(
                                        res);
                                    goto default;
                                }
                            default:
                                {
                                    Variables.LoadSnapshot(variablesSnapshot);
                                    yield break;
                                }
                        }
                    }
                case ITupleObjectSystem tupleSys:
                    {
                        variablesSnapshot = Variables.SaveSnapshot();
                        for (int tuplePtr = 0; tuplePtr < tupleSys.ColumnLength; tuplePtr++)
                        {
                            switch (((dynamic)Mask).IntersectWith((dynamic)tupleSys[tuplePtr]))
                            {
                                case ISingleTupleObject res:
                                    {
                                        yield return GetCorrespondFromSingleTupleObject(
                                            res);
                                        goto default;
                                    }
                                default:
                                    {
                                        Variables.LoadSnapshot(variablesSnapshot);
                                        continue;
                                    }
                            }
                        }

                        yield break;
                    }
            }

            yield break;
        }

        public IEnumerable<IAttributeComponent> UnfoldAsEnum()
        {
            return UnfoldAsEnumImpl();
        }

        #endregion

        #region Nested types

        private class TupleBasedAttributeComponentOperationExecutorsContainer
            : NonFictionalAttributeComponentOperationExecutorsContainer<
                TupleBasedAttributeComponent<TData>,
                TupleBasedAttributeComponentFactoryArgs<TData>,
                ITupleBasedAttributeComponentFactory<TData>,
                TupleBasedAttributeComponentFactoryArgs<TData>>
        {
            #region Constructors

            public TupleBasedAttributeComponentOperationExecutorsContainer(
                ITupleBasedAttributeComponentFactory<TData> factory)
                : base(
                      factory,
                      () => null,
                      () => new IntersectionOperator<TData>(),
                      () => null,
                      () => null,
                      () => null,
                      () => null,
                      () => null,
                      () => null)
            { }

            #endregion
        }

        #endregion
    }

    public class TupleBasedAttributeComponentPower
        : FiniteEnumerableAttributeComponentPower
    {
        #region Constructors

        static TupleBasedAttributeComponentPower()
        {
            return;
        }

        #endregion

        #region Instance methods

        protected override int CompareToZero<TData>(AttributeComponent<TData> ac)
        {
            var tupleBased = (ac as TupleBasedAttributeComponent<TData>);
            return (tupleBased.Sample.IsEmpty() || tupleBased.Mask.IsEmpty()) ? 0 : 1;
        }

        public override int CompareToNonFictional<TData>(
            AttributeComponentPower second,
            AttributeComponent<TData> ac1,
            AttributeComponent<TData> ac2)
        {
            /*
             * Если вызывается этот метод, то мощность заведомо равняется
             * мощности некоторой нефиктивной компоненты, не пустой и 
             * не полной. Определить более точно, какая мощность больше,
             * можно лишь с затратой времени и ресурсов.
             */
            return 0;
        }

        public override bool EqualsContinuum<TData>(AttributeComponent<TData> ac)
        {
            var tupleBased = (ac as TupleBasedAttributeComponent<TData>);
            return tupleBased.Sample.IsFull() && tupleBased.Mask.IsFull();
        }

        #endregion}
    }
}
