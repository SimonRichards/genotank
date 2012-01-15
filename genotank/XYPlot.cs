using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace genotank {
    public partial class XYPlot : Form {
        public XYPlot(Series[] data) {
            InitializeComponent();
            foreach (Series s in data) {
                s.ChartType = SeriesChartType.Spline;
                chart1.Series.Add(s);
            }
        }
    }
}
