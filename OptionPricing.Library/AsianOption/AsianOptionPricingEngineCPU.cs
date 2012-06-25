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

// GPU.NET Example Project : Option Pricing Library (C#)
// More examples available at http://github.com/tidepowerd

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace TidePowerd.Example.OptionPricing.Library.AsianOption
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("SimulationCount = {m_simulationCount}, TimestepLength = {TimestepLength}")]
    public sealed class AsianOptionPricingEngineCPU : IAsianOptionPricingEngine
    {
        #region Fields

        //
        private readonly int m_simulationCount;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulationCount"></param>
        public AsianOptionPricingEngineCPU(int simulationCount)
        {
            // Preconditions
            Contract.Requires(simulationCount > 0);

            // Postconditions
            Contract.Requires(m_simulationCount > 0);

            // Set field values.
            m_simulationCount = simulationCount;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        int IAsianOptionPricingEngine.SimulationCount
        {
            get { return m_simulationCount; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="r"></param>
        /// <param name="sigma"></param>
        /// <param name="dt"></param>
        private static void GeneratePaths(float[,] paths, float r, float sigma, float dt)
        {
            // Compute parameters
            float drift = (r - (0.5f * sigma * sigma)) * dt;
            float diffusion = sigma * (float)Math.Sqrt(dt);

            // Simulate the paths
            // NOTE : paths.GetLength(0) is the number of simulations we want to perform.

            Parallel.For(0, paths.GetLength(0), simulationIndex =>
            {
                // Handle the path simulation for each simulation; don't nest Parallel.For(...) calls here,
                // because it'd only help in some special cases where we had a large number of timesteps and very small number of simulations.

                // Rudimentary -- but fast -- pseudo-RNG.
                // NOTE : This tends to produce a fairly poor distribution of numbers, so using it speeds up path generation
                // but slows down the convergence speed of the result.
                var SimpleRng = new Random();

                // Cryptographically-strong -- but slow -- pseudo-RNG.
                //var CryptoRng = new System.Security.Cryptography.RNGCryptoServiceProvider();

                // Compute the random price-path for the current simulation.
                int NumTimesteps = paths.GetLength(1);
                float s = 1.0f;
                for (int TimestepIndex = 0; TimestepIndex < NumTimesteps; TimestepIndex++)
                {
                    s *= (float)Math.Exp(drift + (diffusion * (float)SimpleRng.NextDoubleNormal()));
                    //s *= (float)Math.Exp(drift + (diffusion * (float)CryptoRng.NextDoubleNormal()));
                    paths[simulationIndex, TimestepIndex] = s;
                }
            });
        }

        /// <summary>
        /// This computes the average _final_ value of the option based on the simulations.
        /// </summary>
        /// <param name="paths"></param>
        /// <param name="optionSpotPrice"></param>
        /// <param name="optionStrikePrice"></param>
        /// <param name="optionType"></param>
        /// <returns></returns>
        private static float ComputeValue(float[,] paths, float optionSpotPrice, float optionStrikePrice, OptionType optionType)
        {
            // The number of simulations.
            int NumSimulations = paths.GetLength(0);

            // Holds the payoff for each simulation.
            var Payoffs = new float[NumSimulations];

            //
            Parallel.For(0, NumSimulations, simulationIndex =>
            {
                // Compute the arithmatic average price of the option along the simulated path.
                // This is done by averaging the random change at each step in the path, then multiplying this by the option's spot price (what the option is valued at before traversing the random path).
                int NumTimesteps = paths.GetLength(1);
                float PathSum = 0f;
                for (int TimestepIndex = 0; TimestepIndex < NumTimesteps; TimestepIndex++)
                {
                    PathSum += paths[simulationIndex, TimestepIndex];
                }
                float avg = PathSum * optionSpotPrice / NumTimesteps;

                // Compute the payoff and store it into the payoff array.
                Payoffs[simulationIndex] = Math.Max(0f, optionType == OptionType.Call ? avg - optionStrikePrice : optionStrikePrice - avg);
            });

            // Compute and return the average payoff.
            return Payoffs.Average();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        float IAsianOptionPricingEngine.CalculatePrice(AsianOptionSingle option)
        {
            // The number of timesteps to simulate.
            int TimestepCount = (int)(option.Tenor / option.dt);

            // Holds the path values.
            float[,] Paths = new float[this.m_simulationCount, TimestepCount];

            // Generate paths
            var r = option.RiskFreeRate;
            var sigma = option.Volatility;
            var dt = option.dt;
            var numSims = this.m_simulationCount;
            GeneratePaths(Paths, r, sigma, dt);

            // Compute the average price of the option at the end of the simulated time interval.
            var spot = option.SpotPrice;
            var strike = option.StrikePrice;
            var type = option.Type;
            var MeanPrice = ComputeValue(Paths, spot, strike, type);

            // Discount the average price to the present time, then return the discounted value.
            var DiscountFactor = (float)Math.Exp(-1.0d * option.RiskFreeRate * option.Tenor);
            var DiscountedPrice = MeanPrice * DiscountFactor;
            return DiscountedPrice;
        }

        #endregion
    }
}
