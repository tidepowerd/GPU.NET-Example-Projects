/*  The MIT License

Copyright (c) 2011-2012 TidePowerd Limited (http://www.tidepowerd.com)

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

// GPU.NET Example Project : Monte Carlo (C# Library)
// More examples available at http://github.com/tidepowerd

using System;
using TidePowerd.DeviceMethods;
using TidePowerd.DeviceMethods.Random;

namespace TidePowerd.Example.CSharp.MonteCarlo.Library.OptionPricing
{
    /// <summary>
    /// 
    /// </summary>
    public static class AsianOptionPricingEngineKernels
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private const int c_kernelBlockSize = 0x80;

        #pragma warning disable 0649        // Hides the warning about 'm_reductionScratchPad' never being assigned to

        /// <summary>
        /// 
        /// </summary>
        [SharedMemory(c_kernelBlockSize)]
        private static readonly float[] m_reductionScratchPad;

        #pragma warning restore 0649
        #endregion

        #region Methods

        //
        [Kernel]
        private static void GeneratePaths(float[] paths, float r, float sigma, float dt, int numSims, int numTimesteps)
        {
            //
            int tid = (BlockIndex.X * BlockDimension.X) + ThreadIndex.X;
            int step = GridDimension.X * BlockDimension.X;

            // Compute parameters
            float drift = (r - (0.5f * sigma * sigma)) * dt;
            float diffusion = sigma * DeviceMath.Sqrt(dt);

            // Simulate the paths
            for (int i = tid; i < numSims; i += step)
            {
                // Current output index
                int output = i;

                // Simulate the path
                float s = 1.0f;
                for (int t = 0; t < numTimesteps; t++, output += numSims)
                {
                    s *= DeviceMath.Exp(drift + (diffusion * NormalRNG.NextFloat()));
                    paths[output] = s;
                }
            }
        }

        //
        [Kernel]
        private static void ComputeValue(float[] values, float[] paths, float optionSpotPrice, float optionStrikePrice, OptionType optionType, int numSims, int numTimesteps)
        {
            // Determine thread id
            int bid = BlockIndex.X;
            int tid = (BlockIndex.X * BlockDimension.X) + ThreadIndex.X;
            int step = GridDimension.X * BlockDimension.X;

            //
            float sumPayoffs = 0f;
            for (int i = tid; i < numSims; i += step)
            {
                //
                int pathIndex = i;

                // Compute the arithmatic average.
                float avg = 0f;
                for (int t = 0; t < numTimesteps; t++, pathIndex += numSims)
                {
                    avg += paths[pathIndex];
                }
                avg = avg * optionSpotPrice / numTimesteps;

                // Compute the payoff
                float payoff = DeviceMath.Max(0f, optionType == OptionType.Call ? avg - optionStrikePrice : optionStrikePrice - avg);

                // Accumulate payoff locally
                sumPayoffs += payoff;
            }

            // Reduce within the block
            // Perform first level of reduction:
            // - Write to shared memory
            int ltid = ThreadIndex.X;
            m_reductionScratchPad[ltid] = sumPayoffs;
            Kernel.SyncThreads();

            // Do reduction in shared mem
            for (int s = BlockDimension.X / 2; s > 0; s >>= 1)
            {
                if (ltid < s)
                {
                    m_reductionScratchPad[ltid] += m_reductionScratchPad[ltid + s];
                }

                Kernel.SyncThreads();
            }
            sumPayoffs = m_reductionScratchPad[0];

            // Store the result
            if (ThreadIndex.X == 0)
            {
                values[bid] = sumPayoffs;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulationCount"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        internal static float CalculatePrice(int simulationCount, AsianOptionSingle option)
        {
            // Preconditions
            if (simulationCount < 1) { throw new ArgumentOutOfRangeException("simulationCount"); }
            else if (option == null) { throw new ArgumentNullException("option"); }

            // The number of timesteps to simulate.
            int TimestepCount = (int)(option.Tenor / option.dt);

            // The grid size to use when executing the kernel.
            int GridSize = (simulationCount + c_kernelBlockSize - 1) / c_kernelBlockSize;

            // Holds the path values (only used on the device, so no data is transferred to/from the host for this array).
            float[] Paths = new float[simulationCount * TimestepCount];

            // Holds the partial prices computed on the GPU (before they're averaged into the final price).
            float[] PartialResults = new float[GridSize];

            // Set execution grid/block sizes.
            Launcher.SetBlockSize(c_kernelBlockSize);
            Launcher.SetGridSize(GridSize);

            // Generate paths
            var r = option.RiskFreeRate;
            var sigma = option.Volatility;
            var dt = option.dt;
            var numSims = simulationCount;
            GeneratePaths(Paths, r, sigma, dt, numSims, TimestepCount);

            // Compute option value (partial sums, final sum/average value computed on the CPU)
            var spot = option.SpotPrice;
            var strike = option.StrikePrice;
            var type = option.Type;
            ComputeValue(PartialResults, Paths, spot, strike, type, numSims, TimestepCount);

            // Complete the final part of the reduction (convert sum to average)
            float SumPrices = 0f;
            for (int BlockId = 0; BlockId < GridSize; BlockId++)
            {
                SumPrices += PartialResults[BlockId];
            }

            // Compute the average price of the option over the simulated time interval.
            float MeanPrice = SumPrices / simulationCount;

            // Discount the average price to the present time, then return the discounted value.
            return MeanPrice * (float)Math.Exp(-1.0d * option.RiskFreeRate * option.Tenor);
        }

        #endregion
    }
}
