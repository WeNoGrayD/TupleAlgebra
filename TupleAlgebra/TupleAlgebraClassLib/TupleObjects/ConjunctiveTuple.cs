using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;

namespace TupleAlgebraClassLib.TupleObjects
{
    using static TupleObjectHelper;

    public class ConjunctiveTuple<TEntity> : SingleTupleObject<TEntity>
        where TEntity : new()
    {
        #region Constructors

        public ConjunctiveTuple(Action<TupleObjectBuilder<TEntity>> onTupleBuilding = null)
            : base(onTupleBuilding)
        { }

        #endregion

        #region Instance methods

        protected override AttributeComponent<TData> GetDefaultFictionalAttributeComponentImpl<TData>(AttributeInfo attribute)
        {
            //var factoryArgs = new AttributeComponentFactoryInfrastructure.AttributeComponentFactoryArgs();
            //factoryArgs.SetAttributeDomainGetter(() => attribute.GetDomain<TData>());
            return null;
            //return AttributeComponent<TData>.FictionalAttributeComponentFactory.CreateFull<TData>(factoryArgs);
        }

        #endregion


        protected override IEnumerator<TEntity> GetEnumeratorImpl()
        {
            /*
             * В предположении, что в схему включен хотя бы один атрибут.
             * Если тип сущности является примитивным, то единственная компонента
             * содержит перечисление сущностей.
             * Иначе производится генерация сущностей с прикреплёнными свойствами-атрибутами.
             */
            return TupleObjectSchema<TEntity>.IsEntityPrimitive ?
                (_components[0].GetEnumerator() as IEnumerator<TEntity>)! :
                IterateOverCartesianProduct();

            IEnumerator<TEntity> IterateOverCartesianProduct()
            {
                IEnumerator[] componentsEnumerators;
                EntityFactoryHandler<TEntity> entityFactory = Schema.GetEntityFactory();

                /*
                 * Если число компонент равно 1, то производится упрощённый перебор значений
                 * одного атрибута.
                 * Если больше 1, то производится поочерёдный перебор значений каждой
                 * компоненты в соответствии с прямым произведением компонент.
                 */
                return _components.Length > 1 ?
                    IterateOverManyAttachedAttributes() :
                    IterateOverOneAttachedAttribute();

                IEnumerator<TEntity> IterateOverOneAttachedAttribute()
                {
                    IEnumerator oneComponentEnumerator = _components[0].GetEnumerator();
                    componentsEnumerators = new[] { oneComponentEnumerator };

                    while (oneComponentEnumerator.MoveNext())
                        yield return entityFactory(componentsEnumerators);

                    yield break;
                }

                /*
                 * TODO: сделать возможность обхода компонентов по желанию пользователя.
                 * Буферизация перечислителей должна происходить иначе, без допкопирования.
                 */
                IEnumerator<TEntity> IterateOverManyAttachedAttributes()
                {
                    /*
                     * Попытка оптимизировать потребление памяти при переборе значений компонент
                     * атрибутов, если пользователь не задавал специального порядка обхода.
                     * Компонента с самой большой мощностью ставится на нулевой индекс
                     * в массиве перечислителей и не буферизируется.
                     */
                    int componentsCount = _components.Length, 
                        largestComponentLoc = -1;
                    IAttributeComponent largestComponent = _components.MaxBy(cmp => cmp.Power), 
                                        component;
                    IEnumerator largestComponentEnumerator = largestComponent.GetEnumerator();
                    componentsEnumerators = new IEnumerator[componentsCount];

                    for (int i = 0; i < componentsCount; i++)
                    {
                        if ((component = _components[i]) is null)
                            continue;

                        if (component.Equals(largestComponent))
                        {
                            largestComponentLoc = i;
                            componentsEnumerators[i] = largestComponentEnumerator;
                        }
                        else
                        {
                            // Производится буферизирование компоненты.
                            componentsEnumerators[i] = component.GetBufferizedEnumerator();
                        }
                            
                    }
                    /*
                     * Вставка крупнейшей компоненты на нулевой индекс массива перечислителей. 
                     */
                    if (largestComponentLoc > 0)
                    {
                        (componentsEnumerators[largestComponentLoc], componentsEnumerators[0]) =
                            (componentsEnumerators[0], componentsEnumerators[largestComponentLoc]);
                    }

                    Stack<IEnumerator> componentsStack = new Stack<IEnumerator>(componentsCount);

                    while (largestComponentEnumerator.MoveNext())
                    {
                        foreach (TEntity entity
                             in EnumerateCartesianProduct(
                                entityFactory,
                                componentsEnumerators,
                                1,
                                componentsStack))
                            yield return entity;
                    }

                    yield break;
                }
            }
        }

        /// <summary>
        /// Перечисление прямого (декартова) произведения нескольких атрибутов.
        /// </summary>
        /// <param name="entityFactory">Сконструированная инструкция по генерации сущностей.</param>
        /// <param name="componentsEnumerators">Массив перечислителей компонент, в соответствии
        /// с которым была построена инструкция по генерации сущностей.</param>
        /// <param name="startComponentLoc">Номер атрибута, с которого начинает производиться
        /// перечисление значений компоненты.</param>
        /// <param name="componentsStack">Стек для обхода компонент.</param>
        /// <returns></returns>
        protected IEnumerable<TEntity> EnumerateCartesianProduct(
            Func<IEnumerator[], TEntity> entityFactory,
            IEnumerator[] componentsEnumerators,
            int startComponentLoc = 0,
            Stack<IEnumerator> componentsStack = null)
        {
            int i, j, componentsCount = componentsEnumerators.Length;
            IEnumerator componentEnumerator;
            if (componentsStack is null)
                componentsStack = new Stack<IEnumerator>(componentsCount);

            i = j = startComponentLoc;
            componentsStack.Push(componentEnumerator = componentsEnumerators[i]);

            do
            {
                if (componentEnumerator.MoveNext())
                {
                    /*
                     * второе условие тут из-за того, что componentsCount передаётся без учёта
                     * startComponentLoc
                     */
                    if (j == componentsCount || (j = i + 1) == componentsCount)
                    {
                        yield return entityFactory(componentsEnumerators);
                    }
                    else
                    {
                        componentsStack.Push(componentsEnumerators[i = j]);
                    }
                }
                else
                {
                    j = --i;
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

        public override TupleObject<TEntity> Convert(TupleObject<TEntity> diagonal)
        {
            throw new NotImplementedException();
        }

        protected override TupleObject<TEntity> ComplementThe()
        {
            return null;
        }

        protected override TupleObject<TEntity> IntersectWith(TupleObject<TEntity> second)
        {
            return null;
        }

        protected override TupleObject<TEntity> UnionWith(TupleObject<TEntity> second)
        {
            return null;
        }

        protected override TupleObject<TEntity> ExceptWith(TupleObject<TEntity> second)
        {
            return null;
        }

        protected override TupleObject<TEntity> SymmetricExceptWith(TupleObject<TEntity> second)
        {
            return null;
        }

        public override TupleObject<TEntity> Diagonal()
        {
            throw new NotImplementedException();
        }
    }
}
