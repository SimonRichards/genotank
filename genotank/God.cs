using System;
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

        delegate Node NodeFactory(int selection, int depth);

        Configuration _config;
        private const int NonTerminalNodes = 3;
        private const int AllNodes = NonTerminalNodes + 2;

        internal God(Configuration config, List<Variable> inputs, int numOutputs) {
            _numOutputs = numOutputs;
            _numInputs = inputs.Count;
            _config = config;
            _inputs = inputs;
        }

        internal Genome BuildGenome() {
            var outputs = new Node[_numOutputs];
            for (int i = 0; i < _numInputs; i++) {
                outputs[i] = RampedHalfAndHalf();
            }
            return new Genome(outputs, this);
        }


        internal Node RampedHalfAndHalf() {
            return _random.Next(2) == 1 ? Full() : Grow();
        }


        private Node Full() {
            return RandomNode(NonTerminalNodes, 0);
        }

        private Node Grow() {
            return RandomNode(AllNodes, 0);
        }

        private Node RandomNode(int selection, int depth) {
            Debug.Assert(depth <= _config.MaxDepth, "Depth exceeded max");
            var factory = depth < _config.MaxDepth ?
                (NodeFactory)RandomNode : RandomTerminal;
            switch (_random.Next(selection)) {
                case 0:
                    return new AddOperator(factory(selection, depth + 1), factory(selection, depth + 1));
                case 1:
                    return new SubtractOperator(factory(selection, depth + 1), factory(selection, depth + 1));
                case 2:
                    return new MultiplyOperator(factory(selection, depth + 1), factory(selection, depth + 1));
                default:
                    return RandomTerminal(0, 0);
            }
        }

        private Node RandomTerminal(int selection, int depth) {
            switch (_random.Next(2)) {
                case 0:
                    return _inputs[_random.Next(_inputs.Count)];
                case 1:
                    return new Constant(_random.NextDouble());
                default:
                    throw new Exception("You fucked up");
            }
        }
    }
}
