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

        internal override double LeftLim { get { return -2; } }
        internal override double RightLim { get { return 2; } }


        public override Series Function {
            get {
                return _sine;
            }
        }

        internal GeneticSine(Configuration configuration) : base(configuration) {
            _inputs = new List<Variable>();
            _x = new Variable("x");
            _inputs.Add(_x);
            _sine = new Series("Sine");
            _sine.ChartType = SeriesChartType.Spline;
            for (double d = LeftLim; d < RightLim; d += 0.1) {
                _sine.Points.AddXY(d, Math.Sin(d));
            }
        }

        internal override double Fitness(Genome individual) {
            double sumOfSquares = 0;
            int i = 0;
            for (double x = LeftLim; x < RightLim; x += 0.1, i++) {
                _inputs[0].Value = x;
                double actual = individual.Outputs[0].Solve();
                double error = _sine.Points[i].YValues[0] - actual;
                sumOfSquares += error * error;
            }
            return sumOfSquares;
        }

        internal override double Fitness(Generation.Solver solution) {
            double sumOfSquares = 0;
            int i = 0;
            for (double x = LeftLim; x < RightLim; x += 0.1, i++) {
                double actual = solution(x);
                double error = _sine.Points[i].YValues[0] - actual;
                sumOfSquares += error * error;
            }
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
