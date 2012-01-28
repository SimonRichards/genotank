using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using Tree;

namespace genotank {
    internal sealed class GeneticTest : GeneticTask {
        private readonly Series _function;

        const double Step = 1;

        internal override double LeftLim { get { return -10; } }
        internal override double RightLim { get { return 10; } }

        public override Series Function {
            get {
                return _function;
            }
        }

        internal GeneticTest(Configuration configuration)
            : base(configuration) {
            _function = new Series("Test Function", 100) {ChartType = SeriesChartType.Spline};
            for (double d = LeftLim; d < RightLim; d += Step) {
                _function.Points.AddXY(d, 5*Math.Pow(d,2) + 8*Math.Pow(d,3) + 28);
            }
        }

        internal override double Fitness(Genome individual) {
            double sumOfSquares = 0;
            int i = 0;
            for (double x = LeftLim; x < RightLim; x += Step, i++) {
                double actual = individual.Output(x);
                double error = _function.Points[i].YValues[0] - actual;
                sumOfSquares += Math.Abs(error);// *error;
            }
            return sumOfSquares;
        }

        internal override double Fitness(Generation.Solver solution) {
            return 0;
        }

        internal override IEnumerable<string> Inputs {
            get {
                return new[] {"x"};
            }
        }

        internal override int NumOutputs {
            get {
                return 1;
            }
        }
    }
}
