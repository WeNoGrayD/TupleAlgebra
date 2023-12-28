using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.TupleObjectInfrastructure;

namespace TupleAlgebraTests.ACTupleZipTests
{
    internal interface IEquivalenceClassBearer
    {
        void SetEntityProperty<TEntity>(TEntity entity);

        IEquivalenceClassBearer Reproduce();

        bool Equals(IEquivalenceClassBearer second);
    }

    internal class EquivalenceClassBearer<TData> : IEquivalenceClassBearer
        where TData : IEquatable<TData>
    {
        //private static Delegate _propertyGetter;

        public TData Value { get; set; }

        /*
        public static void SetPropertyGetter<TEntity>(Func<TEntity, TData> propertyGetter)
        {
            _propertyGetter = propertyGetter;

            return;
        }
        */

        void IEquivalenceClassBearer.SetEntityProperty<TEntity>(TEntity entity)
        {
            throw new NotImplementedException();
            /*
            Value = (_propertyGetter as Func<TEntity, TData>)!(entity);

            return;
            */
        }

        IEquivalenceClassBearer IEquivalenceClassBearer.Reproduce()
        {
            return new EquivalenceClassBearer<TData>();
        }

        public bool Equals(IEquivalenceClassBearer second)
        {
            return Value.Equals((second as EquivalenceClassBearer<TData>)!.Value);
        }

        public override bool Equals(object? second)
        {
            return (second is IEquivalenceClassBearer secondEquClassBearer) ? 
                Equals(secondEquClassBearer) : false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    internal class EquivalenceClassSchema<TEntity>
    {
        #region Instance fields

        private IEquivalenceClassBearer[] _equClasses;

        private bool _hashcodeCalculated = false;

        private int _hashcode;

        #endregion

        #region Static fields

        private static int _equClassCount;

        private static IAttributeInfo[] _attributeSchema;

        private static IEquivalenceClassBearer[] _equClassesPattern;

        //private static TupleObjectBuilder<TEntity> _builder;

        #endregion

        public EquivalenceClassSchema(TEntity entity)
        {
            IEquivalenceClassBearer[] equClasses = _equClasses = _equClassesPattern;

            for (int i = 0; i < _equClassCount; i++)
                _attributeSchema[i].SetAttributeTo(entity, equClasses[i]);

            return;
        }

        public static void InitAttributeSchema(IAttributeInfo[] attributeSchema)
        {
            _attributeSchema = attributeSchema;
            _equClassCount = attributeSchema.Length;
            _equClassesPattern = new IEquivalenceClassBearer[_equClassCount];

            for (int i = 0; i < _equClassCount; i++)
                _equClassesPattern[i] = attributeSchema[i].ReturnECBearer();

            return;
        }

        public void ReinitEquClassesPattern()
        {
            _equClassesPattern = CopyEquClasses(_equClasses);

            return;
        }

        public IEquivalenceClassBearer[] CopyEquClasses(
            IEquivalenceClassBearer[] copyFrom)
        {
            IEquivalenceClassBearer[] equClasses =
                new IEquivalenceClassBearer[_equClassCount];

            for (int i = 0; i < _equClassCount; i++)
                equClasses[i] = copyFrom[i].Reproduce();

            return equClasses;
        }

        public override bool Equals(object? obj)
        {
            EquivalenceClassSchema<TEntity> second = (obj as EquivalenceClassSchema<TEntity>)!;
            int j = _equClassCount >> 1, // Индекс с середины цикла, движущийся влево.
                n = _equClassCount - 1; 
            bool equals = true;

            if ((n & 0b1) == 0 && !_equClasses[j].Equals(second._equClasses[j]))
            {
                CaseNotEquals();
                return equals;
            }

            j--;

            for (int i = 0; i <= j; i++, j--)
            {
                if (!_equClasses[i].Equals(second._equClasses[i])) goto NOT_EQUALS;
                if (!_equClasses[j].Equals(second._equClasses[j])) goto NOT_EQUALS;
                if (!_equClasses[n - j].Equals(second._equClasses[n - j])) goto NOT_EQUALS;
                if (!_equClasses[n - i].Equals(second._equClasses[n - i])) goto NOT_EQUALS;

                continue;

            NOT_EQUALS:

                CaseNotEquals();

                break;
            }

            return equals;

            void CaseNotEquals()
            {
                equals = false;
                ReinitEquClassesPattern();

                return;
            }
        }

        public override int GetHashCode()
        {
            //if (_hashcodeCalculated) return _hashcode;
            //_hashcodeCalculated = true;

            int hc = 0;

            for (int i = 0; i < _equClassCount; i++)
                hc += _equClasses[i].GetHashCode();

            return _hashcode = hc;// % 9013;
        }
    }

    internal class EquivalenceClassTable<TEntity>
    {
        private IDictionary<EquivalenceClassSchema<TEntity>, IList<TEntity>> _equClassPartitions;

        public IDictionary<EquivalenceClassSchema<TEntity>, IList<TEntity>>
            EquivalenceClassPartitions { get => _equClassPartitions; }

        public EquivalenceClassTable()
        {
            _equClassPartitions = new Dictionary<EquivalenceClassSchema<TEntity>, IList<TEntity>>();

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
            EquivalenceClassSchema<TEntity> ecSchema = new EquivalenceClassSchema<TEntity>(entity);
            
            if (!_equClassPartitions.ContainsKey(ecSchema))
            {
                ecSchema.ReinitEquClassesPattern();
                _equClassPartitions.Add(ecSchema, new List<TEntity>(1) { entity });
            }
            else _equClassPartitions[ecSchema].Add(entity);

            return;
        }
    }
}
