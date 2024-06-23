using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using System.Linq.Expressions;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjects;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.Variable;
using TupleAlgebraClassLib.AttributeComponents;
using System.Xml.Linq;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public static class TupleObjectHelper
    {
        #region Constants

        private const string STRING_TYPE_NAME = nameof(String);

        #endregion

        #region Delegates

        public delegate void TupleObjectBuildingHandler<TEntity>(
            TupleObjectBuilder<TEntity> builder);

        public delegate TData AttributeGetterHandler<TEntity, TData>(TEntity entity);

        public delegate TEntity AttributeGetterHandler<TEntity>(TEntity entity);

        public delegate TEntity PrimitiveEntityFactoryHandler<TEntity>(IEnumerator<TEntity> property);

        public delegate ITupleObjectAttributeSetupWizard
            AttributeSetupWizardFactoryHandler(
                ITupleObjectSchemaProvider schema,
                AttributeName attrName);

        public delegate ITupleObjectAttributeSetupWizard<TAttribute>
            AttributeSetupWizardFactoryHandler<TAttribute>(
                ITupleObjectSchemaProvider schema,
                AttributeName attrName);

        #endregion

        #region Static properties

        public static AttributeMemberExtractor MemberExtractor { get; } =
            new AttributeMemberExtractor();

        #endregion

        #region Static methods

        public static bool IsEntityTypePrimitive(TypeInfo entityType)
        {
            return entityType.IsPrimitive || entityType.Name == STRING_TYPE_NAME;
        }

        public static SingleTupleObjectFactoryArgs<TEntity, TAttribute>
            SetAC<TEntity, TAttribute>(
                Expression<AttributeGetterHandler<TEntity, TAttribute>> getter,
                NonFictionalAttributeComponentFactoryArgs<TAttribute> componentFactoryArgs)
        {
            return new SingleTupleObjectFactoryArgs<TEntity, TAttribute>(
                getter, componentFactoryArgs);
        }

        public static NamedComponentFactoryArgs<IAttributeComponent>
            SetAC<TEntity, TAttribute>(
                Expression<AttributeGetterHandler<TEntity, TAttribute>> getter,
                IAttributeComponent componentFactoryArgs,
                TupleObjectBuilder<TEntity> builder)
        {
            return new NamedComponentFactoryArgs<IAttributeComponent>(
                getter,
                builder,
                componentFactoryArgs);
        }

        public static NamedComponentFactoryArgs<IAttributeComponent>
            SetVariable<TEntity, TAttribute>(
                Expression<AttributeGetterHandler<TEntity, TAttribute>> getter,
                string name,
                TupleObjectBuilder<TEntity> builder)
        {
            return new NamedComponentFactoryArgs<IAttributeComponent>(
                getter,
                builder,
                builder.Attribute(getter).CreateManager().CreateVariable(name));
        }

        /*
        public static TupleObject<TEntity> CreateMasked<TEntity>(
            TupleObjectFactory factory,
            params Expression<AttributeGetterHandler<TEntity, object>>[] maskedAttributes)
            where TEntity : new()
        {
            return factory.CreateConjunctiveTuple();

            IEnumerable<IVariableAttributeComponent> GetFactoryArgs()
            {
                foreach (var e in maskedAttributes)
                {
                    yield return 
                }

                yield break;
            }

            IVariableAttributeComponent GetVariable<TAttribute>(
                Expression<AttributeGetterHandler<TEntity, TAttribute>> getter)
            {
                return new VariableAttributeComponentFactoryArgs(
            }
        }
        */

        public enum Overlay : sbyte
        {
            HaveNotIntersection = -1,
            HaveIntersection = 0,
            FullyMatch = 1
        }

        public static (Overlay Overlay, IEnumerable<AttributeName> Intersected)
            DefineSchemasOverlay<TEntity>(
            TupleObjectSchema<TEntity> first,
            TupleObjectSchema<TEntity> second)
        {
            AttributeName[] intersected = first.PluggedAttributeNames
                .ToHashSet()
                .Intersect(second.PluggedAttributeNames)
                .ToArray();

            return (intersected.Length switch
                {
                    0 => Overlay.HaveNotIntersection,
                    int x when x == first.PluggedAttributesCount && 
                               x == second.PluggedAttributesCount => 
                         Overlay.FullyMatch,
                    _ => Overlay.HaveIntersection
                },
                intersected);
        }

        public static bool ConjunctiveTupleObjectMatchAny<TEntity>(
            TupleObject<TEntity> cObject,
            TupleObject<TEntity> filter)
            where TEntity : new()
        {
            (Overlay overlay, _) =
                DefineSchemasOverlay(cObject.Schema, filter.Schema);
            // Если в D-кортеже/систему отсутствуют эти атрибуты, то он эквивалентен формуле
            // "для всех _filter.Schema (dTuple & _filter)"
            if (overlay == Overlay.HaveNotIntersection) return true;

            return ConjunctiveTupleObjectMatchAll(cObject, filter);
        }

        public static bool DisjunctiveTupleObjectMatchAny<TEntity>(
            TupleObject<TEntity> dObject,
            TupleObject<TEntity> filter)
            where TEntity : new()
        {
            (Overlay overlay, _) =
                DefineSchemasOverlay(dObject.Schema, filter.Schema);
            // Если в D-кортеже/систему отсутствуют эти атрибуты, то он эквивалентен формуле
            // "для всех _filter.Schema (dObject & _filter)"
            if (overlay == Overlay.HaveNotIntersection) return true;
            TupleObject<TEntity> firstTrimmed = dObject.AlignWithSchema(
                filter.Schema,
                filter.Factory,
                null);
            // Если D-кортеж/система, приведённые к _filter.Schema, эквивалентен пустому АК-объекту,
            // то он эквивалентен формуле
            // "для всех _filter.Schema (dObject & _filter), если dObject is not empty"
            if (firstTrimmed.IsEmpty()) return !dObject.IsFalse();

            return !(firstTrimmed & filter).IsFalse();
        }

        public static bool ConjunctiveTupleObjectMatchAll<TEntity>(
            TupleObject<TEntity> cObject,
            TupleObject<TEntity> filter)
            where TEntity : new()
        {
            TupleObject<TEntity> firstTrimmed = cObject.AlignWithSchema(
                filter.Schema,
                filter.Factory,
                null);
            // Если C-кортеж/система, приведённые к _filter.Schema, эквивалентен пустому АК-объекту,
            // то он эквивалентен формуле
            // "для всех _filter.Schema (cObject & _filter)"
            if (firstTrimmed.IsEmpty()) return false;
            if (firstTrimmed.IsFull()) return true;

            return (filter / firstTrimmed).IsFalse();
        }

        public static bool DisjunctiveTupleObjectMatchAll<TEntity>(
            TupleObject<TEntity> dObject,
            TupleObject<TEntity> filter)
            where TEntity : new()
        {
            (Overlay overlay, _) =
                DefineSchemasOverlay(dObject.Schema, filter.Schema);
            // Если в D-кортеже/систему отсутствуют эти атрибуты, то он эквивалентен формуле
            // "для всех _filter.Schema (dObject & _filter)"
            if (overlay == Overlay.HaveNotIntersection) return true;
            TupleObject<TEntity> firstTrimmed = dObject.AlignWithSchema(
                filter.Schema,
                filter.Factory,
                null);
            // Если D-кортеж/система, приведённые к _filter.Schema, эквивалентен пустому АК-объекту,
            // то он эквивалентен формуле
            // "для всех _filter.Schema (dObject & _filter), если dObject is not empty"
            if (firstTrimmed.IsEmpty()) return !dObject.IsFalse();

            return (filter / firstTrimmed).IsFalse();
        }

        #endregion
    }
}
