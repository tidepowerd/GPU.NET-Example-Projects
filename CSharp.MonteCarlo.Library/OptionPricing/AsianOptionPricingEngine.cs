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

// GPU.NET Example Project : Monte Carlo (C# Library)
// More examples available at http://github.com/tidepowerd

using System;
using System.Diagnostics;

namespace TidePowerd.Example.CSharp.MonteCarlo.Library.OptionPricing
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("SimulationCount = {SimulationCount}, TimestepLength = {TimestepLength}")]
    public sealed class AsianOptionPricingEngine
    {
        #region Fields

        //
        public readonly int SimulationCount;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="simulationCount"></param>
        public AsianOptionPricingEngine(int simulationCount)
        {
            // Preconditions
            if (simulationCount < 1) { throw new ArgumentOutOfRangeException("simulationCount"); }

            // Postconditions
            //

            // Set field values
            SimulationCount = simulationCount;
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
