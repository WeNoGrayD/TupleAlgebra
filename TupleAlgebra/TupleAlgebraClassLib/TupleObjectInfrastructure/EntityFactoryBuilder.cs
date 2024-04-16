using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    using static TupleObjectHelper;

    public class EntityFactoryBuilder
    {
        private const string PROP_SOURCE_ENUMS = "properties";
        private const string ENUM_CURRENT_ITEM = nameof(IEnumerator.Current);

        /// <summary>
        /// Построение генератора сущностей.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public EntityFactoryHandler<TEntity> Build<TEntity>(PropertyInfo[] attributes)
        {
            Type entityType = typeof(TEntity);

            /*
             * Параметр генератора сущностей - это массив перечислителей значений компонент.
             * IEnumerator[] properties
             */
            ParameterExpression propertySourceEnumeratorsExpr =
                Expression.Parameter(typeof(IEnumerator[]), PROP_SOURCE_ENUMS);
            /*
             * Создание нового объекта типа entityType с инициализаторами свойств-атрибутов.
             */
            Expression constructorExpr = Expression.MemberInit(
                Expression.New(entityType),
                ConstructMembersAssignment());
            /*
             * Результирующая функция будет выглядеть следующим образом 
             * (TEntity известен на этапе выполнения функции и является entityType):
             * TEntity EntityFactory(IEnumerator[] properties)
             * {
             *      return new TEntity() {
             *          Property0 = (properties[0] as IEnumerator<TProperty>).Current,
             *          Property1 = (properties[1] as IEnumerator<TProperty>).Current,
             *          ...
             *          PropertyN = (properties[N] as IEnumerator<TProperty>).Current
             *      };
             * }
             */
            LambdaExpression entityFactoryExprTree =
                Expression.Lambda(constructorExpr, propertySourceEnumeratorsExpr);

            return (entityFactoryExprTree.Compile() as EntityFactoryHandler<TEntity>)!;

            /*
             * Создание массива инициализаторов свойств в порядке, предоставленном
             * массивом attributes.
             */
            MemberAssignment[] ConstructMembersAssignment()
            {
                MemberAssignment[] members = new MemberAssignment[attributes.Length];

                for (int i = 0; i < attributes.Length; i++)
                {
                    members[i] = Expression.Bind(attributes[i], GetPropertyValueOf(i));
                }

                return members;
            }

            /*
             * Значение очередного свойства будет выглядеть так:
             * Property = (attributes[attrId] as IEnumerator<TProperty>).Current
             * То есть свойство инициализируется значением конкретного типа
             * (потому что перечислитель обобщённый), которое содержится в enumerator.Current.
             */
            Expression GetPropertyValueOf(int attrId)
            {
                Type propertyType = attributes[attrId].PropertyType,
                     genericEnumerator = typeof(IEnumerator<>).MakeGenericType(propertyType);
                PropertyInfo getCurrentInfo = genericEnumerator.GetProperty(ENUM_CURRENT_ITEM)!;
                Expression ithEnumerator =
                    Expression.ArrayAccess(propertySourceEnumeratorsExpr, Expression.Constant(attrId)),
                           downcastedIthEnumerator =
                    Expression.TypeAs(ithEnumerator, genericEnumerator),
                           getCurrent =
                    Expression.MakeMemberAccess(downcastedIthEnumerator, getCurrentInfo);

                return getCurrent;
            }
        }
    }


    /*
public class EntityFactoryBuilder //: ExpressionVisitor
{
    private const string PROP_SOURCE_ENUMS = "properties";
    private const string ENUM_CURRENT_ITEM = nameof(IEnumerator.Current);

    /*
    /// <summary>
    /// Дерево выражений, представляющее инструкцию создания сущности с прикреплёнными 
    /// свойствами-атрибутами.
    /// </summary>
    private Expression _entityFactoryExprTree;

    /// <summary>
    /// Единственный аргумент генератора сущностей.
    /// Массив перечислителей-источников значений свойств-атрибутов.
    /// </summary>
    private ParameterExpression? _propertiesParam;

    /// <summary>
    /// Индексы присутствующих свойств, которым соответствуют перечислители-источники значений
    /// в массиве-аргументе генератора сущностей.
    /// </summary>
    private IDictionary<PropertyInfo, int>? _propertiesLocations;

    /// <summary>
    /// Массив информации о прикреплённых атрибутах.
    /// </summary>
    private PropertyInfo[]? _attachedAttributes;

    /// <summary>
    /// Множество имён откреплённых атрибутов.
    /// </summary>
    private HashSet<string>? _detachedAttributesNames;

    private int _changeIndexTo;
    */


    /*
    public Expression Construct(
        Type entityType,
        PropertyInfo[] attachedAttributes)
    {
        ParameterExpression propertySourceEnumeratorsExpr =
            Expression.Parameter(typeof(IEnumerator[]), PROP_SOURCE_ENUMS);
        Expression constructorExpr = Expression.MemberInit(
            Expression.New(entityType),
            ConstructMembersAssignment());
        Expression entityFactoryExprTree =
            Expression.Lambda(constructorExpr, propertySourceEnumeratorsExpr);

        return entityFactoryExprTree;

        MemberAssignment[] ConstructMembersAssignment()
        {
            MemberAssignment[] members = new MemberAssignment[attachedAttributes.Length];

            for (int i = 0; i < attachedAttributes.Length; i++)
            {
                members[i] = Expression.Bind(attachedAttributes[i], GetPropertyValueOf(i));
            }

            return members;
        }

        Expression GetPropertyValueOf(int attrId)
        {
            Type propertyType = attachedAttributes[attrId].PropertyType,
                 genericEnumerator = typeof(IEnumerator<>).MakeGenericType(propertyType);
            PropertyInfo getCurrentInfo = genericEnumerator.GetProperty(ENUM_CURRENT_ITEM)!;
            Expression ithEnumerator =
                Expression.ArrayAccess(propertySourceEnumeratorsExpr, Expression.Constant(attrId)),
                       downcastedIthEnumerator =
                Expression.TypeAs(ithEnumerator, genericEnumerator),
                       getCurrent =
                Expression.MakeMemberAccess(downcastedIthEnumerator, getCurrentInfo);

            return getCurrent;
        }
    }

    public Expression Modify(
        Expression entityFactoryExprTree,
        IDictionary<PropertyInfo, int> propertiesLocations,
        IEnumerable<PropertyInfo> attachedAttributes = null!,
        IEnumerable<string> detachedAttributesNames = null!)
    {
        switch ((attachedAttributes, detachedAttributesNames))
        {
            case (null, null):
                {
                    return entityFactoryExprTree;
                }
            case (null, _):
                {
                    _detachedAttributesNames = detachedAttributesNames.ToHashSet();
                    if (_detachedAttributesNames.Count == 0) return entityFactoryExprTree;
                    _attachedAttributes = null;

                    break;
                }
            case (_, null):
                {
                    _attachedAttributes = attachedAttributes.ToArray();
                    if (_attachedAttributes.Length == 0) return entityFactoryExprTree;
                    _detachedAttributesNames = null!;

                    break;
                }
            default:
                {
                    _attachedAttributes = attachedAttributes.ToArray();
                    _detachedAttributesNames = detachedAttributesNames.ToHashSet();
                    if (_attachedAttributes.Length == 0 && 
                        _detachedAttributesNames.Count == 0) return entityFactoryExprTree;

                    break;
                }
        }

        _propertiesLocations = propertiesLocations;
        // Сохранение входного параметра - массива перечислителей значений свойств-атрибутов.
        _propertiesParam = (entityFactoryExprTree as LambdaExpression)!.Parameters[0];

        return Visit(entityFactoryExprTree);
    }

    protected override Expression VisitMemberInit(MemberInitExpression memberInitExpr)
    {
        System.Collections.ObjectModel.ReadOnlyCollection<MemberBinding> latestBindedAttributes =
            memberInitExpr.Bindings;

        NewExpression newExpr = memberInitExpr.NewExpression;

        int attachedAttributesCount = (_attachedAttributes?.Length ?? 0),
            detachedAttributesCount = (_detachedAttributesNames?.Count ?? 0),
            latestAttributesCount = latestBindedAttributes.Count,
            remainedLatestAttributesCount = latestAttributesCount - detachedAttributesCount,
            resultAttributesCount = remainedLatestAttributesCount + attachedAttributesCount;
        MemberAssignment[] resultAttributesAssignments =
            new MemberAssignment[resultAttributesCount];

        switch ((attachedAttributesCount, detachedAttributesCount))
        {
            /*
             * Откреплённые атрибуты есть, а прикреплённых нет, значит итоговое число 
             * атрибутов меньше чем в прошлый раз, а также требуется переукладка.
             */
    /*
    case (0, _):
        {
            InitLatestBindedAttributesWithoutDetachedMemberAndLayingAssignments();

            break;
        }*/
    /*
     * Откреплённых атрибутов нет, значит старые будут располагаться на своих местах
     * и не потребуют переукладки, а новые просто добавляются в конец массива.
     */
    /*
    case (_, 0):
        {
            InitLatestBindedAttributesMemberAssignments();
            InitAttachedAttributesMemberAssignments();

            break;
        }*/
    /*
     * Есть и прикреплённые, и откреплённые атрибуты. Требуется оптимальная укладка
     * прикреплённых атрибутов таким образом, чтобы пришлось переукладывать как 
     * можно меньше старых атрибутов.
     */
    /*
    case (_, _):
        {
            InitAttributesMemberAssignmentsWithLaying();

            break;
        }
}*/
    /*
    MemberInitExpression resultMemberInitExpr = Expression.MemberInit(
        newExpr,
        resultAttributesAssignments);

    return resultMemberInitExpr;

    MemberAssignment ConstructAttachedAttributePropertySetter(
        PropertyInfo attachedAttributePropertyInfo)
    {
        int attachedAttributeSourceLoc = 
            _propertiesLocations![attachedAttributePropertyInfo];
        MemberAssignment attachedAttributePropertySetter = Expression.Bind(
            attachedAttributePropertyInfo, GetPropertyValueOf(attachedAttributeSourceLoc));

        return attachedAttributePropertySetter;
    }

    void InitAttachedAttributesMemberAssignments()
    {
        for (int i = 0; i < attachedAttributesCount; i++)
        {
            resultAttributesAssignments[i] = ConstructAttachedAttributePropertySetter(
                _attachedAttributes![i]);
        }

        return;
    }

    void InitLatestBindedAttributesWithoutDetachedMemberAndLayingAssignments()
    {
        MemberAssignment attributePropertySetter;
        string attributePropertyName;

        for (int i = 0; 
             i < latestAttributesCount; 
             i++)
        {
            attributePropertySetter =
                (latestBindedAttributes[i - attachedAttributesCount] as MemberAssignment)!;
            attributePropertyName = (attributePropertySetter.Member as PropertyInfo)!.Name;
            if (_detachedAttributesNames.Contains(attributePropertyName))
                _detachedAttributesNames.Remove(attributePropertyName);
            else
                resultAttributesAssignments[i] = attributePropertySetter;
        }

        return;
    }*/

    /*
     * Инициализация уже имевшихся свойств
     */
    /*
    void InitLatestBindedAttributesMemberAssignments()
    {
        for (int i = attachedAttributesCount;
             i < latestAttributesCount;
             i++)
        {
            resultAttributesAssignments[i] = 
                (latestBindedAttributes[i] as MemberAssignment)!;
        }

        return;
    }*/

    /*
     * Инициализация массива выражений-инициализаторов свойств
     * с укладкой прикреплённых атрибутов во фрагментированный массив
     * оставшихся после открепления.
     */
    /*
    void InitAttributesMemberAssignmentsWithLaying()
    {*/
    /*
     * Если число новых атрибутов не меньше старых, то 
     * можно выбрать простой вариант и сначала зафиксировать оставшиеся старые,
     * и затем в фрагментированные места уложить прикреплённые атрибуты.
     */
    /*
    if (attachedAttributesCount >= detachedAttributesCount)
    {
        InitLatestBindedAttributesWithoutDetachedMemberAndLayingAssignments();
    }
    else
    { 
    }

    return;

    void InitAttachedAttributesMemberAssignmentWithLaying()
    {
        int freeLoc = 0;

        for (int i = 0; i < resultAttributesCount; i++)
        { 
        }

        return;
    }
}
}
*/

    /*
     * Получение выражения, соответствующего значению определённого свойства-атрибута.
     */
    /*
    Expression GetPropertyValueOf(int attrId)
    {
        Type propertyType = _attachedAttributes[attrId].PropertyType,
             genericEnumerator = typeof(IEnumerator<>).MakeGenericType(propertyType);
        PropertyInfo getCurrentInfo = genericEnumerator.GetProperty(ENUM_CURRENT_ITEM)!;
        Expression ithEnumerator =
            Expression.ArrayAccess(_propertiesParam, Expression.Constant(attrId)),
                   downcastedIthEnumerator =
            Expression.TypeAs(ithEnumerator, genericEnumerator),
                   getCurrent =
            Expression.MakeMemberAccess(downcastedIthEnumerator, getCurrentInfo);

        return getCurrent;
    }

    /// <summary>
    /// Замена индекса, связанного с индексом перечислителя-источника значений свойства-атрибута,
    /// который присутствовал в прошлый раз при генерировании объектов.
    /// </summary>
    /// <param name="indexExpr"></param>
    /// <returns></returns>
    protected override Expression VisitIndex(IndexExpression indexExpr)
    {
        IndexExpression resultIndexExpr = Expression.ArrayAccess(
            indexExpr.Object!, Expression.Constant(_changeIndexTo));

        return resultIndexExpr;
    }
}
*/
}
