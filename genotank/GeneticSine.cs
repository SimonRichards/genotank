using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using Tree;

namespace genotank {
    internal sealed class GeneticSine : GeneticTask {
        readonly Variable _x;
        private readonly Series _sine;
        private readonly List<Variable> _inputs;

        internal override double LeftLim { get { return -5; } }
        internal override double RightLim { get { return 5; } }
        private const double Step = 0.1;

        public override Series Function {
            get {
                return _sine;
            }
        }

        internal GeneticSine(Configuration configuration) : base(configuration) {
            _inputs = new List<Variable>();
            _x = new Variable("x");
            _inputs.Add(_x);
            _sine = new Series("Sine") {ChartType = SeriesChartType.Spline};
            for (double d = LeftLim; d < RightLim; d += Step) {
                _sine.Points.AddXY(d, Math.Sin(d));
            }
        }

        internal override double Fitness(Population pop) {
            double sumOfSquares = 0;
            int i = 0;
            for (double x = LeftLim; x < RightLim; x += Step, i++) {
                _inputs[0].Value = x;
                double actual = pop.Outputs[0].Solve();
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
