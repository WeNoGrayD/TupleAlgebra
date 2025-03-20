using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.SetOperationExecutorsContainers;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using UniversalClassLib;
using static TupleAlgebraClassLib.AttributeComponents.AttributeComponentHelper;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public class TupleObjectStaticDataStorage
        : ITypeHierarchyRegistrar<TupleObjectStaticDataStorage>
    {
        #region Static fields

        private ITypeHierarchyRegistrar<TupleObjectStaticDataStorage> _registrar 
        { get => this; }

        /// <summary>
        /// Фабрики специфических компонент атрибутов.
        /// </summary>
        private static IDictionary<Type, ITupleObjectStaticData>
            _nodes;

        private TupleObjectFactory _factory;

        #endregion

        #region Static properties

        public static TupleObjectStaticDataStorage Storage { get; set; }

        #endregion

        #region Delegates

        public delegate ITupleObjectOperationExecutorsContainer<TEntity>
            TupleObjectSetOperationsCreationHandler<TEntity>(
                TupleObjectFactory factory)
            where TEntity : new();

        #endregion

        #region Constructors

        /// <summary>
        /// Статический конструктор.
        /// </summary>
        static TupleObjectStaticDataStorage()
        {
            Storage = new TupleObjectStaticDataStorage();
            _nodes = new Dictionary<Type, ITupleObjectStaticData>();

            return;
        }

        private TupleObjectStaticDataStorage()
        {
            _registrar.Init();
            _factory = new TupleObjectFactory(null);

            return;
        }

        #endregion

        #region Instance methods

        #region Type registration methods

        /// <summary>
        /// Регистрация типа.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <typeparam name="TAttributeComponent"></typeparam>
        /// <param name="considerAsNonGeneric"></param>
        /// <param name="acFactory"></param>
        /// <param name="setOperations"></param>
        public void RegisterType<TEntity, TTupleObject>(
            TupleObjectSetOperationsCreationHandler<TEntity> setOperations = null)
            where TEntity : new()
            where TTupleObject : TupleObject<TEntity>
        {
            Type tupleType = typeof(TTupleObject);
            // Если тип зарегистрирован, то дальнейшая регистрация не производится.
            if (_registrar.IsTypeRegistered(tupleType)) return;

            TupleObjectStaticData<TEntity> staticData =
                new TupleObjectStaticData<TEntity>();
            _nodes.Add(tupleType, staticData);
            RegisterSetOperationsForType(
                tupleType,
                staticData,
                setOperations);

            _registrar.RegisterType(tupleType);

            return;
        }

        #endregion

        #region Type static variables registration methods

        /// <summary>
        /// Регистрация контейнера операций над типом компоненты атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="acType"></param>
        /// <param name="setOperations"></param>
        private void RegisterSetOperationsForType<TEntity>(
            Type tupleType,
            TupleObjectStaticData<TEntity> staticData,
            TupleObjectSetOperationsCreationHandler<TEntity> setOperations)
            where TEntity : new()
        {
            staticData.SetOperations = setOperations(_factory);

            return;
        }

        #endregion

        #region Type static variables getters

        /// <summary>
        /// Получени фабрики компоненты атрибута.
        /// </summary>
        /// <param name="ac"></param>
        /// <returns></returns>
        public TupleObjectFactory GetFactory()
        {
            return _factory;
        }

        /// <summary>
        /// Полученик контейнера операций над компонентой атрибута.
        /// </summary>
        /// <typeparam name="TData"></typeparam>
        /// <param name="ac"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public ITupleObjectOperationExecutorsContainer<TEntity>
            GetSetOperations<TEntity>(
                TupleObject<TEntity> tuple)
            where TEntity : new()
        {
            Type tupleType = tuple.GetType();
            if (!_registrar.IsTypeRegistered(tupleType))
            {
                _registrar.RegisterTypeWithForce(tupleType);
                /*
                if (!_setOperationContainers.TryGetValue(acType, out setOpsContainer))
                {
                    throw new InvalidOperationException($"Тип {acType} не выполнил свою регистрацию в классе-помощнике.");
                }
                */
            }

            return _nodes[tupleType].GetSetOperations<TEntity>();
        }

        #endregion

        #endregion

        #region Nested types

        public interface ITupleObjectStaticData
        {
            ITupleObjectOperationExecutorsContainer<TEntity>
                GetSetOperations<TEntity>()
                where TEntity : new()
            {
                return (this as TupleObjectStaticData<TEntity>)!
                    .SetOperations;
            }
        }

        public class TupleObjectStaticData<TEntity>
            : ITupleObjectStaticData
            where TEntity : new()
        {
            public ITupleObjectOperationExecutorsContainer<TEntity>
                SetOperations { get; set; }

            public TupleObjectStaticData()
            { return; }
        }

        #endregion
    }
}
