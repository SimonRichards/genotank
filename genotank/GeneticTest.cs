using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Threading;

namespace genotank {
    internal class GeneticTest : GeneticTask {
        Variable _x;
        public Series _function;

        internal override double LeftLim { get { return -5; } }
        internal override double RightLim { get { return 5; } }

        public override Series Function {
            get {
                return _function;
            }
        }

        internal GeneticTest(Configuration configuration)
            : base(configuration) {
            _inputs = new List<Variable>();
            _x = new Variable("x");
            _inputs.Add(_x);
            _function = new Series("Test Function", 100);
            _function.ChartType = SeriesChartType.Spline;
            for (double d = -5; d < 5; d += 0.1) {
                _function.Points.AddXY(d, 5*d + 3);
            }
        }

        internal override double Fitness(Genome individual) {
            double sumOfSquares = 0;
            int i = 0;
            for (double x = -5; x < 5; x += 0.1, i++) {
                _inputs[0].Value = x;
                double actual = individual.Outputs[0].Solve();
                double error = _function.Points[i].YValues[0] - actual;
                sumOfSquares += error * error;
            }
            return sumOfSquares;
        }

        internal override double Fitness(Generation.Solver solution) {
            return 0;
        }

        internal override List<Variable> Inputs {
            get {
                return _inputs;
            }
        }

        internal override int NumOutputs {
            get {
                return 1;
            }
        }
    }
}
