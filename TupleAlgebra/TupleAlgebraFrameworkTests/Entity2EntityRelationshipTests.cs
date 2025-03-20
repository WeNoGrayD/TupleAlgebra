using System.Collections;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure.OrderedFiniteEnumerable;
using TupleAlgebraClassLib.AttributeComponentFactoryInfrastructure;
using TupleAlgebraClassLib.TupleObjectInfrastructure;
using TupleAlgebraFrameworkTests.DataModels;

namespace TupleAlgebraFrameworkTests;

[TestClass]
public class Entity2EntityRelationshipTests
{
    //private static bool _alphabetWasConfigured = false;

    public void CustomAlphabet(
        TupleObjectBuilder<Alphabet<int, int, int>> builder)
    {
        //if (_alphabetWasConfigured) return;
        //_alphabetWasConfigured = true;

        IAttributeComponentFactory<int> intFactory =
            new OrderedFiniteEnumerableAttributeComponentFactory<int>(
                Enumerable.Range(1, 21));

        builder.Attribute(abc => abc.A).SetFactory(intFactory).Attach();
        builder.Attribute(abc => abc.B).SetFactory(intFactory).Attach();
        builder.Attribute(abc => abc.C).SetFactory(intFactory).Attach();
    }

    public void CustomDominusList(
        TupleObjectBuilder<Alphabet<int, int, int>> builder)
    {
        //if (_alphabetWasConfigured) return;
        //_alphabetWasConfigured = true;

        IAttributeComponentFactory<int> intFactory =
            new OrderedFiniteEnumerableAttributeComponentFactory<int>(
                Enumerable.Range(1, 21));

        builder.Attribute(abc => abc.A).SetFactory(intFactory).Attach();
        builder.Attribute(abc => abc.B).SetFactory(intFactory).Attach();
        builder.Attribute(abc => abc.C).SetFactory(intFactory).Attach();
    }

    public void CustomDominusHashSet(
        TupleObjectBuilder<Alphabet<int, int, int>> builder)
    {
        //if (_alphabetWasConfigured) return;
        //_alphabetWasConfigured = true;

        IAttributeComponentFactory<int> intFactory =
            new OrderedFiniteEnumerableAttributeComponentFactory<int>(
                Enumerable.Range(1, 21));

        builder.Attribute(abc => abc.A).SetFactory(intFactory).Attach();
        builder.Attribute(abc => abc.B).SetFactory(intFactory).Attach();
        builder.Attribute(abc => abc.C).SetFactory(intFactory).Attach();
    }

    public void CustomDominusDictionary(
        TupleObjectBuilder<Alphabet<int, int, int>> builder)
    {
        //if (_alphabetWasConfigured) return;
        //_alphabetWasConfigured = true;

        IAttributeComponentFactory<int> intFactory =
            new OrderedFiniteEnumerableAttributeComponentFactory<int>(
                Enumerable.Range(1, 21));

        builder.Attribute(abc => abc.A).SetFactory(intFactory).Attach();
        builder.Attribute(abc => abc.B).SetFactory(intFactory).Attach();
        builder.Attribute(abc => abc.C).SetFactory(intFactory).Attach();
    }

    [TestMethod]
    public void MakeDominus()
    {
    }
}
