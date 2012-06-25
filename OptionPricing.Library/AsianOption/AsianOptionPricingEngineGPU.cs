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

namespace TidePowerd.Example.OptionPricing.Library.AsianOption
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("SimulationCount = {m_simulationCount}")]
    public sealed class AsianOptionPricingEngineGPU : IAsianOptionPricingEngine
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private readonly int m_simulationCount;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulationCount"></param>
        public AsianOptionPricingEngineGPU(int simulationCount)
        {
            // Preconditions
            Contract.Requires(simulationCount > 0);

            // Postconditions
            Contract.Ensures(Contract.ValueAtReturn<int>(out m_simulationCount) > 0);

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
        /// <param name="option"></param>
        /// <returns></returns>
        float IAsianOptionPricingEngine.CalculatePrice(AsianOptionSingle option)
        {
            return AsianOptionPricingEngineKernels.CalculatePrice(m_simulationCount, option);
        }

        #endregion
    }
}
