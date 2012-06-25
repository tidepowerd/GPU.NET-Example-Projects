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
using System.Diagnostics.Contracts;
using System.Diagnostics;

namespace TidePowerd.Example.OptionPricing.Library.AsianOption
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("Spot = {SpotPrice}, Strike = {StrikePrice}, Rate = {RiskFreeRate}, Sigma = {Volatility}, Tenor = {Tenor}, Type = {Type}")]
    public sealed class AsianOptionSingle
    {
        #region Fields

        //
        public readonly float SpotPrice;

        //
        public readonly float StrikePrice;

        //
        public readonly float RiskFreeRate;

        //
        public readonly float Volatility;

        //
        public readonly float Tenor;

        //
        public readonly OptionType Type;

        //
        public readonly float dt;       // Timestep

        #endregion

        #region Constructors

        //
        public AsianOptionSingle(float spotPrice, float strikePrice, float riskFreeRate, float volatility, float tenor, OptionType type, float timestep)
        {
            // Preconditions
            Contract.Requires(spotPrice >= 0.0f, "The spot price is negative.");
            Contract.Requires(strikePrice >= 0.0f, "The strike price is negative.");
            Contract.Requires(riskFreeRate >= 0.0f, "The risk-free rate is negative.");
            Contract.Requires(volatility >= 0.0f, "The volatility (sigma) is negative.");
            Contract.Requires(tenor >= 0.0f, "The tenor is negative.");
            Contract.Requires(timestep > 0.0f, "The spot price is negative.");

            // Postconditions
            // TODO

            // Set field values
            this.SpotPrice = spotPrice;
            this.StrikePrice = strikePrice;
            this.RiskFreeRate = riskFreeRate;
            this.Volatility = volatility;
            this.Tenor = tenor;
            this.Type = type;
            this.dt = timestep;
        }

        #endregion

        #region Methods

        ////
        //[ContractInvariantMethod]
        //private void ObjectInvariant()
        //{
        //    //
        //}

        #endregion
    }
}
