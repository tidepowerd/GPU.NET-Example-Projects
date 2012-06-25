/*  The MIT License

Copyright (c) 2011-2012 TidePowerd LLC (http://www.tidepowerd.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

*/

// GPU.NET Example Project : Black-Scholes WinForms (C#)
// More examples available at http://github.com/tidepowerd

namespace TidePowerd.Example.BlackScholes.WinForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCalculate = new System.Windows.Forms.Button();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownCpuIterationCount = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownOptionCount = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownGpuIterationCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.backgroundWorkerGpu = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerCpu = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCpuIterationCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOptionCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGpuIterationCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCalculate
            // 
            this.buttonCalculate.Location = new System.Drawing.Point(12, 12);
            this.buttonCalculate.Name = "buttonCalculate";
            this.buttonCalculate.Size = new System.Drawing.Size(150, 23);
            this.buttonCalculate.TabIndex = 0;
            this.buttonCalculate.Text = "Calculate";
            this.buttonCalculate.UseVisualStyleBackColor = true;
            this.buttonCalculate.Click += new System.EventHandler(this.buttonCalculate_Click);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.BackColor = System.Drawing.Color.White;
            this.textBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOutput.Location = new System.Drawing.Point(3, 16);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ReadOnly = true;
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxOutput.Size = new System.Drawing.Size(278, 179);
            this.textBoxOutput.TabIndex = 4;
            this.textBoxOutput.WordWrap = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.numericUpDownCpuIterationCount);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numericUpDownOptionCount);
            this.groupBox1.Controls.Add(this.numericUpDownGpuIterationCount);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 197);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // numericUpDownCpuIterationCount
            // 
            this.numericUpDownCpuIterationCount.Location = new System.Drawing.Point(6, 110);
            this.numericUpDownCpuIterationCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCpuIterationCount.Name = "numericUpDownCpuIterationCount";
            this.numericUpDownCpuIterationCount.Size = new System.Drawing.Size(138, 20);
            this.numericUpDownCpuIterationCount.TabIndex = 3;
            this.numericUpDownCpuIterationCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "# of CPU Iterations";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "# of GPU Iterations";
            // 
            // numericUpDownOptionCount
            // 
            this.numericUpDownOptionCount.Location = new System.Drawing.Point(6, 32);
            this.numericUpDownOptionCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDownOptionCount.Minimum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownOptionCount.Name = "numericUpDownOptionCount";
            this.numericUpDownOptionCount.Size = new System.Drawing.Size(138, 20);
            this.numericUpDownOptionCount.TabIndex = 1;
            this.numericUpDownOptionCount.ThousandsSeparator = true;
            this.numericUpDownOptionCount.Value = new decimal(new int[] {
            4000000,
            0,
            0,
            0});
            // 
            // numericUpDownGpuIterationCount
            // 
            this.numericUpDownGpuIterationCount.Location = new System.Drawing.Point(6, 71);
            this.numericUpDownGpuIterationCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownGpuIterationCount.Name = "numericUpDownGpuIterationCount";
            this.numericUpDownGpuIterationCount.Size = new System.Drawing.Size(138, 20);
            this.numericUpDownGpuIterationCount.TabIndex = 2;
            this.numericUpDownGpuIterationCount.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "# of Options";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBoxOutput);
            this.groupBox2.Location = new System.Drawing.Point(168, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(284, 198);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // backgroundWorkerGpu
            // 
            this.backgroundWorkerGpu.WorkerReportsProgress = true;
            this.backgroundWorkerGpu.WorkerSupportsCancellation = true;
            this.backgroundWorkerGpu.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerGpu_DoWork);
            // 
            // backgroundWorkerCpu
            // 
            this.backgroundWorkerCpu.WorkerReportsProgress = true;
            this.backgroundWorkerCpu.WorkerSupportsCancellation = true;
            this.backgroundWorkerCpu.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerCpu_DoWork);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripStatusLabelProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 300);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(464, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar.Step = 1;
            // 
            // toolStripStatusLabelProgress
            // 
            this.toolStripStatusLabelProgress.Name = "toolStripStatusLabelProgress";
            this.toolStripStatusLabelProgress.Size = new System.Drawing.Size(0, 17);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 322);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonCalculate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "TidePowerd GPU.NET Black-Scholes Calculator Example";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCpuIterationCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOptionCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownGpuIterationCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCalculate;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownOptionCount;
        private System.Windows.Forms.NumericUpDown numericUpDownGpuIterationCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownCpuIterationCount;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.BackgroundWorker backgroundWorkerGpu;
        private System.ComponentModel.BackgroundWorker backgroundWorkerCpu;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelProgress;
    }
}

