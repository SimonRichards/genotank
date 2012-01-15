using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Threading;

namespace genotank {
    internal class GeneticSine : GeneticTask {
        Variable _x;
        public Series _sine;


        public override Series Function {
            get {
                return _sine;
            }
        }

        internal GeneticSine(Configuration configuration) : base(configuration) {
            _inputs = new List<Variable>();
            _x = new Variable("x");
            _inputs.Add(_x);
            _sine = new Series("Sine", 100);
            _sine.ChartType = SeriesChartType.Spline;
            for (double d = -5; d < 5; d += 0.1) {
                _sine.Points.AddXY(d, Math.Sin(d));
            }
        }

        internal override double Fitness(Genome individual) {
            double sumOfSquares = 0;
            int i = 0;
            Series current = new Series("Actual", 100);
            for (double x = -5; x < 5; x += 0.1, i++) {
                _inputs[0].Value = x;
                double actual = individual.Outputs[0].Solve();
                current.Points.AddXY(x, actual);
                double error = _sine.Points[i].YValues[0] - actual;
                sumOfSquares += error * error;
            }
            //new XYPlot(new Series[] { _sine, current }).Show();
            return sumOfSquares;
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
