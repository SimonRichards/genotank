﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace genotank {
    class God {
        readonly Random _random = new Random(Configuration.Seed);
        readonly List<Variable> _inputs;

        readonly int _numInputs;
        readonly int _numOutputs;

        delegate Node NodeFactory();

        readonly List<Type> _allOperators;
        readonly double _numNodes;
        Configuration _config;

        internal God(Configuration config, List<Variable> inputs, int numOutputs) {
            _numOutputs = numOutputs;
            _numInputs = inputs.Count;
            _config = config;
            _inputs = new List<Variable>(_numInputs);

            _allOperators = (from type in Assembly.GetExecutingAssembly().GetTypes()
                        where !type.IsAbstract && type.Name.Contains("Operator")
                        select type).ToList();

            _numNodes = _numInputs + _allOperators.Count;
            _inputs = inputs;
        }

        internal Genome BuildGenome() {
            var outputs = new List<Node>(_numOutputs);
            for (int i = 0; i < _numInputs; i++) {
                outputs.Add(RampedHalfAndHalf());
            }
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
                return RandomTerminal();
            }
        }

        private Node NonTerminalNode() {
// ReSharper disable PossibleNullReferenceException
            return (Node)_allOperators[_random.Next(_allOperators.Count)].GetConstructor(Type.EmptyTypes).Invoke(null);
// ReSharper restore PossibleNullReferenceException
        }

        private Node BuildTree(int currentDepth, int depth, NodeFactory factory) {
            var node = currentDepth == depth ? RandomTerminal() : factory();

            for (int i = 0; i < node.Arity; i++) {
                node.Children[i] = BuildTree(currentDepth + 1, depth, factory);
            }
            return node;
        }

        private Node RandomTerminal() {
            if (_random.NextDouble() > 0.5) {
                return new Constant(_random.NextDouble() * _config.MaxConstant + 1);
            }
            return _inputs[_random.Next(_numInputs)];
        }
    }
}
