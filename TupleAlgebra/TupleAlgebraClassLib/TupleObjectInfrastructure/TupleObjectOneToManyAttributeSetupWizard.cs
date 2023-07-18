using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public class TupleObjectOneToManyAttributeSetupWizard<TAttributeContainer, TAttribute>
        : TupleObjectAttributeSetupWizard<TAttributeContainer>
    {
        protected TupleObjectOneToManyAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            LambdaExpression memberAccess)
            : base(schema, memberAccess)
        { }

        public static TupleObjectOneToManyAttributeSetupWizard<TEnumerable, TAttribute> Construct<TEntity, TEnumerable>(
            ITupleObjectSchemaProvider schema,
            Expression<Func<TEntity, TAttributeContainer>> memberAccess)
            where TEnumerable : TAttributeContainer, IEnumerable<TAttribute>
        {
            return new TupleObjectOneToManyAttributeSetupWizard<TEnumerable, TAttribute>(schema, memberAccess);
        }

        public static TupleObjectOneToManyAttributeSetupWizard<TDictionary, KeyValuePair<TKey, TAttribute>> Construct<TEntity, TDictionary, TKey>(
            ITupleObjectSchemaProvider schema,
            Expression<Func<TEntity, TDictionary>> memberAccess)
            where TDictionary : TAttributeContainer, IDictionary<TKey, TAttribute>
        {
            return new TupleObjectOneToManyAttributeSetupWizard<TDictionary, KeyValuePair<TKey, TAttribute>>(schema, memberAccess);
        }

        public TupleObjectOneToOneAttributeSetupWizard<TAttribute> OneToOneRelation()
        {
            return TupleObjectOneToOneAttributeSetupWizard<TAttribute>.Construct(Schema, _attributeName);
        }

        protected virtual Func<TAttributeContainer, TAttribute> TransitionFunc<TEntity>()
        {
            return null;
        }
    }

    /*
     * Будущий я!
     * Пишу тебе из 07.05.2023 22:20.
     * Надеюсь, ты доберёшься до этого хлама. Возможно, живой.
     * Это пример перевода атрибута из отношения один-ко-многим в отношение один-к-одному.
     * Он ещё недоделанный.
     * Объяснение сути TransitionFunc:
     * 1) имеем геттер one-to-many;
     * 2) имеем геттер one-to-one
     * 2) енумератор Getter() принимает сущность и на первом шаге применяет на ней OTM-getter
     * 4) далее на первом и следующих (если они есть) шагах он просто перечисляет поштучно данные из OTM
     * 5) Getter замыкает OTM;
     * 6) OTO замыкает Getter.
     * Что нужно сделать:
     * 1) отдельный енумератор взамен Getter. Он должен принимать объект сущности. Принять он может только внутри 
     * one-to-one геттера, в котором он уже считается созданным. То бишь нужно сделать отдельный метод 
     * void GiveEntity<TEntity>(TEntity e). 
     * В one-to-one геттере нужно сделать обработку случая останова Getter. И освобождение ресурсов где-то как-то сделать.
     * 2) в Attributeinfo нужно добавить информацию о множественном отношении (one-to-one/one-to-many*ORIG и
     * one-to-one*MODIF, последний означает вот этот вот переход от ван-ту-мэни к ван-ту-ан).
     * 3) В фабричных методах кортежей добавить, соответственно, обработку двух случаев (первые два считаются
     * отношением "один объект к одному объекту", где объект one-to-many - что-то перечислимое).
     * Кортеж без таких one-to-one*MODIF - обычный кортеж с одним значением во всех атрибутах. 
     * С такими - для каждогоо атрибута получается компонента, составленная из всех значений ([1, 2, 3]).
     * напр. User { 1: list LikedUsers, 2: list LikedBy, 3: string Nickname } 
     * с LikedUsers.OneToOneRelation(), LikedBy.OneToOneRelation():
     * wenograydObj { list[Ildar, Gylfie, Polk], list[Gylfie, Dodma], WeNoGrayD}
     * wenograydCTuple --==--
     * раскладывается на:
     * { 1: Ildar, 2: Gylfie, 3: WeNoGrayD }
     * { 1: Gylfie, 2: Gylfie, 3: WeNoGrayD }
     * { 1: Polk, 2: Gylfie, 3: WeNoGrayD }
     * { 1: Ildar, 2: Dodma, 3: WeNoGrayD }
     * { 1: Gylfie, 2: Dodma, 3: WeNoGrayD }
     * { 1: Polk, 2: Dodma, 3: WeNoGrayD }
     * При OneToMany() у обоих свойств было бы следующее:
     * { 1: [Ildar, Gylfie, Polk], 2: [Gylfie, Dodma], WeNoGrayD}
     */

    /*
    public class DictionaryBasedTupleObjectOneToManyAttributeSetupWizard<TKey, TAttribute>
        : TupleObjectOneToManyAttributeSetupWizard<IDictionary<TKey, TAttribute>, TAttribute>
    {
        private DictionaryBasedTupleObjectOneToManyAttributeSetupWizard(
            ITupleObjectSchemaProvider schema,
            LambdaExpression memberAccess)
            : base(schema, memberAccess)
        { }

        public static TupleObjectOneToManyAttributeSetupWizard<TDictionary, KeyValuePair<TKey, TAttribute>> Construct<TEntity, TDictionary, TKey>(
            ITupleObjectSchemaProvider schema,
            Expression<Func<TEntity, TDictionary>> memberAccess)
            where TDictionary : TAttributeContainer, IDictionary<TKey, TAttribute>
        {
            return new TupleObjectOneToManyAttributeSetupWizard<TDictionary, KeyValuePair<TKey, TAttribute>>(schema, memberAccess);
        }

        protected override Func<IDictionary<TKey, TAttribute>, TAttribute> TransitionFunc<TEntity>()
        {
            Func<TEntity, IDictionary<TKey, TAttribute>> oneToManyGetter = 
                Schema[_attributeName].Value.Getter<TEntity, IDictionary<TKey, TAttribute>>();

            IEnumerator<TAttribute> enumerator = Getter();
            Func<TEntity, TAttribute> oneToOneGetter = (e) =>
            {
                enumerator.MoveNext();
                return enumerator.Current;
            }; 

            return null;

            IEnumerator<TAttribute> Getter()
            {
                IEnumerator<KeyValuePair<TKey, TAttribute>> enumerator = oneToManyGetter(default(TEntity)).GetEnumerator();
                while (enumerator.MoveNext())
                    yield return enumerator.Current.Value;
            }
        }
    }
    */
}
