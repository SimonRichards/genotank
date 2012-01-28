using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace genotank {
    internal partial class Report : Form {

        Configuration _config;
        readonly GeneticTask _task;
        readonly Series _bestSeries;
        //Series _medianSeries;

        internal Report() {
            InitializeComponent();
            _config = new Configuration(true);

            _bestSeries = new Series("Best Fitness", _config.Generations) {
                ChartType = SeriesChartType.Line
            };
            /*
            _medianSeries = new Series("Median Fitness", _config.Generations) {
                ChartType = SeriesChartType.Line
            };*/

            progressBar.Maximum = _config.PopSize;
            progressBar.Step = 1;

            _task = new GeneticTest(_config);
            Run();
        }

        private /*async */void Run() {
            int generation = 0;
            var s = Stopwatch.StartNew();
            var population = _task.GeneratePopulation();
            while (++generation < _config.Generations) {
                population.Evaluate();
                var best = population.Best;
                Console.WriteLine("Generation {0} best: {1}\n{2}", generation - 1, best.Value, best.Key.Outputs[0]);
                if (best.Value < _config.Threshold) {
                    break;
                }
                population = population.Next;
                progressBar.PerformStep();
            }
            var winner = population.Best;
            //Console.WriteLine("Generation {0} winner: {1}\n{2}", i, best.Value, best.Key.Outputs[0]);
            //_bestSeries.Points.AddXY(i, best.Value);
            //_medianSeries.Points.AddXY(i, median);
            //var winner = current.Best;
            Console.WriteLine("Time elapsed = " + s.Elapsed.TotalSeconds);
            progressPlot.Series.Add(_bestSeries);
            //progressPlot.Series.Add(_medianSeries);

            resultPlot.Series.Add(_task.Function);
            resultPlot.Series.Add(_task.Plot(winner.Key));

            //Console.WriteLine(winner.Key.Outputs[0]);
            //Console.WriteLine("Fitness = " + winner.Value);
        }
    }
}
