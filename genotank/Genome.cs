using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace genotank {
    class Genome {
        List<Node> _outputs; // Make array?
        God _god;
        static Random _random = new Random(Configuration.Seed);

        internal Genome(List<Node> outputs, God god) {
            _outputs = outputs;
            _god = god;
        }

        internal Genome Clone() {
            List<Node> outputs = new List<Node>(_outputs.Count);
            for (int i = 0; i < _outputs.Count; i++) {
                outputs.Add(_outputs[i].DeepClone());
            }
            return new Genome(outputs, _god);
        }

        internal Genome Crossover(Genome other) {
            Genome genome = Clone();
            genome.SetRandomNode(other.Clone().GetRandomNode());
            return genome;
        }

        internal Genome Mutate() {
            Genome genome = Clone();
            genome.SetRandomNode(_god.RampedHalfAndHalf());
            return genome;
        }

        private Node GetRandomNode() {
            Node head = _outputs[_random.Next(_outputs.Count)];
            List<Node> allNodes = new List<Node>();
            head.AddToList(allNodes);
            return allNodes[_random.Next(allNodes.Count)];
        }

        private void SetRandomNode(Node newNode) {
            int n = _random.Next(_outputs.Count);
            Node head = _outputs[n];
            if (head.Arity > 0) {
                List<Node> allNodes = new List<Node>();
                head.AddToList(allNodes);
                Node toSet;
                do {
                    toSet = allNodes[_random.Next(allNodes.Count)];
                } while (toSet.Arity == 0);
                toSet.children[_random.Next(toSet.Arity)] = newNode;
            } else {
                _outputs[n] = newNode;
            }
        }

        internal List<Node> Outputs {
            get {
                return _outputs;
            }
        }
    }
}
