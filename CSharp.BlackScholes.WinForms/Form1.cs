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

// GPU.NET Example Project : Black-Scholes (C# WinForms)
// More examples available at http://github.com/tidepowerd

using System;
using System.Windows.Forms;

using TidePowerd.DeviceMethods;

namespace TidePowerd.Example.CSharp.BlackScholes.WinForms
{
    public partial class Form1 : Form
    {
        #region Nested Types

        /// <summary>
        /// Holds the input data which needs to be passed to the BackgroundWorker components.
        /// </summary>
        private struct BlackScholesWorkerInputData
        {
            public float[] StockPrices;

            public float[] OptionStrikePrices;

            public float[] OptionTimeToExpiry;

            public int Iterations;

            public float RiskFreeRate;

            public float Volatility;
        }

        /// <summary>
        /// Holds the output data calculated by a BackgroundWorker component.
        /// </summary>
        private struct BlackScholesWorkerOutputData
        {
            public float[] CallPrices;

            public float[] PutPrices;

            public TimeSpan CalculationTime;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Marks whether or not the event-handlers have been added to the CPU BackgroundWorker instances; otherwise, when the calculate button is clicked, it'll attempt to launch the worker twice and break.
        /// </summary>
        private volatile bool m_addedCpuWorkerCompletedHandler = false;

        /// <summary>
        /// Marks whether or not the event-handlers have been added to the GPU BackgroundWorker instances; otherwise, when the calculate button is clicked, it'll attempt to launch the worker twice and break.
        /// </summary>
        private volatile bool m_addedGpuWorkerCompletedHandler = false;

        #endregion

        #region Constructors

        public Form1()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerCpu_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Preconditions
            if (e == null) { throw new ArgumentNullException("e"); }

            // Postconditions
            //

            // Get the argument value, which should be a BlackScholesWorkerInputData value
            BlackScholesWorkerInputData InputData = (BlackScholesWorkerInputData)e.Argument;

            // Create arrays to hold the calculated result values
            float[] CallPrices = new float[InputData.StockPrices.Length];
            float[] PutPrices = new float[InputData.StockPrices.Length];

            // Start thread affinity (to ensure accuracy of timing results)
            System.Threading.Thread.BeginThreadAffinity();

            // Create a stopwatch to measure execution time
            System.Diagnostics.Stopwatch ExecutionStopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Call the CPU-based method the specified number of times
            for (int i = 0; i < InputData.Iterations; i++)
            {
                BlackScholes.BlackScholesCPU(CallPrices, PutPrices, InputData.StockPrices, InputData.OptionStrikePrices, InputData.OptionTimeToExpiry, InputData.RiskFreeRate, InputData.Volatility);
            }

            // Stop the execution timer
            ExecutionStopwatch.Stop();

