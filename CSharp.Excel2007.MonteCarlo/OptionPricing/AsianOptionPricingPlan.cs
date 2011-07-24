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

namespace TidePowerd.Example.CSharp.Excel2007.MonteCarlo.OptionPricing
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class AsianOptionPricingPlan
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        public readonly float[] SpotPrices;

        /// <summary>
        /// 
        /// </summary>
        public readonly float[] StrikePrices;

        /// <summary>
        /// 
        /// </summary>
        public readonly float[] RiskFreeRates;
        
        /// <summary>
        /// 
        /// </summary>
        public readonly float[] Sigma;

        /// <summary>
        /// 
        /// </summary>
        public readonly float[] Tenor;

        /// <summary>
        /// 
        /// </summary>
        public readonly OptionType[] OptionTypes;

        /// <summary>
        /// 
        /// </summary>
        public readonly int ScenarioCount;

        /// <summary>
        /// 
        /// </summary>
        public readonly int TimeStepCount;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spotPrices"></param>
        /// <param name="strikePrices"></param>
        /// <param name="riskFreeRates"></param>
        /// <param name="sigma"></param>
        /// <param name="tenor"></param>
        /// <param name="optionTypes"></param>
        /// <param name="scenarioCount"></param>
        /// <param name="timeStepCount"></param>
        public AsianOptionPricingPlan(float[] spotPrices, float[] strikePrices, float[] riskFreeRates, float[] sigma, float[] tenor, OptionType[] optionTypes, int scenarioCount, int timeStepCount)
        {
            // Preconditions
            if (spotPrices == null) { throw new ArgumentNullException("spotPrices"); }
            else if (strikePrices == null) { throw new ArgumentNullException("strikePrices"); }
            else if (riskFreeRates == null) { throw new ArgumentNullException("riskFreeRates"); }
            else if (sigma == null) { throw new ArgumentNullException("sigma"); }
            else if (tenor == null) { throw new ArgumentNullException("tenor"); }
            else if (optionTypes == null) { throw new ArgumentNullException("optionTypes"); }

            // Set field values
            SpotPrices = spotPrices;
            StrikePrices = strikePrices;
            RiskFreeRates = riskFreeRates;
            Sigma = sigma;
            Tenor = tenor;
            OptionTypes = optionTypes;
            ScenarioCount = scenarioCount;
            TimeStepCount = timeStepCount;
        }

        #endregion

        #region Properties

        //

        #endregion

        #region Methods

        //

        #endregion
    }
}
