using System;
using System.Diagnostics;

namespace TidePowerd.Example.CSharp.MonteCarlo.Library.OptionPricing
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("Spot = {SpotPrice}, Strike = {StrikePrice}, Rate = {RiskFreeRate}, Sigma = {Volatility}, Tenor = {Tenor}, Type = {Type}")]
    public struct AsianOptionSingle
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

        /* TEMP */
        //
        public readonly float dt;

        #endregion

        #region Constructors

        //
        public AsianOptionSingle(float spotPrice, float strikePrice, float riskFreeRate, float volatility, float tenor, OptionType type, float dt)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO
            
            // Set field values
            SpotPrice = spotPrice;
            StrikePrice = strikePrice;
            RiskFreeRate = riskFreeRate;
            Volatility = volatility;
            Tenor = tenor;
            Type = type;
            this.dt = dt;
        }

        #endregion
    }
}
