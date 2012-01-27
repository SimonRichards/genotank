using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace genotank {
    abstract class GeneticTask {
        protected List<Variable> _inputs;

        Population _population;
        Configuration _config;

        public Series Plot(Genome individual) {
            int i = 0;
            Series current = new Series("Actual");
            current.ChartType = SeriesChartType.Spline;
            for (double x = LeftLim; x < RightLim; x += 0.1, i++) {
                _inputs[0].Value = x;
                double actual = individual.Outputs[0].Solve();
                current.Points.AddXY(x, actual);
            }
            return current;
        }

        internal GeneticTask(Configuration configuration) {
            _config = configuration;
        }

        internal Population GeneratePopulation() {
            God factory = new God(_config, Inputs, NumOutputs);
            _population = new Population(_config, this);
            _config.PopSize.Times(() => {
                Genome genome = factory.BuildGenome();
                _population.Add(genome, Fitness(genome));
            });
            return _population;
        }

        internal async Task<Population> Step() {
            return await TaskEx.Run(() => {
                _population = _population.NextGeneration();
                return _population;
            });
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
