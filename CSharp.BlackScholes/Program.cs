// Copyright 2010-2011 -- TidePowerd, Ltd. All rights reserved.
// http://www.tidepowerd.com
//
// GPU.NET Black-Scholes Option Pricing Example (CSharp.BlackScholes)
// Modified: 17-Jan-2011
//
// More examples available at: http://github.com/tidepowerd/GPU.NET-Example-Projects
//

using System;
using System.Diagnostics;

using TidePowerd.DeviceMethods;

namespace TidePowerd.Example.CSharp.BlackScholes
{
    class Program
    {
        #region Constants

        // Set the number of options and iterations
        private const int NumOptions = 4000000;
        private const int NumGPUIterations = 40;
        private const int NumCPUIterations = 1;

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
            float[] StockPricesCPU = new float[NumOptions];
            Array.Copy(StockPrices, StockPricesCPU, NumOptions);
            float[] OptionStrikePricesCPU = new float[NumOptions];
            Array.Copy(OptionStrikePrices, OptionStrikePricesCPU, NumOptions);
            float[] OptionYearsCPU = new float[NumOptions];
            Array.Copy(OptionYears, OptionYearsCPU, NumOptions);

            // Create copies of the option data arrays for use with the GPU calculations
            float[] CallResultsGPU = new float[NumOptions];
            float[] PutResultsGPU = new float[NumOptions];
            float[] StockPricesGPU = new float[NumOptions];
            Array.Copy(StockPrices, StockPricesGPU, NumOptions);
            float[] OptionStrikePricesGPU = new float[NumOptions];
            Array.Copy(OptionStrikePrices, OptionStrikePricesGPU, NumOptions);
            float[] OptionYearsGPU = new float[NumOptions];
            Array.Copy(OptionYears, OptionYearsGPU, NumOptions);

            // Begin thread and processor affinity so we don't jump to a different core and skew our timing results
            System.Threading.Thread.BeginThreadAffinity();
            // TODO: Set processor affinity mask

            // Print message to console
            Console.Write("Performing GPU-based calculations...");

            // Create and start stopwatch for performance calculations
            Stopwatch Watch = Stopwatch.StartNew();

            // Execute the kernel "NumIterations" times
            // (Beta 3):    Note that the call to the kernel method is now "wrapped" by another method which performs the iteration;
            //              As of Beta 3, kernel methods must be decorated with the "private" access-modifier -- meaning they can only be
            //              called by host-based methods in the same class (usually decorated with either the "public" or "internal" access-modifier).
            //              In this case, we've moved the loop into a new host-based method in the BlackScholes class.
            BlackScholes.BlackScholesGPUIterative(CallResultsGPU, PutResultsGPU, StockPricesGPU, OptionStrikePricesGPU, OptionYearsGPU, RiskFree, Volatility, NumGPUIterations);

            // Stop the stopwatch and print GPU timing results
            Watch.Stop();
            double ElapsedMillisecondsPerGPUIteration = Watch.Elapsed.TotalMilliseconds / (double)NumGPUIterations;
            Console.WriteLine("Completed {0} iterations in {1:0.0000} ms.", NumGPUIterations, Watch.Elapsed.TotalMilliseconds);
            Console.WriteLine("Average time per iteration: {0:0.0000} ms.", ElapsedMillisecondsPerGPUIteration);
            Console.WriteLine();

            // Print message to console
            Console.WriteLine("Performing CPU-based calculations...");

            // Restart the stopwatch
            Watch.Reset();
            Watch.Start();

            // Perform CPU-based calculations
            for (int Iteration = 0; Iteration < NumCPUIterations; Iteration++)
            {
                BlackScholes.BlackScholesCPU(CallResultsCPU, PutResultsCPU, StockPricesCPU, OptionStrikePricesCPU, OptionYearsCPU, RiskFree, Volatility);
            }

            // Stop the stopwatch and end thread affinity
            Watch.Stop();
            double ElapsedMillisecondsPerCPUIteration = Watch.Elapsed.TotalMilliseconds / (double)NumCPUIterations;
            System.Threading.Thread.EndThreadAffinity();

            // Print CPU timing results
            Console.WriteLine("Completed {0} iterations in {1:0.0000} ms.", NumCPUIterations, Watch.Elapsed.TotalMilliseconds);
            Console.WriteLine("Average time per iteration: {0:0.0000} ms.", ElapsedMillisecondsPerCPUIteration);
            Console.WriteLine();

            // Print performance comparison data
            Console.WriteLine("Option Count: {0}", 2 * NumOptions);
            double GibibytesTransferred  = (5.0d * (double)NumOptions * (double)sizeof(float) * 2.0d) / Math.Pow(2.0d, 30.0d);  // GiB transferred (per round-trip)
            Console.WriteLine("Effective Memory Bandwidth (avg): {0} GiB/s", GibibytesTransferred / (ElapsedMillisecondsPerGPUIteration / 1000.0d));  // GiB transferred (round trip) per iteration / seconds per iteration
            Console.WriteLine("GPU Speedup vs. CPU: ~{0}x", ElapsedMillisecondsPerCPUIteration / ElapsedMillisecondsPerGPUIteration);
            Console.WriteLine();

            // Print message to console
            Console.Write("Verifying calculations...");

            // Verify that GPU & CPU calculations match (their difference should be within a certain threshold)
            double SumDelta = 0d, SumRef = 0d, MaxDelta = 0d;
            for (int OptionIndex = 0; OptionIndex < CallResultsCPU.Length; OptionIndex++)
            {
                double Delta = Math.Abs(CallResultsCPU[OptionIndex] - CallResultsGPU[OptionIndex]);
                
                if (Delta > MaxDelta) { MaxDelta = Delta; }

                SumDelta += Delta;

                SumRef = Math.Abs(CallResultsCPU[OptionIndex]);
            }
            double L1Norm = SumDelta / SumRef;

            // Print verification message to console
            Console.WriteLine("L1 Norm: {0}", L1Norm);
            Console.WriteLine("Max absolute error: {0}", MaxDelta);

            //// The maximum L1 Norm between GPU and CPU result value
            ////const float MaxL1NormError = 0.000001f;       // 1E-6 or 0.0001%
            //const float MaxL1NormError = 0.001f;       // 1E-3 or 0.1%
            
            //// Print a message denoting if verification has passed or not
            //Console.WriteLine((L1Norm < MaxL1NormError) ? "Verification successful!" : "Verification ERROR!");

            // Wait to exit
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            ConsoleKeyInfo cki = Console.ReadKey();
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
