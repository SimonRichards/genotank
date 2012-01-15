using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace genotank {
    internal partial class Report : Form {

        Configuration _config;
        GeneticTask _task;
        Series _bestSeries;
        Series _medianSeries;

        internal Report() {
            InitializeComponent();
            _config = new Configuration(true);

            _bestSeries = new Series("Best Fitness", _config.Generations);
            _bestSeries.ChartType = SeriesChartType.Line;

            _medianSeries = new Series("Median Fitness", _config.Generations);
            _medianSeries.ChartType = SeriesChartType.Line;

            _task = new GeneticTest(_config);
            Run();
        }

        private async void Run() {
            List<KeyValuePair<Genome, double>> orderedGenomes;
            var s = Stopwatch.StartNew();
            Population current = _task.GeneratePopulation();
            for (int i = 1; i <= _config.Generations; i++) {
                current = await _task.Step();
                orderedGenomes = current.OrderBy((x) => x.Value).ToList();
                double best = orderedGenomes.First().Value;
                double median = orderedGenomes[_config.PopSize / 2].Value;
                _bestSeries.Points.AddXY(i, best);
                _medianSeries.Points.AddXY(i, median);

                Console.WriteLine("Generation {0}: Best: {1}, Median {2}", i, best, median);
            }
            Console.WriteLine("Time elapsed = " + s.Elapsed.TotalSeconds);
            progressPlot.Series.Add(_bestSeries);
            progressPlot.Series.Add(_medianSeries);

            Genome winner = current.OrderBy((x) => x.Value).ToList().First().Key;

            resultPlot.Series.Add(_task.Function);
            resultPlot.Series.Add(_task.Plot(winner));

            Console.WriteLine(winner.Outputs[0].ToString());
            Console.WriteLine("Fitness = " + _task.Fitness(winner));

        }
    }
}
