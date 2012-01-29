using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using Tree;

namespace genotank {
    abstract class GeneticTask {

        Population _population;
        Configuration _config;

        public Series Plot(Genome individual) {
            int i = 0;
            var current = new Series("Actual") {
                ChartType = SeriesChartType.Spline
            };
            for (double x = LeftLim; x < RightLim; x += 0.1, i++) {
                Inputs[0].Value = x;
                double actual = individual.Outputs[0].Solve();
                current.Points.AddXY(x, actual);
            }
            return current;
        }

        internal GeneticTask(Configuration configuration) {
            _config = configuration;
        }

        internal Population GeneratePopulation() {
            var factory = new God(_config, Inputs, NumOutputs);
            _population = new Population(_config, this);
            for (int i = 0; i < _config.PopSize; i++) {
                _population[i] = factory.BuildGenome();
            }
            return _population;
        }
        

        internal void Run() {
            for (int i = 0; i < _config.Generations; i++) {
                _population.Evaluate();
                _population = _population.Next;
            }
        }

        internal abstract List<Variable> Inputs { get; }

        internal abstract int NumOutputs {get;}

        internal abstract double Fitness(Genome individual);
        internal abstract double Fitness(Generation.Solver solution);

        internal abstract double LeftLim { get; }
        internal abstract double RightLim { get; }

        public abstract Series Function { get; }
    }
}
