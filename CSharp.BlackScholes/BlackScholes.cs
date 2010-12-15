// Copyright 2010 -- TidePowerd, Ltd. All rights reserved.
// http://www.tidepowerd.com
//
// GPU.NET Black-Scholes (Console-Based) Example (CSharp.BlackScholes)
// Modified: 01-Dec-2010
//
// More examples available at: http://github.com/tidepowerd/GPU.NET-Example-Projects
//

using System;
using TidePowerd.DeviceMethods;

namespace TidePowerd.Demo.CSharp.BlackScholes
{
    /// <summary>
    /// Contains method to calculate option prices via the Black-Scholes formula.
    /// </summary>
    internal static class BlackScholes
    {
        #region Methods

        /// <summary>
        /// Computes the values of call and put options using the Black-Scholes formula.
        /// </summary>
        /// <param name="callResult">The array in which the call option prices are to be stored.</param>
        /// <param name="putResult">The array in which the put option prices are to be stored.</param>
        /// <param name="stockPrice">Contains stock prices.</param>
        /// <param name="optionStrike">Contains the strike prices of the call/put options.</param>
        /// <param name="optionYears">The time-to-expiration of the options, in years.</param>
        /// <param name="riskFree">The risk-free interest rate.</param>
        /// <param name="volatility">The volatility of the underlying stock.</param>
        /// <remarks>
        /// All of the parameter arrays should have the same length.
        /// </remarks>
        [Kernel(AllowFloatingPointDemotion = true)]
        public static void BlackScholesGPU(float[] callResult, float[] putResult, float[] stockPrice, float[] optionStrike, float[] optionYears, float riskFree, float volatility)
        {
            // Declare constants
            const float A1 = 0.31938153f;
            const float A2 = -0.356563782f;
            const float A3 = 1.781477937f;
            const float A4 = -1.821255978f;
            const float A5 = 1.330274429f;
            const float RSQRT2PI = 0.39894228040143267793994605993438f;

            // Thread index
            int ThreadId = BlockDimension.X * BlockIndex.X + ThreadIndex.X;

            // Total number of threads in execution grid
            int TotalThreads = BlockDimension.X * GridDimension.X;

            // No matter how small execution grid is, or how many options we're processing,
            // by using this loop we'll get perfect memory coalescing
            for (int OptionIndex = ThreadId; OptionIndex < callResult.Length; OptionIndex += TotalThreads)
            {
                float s = stockPrice[OptionIndex];
                float x = optionStrike[OptionIndex];
                float t = optionYears[OptionIndex];

                // Calculate the square root of the time to option expiration, in years
                float SqrtT = DeviceMath.Sqrt(t);

                // Calculate the Black-Scholes parameters
                float d1 = (DeviceMath.Log(s / x) + (riskFree + 0.5f * volatility * volatility) * t) / (volatility * SqrtT);
                float d2 = d1 - volatility * SqrtT;

                // Plug the parameters into the Cumulative Normal Distribution (CND)
                float K1 = 1.0f / (1.0f + 0.2316419f * DeviceMath.Abs(d1));
                float CndD1 = RSQRT2PI * DeviceMath.Exp(-0.5f * d1 * d1) *
                    (K1 * (A1 + K1 * (A2 + K1 * (A3 + K1 * (A4 + K1 * A5)))));
                if (d1 > 0) { CndD1 = 1.0f - CndD1; }

                float K2 = 1.0f / (1.0f + 0.2316419f * DeviceMath.Abs(d2));
                float CndD2 = RSQRT2PI * DeviceMath.Exp(-0.5f * d2 * d2) *
                    (K2 * (A1 + K2 * (A2 + K2 * (A3 + K2 * (A4 + K2 * A5)))));
                if (d2 > 0) { CndD2 = 1.0f - CndD2; }

                // Calculate the discount rate
                float ExpRT = DeviceMath.Exp(-1.0f * riskFree * t);

                // Calculate the values of the call and put options
                callResult[OptionIndex] = s * CndD1 - x * ExpRT * CndD2;
                putResult[OptionIndex] = x * ExpRT * (1.0f - CndD2) - s * (1.0f - CndD1);
            }
        }

        /// <summary>
        /// Computes the values of call and put options using the Black-Scholes formula.
        /// </summary>
        /// <param name="callResult">The array in which the call option prices are to be stored.</param>
        /// <param name="putResult">The array in which the put option prices are to be stored.</param>
        /// <param name="stockPrice">Contains stock prices.</param>
        /// <param name="optionStrike">Contains the strike prices of the call/put options.</param>
        /// <param name="optionYears">The time-to-expiration of the options, in years.</param>
        /// <param name="riskFree">The risk-free interest rate.</param>
        /// <param name="volatility">The volatility of the underlying stock.</param>
        /// <remarks>
        /// All of the parameter arrays should have the same length.
        /// </remarks>
        public static void BlackScholesCPU(float[] callResult, float[] putResult, float[] stockPrice, float[] optionStrike, float[] optionYears, float riskFree, float volatility)
        {
            // Declare constants
            const float A1 = 0.31938153f;
            const float A2 = -0.356563782f;
            const float A3 = 1.781477937f;
            const float A4 = -1.821255978f;
            const float A5 = 1.330274429f;
            const float RSQRT2PI = 0.39894228040143267793994605993438f;

            for (int OptionIndex = 0; OptionIndex < callResult.Length; OptionIndex++)
            {
                float s = stockPrice[OptionIndex];
                float x = optionStrike[OptionIndex];
                float t = optionYears[OptionIndex];

                // Calculate the square root of the time to option expiration, in years
                float SqrtT = (float)Math.Sqrt(t);

                // Calculate the Black-Scholes parameters
                float d1 = ((float)Math.Log(s / x) + (riskFree + 0.5f * volatility * volatility) * t) / (volatility * SqrtT);
                float d2 = d1 - volatility * SqrtT;

                // Plug the parameters into the Cumulative Normal Distribution (CND)
                float K1 = 1.0f / (1.0f + 0.2316419f * Math.Abs(d1));
                float CndD1 = RSQRT2PI * (float)Math.Exp(-0.5f * d1 * d1) * (K1 * (A1 + K1 * (A2 + K1 * (A3 + K1 * (A4 + K1 * A5)))));
                if (d1 > 0) { CndD1 = 1.0f - CndD1; }

                float K2 = 1.0f / (1.0f + 0.2316419f * Math.Abs(d2));
                float CndD2 = RSQRT2PI * (float)Math.Exp(-0.5f * d2 * d2) * (K2 * (A1 + K2 * (A2 + K2 * (A3 + K2 * (A4 + K2 * A5)))));
                if (d2 > 0) { CndD2 = 1.0f - CndD2; }

                // Calculate the discount rate
                float ExpRT = (float)Math.Exp(-1.0f * riskFree * t);

                // Calculate the values of the call and put options
                callResult[OptionIndex] = s * CndD1 - x * ExpRT * CndD2;
                putResult[OptionIndex] = x * ExpRT * (1.0f - CndD2) - s * (1.0f - CndD1);
            }
        }

        #endregion
    }
}
