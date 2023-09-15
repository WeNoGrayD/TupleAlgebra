using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraTests.ACTupleZipTests
{
    internal enum NodeState
    {
        Open,
        ExcludedFromConsideration,
        Closed
    }

    internal interface IAttributeInfoNode<TEntity>
    {
        int ChildrenCount { get; }

        void Place(TEntity entity);

        IEnumerable<TEntity> FindSiblings(TEntity entity);
    }

    /// <summary>
    /// Курсор, проходящий по структуре данных архиватора.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class Cursor<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        private IDictionary<TEntity, EntitySiblingNode<TEntity>>
            _siblingsMatches;

        //private IDictionary<TEntity, HashSet<TEntity>> _siblings;
    }

    internal class AttributeInfoNode<TEntity, TData>
        : IAttributeInfoNode<TEntity>
        where TData : IEquatable<TData>
    {
        private AttributeInfo<TData> _attrInfo;

        private Func<TEntity, TData> _attributeGetter;

        private IDictionary<TData, AttributeValueNode<TEntity, TData>> _childrenNodes;

        public int ChildrenCount
        {
            get => _childrenNodes.Keys.Count;
        }

        public NodeState State { get; set; }

        private Dictionary<TEntity, EntitySiblingNode<TEntity>> _siblingsMatches;

        public AttributeInfoNode(AttributeInfo<TData> attrInfo)
        {
            _attrInfo = attrInfo;
            _attributeGetter = attrInfo.GetAttributeGetter<TEntity>();
            _childrenNodes = new Dictionary<TData, AttributeValueNode<TEntity, TData>>();

            State = NodeState.ExcludedFromConsideration;

            return;
        }

        public void Place(TEntity entity)
        {
            TData attributeValue = _attributeGetter(entity);

            if (!_childrenNodes.ContainsKey(attributeValue))
            {
                /*
                 * Узел атрибута рассматривается как открытый, если содержит
                 * (после добавления новой сущности) лишь одно значение атрибута
                 * для всех сущностей.
                 */
                if (_childrenNodes.Count > 0)
                    State = NodeState.Open;
                _childrenNodes.Add(attributeValue, new AttributeValueNode<TEntity, TData>());
            }

            _childrenNodes[attributeValue].Place(entity);

            return;
        }

        /*
        public void PreliminarySiblingInit()
        {
            
        }
        */

        public IEnumerable<TEntity> FindSiblings(TEntity entity)
        {
            TData attributeValue = _attributeGetter(entity);

            return _childrenNodes[attributeValue].Entities();
        }

        /*
        private void FillSiblingsOccurrencesWithDefault(IEnumerable<TEntity> siblings)
        {
            _siblingsMatches.Clear();
            foreach (TEntity sibling in siblings)
                _siblingsMatches.Add(sibling, new EntitySiblingNode<TEntity>());

            return;
        }
        */

        public void Traverse(
            IEnumerable<IAttributeInfoNode<TEntity>> otherAttributes,
            TEntity entity,
            int priority)
        {
            IEnumerable<TEntity> siblingsByThisAttribute = this.FindSiblings(entity);
            HashSet<TEntity> siblingsByOtherAttribute;
            int associatedEntityCount;
            //FillSiblingsOccurrencesWithDefault(siblingsByThisAttribute);

            foreach (AttributeValueNode<TEntity, TData> attrValueNode 
                 in _childrenNodes.Values)
            {
                associatedEntityCount = attrValueNode.EntityCount;
                
                /*
                 * Если число сущностей, которые имеют заданное значение атрибута,
                 * больше priority, то проход по значениям атрибута заканчиватся,
                 * поскольку узлы значений атрибутов отсортированы в порядке
                 * возрастания данного параметра. Если это число меньше priority,
                 * то продолжается поиск значения атрибута с этим параметром,
                 * равным priority.
                 */
                if (associatedEntityCount > priority) break;
                if (associatedEntityCount < priority ||
                    attrValueNode.State != NodeState.Open) continue;

                /*
                 * Если найден узел значения атрибута с числом соответствующих ему
                 * сущностей == priority, то среди сиблингов сущности entity
                 * ищутся такие, у которых значения атрибутов отличаются 
                 * не более чем в одном атрибуте с entity.
                 */

                foreach (IAttributeInfoNode<TEntity> otherAttribute
                     in otherAttributes)
                {
                    siblingsByOtherAttribute = otherAttribute.FindSiblings(entity).ToHashSet();
                    foreach (TEntity sibling in siblingsByThisAttribute)
                    {
                        if (!siblingsByOtherAttribute.Contains(sibling))
                        {
                            // проверить, изменяется ли значение в квп
                            _siblingsMatches[sibling].MismatchedAttribute = otherAttribute;
                            if (_siblingsMatches[sibling].MismatchCount > 1)
                                _siblingsMatches.Remove(sibling);
                        }
                    }
                }

                if (_siblingsMatches.Count == 0)
                    break;
            }

            /*
             * Все подходящие сиблинги entity размещаются в узлах значений атрибутов,
             * соотносящихся с entity, но не соотносящихся с i-ым сиблингом.
             */

            foreach (TEntity sibling in _siblingsMatches.Keys)
            {
            }
        }
    }

    internal class EntitySiblingNode<TEntity>
    {
        private IAttributeInfoNode<TEntity> _mismatchedAttribute = null;

        public byte MismatchCount { get; private set; } = 0;

        public IAttributeInfoNode<TEntity> MismatchedAttribute 
        { 
            get => _mismatchedAttribute;
            set
            {
                _mismatchedAttribute = value;
                MismatchCount++;
            } 
        }
    }

    internal class AttributeValueNode<TEntity, TData> 
        where TData : IEquatable<TData>
    {
        private TEntity _headEntity;

        private IList<TEntity> _tailEntities = null!;

        private int _lastVisited;

        public int EntityCount 
        { 
            get => _tailEntities is null ? 1 : 1 + _tailEntities!.Count;
        }

        public NodeState State { get; set; }

        //public IEnumerable<TEntity> Entities { get; set; }

        public AttributeValueNode()
        {
            State = NodeState.ExcludedFromConsideration; 

            return;
        }

        public void Place(TEntity entity)
        {
            if (_headEntity is null)
                _headEntity = entity;
            else
            {
                /*
                 * Если значению данного атрибута соответствует больше одной
                 * сущности, то узел считается открытым (есть вероятность,
                 * что найдутся сиблинги сущностям с таким значением атрибута, 
                 * в противном случае сущность будет уникальной, и если
                 * она станет сиблингом, то будет принимать в этом пассивное участие
                 * (не она найдёт сиблинга, а он её найдёт).
                 */
                State = NodeState.Open;
                if (_tailEntities is null) _tailEntities = new List<TEntity>();
                _tailEntities.Add(entity);
            }

            return;
        }

        public IEnumerable<TEntity> Entities()
        {
            yield return _headEntity;

            if (_tailEntities is null)
            {
                _lastVisited = -1;
                yield break;
            }

            foreach (TEntity tailEntity in _tailEntities)
            {
                yield return tailEntity;
                _lastVisited++;
            }

            yield break;
        }
    }

    internal class EntityNode<TEntity>
    {
        HashSet<TEntity> _presumtiveSiblingment;

        public void AddPresumtiveSibling(TEntity sibling)
        {
            if (_presumtiveSiblingment is null)
                _presumtiveSiblingment = new HashSet<TEntity>();

            _presumtiveSiblingment.Add(sibling);

            return;
        }
    }

    internal class ACTupleArchiver<TEntity>
    {
        private static IAttributeInfoNode<TEntity>[] _attributeSchema;

        public static void SetAttributeSchema(
            params IAttributeInfo[] entityAttributeInfos)
        {
            _attributeSchema = new IAttributeInfoNode<TEntity>[entityAttributeInfos.Length];

            for (int i = 0; i < entityAttributeInfos.Length; i++)
            {
                _attributeSchema[i] = entityAttributeInfos[i].ReturnAttributeInfoNode<TEntity>();
            }

            return;
        }

        public void MakePreliminaryPartition(IEnumerable<TEntity> entities)
        {
            int i, attributeCount = _attributeSchema.Length;

            foreach (TEntity entity in entities)
            {
                for (i = 0; i < attributeCount; i++)
                {
                    _attributeSchema[i].Place(entity);
                }
            }

            return;
        }

        public void MakeFinalPartition(IEnumerable<TEntity> entities)
        {
            int maxEntityCount = entities.Count(), 
                attributeCount = _attributeSchema.Length,
                attributeValueCount;

            /*
             * Сжимаем объекты в кортежи, начиная со значений атрибутов, 
             * которые имеет минимум объектов.
             */
            for (int eN = 2; eN < maxEntityCount; eN++)
            {
                /*
                 * Проходим по каждому значению атрибута.
                 */
                for (int ai = 0; ai < attributeCount; ai++)
                {
                    attributeValueCount = _attributeSchema[ai].ChildrenCount;

                    //foreach (
                }
            }

            return;
        }
    }
}
