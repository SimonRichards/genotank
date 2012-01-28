namespace genotank {
    partial class Report {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.progressPlot = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.resultPlot = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.progressPlot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultPlot)).BeginInit();
            this.SuspendLayout();
            // 
            // progressPlot
            // 
            chartArea1.Name = "ChartArea1";
            this.progressPlot.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.progressPlot.Legends.Add(legend1);
            this.progressPlot.Location = new System.Drawing.Point(12, 12);
            this.progressPlot.Name = "progressPlot";
            this.progressPlot.Size = new System.Drawing.Size(825, 266);
            this.progressPlot.TabIndex = 0;
            this.progressPlot.Text = "Best Fitness";
            // 
            // resultPlot
            // 
            chartArea2.Name = "ChartArea1";
            this.resultPlot.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.resultPlot.Legends.Add(legend2);
            this.resultPlot.Location = new System.Drawing.Point(12, 286);
            this.resultPlot.Name = "resultPlot";
            this.resultPlot.Size = new System.Drawing.Size(825, 266);
            this.resultPlot.TabIndex = 0;
            this.resultPlot.Text = "Average Fitness";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 559);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(825, 23);
            this.progressBar.TabIndex = 1;
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 591);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.resultPlot);
            this.Controls.Add(this.progressPlot);
            this.Name = "Report";
            this.Text = "Report";
            ((System.ComponentModel.ISupportInitialize)(this.progressPlot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultPlot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart progressPlot;
        private System.Windows.Forms.DataVisualization.Charting.Chart resultPlot;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}