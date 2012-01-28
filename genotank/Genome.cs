using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace genotank {
    class Genome {
        readonly List<Node> _outputs; // Make array?
        readonly God _god;
        private static readonly Random Random = new Random(Configuration.Seed);

        internal Genome(List<Node> outputs, God god) {
            _outputs = outputs;
            _god = god;
        }

        internal Genome Clone() {
            var outputs = new List<Node>(_outputs.Count);
            outputs.AddRange(_outputs.Select(t => t.DeepClone()));
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
            Node head = _outputs[Random.Next(_outputs.Count)];
            var allNodes = new List<Node>();
            head.AddToList(allNodes);
            return allNodes[Random.Next(allNodes.Count)];
        }

        private void SetRandomNode(Node newNode) {
            int n = Random.Next(_outputs.Count);
            var head = _outputs[n];
            if (head.Arity > 0) {
                var allNodes = new List<Node>();
                head.AddToList(allNodes);
                Node toSet;
                do {
                    toSet = allNodes[Random.Next(allNodes.Count)];
                } while (toSet.Arity == 0);
                toSet.Children[Random.Next(toSet.Arity)] = newNode;
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