            // Set the worker's Result value to a new instance of BlackScholesWorkerOutputData, which simply combines the results into a single value
            e.Result = new BlackScholesWorkerOutputData()
            {
                CalculationTime = ExecutionStopwatch.Elapsed,
                CallPrices = CallPrices,
                PutPrices = PutPrices
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorkerGpu_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Preconditions
            if (e == null) { throw new ArgumentNullException("e"); }

            // Postconditions
            //

            // Get the argument value, which should be a BlackScholesWorkerInputData value
            BlackScholesWorkerInputData InputData = (BlackScholesWorkerInputData)e.Argument;

            // Create arrays to hold the calculated result values
            float[] CallPrices = new float[InputData.StockPrices.Length];
            float[] PutPrices = new float[InputData.StockPrices.Length];

            // Start thread affinity (to ensure accuracy of timing results)
            System.Threading.Thread.BeginThreadAffinity();

            // Create a stopwatch to measure execution time
            System.Diagnostics.Stopwatch ExecutionStopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Call the GPU-based method the specified number of times
            BlackScholes.BlackScholesGPUIterative(CallPrices, PutPrices, InputData.StockPrices, InputData.OptionStrikePrices, InputData.OptionTimeToExpiry, InputData.RiskFreeRate, InputData.Volatility, InputData.Iterations);

            // Stop the execution timer
            ExecutionStopwatch.Stop();

            // Set the worker's Result value to a new instance of BlackScholesWorkerOutputData, which simply combines the results into a single value
            e.Result = new BlackScholesWorkerOutputData()
            {
                CalculationTime = ExecutionStopwatch.Elapsed,
                CallPrices = CallPrices,
                PutPrices = PutPrices
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCalculate_Click(object sender, EventArgs e)
        {
            // Disable the calculate button so we don't launch multiple times
            buttonCalculate.Enabled = false;
            
            // Disable the numeric controls so the values can't be changed; this is important due to the use of closures below
            numericUpDownOptionCount.Enabled = false;
            numericUpDownGpuIterationCount.Enabled = false;
            numericUpDownCpuIterationCount.Enabled = false;

            // Clear the output textbox
            textBoxOutput.Clear();

            //// Get the number of options, GPU iterations, and CPU iterations
            //int NumOptions = (int)numericUpDownOptionCount.Value;
            //int GpuIterations = (int)numericUpDownGpuIterationCount.Value;
            //int CpuIterations = (int)numericUpDownCpuIterationCount.Value;

            // Allocate arrays to hold option data (input values for calculations)
            float[] StockPrices = new float[(int)numericUpDownOptionCount.Value];
            float[] OptionStrikePrices = new float[(int)numericUpDownOptionCount.Value];
            float[] OptionYears = new float[(int)numericUpDownOptionCount.Value];

            // Generate random option data
            Random rand = new Random();
            for (int i = 0; i < (int)numericUpDownOptionCount.Value; i++)
            {
                StockPrices[i] = RandFloat(rand, 5.0f, 30.0f);
                OptionStrikePrices[i] = RandFloat(rand, 1.0f, 100.0f);
                OptionYears[i] = RandFloat(rand, 0.25f, 10.0f);
            }

            // Use an anonymous method to handle the "RunWorkerCompleted" event of the CPU worker, so that it reports it's results, then starts the GPU worker
            if (!m_addedCpuWorkerCompletedHandler)
            {
                m_addedCpuWorkerCompletedHandler = true;  // Set this flag to avoid adding the event handler multiple times

                backgroundWorkerCpu.RunWorkerCompleted += (cpuSender, cpuRunWorkerCompletedEventArgs) =>
                {
                    // Preconditions
                    if (cpuRunWorkerCompletedEventArgs == null) { throw new ArgumentNullException("cpuRunWorkerCompletedEventArgs"); }

                    // Postconditions
                    //

                    // If an error occurred while calculating the results, display the error message in a MessageBox and stop calculating
                    if (cpuRunWorkerCompletedEventArgs.Error != null)
                    {
                        MessageBox.Show(cpuRunWorkerCompletedEventArgs.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        buttonCalculate.Enabled = true;
                        return;
                    }

                    // Get the output values from the CPU-based calculations
                    BlackScholesWorkerOutputData OutputDataCpu = (BlackScholesWorkerOutputData)cpuRunWorkerCompletedEventArgs.Result;

                    // Handle the "RunWorkerCompleted" event for the GPU worker, same as the CPU worker
                    if (!m_addedGpuWorkerCompletedHandler)
                    {
                        m_addedGpuWorkerCompletedHandler = true;    // Set this flag to avoid adding the event handler multiple times

                        backgroundWorkerGpu.RunWorkerCompleted += (gpuSender, gpuRunWorkerCompletedEventArgs) =>
                        {
                            // Preconditions
                            if (gpuRunWorkerCompletedEventArgs == null) { throw new ArgumentNullException("gpuRunWorkerCompletedEventArgs"); }

                            // Postconditions
                            //

                            // If an error occurred while calculating the results, display the error message in a MessageBox and stop calculating
                            if (gpuRunWorkerCompletedEventArgs.Error != null)
                            {
                                MessageBox.Show(gpuRunWorkerCompletedEventArgs.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                buttonCalculate.Enabled = true;
                                return;
                            }

                            // Get the output values from the GPU-based calculations
                            BlackScholesWorkerOutputData OutputDataGpu = (BlackScholesWorkerOutputData)gpuRunWorkerCompletedEventArgs.Result;

                            // Print performance comparison data
                            textBoxOutput.AppendText(String.Format("Option Count: {0}", (int)numericUpDownOptionCount.Value) + Environment.NewLine);
                            double GibibytesTransferred = (5.0d * (double)numericUpDownOptionCount.Value * (double)sizeof(float) * 2.0d) / Math.Pow(2.0d, 30.0d);  // GiB transferred (per round-trip)
                            textBoxOutput.AppendText(String.Format("Effective Host<->Device Memory Bandwidth (avg): {0} GiB/s", GibibytesTransferred / (OutputDataGpu.CalculationTime.TotalMilliseconds / 1000.0d)) + Environment.NewLine);  // GiB transferred (round trip) per iteration / seconds per iteration
                            textBoxOutput.AppendText(String.Format("GPU Speedup vs. CPU: ~{0}x", (OutputDataCpu.CalculationTime.TotalMilliseconds / (int)numericUpDownCpuIterationCount.Value) / (OutputDataGpu.CalculationTime.TotalMilliseconds / (int)numericUpDownGpuIterationCount.Value)) + Environment.NewLine);
                            textBoxOutput.AppendText(Environment.NewLine);

                            // Print message to console
                            textBoxOutput.AppendText("Verifying calculations..." + Environment.NewLine);

                            // Verify that GPU & CPU calculations match (their difference should be within a certain threshold)
                            double OneNorm = 0d, TwoNorm = 0d, MaxNorm = 0d;

                            // Call option verification
                            textBoxOutput.AppendText("Call Option Price Data:" + Environment.NewLine);
                            NormsOfDifferenceVector(OutputDataCpu.CallPrices, OutputDataGpu.CallPrices, out OneNorm, out TwoNorm, out MaxNorm);

                            textBoxOutput.AppendText(String.Format("L1-Norm: {0}", OneNorm) + Environment.NewLine);
                            textBoxOutput.AppendText(String.Format("L2-Norm: {0}", TwoNorm) + Environment.NewLine);
                            textBoxOutput.AppendText(String.Format("Max-Norm: {0}", MaxNorm) + Environment.NewLine);

                            // Put option verification
                            textBoxOutput.AppendText("Put Option Price Data:" + Environment.NewLine);
                            NormsOfDifferenceVector(OutputDataCpu.PutPrices, OutputDataGpu.PutPrices, out OneNorm, out TwoNorm, out MaxNorm);

                            textBoxOutput.AppendText(String.Format("L1 Norm: {0}", OneNorm) + Environment.NewLine);
                            textBoxOutput.AppendText(String.Format("L2-Norm: {0}", TwoNorm) + Environment.NewLine);
                            textBoxOutput.AppendText(String.Format("Max-Norm: {0}", MaxNorm) + Environment.NewLine);

                            // Enable the numeric controls again
                            numericUpDownOptionCount.Enabled = true;
                            numericUpDownGpuIterationCount.Enabled = true;
                            numericUpDownCpuIterationCount.Enabled = true;

                            // Enable the calculate button so the user can run the simulation again
                            buttonCalculate.Enabled = true;
                        };
                    }

                    // Perform the GPU-based calculations, creating a new instance of BlackScholesWorkerInputData which references the input-data arrays and passing it to the worker thread
                    backgroundWorkerGpu.RunWorkerAsync(new BlackScholesWorkerInputData()
                    {
                        Iterations = (int)numericUpDownGpuIterationCount.Value,
                        OptionStrikePrices = OptionStrikePrices,
                        OptionTimeToExpiry = OptionYears,
                        StockPrices = StockPrices
                    });
                };
            }

            // Perform the CPU-based calculations, creating a new instance of BlackScholesWorkerInputData which references the input-data arrays and passing it to the worker thread
            backgroundWorkerCpu.RunWorkerAsync(new BlackScholesWorkerInputData()
            {
                Iterations = (int)numericUpDownCpuIterationCount.Value,
                OptionStrikePrices = OptionStrikePrices,
                OptionTimeToExpiry = OptionYears,
                StockPrices = StockPrices
            });            
        }

        /// <summary>
        /// Subtracts the computed-results vector from the reference-results vector, then computes various norms on the difference vector.
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="results"></param>
        /// <param name="oneNorm">The L1-norm of the difference vector.</param>
        /// <param name="twoNorm">The L2-norm of the difference vector.</param>
        /// <param name="maxNorm">The maximum-norm of the difference vector.</param>
        private static void NormsOfDifferenceVector(float[] reference, float[] results, out double oneNorm, out double twoNorm, out double maxNorm)
        {
            // Preconditions
            if (reference == null) { throw new ArgumentNullException("reference"); }
            else if (results == null) { throw new ArgumentNullException("results"); }
            else if (reference.Length != results.Length) { throw new ArgumentException("The result vector does not have the same length as the reference vector.", "results"); }

            // Postconditions
            // TODO: twoNorm >= 0
            // TODO: maxNorm >= 0

            // Holds the running sum of the difference vector's elements (for the L1-norm)
            double SumOfAbsoluteDifferences = 0d;

            // Holds the running sum of the squares of the difference vector's elements (for the L2-norm)
            double SumOfSquaresOfDifferences = 0d;

            // Holds the largest (by absolute value) single difference between a reference and computed element pair
            double LargestAbsoluteDifference = 0d;

            // Iterate over the vectors, computing the differences between the elements and updating the temporary variables
            for (int i = 0; i < reference.Length; i++)
            {
                // Compute the difference between the current reference and result elements
                double Diff = reference[i] - results[i];

                // Update temporary variables
                SumOfAbsoluteDifferences += Math.Abs(Diff);
                SumOfSquaresOfDifferences += Math.Pow(Diff, 2.0d);
                LargestAbsoluteDifference = Math.Max(LargestAbsoluteDifference, Math.Abs(Diff));
            }

            // Set the output values
            oneNorm = SumOfAbsoluteDifferences;
            twoNorm = Math.Sqrt(SumOfSquaresOfDifferences);
            maxNorm = LargestAbsoluteDifference;
        }

        /// <summary>
        /// Generates a random floating point number within a specified range.
        /// </summary>
        /// <param name="rand"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        private static float RandFloat(Random rand, float low, float high)
        {
            float t = (float)rand.NextDouble();
            return (1.0f - t) * low + t * high;
        }

        #endregion
    }
}
