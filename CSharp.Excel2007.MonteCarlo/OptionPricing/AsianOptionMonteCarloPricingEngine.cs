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

// GPU.NET Example Project : Monte Carlo (C# Excel2007)
// More examples available at http://github.com/tidepowerd

using System;
using TidePowerd.DeviceMethods;
using TidePowerd.DeviceMethods.Random;

namespace TidePowerd.Example.CSharp.Excel2007.MonteCarlo.OptionPricing
{
    /// <summary>
    /// 
    /// </summary>
    internal static class AsianOptionPricingEngine
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private const int c_kernelBlockSize = 128;

        #pragma warning disable 0649        // Hides the warning about 'm_reductionScratchPad' never being assigned to

        /// <summary>
        /// 
        /// </summary>
        [SharedMemory(0x800)]   // 0x100
        private static readonly float[] m_reductionScratchPad;

        #pragma warning restore 0649
        #endregion

        #region Methods

        //
        [Kernel]
        private static void AsianOptionPricingKernel(float[] result, float[] spotPrice, float[] strikePrice, float[] riskFreeRate, float[] sigma, float[] tenor, /* OptionType[] optionType, */ int scenarios, int timeSteps)
        {
            // Calculate option index, scenario index, and scenario stride.
            int OptionIndex = BlockIndex.X;
            int ScenarioIndex = (BlockIndex.Y * BlockDimension.X) + ThreadIndex.X;
            int ScenarioStride = GridDimension.Y * BlockDimension.X;

            // Read the option parameters
            float SpotPrice = spotPrice[OptionIndex];
            float StrikePrice = strikePrice[OptionIndex];
            float RiskFreeRate = riskFreeRate[OptionIndex];
            float Sigma = sigma[OptionIndex];
            float Tenor = tenor[OptionIndex];
            OptionType OptionType = OptionType.Call;    // TEMP : Should be read from the 'optionType' parameter

            // Calculate drift/diffusion per timestep
            float dt = Tenor / (float)(timeSteps - 1);
            float Drift = (RiskFreeRate - (0.5f * Sigma * Sigma)) * dt;
            float Diffusion = Sigma * DeviceMath.Sqrt(dt);

            float valueA = 0f;
            float valueQ = 0f;

            //
            for (int Count = 1; ScenarioIndex < scenarios; ScenarioIndex += ScenarioStride, Count++)
            {
                float s = 1.0f;
                float avgArithmetic = 1.0f;

                //
                for (int TimeStep = 1; TimeStep < timeSteps; TimeStep++)
                {
                    s *= DeviceMath.Exp(Drift + (Diffusion * NormalRNG.NextFloat()));
                    avgArithmetic += s;
                }

                // Scale to the original spot price
                s *= SpotPrice;
                avgArithmetic = SpotPrice * avgArithmetic / (float)timeSteps;

                // Payoff
                float payoffArithmetic = DeviceMath.Max(0f, OptionType == OptionType.Call ? avgArithmetic - StrikePrice : StrikePrice - avgArithmetic);

                //
                valueQ += (Count - 1) * (payoffArithmetic - valueA) * (payoffArithmetic - valueA) / Count;
                valueA = valueA + (payoffArithmetic - valueA) / Count;
            }

            // Reduce (average) across threads within the block
            int tid = ThreadIndex.X;
            m_reductionScratchPad[tid] = valueA;
            Kernel.SyncThreads();

            // Do reduction in shared mem
            for (int s = BlockDimension.X / 2; s > 0; s >>= 1)
            {
                if (tid < s)
                {
                    m_reductionScratchPad[tid] += m_reductionScratchPad[tid + s];
                }

                Kernel.SyncThreads();
            }

            valueA = m_reductionScratchPad[0] / (float)BlockDimension.X;
            
            // Reduce (sum) across blocks for the same option (i.e. across block.x)
            // Discount to current time
            if (ThreadIndex.X == 0)
            {
                Atomic.Add(ref result[OptionIndex], valueA * DeviceMath.Exp(-1f * RiskFreeRate * Tenor));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        public static float[] CalculatePrices(this AsianOptionPricingPlan plan)
        {
            // Preconditions
            if (plan == null) { throw new ArgumentNullException("plan"); }

            // The number of options to simulate/price.
            int OptionCount = plan.SpotPrices.Length;

            // Create an array to hold the results (prices) from the kernel.
            float[] Prices = new float[OptionCount];

            /* WORKAROUND : Assign the arrays from the pricing plan to local variables and pass them to the kernel that way (instead of passing them directly from the fields of the plan instance). */
            float[] SpotPrices = plan.SpotPrices;
            float[] StrikePrices = plan.StrikePrices;
            float[] RiskFreeRates = plan.RiskFreeRates;
            float[] Sigma = plan.Sigma;
            float[] Tenor = plan.Tenor;
            int ScenarioCount = plan.ScenarioCount;
            int TimeStepCount = plan.TimeStepCount;

            // Set execution grid/block sizes.
            Launcher.SetBlockSize(c_kernelBlockSize);

            int GridHeight = (ScenarioCount + c_kernelBlockSize - 1) / c_kernelBlockSize;
            Launcher.SetGridSize(OptionCount, GridHeight);

            // Call the kernel method using the data from the plan.
            AsianOptionPricingKernel(Prices, SpotPrices, StrikePrices, RiskFreeRates, Sigma, Tenor, ScenarioCount, TimeStepCount);

            // Complete the final part of the reduction (convert sum to average)
            for (int OptionIndex = 0; OptionIndex < Prices.Length; OptionIndex++)
            {
                Prices[OptionIndex] /= (float)GridHeight;
            }

            // Return the computed prices
            return Prices;
        }

        #endregion
    }
}
