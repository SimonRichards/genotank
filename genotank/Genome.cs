using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace genotank {
    class Genome {
        readonly Node[] _outputs;
        private readonly Node Head;
        readonly God _god;
        private static readonly Random Random = new Random(Configuration.Seed);

        public double Fitness { get; set; }

        internal Genome(Node[] outputs, God god) {
            _outputs = outputs;
            Head = outputs[0];
            _god = god;
        }

        internal Genome Crossover(Genome other) {
            return new Genome(new[] {ReplaceRandomWith(other.GetRandom())}, _god);
        }

        internal Genome Mutate() {
            return new Genome(new[] { ReplaceRandomWith(_god.RampedHalfAndHalf()) }, _god);
        }

        private Node ReplaceRandomWith(Node newNode) {
            return _outputs[0].Clone(GetRandomChain(), newNode);
        }

        private Stack<Node> GetRandomChain() {
            var stack = new Stack<Node>();
            var target = GetRandom();
            Head.Find(stack, target);
            return stack;
        }

        private Node GetRandom() {
            int index = Random.Next(Head.Count);
            int count = 0;
            Node result = Head;
            Head.Accept(node => {
                if (count++ == index) {
                    result = node;
                }
            });
            return result;
        }

        internal Node[] Outputs {
            get {
                return _outputs;
            }
        }
    }
}
