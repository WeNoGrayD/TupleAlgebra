using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using UniversalClassLib;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;
    using static TupleAlgebraClassLib.TupleObjectInfrastructure.TupleObjectOperators.TupleObjectFactoryMethods;

    public interface ITupleObjectSystem
    {
        public ISingleTupleObject this[int tuplePtr]
        { get; }

        public int RowLength { get; }

        public int ColumnLength { get; }
    }

    public interface ITupleObjectSystem<
        TAttributeComponent,
        TSingleTupleObject>
        where TSingleTupleObject : ISingleTupleObject<TAttributeComponent>
    {
        public TSingleTupleObject this[int tuplePtr]
        { get; set; }

        public TAttributeComponent this[int tuplePtr, int attrPtr]
        {
            get => this[tuplePtr][attrPtr];
            set => this[tuplePtr][attrPtr] = value;
        }

        public TAttributeComponent this[int tuplePtr, AttributeName attrName] 
        { 
            get => this[tuplePtr][attrName]; 
            set => this[tuplePtr][attrName] = value;
        }

        public bool IsDiagonal { get; internal set; }

        public bool IsOrthogonal { get; internal set; }

        public int RowLength { get; }

        public int ColumnLength { get; }

        public void TrimRedundantRows(int len);
    }

    public interface ITupleObjectSystem<TSingleTupleObject>
        : ITupleObjectSystem,
          ITupleObjectSystem<IAttributeComponent, TSingleTupleObject>
        where TSingleTupleObject : ISingleTupleObject
    {
        public IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory);
    }

    public abstract class TupleObjectSystem<
        TEntity, 
        TSingleTupleObject> 
        : TupleObject<TEntity>,
          ITupleObjectSystem<TSingleTupleObject>
        where TEntity : new()
        where TSingleTupleObject : SingleTupleObject<TEntity>
    {
        protected TSingleTupleObject[] _tuples;

        public TSingleTupleObject[] Tuples
        {
            get => _tuples;
        }

        public TSingleTupleObject this[int tuplePtr]
        {
            get => _tuples[tuplePtr];
            set => _tuples[tuplePtr] = value;
        }

        ISingleTupleObject ITupleObjectSystem.this[int tuplePtr]
        {
            get => this[tuplePtr];
        }

        public int RowLength { get => Schema.PluggedAttributesCount; }

        public int ColumnLength { get => _tuples.Length; }

        public bool IsDiagonal { get; set; } = false;

        public bool IsOrthogonal { get; set; } = false;

        public TupleObjectSystem(
            TupleObjectBuildingHandler<TEntity> onTupleBuilding)
            : base(onTupleBuilding)
        {
            return;
        }

        protected TupleObjectSystem(
            TupleObjectSchema<TEntity> schema,
            int len)
            : base(schema)
        {
            _tuples = new TSingleTupleObject[len];

            return;
        }

        protected TupleObjectSystem(
            TupleObjectSchema<TEntity> schema,
            IList<ISingleTupleObject> tuples)
            : base(schema)
        {
            _tuples = tuples.OfType<TSingleTupleObject>().ToArray();

            return;
        }

        public override TupleObject<TEntity> AlignWithSchema(
            TupleObjectSchema<TEntity> schema,
            TupleObjectFactory factory,
            TupleObjectBuilder<TEntity> builder = null)
        {
            if (object.ReferenceEquals(Schema, schema)) return this;

            builder = builder ?? factory.GetBuilder<TEntity>();

            return factory.CreateConjunctiveTupleSystem(
                AlignTuples(), 
                schema.PassToBuilder, 
                builder);

            IEnumerable<TupleObject<TEntity>> AlignTuples()
            {
                for (int i = 0; i < _tuples.Length; i++)
                {
                    yield return _tuples[i].AlignWithSchema(schema, factory, builder);
                }
            }
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override bool IsFull()
        {
            return false;
        }

        public void TrimRedundantRows(int len)
        {
            if (len < _tuples.Length)
                _tuples = _tuples[..len];

            return;
        }

        public abstract TupleObject<TEntity> Reproduce(
            IEnumerable<TupleObject<TEntity>> tuples,
            TupleObjectFactory factory,
            TupleObjectBuildingHandler<TEntity> onTupleBuilding,
            TupleObjectBuilder<TEntity> builder);

        /// <summary>
        /// Освобождение ресурсов 
        /// </summary>
        protected override void DisposeImpl()
        {
            //foreach (SingleTupleObject<TEntity> tuple in _tuples)
            //    tuple.Dispose();

            for (int i = 0; i < _tuples.Length; i++)
            {
                _tuples[i].Dispose();
            }

            return;
        }

        public abstract IAttributeComponent<TAttribute>
            GetDefaultFictionalAttributeComponent<TAttribute>(
                IAttributeComponentFactory<TAttribute> factory);
    }

    internal struct RectangleInfo
    {
        /// <summary>
        /// Флаг диагональности прямоугольника и его возможности
        /// оказаться пустым. (IsDiagonal == true => 100% IsNonEmpty)
        /// </summary>
        public bool IsDiagonal { get; init; }

        public int LowAttrLoc { get; init; }

        public int HighAttrLoc { get; init; }

        public IndexedComponentFactoryArgs<IAttributeComponent>[]
            Components
        { get; init; }

        public bool IsEmpty { get => LowAttrLoc < 0; }

        private IAttributeComponent DiagonalComponent
        {
            get => this[LowAttrLoc];
        }

        private IAttributeComponent this[int attrLoc]
        {
            get => Components[attrLoc].ComponentSource;
        }

        private RectangleInfo(
            IndexedComponentFactoryArgs<IAttributeComponent>[] components,
            int diagonalAttrLoc)
            : this(components, diagonalAttrLoc, diagonalAttrLoc, true)
        { }

        private RectangleInfo(
            IndexedComponentFactoryArgs<IAttributeComponent>[] components,
            int lowAttrLoc,
            int highAttrLoc)
            : this(components, lowAttrLoc, highAttrLoc, false)
        { }

        private RectangleInfo(
            IndexedComponentFactoryArgs<IAttributeComponent>[] components,
            int lowAttrLoc,
            int highAttrLoc,
            bool isDiagonal)
        {
            Components = components;
            LowAttrLoc = lowAttrLoc;
            HighAttrLoc = highAttrLoc;
            IsDiagonal = isDiagonal;

            return;
        }

        private bool ContainsAttribute(int attrLoc)
        {
            return !Components[attrLoc].IsDefault;
        }

        private RectangleInfo CloneWith(
            IndexedComponentFactoryArgs<IAttributeComponent>[] newComponents)
        {
            IndexedComponentFactoryArgs<IAttributeComponent>[] components =
                Components;
            int lowAttrLoc = LowAttrLoc;
            int highAttrLoc = HighAttrLoc;
            Array.Copy(
                components,
                lowAttrLoc,
                newComponents,
                lowAttrLoc,
                highAttrLoc - lowAttrLoc + 1);

            return new(newComponents, lowAttrLoc, highAttrLoc, IsDiagonal);
        }

        public static RectangleInfo MakeDefault(
            IndexedComponentFactoryArgs<IAttributeComponent>[] components)
        {
            return new(components, -1);
        }

        public static RectangleInfo MakeDiagonal(
            IndexedComponentFactoryArgs<IAttributeComponent>[] components,
            int diagonalAttrLoc)
        {
            return new(components, diagonalAttrLoc);
        }

        public static RectangleInfo MakeGeneral(
            IndexedComponentFactoryArgs<IAttributeComponent>[] components,
            int lowAttrLoc,
            int highAttrLoc)
        {
            return new(components, lowAttrLoc, highAttrLoc);
        }

        public static IEnumerable<RectangleInfo> FromTuple(
            ISingleTupleObject dTuple,
            IndexedComponentFactoryArgs<IAttributeComponent>[] components)
        {
            IndexedComponentFactoryArgs<IAttributeComponent> bufFarg;
            IAttributeComponent component;

            for (int attrLoc = 0; attrLoc < components.Length; attrLoc++)
            {
                component = dTuple[attrLoc];
                if (component.IsDefault)
                {
                    continue;
                    //yield return MakeDefault(components);
                }

                components[attrLoc] = new(
                    attrLoc,
                    components[attrLoc].TupleManager,
                    component);
                yield return MakeDiagonal(components, attrLoc);
            }

            yield break;
        }

        public static RectangleInfo Intersect(
            RectangleInfo first,
            RectangleInfo second)
        {
            IndexedComponentFactoryArgs<IAttributeComponent>[] resultFactoryArgs =
                second.Components;

            return (first.IsDiagonal, second.IsDiagonal) switch
            {
                (true, true) => IntersectTwoDiagonal(),
                (false, true) => IntersectGeneralAndDiagonal(first, second),
                (true, false) => IntersectGeneralAndDiagonal(second, first),
                _ => IntersectTwoGeneral()
            };

            IAttributeComponent IntersectDiagonalComponents(
                int diagonalAttrLoc)
            {
                return IntersectComponents(
                    first,
                    second,
                    diagonalAttrLoc,
                    diagonalAttrLoc);
            }

            IAttributeComponent IntersectComponents(
                RectangleInfo op1Rect,
                RectangleInfo op2Rect,
                int attrLoc1,
                int attrLoc2)
            {
                return op1Rect[attrLoc1].IntersectWith(op2Rect[attrLoc2]);
            }

            IndexedComponentFactoryArgs<IAttributeComponent> CreateFactoryArg(
                int attrLoc,
                RectangleInfo source)
            {
                return new(
                    attrLoc,
                    resultFactoryArgs[attrLoc].TupleManager,
                    source[attrLoc]);
            }

            IndexedComponentFactoryArgs<IAttributeComponent>
                CreateDiagonalFactoryArg(int attrLoc)
            {
                return new(
                    attrLoc,
                    resultFactoryArgs[attrLoc].TupleManager,
                    IntersectDiagonalComponents(attrLoc));
            }

            RectangleInfo IntersectTwoDiagonal()
            {
                int diagonalAttrLoc1 = first.LowAttrLoc,
                    diagonalAttrLoc2 = second.LowAttrLoc;

                if (diagonalAttrLoc1 == diagonalAttrLoc2)
                {
                    resultFactoryArgs[diagonalAttrLoc1] =
                        CreateDiagonalFactoryArg(diagonalAttrLoc1);

                    return MakeDiagonal(resultFactoryArgs, diagonalAttrLoc1);
                }

                resultFactoryArgs[diagonalAttrLoc1] =
                    CreateFactoryArg(diagonalAttrLoc1, first);

                int lowAttrLoc, highAttrLoc;
                if (diagonalAttrLoc1 <= diagonalAttrLoc2)
                {
                    lowAttrLoc = diagonalAttrLoc1;
                    highAttrLoc = diagonalAttrLoc2;
                }
                else
                {
                    lowAttrLoc = diagonalAttrLoc2;
                    highAttrLoc = diagonalAttrLoc1;
                }

                return MakeGeneral(
                    resultFactoryArgs,
                    lowAttrLoc,
                    highAttrLoc);
            }

            RectangleInfo IntersectGeneralAndDiagonal(
                RectangleInfo general,
                RectangleInfo diagonal)
            {
                IndexedComponentFactoryArgs<IAttributeComponent>[]
                    generalFactoryArgs = general.Components;
                int diagonalAttrLoc1 = diagonal.LowAttrLoc,
                    lowAttrLoc2 = general.LowAttrLoc,
                    highAttrLoc2 = general.HighAttrLoc,
                    lowAttrLocRes = lowAttrLoc2,
                    highAttrLocRes = highAttrLoc2;

                if (diagonalAttrLoc1 < lowAttrLoc2)
                {
                    lowAttrLocRes = diagonalAttrLoc1;
                    return SimpleIntersect();
                }
                if (diagonalAttrLoc1 > highAttrLoc2)
                {
                    highAttrLocRes = diagonalAttrLoc1;
                    return SimpleIntersect();
                }

                if (!Object.ReferenceEquals(generalFactoryArgs, resultFactoryArgs))
                {
                    Array.Copy(
                        generalFactoryArgs,
                        lowAttrLoc2,
                        resultFactoryArgs,
                        lowAttrLoc2,
                        diagonalAttrLoc1 - lowAttrLoc2);
                    lowAttrLoc2 = diagonalAttrLoc1 + 1;
                    Array.Copy(
                        generalFactoryArgs,
                        lowAttrLoc2,
                        resultFactoryArgs,
                        lowAttrLoc2,
                        highAttrLoc2 - diagonalAttrLoc1);
                }
                if (general.ContainsAttribute(diagonalAttrLoc1))
                {
                    resultFactoryArgs[diagonalAttrLoc1] =
                        CreateDiagonalFactoryArg(diagonalAttrLoc1);
                }

                return MakeGeneral(
                    resultFactoryArgs,
                    lowAttrLocRes,
                    highAttrLocRes);

                RectangleInfo SimpleIntersect()
                {
                    if (!Object.ReferenceEquals(
                        generalFactoryArgs, resultFactoryArgs))
                    {
                        Array.Copy(
                            generalFactoryArgs,
                            lowAttrLoc2,
                            resultFactoryArgs,
                            lowAttrLoc2,
                            highAttrLoc2 - lowAttrLoc2 + 1);
                    }
                    else
                    {
                        resultFactoryArgs[diagonalAttrLoc1] =
                            CreateFactoryArg(diagonalAttrLoc1, diagonal);
                    }

                    return MakeGeneral(
                        resultFactoryArgs,
                        lowAttrLocRes,
                        highAttrLocRes);
                }
            }

            RectangleInfo IntersectTwoGeneral()
            {
                int lowAttrLoc1 = first.LowAttrLoc,
                    highAttrLoc1 = first.HighAttrLoc,
                    lowAttrLoc2 = second.LowAttrLoc,
                    highAttrLoc2 = second.HighAttrLoc,
                    lowAttrLoc, highAttrLoc;

                if (lowAttrLoc1 <= lowAttrLoc2)
                {
                    lowAttrLoc = lowAttrLoc1;
                }
                else
                {
                    lowAttrLoc = lowAttrLoc2;
                }
                if (highAttrLoc1 >= highAttrLoc2)
                {
                    highAttrLoc = highAttrLoc1;
                }
                else
                {
                    highAttrLoc = highAttrLoc2;
                }

                for (int attrLoc = lowAttrLoc; attrLoc <= highAttrLoc; attrLoc++)
                {
                    switch (first.ContainsAttribute(attrLoc),
                            second.ContainsAttribute(attrLoc))
                    {
                        case (false, false): break;
                        case (true, false):
                            {
                                resultFactoryArgs[attrLoc] =
                                    CreateFactoryArg(attrLoc, first);

                                break;
                            }
                        case (false, true):
                            {
                                break;
                            }
                        case (true, true):
                            {
                                resultFactoryArgs[attrLoc] =
                                    CreateDiagonalFactoryArg(attrLoc);

                                break;
                            }
                    }
                }

                return MakeGeneral(
                    resultFactoryArgs,
                    lowAttrLoc,
                    highAttrLoc);
            }
        }

        public static RectangleInfo Union(
            RectangleInfo first,
            RectangleInfo second)
        {
            IndexedComponentFactoryArgs<IAttributeComponent>[] resultFactoryArgs =
                second.Components;

            return (first.IsDiagonal, second.IsDiagonal) switch
            {
                (true, true) => UnionTwoDiagonal(),
                (false, true) => UnionGeneralAndDiagonal(first, second),
                (true, false) => UnionGeneralAndDiagonal(second, first),
                _ => UnionTwoGeneral()
            };

            IAttributeComponent UnionDiagonalComponents(
                int diagonalAttrLoc)
            {
                return UnionComponents(
                    first,
                    second,
                    diagonalAttrLoc,
                    diagonalAttrLoc);
            }

            IAttributeComponent UnionComponents(
                RectangleInfo op1Rect,
                RectangleInfo op2Rect,
                int attrLoc1,
                int attrLoc2)
            {
                return op1Rect[attrLoc1].UnionWith(op2Rect[attrLoc2]);
            }

            IndexedComponentFactoryArgs<IAttributeComponent> CreateFactoryArg(
                int attrLoc,
                RectangleInfo source)
            {
                return new(
                    attrLoc,
                    resultFactoryArgs[attrLoc].TupleManager,
                    source[attrLoc]);
            }

            IndexedComponentFactoryArgs<IAttributeComponent>
                CreateDiagonalFactoryArg(int attrLoc)
            {
                return new(
                    attrLoc,
                    resultFactoryArgs[attrLoc].TupleManager,
                    UnionDiagonalComponents(attrLoc));
            }

            RectangleInfo UnionTwoDiagonal()
            {
                int diagonalAttrLoc1 = first.LowAttrLoc,
                    diagonalAttrLoc2 = second.LowAttrLoc;

                if (diagonalAttrLoc1 == diagonalAttrLoc2)
                {
                    resultFactoryArgs[diagonalAttrLoc1] =
                        CreateDiagonalFactoryArg(diagonalAttrLoc1);

                    return MakeDiagonal(resultFactoryArgs, diagonalAttrLoc1);
                }

                resultFactoryArgs[diagonalAttrLoc1] =
                    CreateFactoryArg(diagonalAttrLoc1, first);

                int lowAttrLoc, highAttrLoc;
                if (diagonalAttrLoc1 <= diagonalAttrLoc2)
                {
                    lowAttrLoc = diagonalAttrLoc1;
                    highAttrLoc = diagonalAttrLoc2;
                }
                else
                {
                    lowAttrLoc = diagonalAttrLoc2;
                    highAttrLoc = diagonalAttrLoc1;
                }

                return MakeGeneral(
                    resultFactoryArgs,
                    lowAttrLoc,
                    highAttrLoc);
            }

            RectangleInfo UnionGeneralAndDiagonal(
                RectangleInfo general,
                RectangleInfo diagonal)
            {
                IndexedComponentFactoryArgs<IAttributeComponent>[]
                    generalFactoryArgs = general.Components;
                int diagonalAttrLoc1 = diagonal.LowAttrLoc,
                    lowAttrLoc2 = general.LowAttrLoc,
                    highAttrLoc2 = general.HighAttrLoc,
                    lowAttrLocRes = lowAttrLoc2,
                    highAttrLocRes = highAttrLoc2;

                if (diagonalAttrLoc1 < lowAttrLoc2)
                {
                    lowAttrLocRes = diagonalAttrLoc1;
                    return SimpleUnion();
                }
                if (diagonalAttrLoc1 > highAttrLoc2)
                {
                    highAttrLocRes = diagonalAttrLoc1;
                    return SimpleUnion();
                }

                if (!Object.ReferenceEquals(generalFactoryArgs, resultFactoryArgs))
                {
                    Array.Copy(
                        generalFactoryArgs,
                        lowAttrLoc2,
                        resultFactoryArgs,
                        lowAttrLoc2,
                        diagonalAttrLoc1 - lowAttrLoc2);
                    lowAttrLoc2 = diagonalAttrLoc1 + 1;
                    Array.Copy(
                        generalFactoryArgs,
                        lowAttrLoc2,
                        resultFactoryArgs,
                        lowAttrLoc2,
                        highAttrLoc2 - diagonalAttrLoc1);
                }
                if (general.ContainsAttribute(diagonalAttrLoc1))
                {
                    resultFactoryArgs[diagonalAttrLoc1] =
                        CreateDiagonalFactoryArg(diagonalAttrLoc1);
                }

                return MakeGeneral(
                    resultFactoryArgs,
                    lowAttrLocRes,
                    highAttrLocRes);

                RectangleInfo SimpleUnion()
                {
                    if (!Object.ReferenceEquals(
                        generalFactoryArgs, resultFactoryArgs))
                    {
                        Array.Copy(
                            generalFactoryArgs,
                            lowAttrLoc2,
                            resultFactoryArgs,
                            lowAttrLoc2,
                            highAttrLoc2 - lowAttrLoc2 + 1);
                    }
                    else
                    {
                        resultFactoryArgs[diagonalAttrLoc1] =
                            CreateFactoryArg(diagonalAttrLoc1, diagonal);
                    }

                    return MakeGeneral(
                        resultFactoryArgs,
                        lowAttrLocRes,
                        highAttrLocRes);
                }
            }

            RectangleInfo UnionTwoGeneral()
            {
                int lowAttrLoc1 = first.LowAttrLoc,
                    highAttrLoc1 = first.HighAttrLoc,
                    lowAttrLoc2 = second.LowAttrLoc,
                    highAttrLoc2 = second.HighAttrLoc,
                    lowAttrLoc, highAttrLoc;

                if (lowAttrLoc1 <= lowAttrLoc2)
                {
                    lowAttrLoc = lowAttrLoc1;
                }
                else
                {
                    lowAttrLoc = lowAttrLoc2;
                }
                if (highAttrLoc1 >= highAttrLoc2)
                {
                    highAttrLoc = highAttrLoc1;
                }
                else
                {
                    highAttrLoc = highAttrLoc2;
                }

                for (int attrLoc = lowAttrLoc; attrLoc <= highAttrLoc; attrLoc++)
                {
                    switch (first.ContainsAttribute(attrLoc),
                            second.ContainsAttribute(attrLoc))
                    {
                        case (false, false): break;
                        case (true, false):
                            {
                                resultFactoryArgs[attrLoc] =
                                    CreateFactoryArg(attrLoc, first);

                                break;
                            }
                        case (false, true):
                            {
                                break;
                            }
                        case (true, true):
                            {
                                resultFactoryArgs[attrLoc] =
                                    CreateDiagonalFactoryArg(attrLoc);

                                break;
                            }
                    }
                }

                return MakeGeneral(
                    resultFactoryArgs,
                    lowAttrLoc,
                    highAttrLoc);
            }
        }

        public static void ResetFactoryArgs(RectangleInfo rect)
        {
            IndexedComponentFactoryArgs<IAttributeComponent>[]
                resultFactoryArgs = rect.Components;
            int lowAttrLoc = rect.LowAttrLoc;

            resultFactoryArgs[lowAttrLoc] =
                CreateDefaultFactoryArg(lowAttrLoc, resultFactoryArgs);

            if (rect.IsDiagonal) return;

            int highAttrLoc = rect.HighAttrLoc;

            for (lowAttrLoc++; lowAttrLoc <= highAttrLoc; lowAttrLoc++)
            {
                if (resultFactoryArgs[lowAttrLoc].IsDefault) continue;

                resultFactoryArgs[lowAttrLoc] =
                    CreateDefaultFactoryArg(lowAttrLoc, resultFactoryArgs);
            }

            return;
        }

        public static RectangleInfo BranchAccumulatorFactoryForIntersection(
            BranchAction action,
            RectangleInfo first,
            RectangleInfo second)
        {
            return action switch
            {
                BranchAction.CutOffBranchSelectFirst => first.CloneWith(second.Components),
                BranchAction.CutOffBranchSelectSecond => second,
                BranchAction.ContinueBranchTillEnd => Intersect(first, second)
            };
        }

        public static RectangleInfo BranchAccumulatorFactoryForUnion(
            BranchAction action,
            RectangleInfo first,
            RectangleInfo second)
        {
            return action switch
            {
                BranchAction.CutOffBranchSelectFirst => first.CloneWith(second.Components),
                BranchAction.CutOffBranchSelectSecond => second,
                BranchAction.ContinueBranchTillEnd => Union(first, second)
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BranchAction DefineBranchAction(
            RectangleInfo first,
            RectangleInfo second,
            BranchAction firstIncludesSecond,
            BranchAction secondIncludesFirst)
        {
            // заглушка, нужно продумать алгоритм до конца 
            // (работает с конъюнкцией - убирает лишние кортежи по-максимуму,
            // но с дизъюнкцией уже нет)
            return BranchAction.ContinueBranchTillEnd;

            if (second.IsDiagonal)
            {
                if (first.ContainsAttribute(second.LowAttrLoc))
                {
                    IAttributeComponent firstAC = first[second.LowAttrLoc],
                                        secondAC = second.DiagonalComponent;

                    if (secondAC.IncludesOrEqualsTo(firstAC))
                    {
                        return secondIncludesFirst;
                    }

                    if (first.IsDiagonal)
                    {
                        /*
                        if (firstAC.IncludesOrEqualsTo(secondAC))
                            return firstIncludesSecond;
                        */
                        return BranchAction.ContinueBranchTillEnd;
                    }

                    return BranchAction.ConsiderSubBranchLater;
                }

                return BranchAction.ConsiderSubBranchLater;
            }

            return BranchAction.NonDefined;
        }

        public static BranchAction DefineBranchActionForIntersection(
            RectangleInfo first,
            RectangleInfo second)
        {
            return DefineBranchAction(
                first, 
                second, 
                BranchAction.CutOffBranchSelectSecond,
                BranchAction.CutOffBranchSelectFirst);
        }

        public static BranchAction DefineBranchActionForUnion(
            RectangleInfo first,
            RectangleInfo second)
        {
            return DefineBranchAction(
                first,
                second,
                BranchAction.CutOffBranchSelectFirst,
                BranchAction.CutOffBranchSelectSecond);
        }

        /*
        public static BranchAction DefineBranchAction(
            RectangleInfo first,
            RectangleInfo second)
        {
            return BranchAction.ContinueBranchTillEnd;
        }
        */
    }
}
