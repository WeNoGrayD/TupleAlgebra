using Microsoft.Analytics.Interfaces;
using Microsoft.Analytics.Types.Sql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UniversalClassLib
{
    public interface IInspectionRule<TNode>
    {
        Exception OnFailureException { get; }

        bool Inspect(object data)
        {
            return this switch
            {
                SimpleNodeInspectionRule<TNode> rule => 
                    VisitSimpleNodeInspectionRule(rule, (TNode)data),
                NodeWithMemoryInspectionRule<TNode> rule => 
                    VisitNodeWithMemoryInspectionRule(rule, (NodeWithMemory<TNode>)data)
            };
        }

        private bool VisitSimpleNodeInspectionRule(
            SimpleNodeInspectionRule<TNode> rule,
            TNode data)
        {
            return rule.Rule(data);
        }

        private bool VisitNodeWithMemoryInspectionRule(
            NodeWithMemoryInspectionRule<TNode> rule,
            NodeWithMemory<TNode> data)
        {
            return rule.Rule(data);
        }
    }

    public struct NodeWithMemory<TNode>
    {
        public TNode Node { get; init; }

        public object Memory { get; init; }

        private NodeWithMemory(TNode node, object memory)
        {
            Node = node;
            Memory = memory;

            return;
        }

        public static NodeWithMemory<TNode> FromOtherInspectorCurrentNode<TMemory>(
            TNode node,
            TMemory memory)
        {
            return new NodeWithMemory<TNode>(node, memory);
        }

        public static NodeWithMemory<TNode> FromOtherInspectorCurrentNode<TMemory>(
            TNode node,
            NodeTreeInspector<TMemory> otherInspector)
        {
            return new NodeTreeInspector<TNode>(
                node,
                otherInspector.CurrentNode switch
                {
                    NodeWithMemory<TMemory> nMem => nMem.Node,
                    _ => otherInspector.CurrentNode
                };
        }
    }

    public struct SimpleNodeInspectionRule<TNode> : IInspectionRule<TNode>
    {
        public Predicate<TNode> Rule { get; init; }

        public Exception OnFailureException { get; init; }

        public SimpleNodeInspectionRule(
            Predicate<TNode> rule,
            Exception onFailureException = null)
        {
            Rule = rule;
            OnFailureException = onFailureException;

            return;
        }
    }

    public struct NodeWithMemoryInspectionRule<TNode> : IInspectionRule<TNode>
    {
        public Predicate<NodeWithMemory<TNode>> Rule { get; init; }

        public Exception OnFailureException { get; init; }

        public SimpleNodeInspectionRule(
            Predicate<NodeWithMemory<TNode>> rule,
            Exception onFailureException = null)
        {
            Rule = rule;
            OnFailureException = onFailureException;

            return;
        }
    }

    public interface INodeTreeInspector
    {
        object CurrentNode { get; }

        bool InspectNode(object currentNode);

        bool InspectionEndedOk();
    }

    public class NodeTreeInspector<TNode> : INodeTreeInspector
    {
        private bool _requestForStop = false;

        protected IInspectionRule<TNode>[] _rules;

        protected System.Collections.IEnumerator _tapeReader;

        public string Name { get; private set; }

        public object CurrentNode { get; private set; }

        public NodeTreeInspector(
            string name, 
            params IInspectionRule<TNode>[] rules)
        {
            Name = name;
            _rules = rules;
            _tapeReader = ReadTape();

            return;
        }

        private IEnumerator ReadTape()
        {
            IInspectionRule<TNode> rule;

            for (int i = 0; i < _rules.Length; i++)
            {
                rule = _rules[i];

                if (_requestForStop)
                    throw new Exception($"{Name} не закончил инспекцию по списку правил, однако был получен запрос на остановку.");
                if(!rule.Inspect(CurrentNode)) throw rule.OnFailureException;

                yield return null;
            }

            if (!_requestForStop)
                throw new Exception($"{Name} закончил инспецию по списку правил, однако была предпринята попытка инспекции узла {CurrentNode}.");

            yield break;
        }

        public bool InspectNode(object currentNode)
        {
            CurrentNode = currentNode;

            return _tapeReader.MoveNext();
        }

        public bool InspectionEndedOk()
        {
            _requestForStop = true;

            return !_tapeReader.MoveNext();
        }
    }   

    public class NodeTypeTreeInspector : NodeTreeInspector<object>
    {
        public NodeTypeTreeInspector(params Type[] nodeTypes)
            : base("Инспектор типов", MakeRules(nodeTypes))
        {
            return;
        }

        private static IEnumerable<IInspectionRule<object>> MakeRules(Type[] nodeTypes)
        {
            Type currentType;

            for (int i = 0; i < nodeTypes.Length; i++)
                currentType = nodeTypes[i];
                yield return new SimpleNodeInspectionRule<object>(
                    d => d is currentType,
                    new Exception($"В инспекторе типов [{i}] ${d} не является {currentType}."));

            yield break;
        }
    }

    public class MetaNodeTreeInspector : NodeTreeInspector<INodeTreeInspector>
    {
        public MetaNodeTreeInspector(params INodeTreeInspector[] inspectors)
            : base(MakeRules(inspectors))
        {
            return;
        }

        private static IEnumerable<IInspectionRule<object>> MakeRules(INodeTreeInspector[] inspectors)
        {
            Type currentType;

            for (int i = 0; i < nodeTypes.Length; i++)
                currentType = nodeTypes[i];
            yield return new SimpleNodeInspectionRule<INodeTreeInspector>(
                inspector => inspector.InspectionEndedOk,
                new Exception($"В мета-инспекторе [{i}] @{d} не завершился корректно."));

            yield break;
        }
    }
}