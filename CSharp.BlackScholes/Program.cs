/*  The MIT License

Copyright (c) 2011 TidePowerd Limited (http://www.tidepowerd.com)

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

// GPU.NET Example Project : Black-Scholes (C# Console)
// More examples available at http://github.com/tidepowerd

using System;
using System.Diagnostics;

using TidePowerd.DeviceMethods;

namespace TidePowerd.Example.CSharp.BlackScholes
{
    class Program
    {
        #region Constants

        // Set the number of options and iterations
        private const int NumOptions = 32 * 1000 * 1000;
        private const int NumGPUIterations = 20;
        private const int NumCPUIterations = 3;

        // Set some simulation parameters
        private const float RiskFree = 0.02f;
        private const float Volatility = 0.30f;

        #endregion

        static void Main(string[] args)
        {
            // Allocate arrays to hold option data and pricing results
            float[] StockPrices = new float[NumOptions];
            float[] OptionStrikePrices = new float[NumOptions];
            float[] OptionYears = new float[NumOptions];

            // Generate random option data
            Random rand = new Random();
            for (int i = 0; i < NumOptions; i++)
            {
                StockPrices[i] = RandFloat(rand, 5.0f, 30.0f);
                OptionStrikePrices[i] = RandFloat(rand, 1.0f, 100.0f);
                OptionYears[i] = RandFloat(rand, 0.25f, 10.0f);
            }

            // Create copies of the option data arrays for use with the CPU calculations
            float[] CallResultsCPU = new float[NumOptions];
            float[] PutResultsCPU = new float[NumOptions];

            // Create copies of the option data arrays for use with the GPU calculations
            float[] CallResultsGPU = new float[NumOptions];
            float[] PutResultsGPU = new float[NumOptions];

            // Begin thread and processor affinity so we don't jump to a different core and skew our timing results
            System.Threading.Thread.BeginThreadAffinity();
            // TODO: Set processor affinity mask

            // Create a stopwatch to measure the calculations via the system high-precision event timer (HPET).
            Stopwatch Watch = new Stopwatch();

#if BENCHMARKING
            // Run a single iteration of the GPU-based method to force initialization of the GPU.NET Runtime and Plugins.
            // This normally occurs upon the first kernel method invocation after an accelerated assembly (i.e., the current assembly)
            // is loaded, but there is some overhead incurred because the .NET CLR needs to load and JIT the assemblies before
            // they can be used. As this occurs only once, calling a single iteration of a kernel method will remove this overhead from
            // within the code we're actually interested in timing.
            Console.WriteLine("Forcing initialization of the GPU.NET Runtime and Plugins...");
            
            Watch.Start();
            BlackScholes.BlackScholesGPUSingleIteration(CallResultsGPU, PutResultsGPU, StockPrices, OptionStrikePrices, OptionYears, RiskFree, Volatility);
            Watch.Stop();

            Console.WriteLine("Initialization completed in {0:0.0000} ms.", Watch.Elapsed.TotalMilliseconds);
#endif
            // Print message to console
            Console.WriteLine("Performing GPU-based calculations...");

            // Reset (if necessary) and start stopwatch to measure GPU calculation speed.
            Watch.Reset();
            Watch.Start();

            // Execute the kernel "NumIterations" times
            BlackScholes.BlackScholesGPUIterative(CallResultsGPU, PutResultsGPU, StockPrices, OptionStrikePrices, OptionYears, RiskFree, Volatility, NumGPUIterations);

            // Stop the stopwatch and print GPU timing results
            Watch.Stop();
            double ElapsedMillisecondsPerGPUIteration = Watch.Elapsed.TotalMilliseconds / (double)NumGPUIterations;
            Console.WriteLine("Completed {0} iterations in {1:0.0000} ms.", NumGPUIterations, Watch.Elapsed.TotalMilliseconds);
            Console.WriteLine("Average time per iteration: {0:0.0000} ms.", ElapsedMillisecondsPerGPUIteration);
            Console.WriteLine();

            // Print message to console
            Console.WriteLine("Performing CPU-based calculations on {0} core(s)...", System.Environment.ProcessorCount);

            // Restart the stopwatch
            Watch.Reset();
            Watch.Start();

            // Perform CPU-based calculations
            for (int Iteration = 0; Iteration < NumCPUIterations; Iteration++)
            {
                BlackScholes.BlackScholesCPU(CallResultsCPU, PutResultsCPU, StockPrices, OptionStrikePrices, OptionYears, RiskFree, Volatility);
            }

            // Stop the stopwatch and print CPU timing results
            Watch.Stop();
            double ElapsedMillisecondsPerCPUIteration = Watch.Elapsed.TotalMilliseconds / (double)NumCPUIterations;
            Console.WriteLine("Completed {0} iterations in {1:0.0000} ms.", NumCPUIterations, Watch.Elapsed.TotalMilliseconds);
            Console.WriteLine("Average time per iteration: {0:0.0000} ms.", ElapsedMillisecondsPerCPUIteration);
            Console.WriteLine();

            // End thread affinity now that we've finished using Stopwatch          
            System.Threading.Thread.EndThreadAffinity();

            // Print performance comparison data
            Console.WriteLine("Option Count: {0}", NumOptions);
            double GibibytesTransferred = (5.0d * (double)NumOptions * (double)sizeof(float)) / Math.Pow(2.0d, 30.0d);  // GiB transferred (per round-trip)
            Console.WriteLine("Effective Host<->Device Memory Bandwidth (avg): {0} GiB/s", GibibytesTransferred / (ElapsedMillisecondsPerGPUIteration / 1000.0d));  // GiB transferred (round trip) per iteration / seconds per iteration
            Console.WriteLine("GPU Speedup vs. CPU: ~{0}x", ElapsedMillisecondsPerCPUIteration / ElapsedMillisecondsPerGPUIteration);
            Console.WriteLine();

            // Print message to console
            Console.WriteLine("Verifying calculations...");

            // Verify that GPU & CPU calculations match (their difference should be within a certain threshold)
            double OneNorm = 0d, TwoNorm = 0d, MaxNorm = 0d;

            // Call option verification
            Console.WriteLine("Call Option Price Data:");
            NormsOfDifferenceVector(CallResultsCPU, CallResultsGPU, out OneNorm, out TwoNorm, out MaxNorm);

            Console.WriteLine("L1-Norm: {0}", OneNorm);
            Console.WriteLine("L2-Norm: {0}", TwoNorm);
            Console.WriteLine("Max-Norm: {0}", MaxNorm);

            // Put option verification
            Console.WriteLine("Put Option Price Data:");
            NormsOfDifferenceVector(PutResultsCPU, PutResultsGPU, out OneNorm, out TwoNorm, out MaxNorm);

            Console.WriteLine("L1 Norm: {0}", OneNorm);
            Console.WriteLine("L2-Norm: {0}", TwoNorm);
            Console.WriteLine("Max-Norm: {0}", MaxNorm);

            //// If the max-norm is less than a reasonable threshold, verification has passed
            //double ErrorRatio = (OneNorm - (NumOptions * (double)Single.Epsilon)) / (NumOptions * (double)Single.Epsilon);
            //Console.WriteLine("Error Ratio: {0}", ErrorRatio);
            
            //// Print a message denoting if verification has passed or not
            //Console.WriteLine((L1Norm < MaxL1NormError) ? "Verification successful!" : "Verification ERROR!");

            // Wait to exit
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            ConsoleKeyInfo cki = Console.ReadKey();
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
    }
}
