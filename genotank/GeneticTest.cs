using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using Tree;

namespace genotank {
    internal sealed class GeneticTest : GeneticTask {
        readonly Variable _x;
        private readonly Series _function;
        private readonly List<Variable> _inputs;

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
            _inputs = new List<Variable>();
            _x = new Variable("x");
            _inputs.Add(_x);
            _function = new Series("Test Function", 100) {ChartType = SeriesChartType.Spline};
            for (double d = LeftLim; d < RightLim; d += Step) {
                _function.Points.AddXY(d, 5*Math.Pow(d,2) + 8*Math.Pow(d,3) + 28);
            }
        }

        internal override void Fitness(Population pop) {
            var sumsOfSquares = new double[pop.Size];
            sumsOfSquares.Initialize();

            int i = 0;
            for (double x = LeftLim; x < RightLim; x += Step, i++) {
                _inputs[0].Value = x;
                Parallel.For(0, pop.Size, (j, loop) => {
                    double actual = pop[j].Outputs[0].Solve();
                    double error = _function.Points[i].YValues[0] - actual;
                    sumsOfSquares[j] += Math.Abs(error); // *error;
                });
            }
            Parallel.For(0, pop.Size, (j, loop) => {
                pop[j].Fitness = sumsOfSquares[j];
            });
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
