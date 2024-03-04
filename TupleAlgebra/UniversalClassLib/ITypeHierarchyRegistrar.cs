using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UniversalClassLib
{
    public interface ITypeHierarchyRegistrar<TRegistrar>
        where TRegistrar : ITypeHierarchyRegistrar<TRegistrar>
    {
        #region Static properties

        /// <summary>
        /// Множество зарегистрированных типов.
        /// </summary>
        public static HashSet<Type> RegisteredTypes { get; private set; }

        #endregion

        #region Instance methods

        public void Init()
        {
            RegisteredTypes = new HashSet<Type>();
            RegisterType(typeof(object));

            return;
        }

        #region Type registration methods

        /// <summary>
        /// Занесение типа в список зарегистрированных.
        /// </summary>
        /// <param name="acType"></param>
        public void RegisterType(Type acType)
        {
            RegisteredTypes.Add(acType);

            return;
        }

        /// <summary>
        /// Проверка типа на регистрацию.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsTypeRegistered(Type type) => RegisteredTypes.Contains(type);

        /// <summary>
        /// Регистрирование класса при помощи силы.
        /// </summary>
        /// <param name="type"></param>
        public void RegisterTypeWithForce(Type type)
        {
            /*
             * Используется вызов статического конструктора регистрируемого типа.
             * Предполагается, что в статическом конструкторе содержатся инструкции по регистрации типа.
             */
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            return;
        }

        #endregion

        #region Auxilary methods

        /// <summary>
        /// Получение определения типа.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type GetDefinition(Type type) =>
            type.IsGenericType ? type.GetGenericTypeDefinition() : type;

        /// <summary>
        /// Поиск в иерархии классов подходящего предка с выполнением какой-либо задачи.
        /// </summary>
        /// <param name="leafType"></param>
        /// <param name="getTypeDefinition"></param>
        /// <param name="stopCondition"></param>
        /// <param name="toExecute"></param>
        /// <param name="onFailure"></param>
        public void FindRelevantTypeAncestorAndExecute(
            Type leafType,
            Func<Type, Type> getTypeDefinition,
            Predicate<Type> stopCondition,
            Action<Type> toExecute,
            Exception onFailure)
        {
            Type superType = leafType, superTypeDefinition, objectType = typeof(object);
            bool stoppedWithResult;

            while (stoppedWithResult = !superType.Equals(objectType))
            {
                superTypeDefinition = getTypeDefinition(superType);
                if (stoppedWithResult = stopCondition(superTypeDefinition))
                {
                    toExecute(superTypeDefinition);
                    break;
                }
                else superType = superType.BaseType!;
            }

            if (onFailure is not null && !stoppedWithResult) throw onFailure;

            return;
        }

        #endregion

        #endregion
    }

    public abstract class TypeHierarchyNode<TNode>
        where TNode : TypeHierarchyNode<TNode>
    {
        protected PropertyNode<TProperty> InitProperty<TProperty>(
            TProperty property)
        {
            return new PropertyNode<TProperty>() { Value = property };
        }

        protected PropertyNode<TProperty> InitProperty<TProperty>(
            TNode ancestorNode,
            Func<TNode, PropertyNode<TProperty>> ancestorProperty)
        {
            return ancestorProperty(ancestorNode);
        }

        protected PropertyNode<TProperty> InitProperty<TProperty>(
            Func<TProperty> propertySetter)
        {
            return new LazyPropertyNode<TProperty>(propertySetter);
        }
    }

    public class PropertyNode<TProperty>
    {
        public virtual TProperty Value { get; set; }
    }

    public class LazyPropertyNode<TProperty>
        : PropertyNode<TProperty>
    {
        private Lazy<TProperty> _property;

        public override TProperty Value
        {
            get => _property.Value;
        }

        public LazyPropertyNode(Func<TProperty> propertySetter)
        {
            _property = new Lazy<TProperty>(propertySetter);

            return;
        }
    }
}
