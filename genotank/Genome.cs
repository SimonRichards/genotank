using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace genotank {
    class Genome {
        readonly Node[] _outputs;
        readonly God _god;
        private static readonly Random Random = new Random(Configuration.Seed);

        public double? Fitness { get; set; }

        internal Genome(Node[] outputs, God god) {
            _outputs = outputs;
            _god = god;
        }

        private Genome Copy() {
            return new Genome(_outputs, _god);
        }

        internal Genome Clone() {
            return this;
        }

        internal Genome Crossover(Genome other) {
            Genome genome = Copy();
            return genome;
        }

        internal Genome Mutate() {
            Genome genome = Copy();
            return genome;
        }

        internal Node[] Outputs {
            get {
                return _outputs;
            }
        }
    }
}
