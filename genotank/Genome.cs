using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace genotank {
    class Genome {
        //readonly List<Expression<Func<double, double>>> _outputs; // Make array?
        readonly God _god;
        private static readonly Random RNG = new Random(Configuration.Seed);
        private readonly Expression<Func<double, double>> _sauce;

        public double? Fitness { get; set; }

        internal Genome(Expression<Func<double, double>> func, God god) {
            Output = func.Compile();
            _sauce = func;
            _god = god;
        }

        internal Expression<Func<double, double>> Sauce { get; set; }

        internal Func<double, double> Output { get; private set; }

        internal Genome Clone() {
            return new Genome(_sauce, _god);
        }

        internal Genome Crossover(Genome other) {
            return new Genome(_sauce, _god);
        }

        internal Genome Mutate() {
            return new Genome(_sauce, _god);
        }
    }
}
