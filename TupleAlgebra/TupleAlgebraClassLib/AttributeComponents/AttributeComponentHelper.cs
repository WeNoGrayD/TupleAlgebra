using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using System.Reflection;

namespace TupleAlgebraClassLib.AttributeComponents
{
    public static class AttributeComponentHelper
    {
        #region Static fields

        private static HashSet<Type> _registeredTypes;

        #endregion

        #region Static properties

        public static IDictionary<Type, AttributeComponentFactory> Factories { get; }

        #endregion

        #region Constructors

        static AttributeComponentHelper()
        {
            _registeredTypes = new HashSet<Type>();

            Factories = new Dictionary<Type, AttributeComponentFactory>();
        }

        #endregion

        #region Static methods

        private static bool IsTypeRegistered(Type type) => _registeredTypes.Contains(type);

        public static void RegisterType<TData>(
            Type acType,
            bool considerAsNonGeneric = false,
            AttributeComponentFactory acFactory = null)
        {
            Type dataType = typeof(TData), rootAcType = typeof(AttributeComponent<TData>);
            if (!acType.IsSubclassOf(rootAcType) && !acType.Equals(rootAcType))
            {
                throw new ArgumentException($"Тип {acType.Name} с параметром типа {dataType.Name}" +
                    $"должен наследоваться от типа {rootAcType.Name}!");
            }

            if (acType.IsGenericType && !considerAsNonGeneric)
            {
                RegisterGenericTypeDefinition<TData>(acType, acFactory);
            }
            else
            {
                RegisterNonGenericType<TData>(acType, acFactory);
            }

            return;
        }

        private static void FindRelevantTypeAncestorAndExecute(
            Type leafType, 
            Predicate<Type> stopCondition, 
            Action<Type> toExecute,
            Exception onFailure)
        {
            Type superType = leafType, superTypeDefinition, objectType = typeof(object);
            bool stoppedWithResult;

            while (stoppedWithResult = !superType.Equals(objectType))
            {
                superTypeDefinition = GetDefinition(superType);
                if (stoppedWithResult = stopCondition(superTypeDefinition))
                {
                    toExecute(superTypeDefinition);
                    break;
                }
                else superType = superType.BaseType;
            }

            if (onFailure is not null && !stoppedWithResult) throw onFailure;

            return;
        }

        private static void RegisterFactoryForType(
            Type acType,
            AttributeComponentFactory acFactory)
        {
            if (acFactory is null)
            {
                FindRelevantTypeAncestorAndExecute(
                    acType.BaseType,
                    (t) => Factories.ContainsKey(t),
                    (registeredType) => Factories.Add(acType, Factories[registeredType]),
                    new ArgumentNullException(
                        "acFactory", 
                        "Фабрика компоненты должна быть предоставлена для типа " +
                        $"{acType.Name} или быть определена ранее для одного из его базовых типов."));
            }
            else Factories.Add(acType, acFactory);

            return;
        }

        private static void RegisterGenericTypeDefinition<TData>(
            Type acType,
            AttributeComponentFactory acFactory = null)
        {
            Type acGenericTypeDefinition = acType.GetGenericTypeDefinition();
            if (IsTypeRegistered(acGenericTypeDefinition)) return;

            RegisterTypeWithForce(acType.BaseType);
            RegisterFactoryForType(acGenericTypeDefinition, acFactory);

            _registeredTypes.Add(acGenericTypeDefinition);

            return;
        }

        private static void RegisterNonGenericType<TData>(
            Type acType,
            AttributeComponentFactory acFactory)
        {
            if (IsTypeRegistered(acType)) return;

            RegisterTypeWithForce(acType.BaseType);
            RegisterFactoryForType(acType, acFactory);

            _registeredTypes.Add(acType);

            return;
        }

        #endregion

        public static AttributeComponentFactory GetFactory(object ac)
        {
            return GetFactory(ac.GetType());
        }

        public static AttributeComponentFactory GetFactory(Type acType)
        {
            Type acTypeDefinition = GetDefinition(acType);
            if (!Factories.ContainsKey(acTypeDefinition))
            {
                RegisterTypeWithForce(acType);
                if (!Factories.ContainsKey(acTypeDefinition))
                {
                    throw new InvalidOperationException($"Тип {acType} не выполнил свою регистрацию в классе-помощнике.");
                }
            }

            return Factories[acTypeDefinition];
        }

        private static void RegisterTypeWithForce(Type type)
        {
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);

            return;
        }

        private static Type GetDefinition(Type type) =>
            type.IsGenericType ? type.GetGenericTypeDefinition() : type;
    }
}
