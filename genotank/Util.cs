using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace genotank {
    static class Util {
        private static Random _random = new Random(Configuration.Seed);

        internal static void Times(this int i, Action a) {
            for (int j = 0; j < i; j++) {
                a();
            }
        }

        internal static void StepTo(this double d, double step, double to, Action<double> a) {
            double current = d;
            while (current < to) {
                a(current);
                current += step;
            }
        }
    }
}
