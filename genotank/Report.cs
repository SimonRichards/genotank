using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace genotank {
    internal partial class Report : Form {

        Configuration _config;
        readonly GeneticTask _task;
        readonly Series _bestSeries;

        internal Report() {
            InitializeComponent();
            _config = new Configuration(true);

            _bestSeries = new Series("Best Fitness", _config.Generations) {
                ChartType = SeriesChartType.Line
            };

            progressBar.Maximum = _config.PopSize;
            progressBar.Step = 1;

            _task = new GeneticTest(_config);
            Run();
        }

        private async void Run() {
            int generation = 0;
            var s = Stopwatch.StartNew();
            var population = _task.GeneratePopulation();
            while (++generation < _config.Generations) {
                await TaskEx.Run(population.Evaluate);
                var best = population.Best;
                Console.WriteLine("Generation {0} best: {1}\n{2}", generation - 1, best.Value, best.Key.Outputs[0]);
                if (best.Value < _config.Threshold) {
                    Console.WriteLine("Threshold reached");
                    break;
                }
                population = population.Next;
                progressBar.PerformStep();
            }
            await TaskEx.Run(population.Evaluate);
            var winner = population.Best;
            Console.WriteLine("Time elapsed = " + s.Elapsed.TotalSeconds);
            progressPlot.Series.Add(_bestSeries);

            resultPlot.Series.Add(_task.Function);
            resultPlot.Series.Add(_task.Plot(winner.Key));

            Console.WriteLine(winner.Key.Outputs[0]);
            Console.WriteLine("Fitness = " + winner.Value);
        }
    }
}
