using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace genotank {
    class God {
        Random _random = new Random(Configuration.Seed);
        List<Node> _terminals;
        List<Variable> _inputs;

        int _numInputs, _numOutputs;

        delegate Node NodeFactory();

        List<Type> _allOperators;
        double _numNodes;
        Configuration _config;

        internal God(Configuration config, List<Variable> inputs, int numOutputs) {
            _numOutputs = numOutputs;
            _numInputs = inputs.Count;
            _config = config;
            int numConstants = Math.Max(_random.Next(_numInputs + 1) + _numInputs / 2, 2);
            int numTerminals = numConstants + _numInputs;
            _inputs = new List<Variable>(_numInputs);
            _terminals = new List<Node>(numTerminals);

            _allOperators = (from type in Assembly.GetExecutingAssembly().GetTypes()
                        where !type.IsAbstract && type.Name.Contains("Operator")
                        select type).ToList();

            _numNodes = numTerminals + _allOperators.Count;

            numConstants.Times(() => {
                _terminals.Add(new Constant(_random.NextDouble()*5));
            });

            foreach (Variable v in inputs) {
                _terminals.Add(v);
            }
            _inputs = inputs;
        }

        internal Genome BuildGenome() {
            List<Node> outputs = new List<Node>(_numOutputs);
            _numOutputs.Times(() => {
                outputs.Add(RampedHalfAndHalf());
            });
            return new Genome(outputs, this);
        }


        internal Node RampedHalfAndHalf() {
            int depth = _config.MaxDepth - _random.Next(_config.MaxDepth - _config.MinDepth + 1);
            NodeFactory factory  = _random.Next(2) == 1 ? (NodeFactory)AnyNode : NonTerminalNode;
            return BuildTree(0, depth, factory);
        }

        private Node AnyNode() {
            double choice = _random.NextDouble();
            if (choice < _allOperators.Count / _numNodes) {
                return NonTerminalNode();
            } else {
                return _terminals[_random.Next(_terminals.Count)];
            }
        }

        private Node NonTerminalNode() {
            return (Node)_allOperators[_random.Next(_allOperators.Count)].GetConstructor(Type.EmptyTypes).Invoke(null);
        }

        private Node BuildTree(int currentDepth, int depth, NodeFactory factory) {
            Node node;
            if (currentDepth > depth) {
                Debugger.Break();
            }
            if (currentDepth == depth) {
                node = RandomTerminal();
            } else {
                node = factory();
            }

            for (int i = 0; i < node.Arity; i++) {
                node.children[i] = BuildTree(currentDepth + 1, depth, factory);
            }
            return node;
        }

        private Node RandomTerminal() {
            return _terminals[_random.Next(_terminals.Count)];
        }
    }
}
