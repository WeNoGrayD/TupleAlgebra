using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TupleAlgebraClassLib.AttributeComponents;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.PredicateBased.TupleBased;
using TupleAlgebraClassLib.NonFictionalAttributeComponentImplementations.Variable;

namespace TupleAlgebraClassLib.TupleObjectInfrastructure
{
    public interface IVariableContainer
        : IEnumerable<IVariableAttributeComponent>
    {
        public void Reset();

        public IEnumerable<string> GetVariableNames();

        public bool AnyIsEmpty(IAttributeComponentWithVariables ac)
        {
            return ac.Variables.Any(v => ContainsVariable(v) && v.IsEmpty);
        }

        public bool ContainsVariable(IVariableAttributeComponent variable);

        public void AddVariable(IVariableAttributeComponent variable);

        public void AddVariableRange(IAttributeComponentWithVariables ac);

        public void AddVariableRange(
            IEnumerable<IVariableAttributeComponent> variables);

        public IVariableContainerSnapshot SaveSnapshot();

        public void LoadSnapshot(IVariableContainerSnapshot snapshot);
    }

    public interface IVariableContainerSnapshot
    {
        public IAttributeComponent this[string attrName] { get; }
    }

    public class VariableContainerSnapshot
        : IVariableContainerSnapshot
    {
        private IDictionary<string, IAttributeComponent> _variables;

        public IAttributeComponent this[string attrName] 
        { get => _variables[attrName]; }

        public VariableContainerSnapshot(VariableContainer container)
        {
            _variables = new Dictionary<string, IAttributeComponent>();
            foreach (string variableName in container.GetVariableNames())
                _variables.Add(variableName, container[variableName]);

            return;
        }
    }

    public class VariableContainer 
        : IVariableContainer, 
          IVariableContainerSnapshot
    {
        private IDictionary<string, IVariableAttributeComponent> _variables;

        public IAttributeComponent this[string attrName]
        { get => _variables[attrName].CurrentValue; }

        public VariableContainer()
        {
            _variables = new Dictionary<string, IVariableAttributeComponent>();
        }

        public static VariableContainer Empty()
        {
            return new VariableContainer();
        }

        public IEnumerable<string> GetVariableNames()
        {
            return _variables.Keys;
        }

        public void Reset()
        {
            foreach (var variable in _variables.Values)
                variable.Reset();

            return;
        }

        public bool ContainsVariable(IVariableAttributeComponent variable)
        {
            IVariableAttributeComponent containingVariable;
            if (_variables.TryGetValue(variable.Name, out containingVariable))
            {
                if (object.ReferenceEquals(variable, containingVariable))
                {
                    return true;
                }
            }

            return false;
        }

        public void AddVariable(IVariableAttributeComponent variable)
        {
            IVariableAttributeComponent containingVariable;
            if (_variables.TryGetValue(variable.Name, out containingVariable))
            {
                if (!object.ReferenceEquals(variable, containingVariable))
                {
                    throw new Exception($"Провалена попытка добавить переменную {variable.Name}: уже существует другая переменная с идентичным именем.");
                }
            }
            else
                _variables.Add(variable.Name, variable);

            return;
        }

        public void AddVariableRange(IAttributeComponentWithVariables ac)
        {
            foreach (var variable in ac.Variables)
                AddVariable(variable);

            return;
        }

        public void AddVariableRange(
            IEnumerable<IVariableAttributeComponent> variables)
        {
            foreach (var variable in variables)
                AddVariable(variable);

            return;
        }

        public IVariableContainerSnapshot SaveSnapshot()
        {
            return new VariableContainerSnapshot(this);
        }

        public void LoadSnapshot(IVariableContainerSnapshot snapshot)
        {
            foreach (string variableName in GetVariableNames())
                _variables[variableName].CurrentValue = snapshot[variableName];
        }

        public IEnumerator<IVariableAttributeComponent> GetEnumerator()
        {
            return _variables.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
