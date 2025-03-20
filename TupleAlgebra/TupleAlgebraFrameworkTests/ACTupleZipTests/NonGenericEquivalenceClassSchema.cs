using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TupleAlgebraFrameworkTests.ACTupleZipTests
{
    internal interface IAttributeInfo
    {
        void SetAttributeTo<TEntity>(
            TEntity entity,
            IEquivalenceClassBearer equClassBearer);

        IEquivalenceClassBearer ReturnECBearer();

        IEquivalenceClassBearer NonGenericReturnECBearer(object value);

        IAttributeInfoNode<TEntity> ReturnAttributeInfoNode<TEntity>();
    }

    internal class AttributeInfo<TData> : IAttributeInfo
        where TData : IEquatable<TData>
    {
        private Delegate _attributeGetter;

        public Func<TEntity, TData> GetAttributeGetter<TEntity>() =>
            (_attributeGetter as Func<TEntity, TData>)!;

        public void SetAttributeGetter<TEntity>(
            Func<TEntity, TData> attributeGetter)
        {
            _attributeGetter = attributeGetter;

            return;
        }

        public void SetAttributeTo<TEntity>(
            TEntity entity,
            IEquivalenceClassBearer equClassBearer)
        {
            (equClassBearer as EquivalenceClassBearer<TData>)!
                .Value = (_attributeGetter as Func<TEntity, TData>)!(entity);

            return;
        }

        public IEquivalenceClassBearer ReturnECBearer()
        {
            return new EquivalenceClassBearer<TData>();
        }

        public IEquivalenceClassBearer NonGenericReturnECBearer(object value)
        {
            return new NonGenericEquivalenceClassBearer<TData>((TData)value);
        }

        public IAttributeInfoNode<TEntity> ReturnAttributeInfoNode<TEntity>()
        {
            return new AttributeInfoNode<TEntity, TData>(this);
        }
    }

    internal class NonGenericEquivalenceClassBearer<TData> : IEquivalenceClassBearer
        where TData : IEquatable<TData>
    {
        public TData Value { get; set; }

        public NonGenericEquivalenceClassBearer(TData value)
        {
            Value = value;

            return;
        }

        public static void SetPropertyGetter<TEntity>(Func<TEntity, TData> propertyGetter)
        {
            throw new NotImplementedException();
        }

        void IEquivalenceClassBearer.SetEntityProperty<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
        }

        IEquivalenceClassBearer IEquivalenceClassBearer.Reproduce()
        {
            throw new NotImplementedException();
        }

        bool IEquivalenceClassBearer.Equals(IEquivalenceClassBearer second)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object? second)
        {
            return Value.Equals(second!);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    internal class NonGenericEquivalenceClassSchema<TEntity>
    {
        #region Instance fields

        private IEquivalenceClassBearer[] _equClasses;

        #endregion

        #region Static fields

        private static int _equClassCount;

        private static IAttributeInfo[] _attributeSchema;

        private static object[] _attributes;

        private static Func<TEntity, object>[] _attributeGetters;

        #endregion

        public NonGenericEquivalenceClassSchema(TEntity entity)
        {
            for (int i = 0; i < _equClassCount; i++)
                _attributes[i] = _attributeGetters[i](entity);

            return;
        }

        public static void SetAttributeGetters(
            IAttributeInfo[] attributeSchema,
            params Func<TEntity, object>[] attributeGetters)
        {
            _attributeSchema = attributeSchema;
            _attributeGetters = attributeGetters;
            _equClassCount = attributeSchema.Length;
            _attributes = new object[_equClassCount];

            return;
        }

        public void InitEquClasses()
        {
            _equClasses = new IEquivalenceClassBearer[_equClassCount];
            for (int i = 0; i < _equClassCount; i++)
            {
                _equClasses[i] = _attributeSchema[i].NonGenericReturnECBearer(_attributes[i]);
            }

            return;
        }

        public override bool Equals(object? obj)
        {
            NonGenericEquivalenceClassSchema<TEntity> second = 
                (obj as NonGenericEquivalenceClassSchema<TEntity>)!;
            int p = _equClassCount >> 1, // Индекс с середины цикла, движущийся вправо.
                k = p - 1; // Индекс с середины цикла, движущийся влево.
            bool equals = true;

            if ((_equClassCount & 0b1) != 0)
            {
                if (!_equClasses[p].Equals(_attributes[p]))
                {
                    CaseNotEquals();
                    return equals;
                }
            }
            else p--;

            int j = _equClassCount; // Индекс с конца цикла.

            for (int i = 0; i <= k; i++, k--)
            {
                if (!_equClasses[i].Equals(_attributes[i])) goto NOT_EQUALS;
                if (!_equClasses[k].Equals(_attributes[k])) goto NOT_EQUALS;
                if (!_equClasses[++p].Equals(_attributes[p])) goto NOT_EQUALS;
                if (!_equClasses[--j].Equals(_attributes[j])) goto NOT_EQUALS;

                continue;

            NOT_EQUALS:

                CaseNotEquals();

                break;
            }

            return equals;

            void CaseNotEquals()
            {
                equals = false;

                return;
            }
        }

        public override int GetHashCode()
        {
            //if (_hashcodeCalculated) return _hashcode;
            //_hashcodeCalculated = true;

            int hc = 0;

            for (int i = 0; i < _equClassCount; i++)
                hc += _attributes[i].GetHashCode();

            return hc;// % 9013;
        }
    }

    internal class NonGenericEquivalenceClassTable<TEntity>
    {
        private IDictionary<NonGenericEquivalenceClassSchema<TEntity>, IList<TEntity>> _equClassPartitions;

        public IDictionary<NonGenericEquivalenceClassSchema<TEntity>, IList<TEntity>>
            EquivalenceClassPartitions
        { get => _equClassPartitions; }

        public NonGenericEquivalenceClassTable()
        {
            _equClassPartitions = new Dictionary<NonGenericEquivalenceClassSchema<TEntity>, IList<TEntity>>();

            return;
        }

        public void MakePartition(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
                ReferToEquivalenceClass(entity);

            return;
        }

        private void ReferToEquivalenceClass(TEntity entity)
        {
            NonGenericEquivalenceClassSchema<TEntity> ecSchema = 
                new NonGenericEquivalenceClassSchema<TEntity>(entity);

            if (!_equClassPartitions.ContainsKey(ecSchema))
            {
                ecSchema.InitEquClasses();
                _equClassPartitions.Add(ecSchema, new List<TEntity>(1) { entity });
            }
            else _equClassPartitions[ecSchema].Add(entity);

            return;
        }
    }
}
